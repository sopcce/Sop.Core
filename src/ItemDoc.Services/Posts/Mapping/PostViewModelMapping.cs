using System;
using AutoMapper;
using AutoMapper.Configuration;
using ItemDoc.Services.Model;
using ItemDoc.Services.ViewModel;

namespace ItemDoc.Services.Mapping
{
  /// <summary>
  /// 
  /// </summary>
  public static class PostViewModelMapping
  {

    /// <summary>
    /// 转换为数据模型
    /// </summary>
    /// <returns></returns>
    public static PostInfo AsInfo(this PostViewModel source)
    {
      //创建映射配置
      Mapper.Initialize(cfg => cfg.CreateMap<PostViewModel, PostInfo>());
      return Mapper.Map<PostInfo>(source);
    }

    /// <summary>
    /// 转换指定字段为数据模型
    /// </summary>
    /// <param name="model"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public static PostInfo AsModeltoInfo(this PostViewModel model, PostInfo info)
    {
      //创建映射配置 
      Mapper.Initialize(cfg => cfg.CreateMap<PostInfo, PostViewModel>());
      return Mapper.Map<PostViewModel, PostInfo>(model, info);
    }

    /// <summary>
    /// 转换为视图编辑模型
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static PostViewModel AsModel(this PostInfo source)
    {

      if (source == null)
        return null;
      Mapper.Initialize(cfg => cfg.CreateMap<PostInfo, PostViewModel>());
      return Mapper.Map<PostViewModel>(source);
    }

    public static T AsModel<T>(this PostInfo entity)
    {
      Type typeParameterType = typeof(T);

      if (typeParameterType == typeof(PostViewModel))
      {

        Mapper.Initialize(cfg => cfg.CreateMap<PostInfo, PostViewModel>());
        return Mapper.Map<T>(entity);
      }

      return default(T);
    }
  }
}
