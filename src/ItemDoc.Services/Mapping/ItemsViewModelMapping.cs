using AutoMapper;
using AutoMapper.Configuration;
using ItemDoc.Services.Model;
using ItemDoc.Services.ViewModel;

namespace ItemDoc.Services.Mapping
{
  /// <summary>
  /// 
  /// </summary>
  public static class ItemsViewModelMapping
  {

    /// <summary>
    /// 转换为数据模型
    /// </summary>
    /// <returns></returns>
    public static ItemsInfo AsInfo(this ItemsViewModel source)
    {
      return Mapper.Map<ItemsInfo>(source);
    }

    /// <summary>
    /// 转换指定字段为数据模型
    /// </summary>
    /// <param name="model"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public static ItemsInfo AsModeltoInfo(this ItemsViewModel model, ItemsInfo info)
    {
      return Mapper.Map<ItemsViewModel, ItemsInfo>(model, info);
    }

    /// <summary>
    /// 转换为视图编辑模型
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static ItemsViewModel AsModel(this ItemsInfo source)
    {
      return Mapper.Map<ItemsViewModel>(source);
    }
  }
}
