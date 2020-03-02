using Sop.Framework.Repositories;
using Sop.Services.Model;
using System.Collections.Generic;

namespace Sop.Services.Servers
{
    /// <summary>
    /// sop_forum_article  
    /// </summary> 
    public class ForumArticleService
    {
        private readonly IRepository<ForumArticleInfo> _repository;
        public ForumArticleService(IRepository<ForumArticleInfo> repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForumArticleInfo> GetAll()
        {
            //todo:这里栏目使用比较多，但是数据量总体不超过5000条 ，可以考虑加入缓存
            return _repository.Table;
        }
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ForumArticleInfo Get(object id)
        {
            return _repository.Get(id);
        }


        #region Create&&Update&&Delete
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="info"></param>
        public int Create(ForumArticleInfo info)
        {
            _repository.Create(info);
            return  0;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Update(ForumArticleInfo info)
        {
            _repository.Update(info);
            return 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="info"></param>
        public void Delete(ForumArticleInfo info)
        {
            if (info == null)
                return;
            _repository.Delete(info);
        }
        #endregion







    }
}
