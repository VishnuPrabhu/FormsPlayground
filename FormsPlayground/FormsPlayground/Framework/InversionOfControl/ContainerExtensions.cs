using DryIoc;
using ReactiveUI;

namespace FormsPlayground.Framework.InversionOfControl
{
    public static class ContainerExtensions
    {
        public static void RegisterViewForViewModel<TPage, TViewModel>(this IContainer container, 
            string serviceKey = null)
            where TViewModel : class
            where TPage : IViewFor<TViewModel>
        {
            container.Register<TViewModel>(ifAlreadyRegistered:IfAlreadyRegistered.Keep);
            container.Register<IViewFor<TViewModel>, TPage>(serviceKey:serviceKey);
        }
    }
}