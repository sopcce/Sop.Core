using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Services.Mapping
{
  class AutoMapperConfiguration
  {
    public static void Configure()
    {
      Mapper.Initialize(x =>
      {
        x.AddProfile<MappingProfile>();
      });

      Mapper.Configuration.AssertConfigurationIsValid();
    }
  }
}
