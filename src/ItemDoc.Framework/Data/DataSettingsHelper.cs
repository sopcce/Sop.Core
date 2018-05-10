using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Data
{
  public partial class DataSettingsHelper
  {
    ///private static bool? _databaseIsInstalled;

    /// <summary>
    /// Returns a value indicating whether database is already installed
    /// </summary>
    /// <returns></returns>
    public static bool DatabaseIsInstalled()
    {
      //if (!_databaseIsInstalled.HasValue)
      //{
      //  var manager = new DataSettingsManager();
      //  var settings = manager.LoadSettings();
      //  _databaseIsInstalled = settings != null && !String.IsNullOrEmpty(settings.DataConnectionString);
      //}
      //return _databaseIsInstalled.Value;
      return true;
    }

  
  }
}
