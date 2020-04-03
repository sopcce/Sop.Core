using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Sop.Data.Mapping;

namespace Sop.Data.Repository
{
    /// <summary>
    ///     default DbContext
    /// </summary>
    public class BaseDbContext : DbContext
    {
        private OnModelCreatingType _onModelCreatingType = OnModelCreatingType.UseEntity;

        protected BaseDbContext()
        {
        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected void SetOnModelCreatingType(OnModelCreatingType onModelCreatingType = OnModelCreatingType.UseEntity)
        {
            _onModelCreatingType = onModelCreatingType;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = GetCurrentPathAssembly();
            switch (_onModelCreatingType)
            {
                case OnModelCreatingType.UseEntity:
                    foreach (var assembly in assemblies)
                    {
                        var entityTypes = assembly.GetTypes()
                                                  .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                                                  .Where(type => type.IsClass)
                                                  .Where(type => type.BaseType != null)
                                                  .Where(type => typeof(IEntity).IsAssignableFrom(type));

                        foreach (var entityType in entityTypes)
                        {
                            if (modelBuilder.Model.FindEntityType(entityType) != null)
                                continue;
                            modelBuilder.Model.AddEntityType(entityType);
                        }
                    }

                    break;
                case OnModelCreatingType.UseEntityMap:
                    foreach (var assembly in assemblies)
                    {
                        //dynamically load all entity and query type configurations
                        var typeConfigurations = assembly.GetTypes().Where(type =>
                                                                               (type.BaseType?.IsGenericType ?? false)
                                                                               && (type.BaseType
                                                                                       .GetGenericTypeDefinition() ==
                                                                                   typeof(BaseMapEntityTypeConfiguration
                                                                                       <>)
                                                                                   || type
                                                                                     .BaseType
                                                                                     .GetGenericTypeDefinition() ==
                                                                                   typeof(BaseMapQueryTypeConfiguration<
                                                                                   >)));

                        foreach (var typeConfiguration in typeConfigurations)
                        {
                            var configuration = (IMappingConfiguration) Activator.CreateInstance(typeConfiguration);
                            configuration.ApplyConfiguration(modelBuilder);
                        }
                    }

                    break;
            }

            base.OnModelCreating(modelBuilder);
        }


        private List<Assembly> GetCurrentPathAssembly()
        {
            var list = new List<Assembly>();
            switch (_onModelCreatingType)
            {
                case OnModelCreatingType.UseEntity:
                {
                    var dulls = DependencyContext.Default.CompileLibraries
                                                 .Where(x => !x.Name.StartsWith("Microsoft") &&
                                                             !x.Name.StartsWith("System"))
                                                 .ToList();
                    if (dulls.Any())
                        foreach (var dll in dulls)
                            if (dll.Type == "project")
                                list.Add(Assembly.Load(dll.Name));
                    list.Add(Assembly.GetExecutingAssembly());
                }
                    break;
                case OnModelCreatingType.UseEntityMap:
                {
                    var dulls = DependencyContext.Default.CompileLibraries
                                                 .ToList();
                    if (dulls.Any())
                        foreach (var dll in dulls)
                            if (dll.Type == "project")
                                list.Add(Assembly.Load(dll.Name));
                    list.Add(Assembly.GetExecutingAssembly());
                }
                    break;
            }

            return list;
        }
    }

    public enum OnModelCreatingType
    {
        UseEntity,
        UseEntityMap
    }
}