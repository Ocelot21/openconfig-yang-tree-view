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
                .AfterMapping((src, dest) => dest.Children.AddRange(src.Containers.Adapt<List<ContainerViewModel>>()))
                .AfterMapping((src, dest) => dest.Children.AddRange(src.Leafs.Adapt<List<LeafViewModel>>()));


            TypeAdapterConfig<Container, ContainerViewModel>.NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .AfterMapping((src, dest) => dest.Children.AddRange(src.Containers.Adapt<List<ContainerViewModel>>()))
                .AfterMapping((src, dest) => dest.Children.AddRange(src.Lists.Adapt<List<ListViewModel>>()));

            TypeAdapterConfig<YangList, ListViewModel>.NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .AfterMapping((src, dest) => dest.Children.AddRange(src.Containers.Adapt<List<ContainerViewModel>>()))
                .AfterMapping((src, dest) => dest.Children.AddRange(src.Lists.Adapt<List<ListViewModel>>()))
                .AfterMapping((src, dest) => dest.Children.AddRange(src.Leafs.Adapt<List<LeafViewModel>>()));
        }

        private static ObservableCollection<TDestination> ConvertToObservableCollection<TDestination>(IEnumerable<TDestination> sourceList)
        {
            return new ObservableCollection<TDestination>(sourceList);
        }
    }
}
