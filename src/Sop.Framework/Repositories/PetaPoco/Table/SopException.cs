using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Repositories
{
  class SopException
  {
    public static bool OnException(Exception x)
    {
      System.Diagnostics.Debug.WriteLine(x.ToString());
      System.Diagnostics.Debug.WriteLine(new Database().LastCommand);
      return true;
    }
  }
}
