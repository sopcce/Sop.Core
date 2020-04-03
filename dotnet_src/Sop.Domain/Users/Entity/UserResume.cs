using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class UserResume : IEntity
    {
        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// </summary>
        public string WorkName { get; set; }

        /// <summary>
        /// </summary>
        public string WorkIcon { get; set; }

        /// <summary>
        /// </summary>
        public string EduName { get; set; }

        /// <summary>
        /// </summary>
        public string EduIcon { get; set; }
    }
}