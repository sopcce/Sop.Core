


namespace Sop.Core.Api
{
    public class ApiPageResult<T> : ApiResult<T>
    {
        public IPageList<T> Page { get; set; }
          
    }
}
