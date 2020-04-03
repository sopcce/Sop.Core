using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    /// <summary>
    ///     sop_user_external_login
    /// </summary>
    [Serializable]
    public class UserExternalLogin : IEntity
    {
        /// <Summary>
        ///     Id
        /// </Summary>
        public int Id { get; set; }

        /// <Summary>
        ///     UserId
        /// </Summary>
        public long UserId { get; set; }

        /// <Summary>
        ///     LoginProvider
        /// </Summary>
        public string LoginProvider { get; set; }

        /// <Summary>
        ///     ProviderKey
        /// </Summary>
        public string ProviderKey { get; set; }

        /// <Summary>
        ///     DateCreated
        /// </Summary>
        public DateTime DateCreated { get; set; }
    }
}