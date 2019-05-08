namespace Sop.Core.Mvc.SystemMessage
{
    /// <summary>
    /// 系统消息提示
    /// </summary>
    public sealed class SystemMessageData
    {
        /// <summary>
        /// 提示消息类别
        /// </summary>
        public SystemMessageType Type { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="messageType">消息类型</param>
        public SystemMessageData(SystemMessageType messageType)
        {
            this.Type = messageType;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="messageContent">消息内容</param>
        public SystemMessageData(SystemMessageType messageType, string messageContent)
        {
            this.Type = messageType;
            this.Content = messageContent;
        }

        public SystemMessageData()
        {
        }



    }
}
