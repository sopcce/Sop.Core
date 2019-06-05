namespace Sop.FileServer.Base
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExecutable
    {
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        void Execute();
    }
}