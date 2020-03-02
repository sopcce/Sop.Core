using Sop.Framework.Repositories;
using Sop.Services.Model;
using System.Collections.Generic;
using System.Linq;
using Sop.Framework.Utility;
using System;

namespace Sop.Services.Servers
{
    public class CatalogService
    {
        public IRepository<CatalogInfo> _CatalogRepository { get; set; }


        /// <summary>
        /// 获取所有栏目
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CatalogInfo> GetAll()
        {
            return _CatalogRepository.Table;
        }


        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatalogInfo Get(int id)
        {
            return _CatalogRepository.Get(id);
        }





        /// <summary>
        /// 获取顶级栏目
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CatalogInfo> GetRootCatalogs()
        {
            return GetAllByDisplayOrder().Where(x => x.ParentId == 0);
        }
        /// <summary>
        /// 获取所有栏目，排序
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CatalogInfo> GetAllByDisplayOrder()
        {

            return GetAll().OrderByDescending(n => n.DisplayOrder);
        }
        /// <summary>
        /// 把栏目组织成缩进格式
        /// </summary>
        private void OrganizeForIndented(CatalogInfo parentCategory, List<CatalogInfo> organizedCategorys)
        {
            if (parentCategory.ChildCount > 0)
            {
                foreach (CatalogInfo child in GetChildren(parentCategory.Id))
                {
                    organizedCategorys.Add(child);
                    OrganizeForIndented(child, organizedCategorys);
                }
            }
        }
        /// <summary>
        /// 获取子栏目
        /// </summary>
        /// <param name="parentId">parentId</param>
        /// <returns></returns>
        public IEnumerable<CatalogInfo> GetChildren(int parentId)
        {
            return GetAllByDisplayOrder().Where(x => x.ParentId == parentId);
        }
        /// <summary>
        /// 获取子栏目(不包括父栏目自己)
        /// </summary>
        /// <param name="categoryId">栏目ID</param>
        /// <returns></returns>
        public IEnumerable<CatalogInfo> GetAllDescendants(int categoryId)
        {
            var contentcategory = Get(categoryId);
            if (contentcategory == null || contentcategory.ChildCount == 0)
                return null;
            var query = GetAll().Where(a => a.ParentIdList.Contains("," + categoryId.ToString()));
            return query;
        }
        /// <summary>
        /// 获取子栏目
        /// </summary>
        /// <param name="CategoryId">栏目ID</param>
        /// <returns></returns>
        public Dictionary<int, string> GetChildrenDictionary(int CategoryId = 0)
        {
            CatalogInfo info = _CatalogRepository.Get(CategoryId);
            if (info == null)
                return null;
            var folders = GetChildren(info.Id);
            return folders.Where(n => n.Enabled).ToDictionary(n => n.Id, n => StringUtility.Trim(n.Name, 7));
        }

        /// <summary>
        /// 获取一级栏目
        /// </summary>
        /// <param name="exceptCategoryId">要去除的栏目ID</param>
        /// <returns></returns>
        public Dictionary<int, string> GetRootFolderDictionary(int exceptCategoryId = -1)
        {
            var folders = this.GetRootCatalogs();
            return folders?.Where(n => n.Id != exceptCategoryId).ToDictionary(n => n.Id, n => StringUtility.Trim(n.Name, 7));
        }



        /// <summary>
        /// 把fromCategoryId合并到toCategoryId
        /// </summary>
        /// <remarks>
        /// 例如：将栏目fromCategoryId合并到栏目toCategoryId，那么fromCategoryId栏目下的所有子栏目和ContentItem全部归到toCategoryId栏目，同时删除fromCategoryId栏目
        /// </remarks>
        /// <param name="fromCategoryId">被合并的栏目ID</param>
        /// <param name="toCategoryId">合并到的栏目ID</param>
        public void Merge(int fromCategoryId, int toCategoryId)
        {
            var toCategory = Get(toCategoryId);
            if (toCategory == null)
                return;

            var fromCategory = Get(fromCategoryId);
            if (fromCategory == null)
                return;

            //foreach (var childSection in fromCategory.Children)
            //{
            //    childSection.ParentId = toCategoryId;
            //    childSection.Depth = toCategory.Depth + 1;

            //    if (childSection.Depth == 1)
            //        childSection.ParentIdList = childSection.ParentId.ToString();
            //    else
            //        childSection.ParentIdList = toCategory.ParentIdList + "," + childSection.ParentId;

            //    RecursiveUpdateDepthAndParentIdList(childSection);

            //    ContentItemQuery contentItemQuery = new ContentItemQuery()
            //    {
            //        CategoryId = childSection.CategoryId
            //    };

            //    PagingDataSet<ContentItem> contentItems = contentitemrepository.GetContentItems(contentItemQuery, int.MaxValue, 1);

            //    foreach (var contentItem in contentItems)
            //    {
            //        contentItem.ContentCategoryId = toCategoryId;
            //        contentitemrepository.Update(contentItem);
            //    }
            //}

            //ContentItemQuery currentContentItemQuery = new ContentItemQuery()
            //{
            //    CategoryId = fromCategoryId
            //};
            //PagingDataSet<ContentItem> currentContentItems = contentitemrepository.GetContentItems(currentContentItemQuery, int.MaxValue, 1);

            //foreach (var item in currentContentItems)
            //{
            //    item.ContentCategoryId = toCategoryId;
            //    _CatalogRepository.Update(item);
            //}

            //if (fromCategory.ParentId > 0)
            //{
            //    var fromParentCategory = Get(fromCategory.ParentId);
            //    if (fromParentCategory != null)
            //        fromParentCategory.ChildCount -= 1;
            //}

            //toCategory.ChildCount += fromCategory.ChildCount;
            _CatalogRepository.Update(toCategory);
            _CatalogRepository.Delete(fromCategory);
        }

        public IEnumerable<CatalogInfo> GetByItemId(int itemId)
        {
            return _CatalogRepository.Fetch(n => n.ItemId == itemId, order => order.Asc(n => n.DisplayOrder));
        }

        /// <summary>
        /// 把fromCategoryId移动到toCategoryId，作为toCategoryId的子栏目
        /// </summary>
        /// <remarks>
        /// 例如：将栏目fromCategoryId合并到栏目toCategoryId，那么fromCategoryId栏目下的所有子栏目和ContentItem全部归到toCategoryId栏目，同时删除fromCategoryId栏目
        /// </remarks>
        /// <param name="fromCategoryId">被移动的栏目ID</param>
        /// <param name="toCategoryId">移动到的栏目ID</param>
        public void Move(int fromCategoryId, int toCategoryId)
        {
            #region MyRegion
            //ContentCategory fromCategory = Get(fromCategoryId);
            //if (fromCategory == null)
            //    return;

            //if (fromCategory.ParentId > 0)
            //{
            //    ContentCategory fromParentCategory = Get(fromCategory.ParentId);
            //    if (fromParentCategory != null)
            //    {
            //        fromParentCategory.ChildCount -= 1;
            //        contentCategoryrepository.Update(fromParentCategory);
            //    }
            //}

            //if (toCategoryId > 0)
            //{
            //    ContentCategory toCategory = Get(toCategoryId);
            //    if (toCategory == null)
            //        return;

            //    toCategory.ChildCount += 1;
            //    contentCategoryrepository.Update(toCategory);

            //    fromCategory.ParentId = toCategoryId;
            //    fromCategory.Depth = toCategory.Depth + 1;
            //    if (fromCategory.Depth == 1)
            //        fromCategory.ParentIdList = fromCategory.ParentId.ToString();
            //    else
            //        fromCategory.ParentIdList = toCategory.ParentIdList + "," + fromCategory.ParentId;
            //}
            //else //移动到顶层
            //{
            //    fromCategory.Depth = 0;
            //    fromCategory.ParentIdList = string.Empty;
            //    fromCategory.ParentId = 0;
            //}
            //contentCategoryrepository.Update(fromCategory);

            //if (fromCategory.Children != null)
            //{
            //    foreach (var childCategory in fromCategory.Children)
            //    {
            //        childCategory.Depth = fromCategory.Depth + 1;
            //        childCategory.ParentIdList = fromCategory.ParentIdList + "," + fromCategory.CategoryId;
            //        RecursiveUpdateDepthAndParentIdList(childCategory);
            //    }
            //} 
            #endregion
        }


        #region Create && Update && Delete
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="info"></param>
        public void Create(CatalogInfo info)
        {
            if (info.ParentId > 0)
            {
                CatalogInfo parentInfo = Get(info.ParentId);
                if (parentInfo != null)
                {
                    info.Depth = parentInfo.Depth + 1;
                    info.ParentIdList = parentInfo.ParentIdList + "," + info.ParentId;
                    parentInfo.ChildCount += 1;
                    _CatalogRepository.Update(parentInfo);
                }
                else
                {
                    info.ParentId = 0;
                }

            }
            _CatalogRepository.Create(info);
            //todo 不知道会不会出现问题 ，但是当前能够使用

            info.DisplayOrder = info.Id;
            _CatalogRepository.Update(info);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void Update(CatalogInfo info)
        {
            _CatalogRepository.Update(info);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="contentFolder"></param>
        public void Delete(CatalogInfo info)
        {
            if (info == null)
                return;
            //更新父栏目的ChildCount
            if (info.ParentId > 0)
            {
                CatalogInfo parentInfo = Get(info.ParentId);
                if (parentInfo != null)
                {
                    parentInfo.ChildCount = parentInfo.ChildCount == 0
                      ? 0
                      : parentInfo.ChildCount -= 1;
                    _CatalogRepository.Update(parentInfo);
                }
            }
            ////所有后代栏目 删除
            IEnumerable<CatalogInfo> CategoryIdList = GetAllDescendants(info.Id);
            if (CategoryIdList != null)
            {
                foreach (var item in CategoryIdList)
                    _CatalogRepository.Delete(item);
            }
            _CatalogRepository.Delete(info);
        }
        #endregion

    }
}
