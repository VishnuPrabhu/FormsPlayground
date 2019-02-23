using DryIoc;
using ReactiveUI;

namespace FormsPlayground.Infrastructure
{
    public static class ContainerExtensions
    {
        public static void RegisterViewForViewModel<TPage, TViewModel>(this IContainer container)
            where TViewModel : class
            where TPage : IViewFor<TViewModel>
        {
            container.Register<TViewModel>();
            container.Register<IViewFor<TViewModel>, TPage>();
        }
    }
}