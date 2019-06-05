using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sop.Services.Model
{
    /// <summary>
    /// Description 
    /// </summary>
    [Serializable]
    public class ForumArticleInfo
    {


        ///<Summary>
        /// 全局唯一文档id，根据全局唯一ID生成规则生成
        ///</Summary>
        public virtual long Id { get; set; }
        ///<Summary>
        /// 通道内数据的唯一标识,如tsina_3961867459907808
        ///</Summary>
        public virtual string Key { get; set; }
        ///<Summary>
        /// 消息生成/发布时间
        ///</Summary>
        public virtual DateTime PublishTime { get; set; }
        ///<Summary>
        /// 是否因为未获取/爬取到发布时间，而由平台自动写入发布时间，若是则为1，若否则为0。默认值为0，或为空。
        ///</Summary>
        public virtual int IsPtGenerated { get; set; }
        ///<Summary>
        /// 消息获取时间，从1970年1月1日00:00:00开始的秒数
        ///</Summary>
        public virtual DateTime GatherTime { get; set; }
        ///<Summary>
        /// 消息写入数据库时间，从1970年1月1日00:00:00开始的秒数
        ///</Summary>
        public virtual DateTime InsertTime { get; set; }
        ///<Summary>
        /// 消息状态信息更新时间，从1970年1月1日00:00:00开始的秒数
        ///</Summary>
        public virtual DateTime UpdateTime { get; set; }
        ///<Summary>
        /// 文档对应的url（可通过url获取该文档），如"http://weibo.com/1799068005/DpXsdFztm"
        ///</Summary>
        public virtual string Url { get; set; }
        ///<Summary>
        /// 消息的形式，可为文字、纯图片、纯表情符号、音视频、弹幕
        ///</Summary>
        public virtual string Forms { get; set; }
        ///<Summary>
        /// 消息的属性，对于跟帖，可为热门跟帖、最新跟贴或其他
        ///</Summary>
        public virtual string Properties { get; set; }
        ///<Summary>
        /// 文档标题
        ///</Summary>
        public virtual string Title { get; set; }
        ///<Summary>
        /// 消息内容
        ///</Summary>
        public virtual string Content { get; set; }
        ///<Summary>
        /// 原始html消息内容
        ///</Summary>
        public virtual string HtmlContent { get; set; }
        ///<Summary>
        /// 表情符号
        ///</Summary>
        public virtual string Emoticon { get; set; }
        ///<Summary>
        /// 消息的语种
        ///</Summary>
        public virtual string Language { get; set; }
        ///<Summary>
        /// 数据境内外标识
        ///</Summary>
        public virtual string DomainSign { get; set; }
        ///<Summary>
        /// 站点ID
        ///</Summary>
        public virtual string SiteId { get; set; }
        ///<Summary>
        /// 板块/频道名称
        ///</Summary>
        public virtual string BoardName { get; set; }
        ///<Summary>
        /// 状态
        ///</Summary>
        public virtual int Status { get; set; }
        ///<Summary>
        /// 用户当前ip,如1967118912
        ///</Summary>
        public virtual int SendIp { get; set; }
        ///<Summary>
        /// 当前消息评论数
        ///</Summary>
        public virtual int ReplyCount { get; set; }
        ///<Summary>
        /// 跟帖人数
        ///</Summary>
        public virtual int ReplyUserCount { get; set; }
        ///<Summary>
        /// 当前消息转发数
        ///</Summary>
        public virtual int ForwardCount { get; set; }
        ///<Summary>
        /// 点赞量
        ///</Summary>
        public virtual int LikeCount { get; set; }
        ///<Summary>
        /// 被踩量
        ///</Summary>
        public virtual int TrampledCount { get; set; }
        ///<Summary>
        /// 盖章量
        ///</Summary>
        public virtual int SealedCont { get; set; }
        ///<Summary>
        /// 打赏量
        ///</Summary>
        public virtual int RewardedCount { get; set; }
        ///<Summary>
        /// 推送/送达量，要闻推送的新闻数
        ///</Summary>
        public virtual int SendCount { get; set; }
        ///<Summary>
        /// PV量
        ///</Summary>
        public virtual int PvCount { get; set; }
        ///<Summary>
        /// UV量
        ///</Summary>
        public virtual int UvCount { get; set; }
        ///<Summary>
        /// 图片数量
        ///</Summary>
        public virtual int PictureCount { get; set; }
        ///<Summary>
        /// 图片的URL列表，如["http://ww2.sinaimg.cn/large/6b3b9965gw1f2ovawd7nej20mr0sgwn1",        "http://ww2.sinaimg.cn/large/6b3b9965gw1f2ovawm7h5j20280283ya"]
        ///</Summary>
        public virtual string PictureUrls { get; set; }
        ///<Summary>
        /// 图片的文件列表，如["20160408/00273/3961867459907808#6b3b9965gw1f2ovawd7nej20mr0sgwn1","20160408/00273/3961867459907808#6b3b9965gw1f2ovawm7h5j20280283ya"]
        ///</Summary>
        public virtual string PictureFiles { get; set; }
        ///<Summary>
        /// 图片OCR识别内容，如[“pc”,”pc” ]。
        ///</Summary>
        public virtual string PicOcrContents { get; set; }
        ///<Summary>
        /// 视频数
        ///</Summary>
        public virtual int VideoCount { get; set; }
        ///<Summary>
        /// 视频的URL列表，如[“URL1”,”URL2” ,“URL3”]。
        ///</Summary>
        public virtual string VideoUrls { get; set; }
        ///<Summary>
        /// 视频的文件列表，如["20160408/00273/3961867459907808#6b3b9965gw1f2ovawd7nej20mr0sgwn1","20160408/00273/3961867459907808#6b3b9965gw1f2ovawm7h5j20280283ya"]
        ///</Summary>
        public virtual string VideoFiles { get; set; }
        ///<Summary>
        /// 音频数
        ///</Summary>
        public virtual string AudioCount { get; set; }
        ///<Summary>
        /// 音频的URL列表，如[“URL1”,”URL2” ,“URL3”]。
        ///</Summary>
        public virtual string AudioUrls { get; set; }
        ///<Summary>
        /// 音频的文件列表，如["20160408/00273/3961867459907808#6b3b9965gw1f2ovawd7nej20mr0sgwn1","20160408/00273/3961867459907808#6b3b9965gw1f2ovawm7h5j20280283ya"]
        ///</Summary>
        public virtual string AudioFiles { get; set; }
        ///<Summary>
        /// 其他类型数
        ///</Summary>
        public virtual int OtherCount { get; set; }
        ///<Summary>
        /// 其他类型URL列表，如[“URL1”,”URL2” ,“URL3”]。
        ///</Summary>
        public virtual string OtherUrls { get; set; }
        ///<Summary>
        /// 其他类型文件列表，如["20170227/00273/3961867459907808#6b3b9965gw1f2ovawd7nej20mr0sgwn1","20170227/00273/3961867459907808#6b3b9965gw1f2ovawm7h5j20280283ya"]
        ///</Summary>
        public virtual string OtherFiles { get; set; }
        ///<Summary>
        /// 文档中的关键词列表，如[“Key1”,”Key2” ,“Key3”]
        ///</Summary>
        public virtual string Keywords { get; set; }
        ///<Summary>
        /// 关键词（频率）向量，如[{“v”:”习近平”, “w”:7}, {“v”:”访美”, “w”:”1”} ]。
        ///</Summary>
        public virtual string KeywordVector { get; set; }
        ///<Summary>
        /// 根据pt拆分年，格式YYYY
        ///</Summary>
        public virtual int Year { get; set; }
        ///<Summary>
        /// 根据pt拆分月，取值1~12
        ///</Summary>
        public virtual int Month { get; set; }
        ///<Summary>
        /// 根据pt拆分日，取值1~31
        ///</Summary>
        public virtual int Day { get; set; }
        ///<Summary>
        /// 根据pt拆分小时，取值0~23
        ///</Summary>
        public virtual int Hour { get; set; }
        ///<Summary>
        /// 快照时间
        ///</Summary>
        public virtual DateTime SnapshotTime { get; set; }
        ///<Summary>
        /// 快照URL
        ///</Summary>
        public virtual string SnapshotUrl { get; set; }
        ///<Summary>
        /// 快照文件
        ///</Summary>
        public virtual string SnapshotFile { get; set; }
        ///<Summary>
        /// 用户名称
        ///</Summary>
        public virtual string Username { get; set; }

    }
}
