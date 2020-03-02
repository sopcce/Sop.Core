using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Sop.Services.Auth.Model
{
    public class Role : IRole<string>
    {
        public Role()
        {
            Data = "asdasdasdasd";
        }

        public Role(string name)
        {
            this.Name = name;
        }

        public Role(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<User> Users { get; protected set; }
        public virtual string Data { get; set; }
    }


   
}
