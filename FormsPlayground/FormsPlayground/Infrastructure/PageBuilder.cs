using System;
using System.Linq;
using System.Reflection;
using DryIoc;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FormsPlayground.Infrastructure
{
    public class PageBuilder : IPageBuilder
    {
        public Page BuildPage<TViewModel>(object paramsObj = null)
            where TViewModel : class, IFormsViewModel
        {
            try
            {
                var view = Ioc.Container.Resolve<IViewFor<TViewModel>>();
                
                if(!(view is Page page))
                    throw new TypeAccessException("view must be a Page type");
                
                view.ViewModel = ResolveViewModel<TViewModel>(paramsObj);

                return page;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public Page BuildPageFromViewModel(Type viewModelType, object paramsObj = null) 
            => GetType()
                .GetRuntimeMethods()
                .Single(x => x.Name.Equals(nameof(BuildPage)))
                .MakeGenericMethod(viewModelType)
                .Invoke(this, new object[] { null }) as Page;

        private static TViewModel ResolveViewModel<TViewModel>(object paramsObj = null)
        {
            if (paramsObj == null)
                return Ioc.Container.Resolve<TViewModel>();
            
            ParameterSelector resolveParams = null;

            paramsObj
                .GetType()
                .GetRuntimeProperties()
                .ForEach(x =>
                {
                    resolveParams = resolveParams == null
                        ? Parameters.Of.Name(x.Name, n => x.GetValue(paramsObj, null))
                        : resolveParams.Name(x.Name, n => x.GetValue(paramsObj, null));
                });

            return Ioc.Container
                .WithDependencies(resolveParams)
                .Resolve<TViewModel>();
        }
    }
}