using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    [Serializable]
    public class UserProject : IEntity
    {
        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// </summary>
        public string Performance { get; set; }

        /// <summary>
        /// </summary>
        public string ProjectDesc { get; set; }

        /// <summary>
        /// </summary>
        public string Pictures { get; set; }
    }
}