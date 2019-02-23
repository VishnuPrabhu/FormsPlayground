using DryIoc;
using FormsPlayground.Features.Home.Tabs.More;
using FormsPlayground.Features.Home.Tabs.Skia;

namespace FormsPlayground.Infrastructure
{
    public static class CoreBootstrap
    {
        public static void Run(IContainer container)
        {
            container.Register<IPageBuilder, PageBuilder>();

            container.RegisterViewForViewModel<SkiaPage, SkiaViewModel>();
            container.RegisterViewForViewModel<MorePage, MoreViewModel>();
        }
    }
}