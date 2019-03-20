using Sop.Framework.Repositories;
using Sop.Services.Model;
using System.Collections.Generic;
using Sop.Services.Parameter;
using Sop.Services.ViewModel;
using System;
using Sop.Framework.Environment;
using Sop.Framework.Repositories.NHibernate;

namespace Sop.Services.Servers
{
    public class PostService
    {
        public IRepository<PostInfo> _postRepository { get; set; }

        /// <summary>
        /// 获取所有栏目
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PostInfo> GetAll()
        {
            //todo:这里栏目使用比较多，但是数据量总体不超过5000条 ，可以考虑加入缓存
            return _postRepository.Table;
        }
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PostInfo Get(object id)
        {
            return _postRepository.Get(id);
        }




        #region Create&&Update&&Delete
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="info"></param>
        public void Create(PostInfo info)
        {
            //info.DisplayOrder = IdSnowflake.Instance().GetId(); 
            var result = _postRepository.Create(info);


            info.DisplayOrder = (int)result;

            _postRepository.Update(info);


        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Update(PostInfo info)
        {
            _postRepository.Update(info);
            return info.Id;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="info"></param>
        public void Delete(PostInfo info)
        {
            if (info == null)
                return;
            _postRepository.Delete(info);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        //public IPageList<PostViewModel> GetPostList(PostParameter parameter)
        //{
        //  //int cataLog, int pageSize, int pageIndex, string keyword, string sortOrder, string sortName 
        //  //TODO 特殊处理page 数
        //  parameter.pageIndex = (parameter.pageIndex / parameter.pageSize) + 1;

        // // return _postRepository.Gets(paramete.pageIndex, parameter.pageSize,);
        //}
        #endregion

        public PageList<PostInfo> GetPostList(PostParameter parameter)
        {
            //  //int cataLog, int pageSize, int pageIndex, string keyword, string sortOrder, string sortName 
            //  //TODO 特殊处理page 数
            parameter.pageIndex = (parameter.pageIndex / parameter.pageSize) + 1;

            var query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Desc(n => n.DateCreated), parameter.pageSize, parameter.pageIndex);
            if (parameter.sortOrder == SortOrder.Asc)
            {
                switch (parameter.sortName)
                {
                    case SortName.DateCreated:
                        query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Asc(n => n.DateCreated), parameter.pageSize, parameter.pageIndex);
                        break;
                    case SortName.Title:
                        query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Asc(n => n.Title), parameter.pageSize, parameter.pageIndex);
                        break;
                    case SortName.ViewCount:
                        query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Asc(n => n.ViewCount), parameter.pageSize, parameter.pageIndex);
                        break;
                    case SortName.DisplayOrder:
                        query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Asc(n => n.DisplayOrder), parameter.pageSize, parameter.pageIndex);
                        break;
                    default:
                        break;
                }
            }
            if (parameter.sortOrder == SortOrder.Desc)
            {
                switch (parameter.sortName)
                {
                    case SortName.DateCreated:
                        query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Desc(n => n.DateCreated), parameter.pageSize, parameter.pageIndex);
                        break;
                    case SortName.Title:
                        query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Desc(n => n.Title), parameter.pageSize, parameter.pageIndex);
                        break;
                    case SortName.ViewCount:
                        query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Desc(n => n.ViewCount), parameter.pageSize, parameter.pageIndex);
                        break;
                    case SortName.DisplayOrder:
                        query = _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, order => order.Desc(n => n.DisplayOrder), parameter.pageSize, parameter.pageIndex);
                        break;
                    default:
                        break;
                }
            }

            
            return query;

        }
    }
}
