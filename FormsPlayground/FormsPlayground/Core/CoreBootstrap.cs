using DryIoc;
using FormsPlayground.Features.Home.Tabs.Design;
using FormsPlayground.Features.Home.Tabs.Forms;
using FormsPlayground.Features.Home.Tabs.Libs;
using FormsPlayground.Features.Home.Tabs.More;
using FormsPlayground.Features.Home.Tabs.Skia;
using FormsPlayground.Infrastructure;

namespace FormsPlayground.Core
{
    public static class CoreBootstrap
    {
        public static void Run(IContainer container)
        {
            container.Register<IPageBuilder, PageBuilder>();

            container.RegisterViewForViewModel<SkiaPage, SkiaViewModel>();
            container.RegisterViewForViewModel<MorePage, MoreViewModel>();
            container.RegisterViewForViewModel<DesignPage, DesignViewModel>();
            container.RegisterViewForViewModel<FormsPage, FormsViewModel>();
            container.RegisterViewForViewModel<LibsPage, LibsViewModel>();
        }
    }
}