using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sop.Services.Model;

namespace Sop.Services.ViewModel
{

  public class ItemsViewModel
  {

    public int Id { get; set; }
  
    public string Name { get; set; }
    public string ParentName { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }

    public string PassWord { get; set; }

    public int ParentId { get; set; }

    public bool Enabled { get; set; } = true;

    public int DisplayOrder { get; set; }

  }
}
