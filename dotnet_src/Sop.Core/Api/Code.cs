namespace Sop.Core.Api
{
    public enum Code
    { 
        /// <summary>
        /// 请求成功
        /// </summary>
        OK = 200,
        /// <summary>
        /// 创建成功
        /// </summary>
        CREATED = 201,
        /// <summary>
        /// 删除成功
        /// </summary>
        DELETED = 204,
        /// <summary>
        /// 请求的地址不存在或者包含不支持的参数   
        /// </summary>
        BAD_REQUEST    = 400,
        /// <summary>
        /// 未授权
        /// </summary>
        UNAUTHORIZED = 401,
        /// <summary>
        /// 被禁止访问
        /// </summary>
        FORBIDDEN = 403,
        /// <summary>
        /// 请求的资源不存在
        /// </summary>
        NOT_FOUND  = 404,
        /// <summary>
        /// [POST/PUT/PATCH] 当创建一个对象时，发生一个验证错误  
        /// </summary>
        Unprocesable_Entity = 422,
        /// <summary>
        /// 内部错误
        /// </summary>
        INTERNAL_SERVER_ERROR = 500,   









    }
}
