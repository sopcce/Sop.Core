using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    public class UserSkill : IEntity
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
        public int Percent { get; set; }

        /// <summary>
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// </summary>
        public int Type { get; set; }
    }
}