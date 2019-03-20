using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using Sop.Core.API;

namespace Sop.Core.Mvc.SystemMessage
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemMessage
    {
        public const string TempDataKey = "_SopSystemMessage";


        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemMessage()
        {
            BodyLink = new Dictionary<string, string>();
            ButtonLink = new Dictionary<string, string>();
            SystemMessageType = SystemMessageType.Error;
        }

        /// <summary>
        /// 消息提示页面的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 主要提示信息（如果需要添加链接在提示信息上添加占位符如：{0}）
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 返回上一页中的连接
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 主要提示信息上的连接（根据字典中的位置，在提示信息上生成连接，并且替换掉占位符）
        /// </summary>
        public Dictionary<string, string> BodyLink { get; set; }

        /// <summary>
        /// 按钮的连接（会根据字典中的值，生成按钮并且展示在页面上）
        /// </summary>
        public Dictionary<string, string> ButtonLink { get; set; }

        /// <summary>
        /// 消息发送状态（根据状态的不同改变图标）
        /// </summary>
        public SystemMessageType SystemMessageType { get; set; }


        /// <summary>
        /// 返回逻辑错误
        /// </summary>
        /// <param name="result"></param>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static HttpResponseMessage Result(object result, HttpStatusCode code = HttpStatusCode.OK, string desc = "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var content = new DataPackage(code, result, desc);

            var response = new HttpResponseMessage { Content = new StringContent(serializer.Serialize(content), Encoding.UTF8, "application/json") };

            return response;
        }
    }
}
