using DryIoc;
using FormsPlayground.Features.Home.ViewModels;
using FormsPlayground.Features.Home.Views;
using FormsPlayground.Features.Labels;
using FormsPlayground.Framework.InversionOfControl;
using FormsPlayground.Framework.Mvvm;

namespace FormsPlayground.Core
{
    public static class CoreBootstrap
    {
        public static void Run(IContainer container)
        {
            container.Register<IPageBuilder, PageBuilder>();

            container.RegisterViewForViewModel<FeatureListPage, FeatureListViewModel>();
            container.RegisterViewForViewModel<MorePage, MoreViewModel>();
            container.RegisterViewForViewModel<LabelsPage, LabelsViewModel>();
        }
    }
}