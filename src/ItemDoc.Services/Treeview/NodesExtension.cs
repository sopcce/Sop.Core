using System.Collections.Generic;
using System.Linq;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Treeview
{
  public static class NodesExtension
  {
     

    public static IEnumerable<Node> ToNodes(this IEnumerable<CatalogInfo> listInfos)
    {
      var infos = listInfos as CatalogInfo[] ?? listInfos.ToArray();

      var rootCategorys = infos.Where(x => x.ParentId == 0);

      List<Node> nodes = new List<Node>();
      foreach (var info in rootCategorys)
      {
        Node node = new Node();
        node.Id = info.Id.ToString();
        node.Text = info.Name;
        node.Tags = new string[] { info.ChildCount.ToString() };
        OrganizeForIndented(node.Nodes, node, info, infos);
        nodes.Add(node);

      }
      return nodes;


    }
    private static void OrganizeForIndented(List<Node> nodes, Node node, CatalogInfo info, IEnumerable<CatalogInfo> lists)
    {
      if (info.ChildCount > 0)
      {
        nodes = new List<Node>();
        var newlists = lists.Where(x => x.ParentId == info.Id);
        foreach (var info1 in newlists)
        {
          Node node1 = new Node
          {
            Id = info1.Id.ToString(),
            Text = info1.Name,
            Tags = new string[] { info1.ChildCount.ToString()  }
          };
          OrganizeForIndented(node1.Nodes, node1, info1, lists);
          nodes.Add(node1);
        }
        node.Nodes = nodes;
      }
    }


   













  }


}
