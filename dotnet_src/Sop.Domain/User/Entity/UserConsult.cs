using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class UserConsult : IEntity
    {
        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}