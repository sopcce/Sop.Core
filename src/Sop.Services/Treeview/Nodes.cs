using System;
using System.Collections.Generic;

namespace Sop.Services.Treeview
{
  [Serializable]
  public class Node
  {
  
    public string Id { get; set; }
    /// <summary>
    /// 列表树节点上的文本，通常是节点右边的小图标。
    /// </summary> 

    public string Text { get; set; }
    /// <summary>
    /// 列表树节点上的图标，通常是节点左边的图标。
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// 当某个节点被选择后显示的图标，通常是节点左边的图标。
    /// </summary> 
    public string SelectedIcon { get; set; }
    /// <summary>
    /// 结合全局enableLinks选项为列表树节点指定URL。
    /// </summary>
   
    public string Href { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Nodes"/> is selectable.
    /// </summary>
    /// <value>
    ///   <c>true</c> if selectable; otherwise, <c>false</c>.
    /// </value>
  
    public bool Selectable { get; set; } = true;
    /// <summary>
    /// 
    /// </summary>

    public State State { get; set; }
    /// <summary>
    /// 节点的前景色，覆盖全局的前景色选项。
    /// </summary>
   
    public string Color { get; set; }
    /// <summary>
    /// 节点的背景色，覆盖全局的背景色选项。
    /// </summary>
  
    public string BackColor { get; set; }
    /// <summary>
    /// 通过结合全局showTags选项来在列表树节点的右边添加额外的信息。
    /// </summary>
   
    public string[] Tags { get; set; }

 
    public List<Node> Nodes { get; set; }


    
  }
  /// <summary>
  /// 一个节点的初始状态。
  /// </summary>
  public class State
  {
    /// <summary>
    ///指示一个节点是否处于checked状态，用一个checkbox图标表示。
    /// </summary>
    /// <value>
    ///   <c>true</c> if checked; otherwise, <c>false</c>.
    /// </value>
    public bool Checked { get; set; } = false;
    /// <summary>
    /// 指示一个节点是否处于disabled状态。（不是selectable，expandable或checkable）
    /// </summary> 

    public bool Disabled { get; set; } = false;
    /// <summary>
    /// 指示一个节点是否处于展开状态 
    /// </summary> 
    public bool Expanded { get; set; } = false;
    /// <summary>
    /// 指示一个节点是否可以被选择。
    /// </summary>

    public bool Selected { get; set; } = false;

  }

}
