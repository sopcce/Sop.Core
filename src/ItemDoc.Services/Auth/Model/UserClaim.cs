using System;
using System.Security.Claims;
using ItemDoc.Services.Auth.Model;
using Newtonsoft.Json;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ItemDoc.Services.Auth.Identity
{
  public class UserClaim
    {
        public UserClaim(Claim claim)
        {
            if (claim == null) throw new ArgumentNullException("claim");

            Type = claim.Type;
            Value = claim.Value;
        }

        [JsonConstructor]
        public UserClaim(string type, string value)
        {
            if (type == null) throw new ArgumentNullException("claimType");
            if (value == null) throw new ArgumentNullException("claimValue");

            Type = type;
            Value = value;
        }
        public virtual string Id { get; private set; }
        public virtual string Type { get; private set; }
        public virtual string Value { get; private set; }
        public virtual User Users { get; set; }
    }
  
}
