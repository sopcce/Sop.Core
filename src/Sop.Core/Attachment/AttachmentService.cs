using Aliyun.OpenServices.OpenStorageService;
using Common.Logging;
using HuobanYun.Core.Enums;
using HuobanYun.Core.Extensions;
using HuobanYun.Core.Models;
using HuobanYun.Core.Search;
using HuobanYun.Database;
using HuobanYun.Redis;
using HuobanYun.Search;
using HuobanYun.Utilities;
using ImageResizer;
using ImageResizer.Configuration;
using ImageResizer.Plugins.AnimatedGifs;
using ImageResizer.Plugins.Basic;
using ImageResizer.Plugins.PrettyGifs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace HuobanYun.Core.Services
{
    /// <summary>
    /// 附件服务
    /// </summary>
    public class AttachmentService
    {
        private static readonly ILog logger = LogManager.GetLogger<AttachmentService>();
        private static readonly string imageFileExtensions = "jpg,jpeg,png,gif";
        private static readonly string audioFileExtensions = "mp3,wav,wma,mpa";
        private static readonly string videoFileExtensions = "avi,asf,rm,rmvb,mpeg,mpg,mp4,mkv,flv,mov,wmv,3gp";
        private static readonly string officeFileExtensions = "doc,docx,docm,xls,xlsx,ppt,pptx,rtf,ods,odt";

        public IRepository<Attachment> attachmentRepository { get; set; }
        public IRepository<AttachmentScope> attachmentScopeRepository { get; set; }
        public IRepository<Comment> commentRepository { get; set; }
        public UserService userService { get; set; }
        public ActivityService activityService { get; set; }
        public FavoriteService favoriteService { get; set; }
        public KnowledgeBaseService knowledgeBaseService { get; set; }
        public CommentService commentService { get; set; }
        public IIndexer<KnowledgeMapper> knowledgeIndexer { get; set; }
        public RedisCache cache { get; set; }
        public RedisQueue queue { get; set; }
        public RedisCounter counter { get; set; }

        /// <summary>
        /// 用于内网连接（API访问）的OSS客户端
        /// </summary>
        private OssClient _ossClientAPI;
        private OssClient OssClientAPI
        {
            get
            {
                if (this._ossClientAPI != null) return this._ossClientAPI;

                string endpoint = ConfigurationManager.AppSettings["AliyunOSS.Endpoint.API"];
                string accessKeyID = ConfigurationManager.AppSettings["AliyunOSS.AccessKeyID"];
                string accessKeySecret = ConfigurationManager.AppSettings["AliyunOSS.AccessKeySecret"];

                this._ossClientAPI = new OssClient(endpoint, accessKeyID, accessKeySecret);

                return this._ossClientAPI;
            }
        }

        /// <summary>
        /// 用于外网连接（URL访问）的OSS客户端
        /// </summary>
        private OssClient _ossClientAttachment;
        private OssClient OssClientAttachment
        {
            get
            {
                if (this._ossClientAttachment != null) return this._ossClientAttachment;

                string endpoint = ConfigurationManager.AppSettings["AliyunOSS.Endpoint.Attachment"];
                string accessKeyID = ConfigurationManager.AppSettings["AliyunOSS.AccessKeyID"];
                string accessKeySecret = ConfigurationManager.AppSettings["AliyunOSS.AccessKeySecret"];

                this._ossClientAttachment = new OssClient(endpoint, accessKeyID, accessKeySecret);

                return this._ossClientAttachment;
            }
        }

        private string bucket = ConfigurationManager.AppSettings["AliyunOSS.Bucket.Attachment"];

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="user">上传的操作者</param>
        /// <param name="file">客户端Post的文件对象</param>
        /// <returns>上传到OSS的object key</returns>
        public string Upload(User user, HttpPostedFileBase file)
        {
            var metadata = new ObjectMetadata();
            metadata.UserMetadata.Add("FileName", file.FileName);
            metadata.UserMetadata.Add("ContentType", file.ContentType);

            //在创建附件对象前，全部暂存至temp目录
            string key = string.Format("temp/{0}.{1}", Guid.NewGuid().ToString(), FileUtility.GetFileExtension(file.FileName));
            string bucket = ConfigurationManager.AppSettings["AliyunOSS.Bucket.Attachment"];

            //上传文档至OSS
            using (Stream stream = file.InputStream)
            {
                OssClientAPI.PutObject(bucket, key, stream, metadata);
            }

            return key;
        }

        /// <summary>
        /// 直接上传附件
        /// </summary>
        /// <param name="user">上传的操作者</param>
        /// <param name="file">客户端Post的文件对象</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="tenantType">租户类型</param>
        /// <returns>上传到OSS的object key</returns>
        public Attachment DirectCreate(User user, HttpPostedFileBase file, long tenantId, string tenantType)
        {
            string fileName = file.FileName;
            //去掉win10 edge浏览器文件名的盘符
            if (file.FileName.IndexOf('\\') != -1)
            {
                int lastIndex = fileName.LastIndexOf('\\');
                fileName = fileName.Substring(lastIndex + 1, fileName.Length - lastIndex - 1);
            }

            var metadata = new ObjectMetadata();
            metadata.UserMetadata.Add("FileName", fileName);
            metadata.UserMetadata.Add("ContentType", file.ContentType);

            string key = string.Format("{0}/{1}/{2}.{3}", user.Company.Id, user.Id, Guid.NewGuid().ToString(), FileUtility.GetFileExtension(fileName));

            //上传文档至OSS
            using (Stream stream = file.InputStream)
            {
                OssClientAPI.PutObject(bucket, key, stream, metadata);
            }

            metadata = OssClientAPI.GetObjectMetadata(bucket, key);

            //磁盘空间计数
            counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().User(), user.Id, metadata.ContentLength);
            counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().Company(), user.Company.Id, metadata.ContentLength);

            var extension = FileUtility.GetFileExtension(key);

            var attachment = new Attachment()
            {
                TenantId = tenantId,
                TenantType = tenantType,
                Name = metadata.UserMetadata["FileName"],
                Path = key,
                Extension = extension,
                Size = metadata.ContentLength,
                ContentType = metadata.UserMetadata["ContentType"],
                MediaType = getMediaType(metadata.ContentType, extension),
                Version = 1,
                UserCreated = user,
                UserModified = user,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            attachmentRepository.Create(attachment);

            //图片缩放
            if (attachment.MediaType == MediaType.Image)
            {
                this.resizeImage(attachment.Path, user);
            }

            //加入Redis队列，由文件服务器进行转换
            queue.Push("Queue:ConvertAttachment", attachment.Id);

            return attachment;
        }

        /// <summary>
        /// 替换更新
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="file">文件</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="tenantType">租户类型</param>
        /// <param name="oldAttachment">源版本</param>
        /// <returns></returns>
        public Attachment UploadAndUpdate(User user, HttpPostedFileBase file, long tenantId, string tenantType, Attachment oldAttachment)
        {
            
            string fileName = file.FileName;
            //去掉win10 edge浏览器文件名的盘符
            if (file.FileName.IndexOf('\\') != -1)
            {
                int lastIndex = fileName.LastIndexOf('\\');
                fileName = fileName.Substring(lastIndex + 1, fileName.Length - lastIndex - 1);
            }

            #region 上传

            var metadata = new ObjectMetadata();
            metadata.UserMetadata.Add("FileName", fileName);
            metadata.UserMetadata.Add("ContentType", file.ContentType);

            string key = string.Format("{0}/{1}/{2}.{3}", user.Company.Id, user.Id, Guid.NewGuid().ToString(), FileUtility.GetFileExtension(fileName));

            //上传文档至OSS
            using (Stream stream = file.InputStream)
            {
                OssClientAPI.PutObject(bucket, key, stream, metadata);
            }

            metadata = OssClientAPI.GetObjectMetadata(bucket, key);

            //磁盘空间计数
            counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().User(), user.Id, metadata.ContentLength);
            counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().Company(), user.Company.Id, metadata.ContentLength);

            var extension = FileUtility.GetFileExtension(key);

            #endregion

            #region 创建 Attachment

            var attachment = new Attachment()
                {
                    TenantId = tenantId,
                    TenantType = tenantType,
                    Name = metadata.UserMetadata["FileName"],
                    Path = key,
                    Extension = extension,
                    Size = metadata.ContentLength,
                    ContentType = metadata.UserMetadata["ContentType"],
                    MediaType = getMediaType(metadata.ContentType, extension),
                    Version = 1,
                    UserCreated = user,
                    UserModified = user,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };

            attachmentRepository.Create(attachment);

            #endregion

            #region 图片缩放

            if (attachment.MediaType == MediaType.Image)
            {
                this.resizeImage(attachment.Path, user);
            }

            #endregion


            #region 版本更新

            Attachment temp = (Attachment)oldAttachment.Clone();

            //将源版本更新成最新版本
            oldAttachment.Path = attachment.Path;
            oldAttachment.Version++;
            oldAttachment.DateModified = DateTime.UtcNow;
            oldAttachment.DateCreated = DateTime.UtcNow;
            oldAttachment.ContentType = attachment.ContentType;
            oldAttachment.Extension = attachment.Extension;
            oldAttachment.HasThumbnail = attachment.HasThumbnail;
            oldAttachment.HtmlPreview = attachment.HtmlPreview;
            oldAttachment.MediaType = attachment.MediaType;
            oldAttachment.Name = attachment.Name;
            oldAttachment.PageCount = attachment.PageCount;
            oldAttachment.Size = attachment.Size;
            oldAttachment.UserModified = attachment.UserModified;
            //oldAttachment.VideoPlay = attachment.VideoPlay;
            this.Update(oldAttachment, true);

            //将新上传的版本更新成历史版本
            attachment.Path = temp.Path;
            attachment.Version = temp.Version;
            attachment.ContentType = temp.ContentType;
            attachment.Extension = temp.Extension;
            attachment.HasThumbnail = temp.HasThumbnail;
            attachment.HtmlPreview = temp.HtmlPreview;
            attachment.MediaType = temp.MediaType;
            attachment.Name = temp.Name;
            attachment.PageCount = temp.PageCount;
            attachment.Size = temp.Size;
            //attachment.VideoPlay = temp.VideoPlay;
            attachment.Reference = temp;
            attachment.DateCreated = temp.DateCreated;
            attachment.DateModified = temp.DateModified;
            this.Update(attachment);

            this.resetAttachmentCache(oldAttachment);

            #endregion

            //加入Redis队列，由文件服务器进行转换
            queue.Push("Queue:ConvertAttachment", oldAttachment.Id);

            return oldAttachment;
        }

        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="key">附件文件在OSS上的object key</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="tenantType">租户类型</param>
        /// <param name="user">附件的创建者</param>
        /// <returns>创建后的新附件对象</returns>
        public Attachment Create(string key, long tenantId, string tenantType, User user)
        {
            var metadata = OssClientAPI.GetObjectMetadata(bucket, key);
            if (metadata == null)
            {
                return null;
            }
            string targetKey = key.Replace("temp/", string.Format("{0}/{1}/", user.Company.Id, user.Id));

            //将文件从oss的temp目录移动到用户目录
            OssClientAPI.CopyObject(new CopyObjectRequest(bucket, key, bucket, targetKey));
            OssClientAPI.DeleteObject(bucket, key);

            //磁盘空间计数
            counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().User(), user.Id, metadata.ContentLength);
            counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().Company(), user.Company.Id, metadata.ContentLength);

            var extension = FileUtility.GetFileExtension(key);

            var attachment = new Attachment()
            {
                TenantId = tenantId,
                TenantType = tenantType,
                Name = metadata.UserMetadata["FileName"],
                Path = targetKey,
                Extension = extension,
                Size = metadata.ContentLength,
                ContentType = metadata.UserMetadata["ContentType"],
                MediaType = getMediaType(metadata.ContentType, extension),
                Version = 1,
                UserCreated = user,
                UserModified = user,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            attachmentRepository.Create(attachment);

            //图片缩放
            if (attachment.MediaType == MediaType.Image)
            {
                this.resizeImage(attachment.Path, user);
            }

            //加入Redis队列，由文件服务器进行转换
            queue.Push("Queue:ConvertAttachment", attachment.Id);

            return attachment;
        }

        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="attachment">附件对象</param>
        /// <param name="updateIndex">是否更新索引</param>
        public void Update(Attachment attachment, bool updateIndex = false)
        {
            attachmentRepository.Update(attachment);

            if (updateIndex)
            {
                var kib = knowledgeBaseService.GetKib(attachment.Id, TenantTypes.Instance().Attachment());
                if (kib != null)
                {
                    //文库里的附件需要更新索引
                    knowledgeIndexer.Index(new KnowledgeMapper(kib));
                }
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="attachment">附件对象</param>
        public void Delete(Attachment attachment)
        {
            //查找所有的历史版本
            var referenceAttachments = attachmentRepository.Fetch(a => a.Reference.Id == attachment.Id).ToList();
            referenceAttachments.Add(attachment);

            long deleteTotalSize = 0;

            //删除附件在OSS上的文件
            foreach (var item in referenceAttachments)
            {
                //删除附件对象
                attachmentRepository.Delete(item);

                //删除物理文件
                var key = item.Path.Replace("." + item.Extension, string.Empty);
                var listResult = OssClientAPI.ListObjects(bucket, key);
                var objectSummaries = listResult.ObjectSummaries;
                var deleteSize = objectSummaries.Sum(n => n.Size);
                var req = new DeleteObjectsRequest(bucket, objectSummaries.Select(n => n.Key).ToList(), true);
                OssClientAPI.DeleteObjects(req);

                //更新用户磁盘空间计数
                counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().User(), item.UserCreated.Id, -deleteSize);
                deleteTotalSize += deleteSize;
            }

            //更新企业磁盘空间计数
            counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().Company(), attachment.UserCreated.Company.Id, -deleteTotalSize);

            //删除附件关联的对我有用
            var users = favoriteService.GetFavoriteUsers(attachment.Id, TenantTypes.Instance().Attachment());
            foreach (var user in users)
            {
                favoriteService.CancelFavorite(attachment.Id, TenantTypes.Instance().Attachment(), user.Id);
            }

            //删除附件关联的回复
            commentService.Delete(TenantTypes.Instance().Attachment(), attachment.Id);
        }

        /// <summary>
        /// 删除租户对象关联的所有附件
        /// </summary>
        /// <param name="tenantId">租户Id</param>
        /// <param name="tenantType">租户类型</param>
        public void Delete(long tenantId, string tenantType)
        {
            var attachments = this.Gets(tenantId, tenantType);

            foreach (var attachment in attachments)
            {
                this.Delete(attachment);
            }
        }

        /// <summary>
        /// 创建附件的可见范围
        /// </summary>
        /// <param name="attachment">附件对象</param>
        /// <param name="tenantId">租户Id，例如UserId或DepartmentId</param>
        /// <param name="tenantType">租户类型</param>
        public void CreateScope(Attachment attachment, long tenantId, string tenantType)
        {
            var attachmentScope = new AttachmentScope()
            {
                Attachment = attachment,
                TenantId = tenantId,
                TenantType = tenantType
            };

            attachmentScopeRepository.Create(attachmentScope);
        }
        
        /// <summary>
        /// 获取附件的正文内容
        /// </summary>
        /// <param name="attachment">附件实例</param>
        /// <returns>附件的正文内容</returns>
        public string GetAttachmentIndexBody(Attachment attachment)
        {
            string indexFileExtensions = "doc,docx,docm,rtf,odt,htm,html,mht,xml,txt,xls,xlsx,ods,ppt,pptx,pdf";
            if (indexFileExtensions.Contains(attachment.Extension))
            {
                var ossObject = OssClientAPI.GetObject(new GetObjectRequest(bucket, attachment.Path));

                using (Stream stream = ossObject.Content)
                {
                    int flag = 0;
                    List<byte> bytes = new List<byte>();

                    while (flag != -1)
                    {
                        flag = stream.ReadByte();
                        bytes.Add((byte)flag);
                    }

                    if (bytes.Any())
                    {
                        return Convert.ToBase64String(bytes.ToArray());
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取一个附件
        /// </summary>
        /// <param name="id">附件Id</param>
        /// <returns>附件对象</returns>
        public Attachment Get(long id)
        {
            return attachmentRepository.Get(id);
        }

        /// <summary>
        /// 获取某个租户的所有关联附件
        /// </summary>
        /// <param name="tenantId">租户Id</param>
        /// <param name="tenantType">租户类型</param>
        /// <returns>附件对象集合</returns>
        public IEnumerable<Attachment> Gets(long tenantId, string tenantType)
        {
            return attachmentRepository.Fetch(t => t.TenantId == tenantId && t.TenantType == tenantType);
        }

        /// <summary>
        /// 根据id批量获取附件对象
        /// </summary>
        /// <param name="ids">附件Id列表</param>
        /// <returns>附件对象集合</returns>
        public IEnumerable<Attachment> Gets(IEnumerable<long> ids)
        {
            return attachmentRepository.Fetch(t => ids.Contains(t.Id));
        }

        /// <summary>
        /// 我发出的附件
        /// </summary>
        /// <param name="user">我</param>
        /// <param name="tenantType">租户类型</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>附件对象的分页集合</returns>
        public PagingDataSet<Attachment> GetUserPublished(User user, string tenantType, int pageSize, int pageIndex)
        {
            var query = from attachment in attachmentRepository.Table
                        join attachmentScope in attachmentScopeRepository.Table on attachment equals attachmentScope.Attachment
                        where attachment.UserCreated == user
                        orderby attachment.Id descending
                        select attachment;

            if (!string.IsNullOrEmpty(tenantType))
            {
                query = query.Where(attachment => attachment.TenantType.StartsWith(tenantType) || commentRepository.Table.Any(comment => attachment.TenantId == comment.Id
                        && attachment.TenantType == TenantTypes.Instance().Comment()
                        && comment.TenantType == tenantType));
            }

            query = query.Distinct();

            return new PagingDataSet<Attachment>(query.Skip(pageSize * (pageIndex - 1)).Take(pageSize))
            {
                TotalRecords = query.LongCount(),
                PageSize = pageSize,
                PageIndex = pageIndex
            };
        }

        /// <summary>
        /// 我收到的附件
        /// </summary>
        /// <param name="user">我</param>
        /// <param name="tenantType">租户类型</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>附件对象的分页集合</returns>
        public PagingDataSet<Attachment> GetUserRecieved(User user, string tenantType, int pageSize, int pageIndex)
        {
            // 获取用户所在的直属部门及其上级部门
            var departmentIds = userService.GetAllDepartmentIds(user);

            var query = from attachment in attachmentRepository.Table
                        join attachmentScope in attachmentScopeRepository.Table on attachment equals attachmentScope.Attachment
                        where (attachmentScope.TenantType == TenantTypes.Instance().User() && attachmentScope.TenantId == user.Id)
                              ||
                              (attachmentScope.TenantType == TenantTypes.Instance().Department() && departmentIds.Contains(attachmentScope.TenantId))
                        orderby attachment.Id descending
                        select attachment;

            if (!string.IsNullOrEmpty(tenantType))
            {
                query = query.Where(attachment => attachment.TenantType.StartsWith(tenantType)
                     || commentRepository.Table.Any(comment => attachment.TenantId == comment.Id
                         && attachment.TenantType == TenantTypes.Instance().Comment()
                         && comment.TenantType == tenantType));
            }

            return new PagingDataSet<Attachment>(query.Skip(pageSize * (pageIndex - 1)).Take(pageSize))
            {
                TotalRecords = query.LongCount(),
                PageSize = pageSize,
                PageIndex = pageIndex
            };
        }

        /// <summary>
        /// 生成附件的下载链接
        /// </summary>
        /// <param name="attachment">附件对象</param>
        /// <returns>链接地址</returns>
        public string GenerateAttachmentDownloadUrl(Attachment attachment)
        {
            var req = new GeneratePresignedUriRequest(bucket, attachment.Path, SignHttpMethod.Get);
            req.Expiration = DateTime.UtcNow.AddMinutes(1);
            req.ResponseHeaders.ContentEncoding = "utf-8";
            req.ResponseHeaders.ContentType = attachment.ContentType;
            req.ResponseHeaders.ContentDisposition = string.Format("attachment; filename=\"{0}\"", attachment.Name);

            return OssClientAttachment.GeneratePresignedUri(req).ToString();
        }

        /// <summary>
        /// 生成附件的预览链接
        /// </summary>
        /// <param name="attachment">附件对象</param>
        /// <returns>链接地址</returns>
        public string GenerateAttachmentPreviewUrl(Attachment attachment)
        {
            if (!attachment.HtmlPreview.HasValue || !attachment.HtmlPreview.Value)
            {
                return string.Empty;
            }

            string previewKey = attachment.Path.Replace("." + attachment.Extension, "/preview.html");
            var req = new GeneratePresignedUriRequest(bucket, previewKey, SignHttpMethod.Get);
            req.Expiration = DateTime.UtcNow.AddMinutes(1);
            req.ResponseHeaders.ContentType = "text/html";
            req.ResponseHeaders.ContentDisposition = "inline";

            return OssClientAttachment.GeneratePresignedUri(req).OriginalString;
        }

        /// <summary>
        /// 生成图片的预览图链接
        /// </summary>
        /// <param name="attachment">附件对象</param>
        /// <returns>链接地址</returns>
        public string GenerateImagePreviewUrl(Attachment attachment)
        {
            string cacheKey = "Attachment:ImagePreview#" + attachment.Id;

            string url = cache.Get<string>(cacheKey);

            if (string.IsNullOrEmpty(url))
            {
                var req = new GeneratePresignedUriRequest(bucket, getImagePreviewKey(attachment.Path), SignHttpMethod.Get);
                req.Expiration = DateTime.UtcNow.AddDays(1);
                req.ResponseHeaders.ContentType = attachment.ContentType;
                req.ResponseHeaders.ContentDisposition = "inline";

                url = OssClientAttachment.GeneratePresignedUri(req).OriginalString;
                cache.Set(cacheKey, url, new TimeSpan(1, 0, 0));
            }

            return url;
        }

        /// <summary>
        /// 生成图片详情页大图链接
        /// </summary>
        /// <param name="attachment">附件对象</param>
        /// <returns>链接地址</returns>
        public string GenerateImageDetailUrl(Attachment attachment)
        {
            string cacheKey = "Attachment:ImageDetail#" + attachment.Id;

            string url = cache.Get<string>(cacheKey);

            if (string.IsNullOrEmpty(url))
            {
                var req = new GeneratePresignedUriRequest(bucket, getImageDetailKey(attachment.Path), SignHttpMethod.Get);
                req.Expiration = DateTime.UtcNow.AddDays(1);
                req.ResponseHeaders.ContentType = attachment.ContentType;
                req.ResponseHeaders.ContentDisposition = "inline";

                url = OssClientAttachment.GeneratePresignedUri(req).OriginalString;
                cache.Set(cacheKey, url, new TimeSpan(1, 0, 0));
            }

            return url;
        }

        /// <summary>
        /// 生成图片的原始图片链接
        /// </summary>
        /// <param name="attachment">附件对象</param>
        /// <returns>链接地址</returns>
        public string GenerateImageOriginalUrl(Attachment attachment)
        {
            string cacheKey = "Attachment:ImageOriginal#" + attachment.Id;

            string url = cache.Get<string>(cacheKey);

            if (string.IsNullOrEmpty(url))
            {
                var req = new GeneratePresignedUriRequest(bucket, attachment.Path, SignHttpMethod.Get);
                req.Expiration = DateTime.UtcNow.AddDays(1);
                req.ResponseHeaders.ContentType = attachment.ContentType;
                req.ResponseHeaders.ContentDisposition = "inline";

                url = OssClientAttachment.GeneratePresignedUri(req).OriginalString;
                cache.Set(cacheKey, url, new TimeSpan(1, 0, 0));
            }

            return url;
        }

        /// <summary>
        /// 附件版本更新时清楚缓存
        /// </summary>
        /// <param name="attachment">附件对象</param>
        private void resetAttachmentCache(Attachment attachment)
        {
            string[] cacheKeyArray = new string[] { 
                "Attachment:ImagePreview#" + attachment.Id,
                "Attachment:ImageDetail#" + attachment.Id,
                "Attachment:ImageOriginal#" + attachment.Id
            };

            foreach (var cacheKey in cacheKeyArray)
            {
                cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// 图片缩放和裁剪
        /// </summary>
        /// <param name="key">图片在OSS的key</param>
        /// <param name="user">操作用户</param>
        private void resizeImage(string key, User user)
        {
            var extension = FileUtility.GetFileExtension(key);

            var imageResizerConfig = new Config();
            new AnimatedGifs().Install(imageResizerConfig);
            new PrettyGifs().Install(imageResizerConfig);

            var ossObject = OssClientAPI.GetObject(new GetObjectRequest(bucket, key));

            try
            {
                using (var sourceStream = new MemoryStream())
                {
                    using (Stream stream = ossObject.Content)
                    {
                        stream.CopyTo(sourceStream);
                    }

                    //动态预览图（裁剪）
                    sourceStream.Position = 0;
                    using (Stream destStream = new MemoryStream())
                    {
                        var resizeSettings = new ResizeSettings()
                        {
                            MaxWidth = 240,
                            MaxHeight = 180,
                            Mode = FitMode.Crop
                        };

                        imageResizerConfig.CurrentImageBuilder.Build(sourceStream, destStream, resizeSettings, false);

                        destStream.Position = 0;

                        OssClientAPI.PutObject(bucket, getImagePreviewKey(key), destStream);

                        //磁盘空间计数
                        counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().User(), user.Id, destStream.Length);
                        counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().Company(), user.Company.Id, destStream.Length);
                    }

                    //详情页图（等比例缩放）
                    sourceStream.Position = 0;
                    using (Stream destStream = new MemoryStream())
                    {
                        var resizeSettings = new ResizeSettings()
                        {
                            MaxWidth = 1024,
                            MaxHeight = 10000
                        };

                        imageResizerConfig.CurrentImageBuilder.Build(sourceStream, destStream, resizeSettings, false);

                        destStream.Position = 0;

                        OssClientAPI.PutObject(bucket, getImageDetailKey(key), destStream);

                        //磁盘空间计数
                        counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().User(), user.Id, destStream.Length);
                        counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().Company(), user.Company.Id, destStream.Length);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        /// <summary>
        /// 获取图片预览图文件再OSS中的object key
        /// </summary>
        /// <param name="key">原始文件的object key</param>
        /// <returns>预览图文件的object key</returns>
        private string getImagePreviewKey(string key)
        {
            return key.Replace(".", ".preview.");
        }

        /// <summary>
        /// 获取图片详情页大图文件再OSS中的object key
        /// </summary>
        /// <param name="key">原始文件的object key</param>
        /// <returns>详情页大图的object key</returns>
        private string getImageDetailKey(string key)
        {
            return key.Replace(".", ".detail.");
        }

        /// <summary>
        /// 获取附件的媒体类型
        /// </summary>
        /// <param name="contentType">MIME类型</param>
        /// <param name="extension">扩展名</param>
        /// <returns>MediaType</returns>
        private MediaType getMediaType(string contentType, string extension)
        {
            MediaType mediaType = MediaType.Other;

            if (imageFileExtensions.Contains(extension))
            {
                mediaType = MediaType.Image;
            }
            else if (videoFileExtensions.Contains(extension))
            {
                mediaType = MediaType.Video;
            }
            else if (audioFileExtensions.Contains(extension))
            {
                mediaType = MediaType.Audio;
            }
            else if (officeFileExtensions.Contains(extension))
            {
                mediaType = MediaType.Office;
            }
            else if (contentType.StartsWith("text"))
            {
                mediaType = MediaType.Text;
            }
            else if (contentType.EndsWith("x-shockwave-flash"))
            {
                mediaType = MediaType.Flash;
            }
            else if (extension.Equals("pdf"))
            {
                mediaType = MediaType.PDF;
            }

            return mediaType;
        }

        /// <summary>
        /// 历史版本分页
        /// </summary>
        /// <param name="attachmentId">文章Id</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<Attachment> GetsHistory(long attachmentId, int pageSize, int pageIndex)
        {
            var query = attachmentRepository.Fetch(t => t.Reference.Id == attachmentId).OrderByDescending(t => t.Version);

            return new PagingDataSet<Attachment>(query.Skip(pageSize * (pageIndex - 1)).Take(pageSize))
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = query.LongCount()
            };
        }

        /// <summary>
        /// 回滚附件版本
        /// </summary>
        /// 
        /// <param name="toAttachmentId">要回滚到的附件Id</param>
        /// <param name="user">操作人</param>
        public void Rollback(long toAttachmentId, User user)
        {
            Attachment historyAttachment = attachmentRepository.Get(toAttachmentId);

            Attachment currentAttachment = attachmentRepository.Get(historyAttachment.Reference.Id);

            if (historyAttachment.Version >= currentAttachment.Version)
            {
                return;
            }

            //将当前版本保存为一个新的历史版本
            Attachment currentHistoryAttachment = (Attachment)currentAttachment.Clone();
            currentHistoryAttachment.Reference = currentAttachment;
            attachmentRepository.Create(currentHistoryAttachment);

            //将主版本的信息更新为要回滚的版本信息，同时递增版本号
            currentAttachment.Name = historyAttachment.Name;
            currentAttachment.Path = historyAttachment.Path;
            currentAttachment.Version++;
            currentAttachment.DateModified = DateTime.UtcNow;
            currentAttachment.DateCreated = DateTime.UtcNow;
            currentAttachment.ContentType = historyAttachment.ContentType;
            currentAttachment.Extension = historyAttachment.Extension;
            currentAttachment.HasThumbnail = historyAttachment.HasThumbnail;
            currentAttachment.HtmlPreview = historyAttachment.HtmlPreview;
            currentAttachment.MediaType = historyAttachment.MediaType;
            currentAttachment.PageCount = historyAttachment.PageCount;
            currentAttachment.Size = historyAttachment.Size;
            currentAttachment.UserModified = user;
            currentAttachment.VideoPlay = historyAttachment.VideoPlay;
            this.Update(currentAttachment, true);
        }
    }
}
