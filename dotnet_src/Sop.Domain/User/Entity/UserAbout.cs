using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    /// <summary>
    ///     sop_user_ablout
    /// </summary>
    [Serializable]
    public class UserAbout : IEntity
    {
        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// </summary>
        public string BackgroundImages { get; set; }

        /// <summary>
        /// </summary>
        public string AvatarImages { get; set; }

        /// <summary>
        /// </summary>
        public string ProfessionalTag { get; set; }

        /// <summary>
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// </summary>
        public string Introduce { get; set; }
    }
}