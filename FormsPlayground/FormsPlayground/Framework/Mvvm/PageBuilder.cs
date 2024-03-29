using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DryIoc;
using FormsPlayground.Framework.InversionOfControl;
using FormsPlayground.Resources;
using MethodTimer;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FormsPlayground.Framework.Mvvm
{
    public class PageBuilder : IPageBuilder
    {
        [Time]
        public Page BuildPage<TViewModel>(object paramsObj = null, string pageKey = null)
            where TViewModel : class, IFormsViewModel
        {
            try
            {
                var view = Ioc.Container.Resolve<IViewFor<TViewModel>>(pageKey);
                
                if(!(view is Page page))
                    throw new TypeAccessException(Strings.PageBuilder_View_type_exception);
                
                view.ViewModel = ResolveViewModel<TViewModel>(paramsObj);

                return page;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debugger.Break();
                throw;
            }
        }
        
        public Page BuildPageFromViewModel(Type viewModelType, object paramsObj = null, string pageKey = null) 
            => GetType()
                .GetRuntimeMethods()
                .Single(x => x.Name.Equals(nameof(BuildPage)))
                .MakeGenericMethod(viewModelType)
                .Invoke(this, new [] { paramsObj, pageKey }) as Page;

        [Time]
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