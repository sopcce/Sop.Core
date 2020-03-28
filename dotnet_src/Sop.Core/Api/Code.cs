namespace Sop.Core.Api
{
    /// <summary>
    /// 
    /// </summary>
    public enum Code
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        ok = 200,
        /// <summary>
        /// 创建成功
        /// </summary>
        created = 201,
        /// <summary>
        /// 删除成功
        /// </summary>
        deleted = 204,
        /// <summary>
        /// 请求的地址不存在或者包含不支持的参数
        /// </summary>
        badrequest = 400,
        /// <summary>
        /// 未授权
        /// </summary>
        un_authorized = 401,
        /// <summary>
        /// 被禁止访问
        /// </summary>
        forbidden = 403,
        /// <summary>
        /// 请求的资源不存在
        /// </summary>
        not_found = 404,
        /// <summary>
        /// [POST/PUT/PATCH] 当创建一个对象时，发生一个验证错误
        /// </summary>
        unprocesable_entity= 422,
        /// <summary>
        /// 内部错误
        /// </summary>
        internal_server_error = 500,  
    }
}
