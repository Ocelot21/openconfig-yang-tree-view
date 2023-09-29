using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using openconfig_yang_tree_view.MVVM.ViewModels;

namespace openconfig_yang_tree_view.TemplateSelectors
{
    public class NodeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ContainerNodeTemplate { get; set; }
        public DataTemplate LeafNodeTemplate { get; set; }
        public DataTemplate ListNodeTemplate { get; set; }
        public DataTemplate GroupingNodeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ContainerViewModel)
            {
                return ContainerNodeTemplate;
            }
            else if (item is ContainerViewModel)
            {
                return LeafNodeTemplate;
            }
            else if (item is ListViewModel)
            {
                return ListNodeTemplate;
            }
            else if (item is GroupingViewModel)
            {
                return GroupingNodeTemplate;
            }
                
            return base.SelectTemplate(item, container);
        }
    }

}
