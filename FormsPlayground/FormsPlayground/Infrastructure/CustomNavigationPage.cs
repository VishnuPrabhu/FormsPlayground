using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using DryIoc;

namespace FormsPlayground.Infrastructure
{
    public class CustomNavigationPage : NavigationPage
    {
        private readonly Type _lazyViewModelType;
        private readonly object _paramsObj;

        public CustomNavigationPage(Page root, Type lazyViewModelType = null, object paramsObj = null)
            : base(root)
        {
            _lazyViewModelType = lazyViewModelType;
            _paramsObj = paramsObj;
        }

        public async Task StartLazyInitialisation()
        {
            if (_lazyViewModelType != null
                && Navigation.NavigationStack.Count == 1
                && Navigation.NavigationStack.First() is EmptyPage emptyPage)
            {
                var page = Ioc.Container
                    .Resolve<IPageBuilder>()
                    .BuildPageFromViewModel(_lazyViewModelType, _paramsObj);
                
                Navigation.InsertPageBefore(page, emptyPage);
                await Navigation.PopToRootAsync(false);
            }
        }
    }
}