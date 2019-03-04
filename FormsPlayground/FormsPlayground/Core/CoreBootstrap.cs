using DryIoc;
using FormsPlayground.Features.Home.More;
using FormsPlayground.Features.Home.ViewModels;
using FormsPlayground.Features.Home.Views;
using FormsPlayground.Features.Labels;
using FormsPlayground.Infrastructure.InversionOfControl;
using FormsPlayground.Infrastructure.Mvvm;

namespace FormsPlayground.Core
{
    public static class CoreBootstrap
    {
        public static void Run(IContainer container)
        {
            container.Register<IPageBuilder, PageBuilder>();

            // HOME
            container.RegisterViewForViewModel<FeatureListPage, FeatureListViewModel>();
            container.RegisterViewForViewModel<MorePage, MoreViewModel>();
            container.RegisterViewForViewModel<LabelsPage, LabelsViewModel>();
        }
    }
}