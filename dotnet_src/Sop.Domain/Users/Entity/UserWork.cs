using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class UserWork : IEntity
    {
        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// </summary>
        public string CompanyAreaCode { get; set; }

        /// <summary>
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// </summary>
        public string JobDescription { get; set; }

        /// <summary>
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// </summary>
        public string JobTitle { get; set; }
    }
}