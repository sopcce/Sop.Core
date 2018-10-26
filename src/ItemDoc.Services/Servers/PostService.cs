using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;
using System.Collections.Generic;
using ItemDoc.Services.Parameter;
using ItemDoc.Services.ViewModel;
using System;

namespace ItemDoc.Services.Servers
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
        public int Create(PostInfo info)
        {
            var result = _postRepository.Create(info);
            int id = Convert.ToInt32(result);
            info.Id = id;
            info.DisplayOrder = id;
            _postRepository.Update(info);

            return id;
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



            return _postRepository.Gets(n => n.CatalogId == parameter.CatalogId, null, parameter.pageSize, parameter.pageIndex);

        }
    }
}
