using AutoMapper;
using ItemDoc.Services.ViewModel;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Mapping
{
  /// <summary>
  /// 
  /// </summary>
  public static class CatalogViewModelMapping
  {

    /// <summary>
    /// 转换为数据模型
    /// </summary>
    /// <returns></returns>
    public static CatalogInfo AsInfo(this CatalogViewModel source)
    {
      //创建映射配置
      Mapper.Initialize(cfg => cfg.CreateMap<CatalogViewModel, CatalogInfo>());
      return Mapper.Map<CatalogInfo>(source);
    }

    /// <summary>
    /// 转换指定字段为数据模型
    /// </summary>
    /// <param name="model"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public static CatalogInfo AsModeltoInfo(this CatalogViewModel model, CatalogInfo info)
    {
      //创建映射配置 
      Mapper.Initialize(cfg => cfg.CreateMap<CatalogInfo, CatalogViewModel>());
      return Mapper.Map<CatalogViewModel, CatalogInfo>(model, info);
    }

    /// <summary>
    /// 转换为视图编辑模型
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static CatalogViewModel AsModel(this CatalogInfo source)
    {

      if (source == null)
        return null;
      Mapper.Initialize(cfg => cfg.CreateMap<CatalogInfo, CatalogViewModel>());
      return Mapper.Map<CatalogViewModel>(source);
    }
  }
}
