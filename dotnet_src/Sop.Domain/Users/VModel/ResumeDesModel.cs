using System.Collections.Generic;
using Sop.Domain.Entity;

namespace Sop.Domain.VModel
{
    public class ResumeDesModel
    {
        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public int UserId { get; set; }

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

        /// <summary>
        /// </summary>
        public List<UserWork> UserWorks { get; set; }
    }
}