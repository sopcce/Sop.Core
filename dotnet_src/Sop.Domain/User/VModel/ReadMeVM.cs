using System.Collections.Generic;
using Sop.Domain.Entity;

namespace Sop.Domain.VModel
{
    /// <summary>
    /// </summary>
    public class ReadMeVm
    {
        /// <summary>
        /// </summary>
        public ReadMeProfile ReadMeProfile { get; set; }

        /// <summary>
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// </summary>
        public UserAbout UserAbout { get; set; }

        /// <summary>
        /// </summary>
        public List<UserWork> UserWorks { get; set; }

        /// <summary>
        /// </summary>
        public List<UserFeatures> UserFeatures { get; set; }

        /// <summary>
        /// </summary>
        public UserConsultVm UserConsultVm { get; set; }

        /// <summary>
        /// </summary>
        public List<UserSkill> UserSkills { get; set; }

        /// <summary>
        /// </summary>
        public List<UserProject> UserProjects { get; set; }
    }
}