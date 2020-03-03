using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class UserFeatures : IEntity
    {
        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}