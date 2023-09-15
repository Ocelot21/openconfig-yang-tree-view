using Mapster;
using openconfig_yang_tree_view.MVVM.Model;
using openconfig_yang_tree_view.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.Mappings
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Grouping, GroupingViewModel>.NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Containers, src => ConvertToObservableCollection(src.Containers))
                .Map(dest => dest.Leafs, src => ConvertToObservableCollection(src.Leafs));

            //TypeAdapterConfig<List<Container>, ObservableCollection<ContainerViewModel>>.NewConfig()
            //    .Map(dest => dest, src => src.Adapt<ObservableCollection<ContainerViewModel>>());

            //TypeAdapterConfig<List<Leaf>, ObservableCollection<LeafViewModel>>.NewConfig()
            //    .Map(dest => dest, src => src.Adapt<ObservableCollection<LeafViewModel>>());

            //TypeAdapterConfig<List<YangList>, ObservableCollection<ListViewModel>>.NewConfig()
            //    .Map(dest => dest, src => src.Adapt<ObservableCollection<ListViewModel>>());

            //TypeAdapterConfig<Container, ContainerViewModel>.NewConfig()
            //    .Map(dest => dest.Name, src => src.Name)
            //    .Map(dest => dest.Description, src => src.Description)
            //    .Map(dest => dest.Containers, src => src.Adapt<ObservableCollection<ContainerViewModel>>())
            //    .Map(dest => dest.Lists, src => src.Adapt<ObservableCollection<ListViewModel>>());
            //TypeAdapterConfig<Leaf, LeafViewModel>.NewConfig()
            //    .Map(dest => dest.Name, src => src.Name)
            //    .Map(dest => dest.Description, src => src.Description)
            //    .Map(dest => dest.Config, src => src.Config)
            //    .Map(dest => dest.Type, src => src.Type);
            //TypeAdapterConfig<YangList, ListViewModel>.NewConfig()
            //    .Map(dest => dest.Name, src => src.Name)
            //    .Map(dest => dest.Description, src => src.Description)
            //    .Map(dest => dest.Key, src => src.Key)
            //    .Map(dest => dest.Containers, src => src.Adapt<ObservableCollection<ContainerViewModel>>())
            //    .Map(dest => dest.Leafs, src => src.Adapt<ObservableCollection<LeafViewModel>>())
            //    .Map(dest => dest.Lists, src => src.Adapt<ObservableCollection<ListViewModel>>());
            // Add additional mappings for nested properties as needed
        }

        private static ObservableCollection<TDestination> ConvertToObservableCollection<TDestination>(IEnumerable<TDestination> sourceList)
        {
            return new ObservableCollection<TDestination>(sourceList);
        }
    }
}
