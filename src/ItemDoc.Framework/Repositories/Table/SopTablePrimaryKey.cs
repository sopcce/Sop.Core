using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Repositories
{
  public class SopTablePrimaryKey : PrimaryKeyAttribute
  {
    public SopTablePrimaryKey(string value) :
      base(value)
    {
      base.AutoIncrement = AutoIncrement;
      base.SequenceName = SequenceName;
    }

    //public string SequenceName { get; set; }
    //public bool AutoIncrement { get; set; }
  }

}
