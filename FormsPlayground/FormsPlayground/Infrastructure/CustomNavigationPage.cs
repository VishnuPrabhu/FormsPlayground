using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using DryIoc;
using FormsPlayground.Resources;

namespace FormsPlayground.Infrastructure
{
    public class CustomNavigationPage : NavigationPage, IFormsNavigation
    {
        private static readonly string BindingContextTypeErrorMessage 
            = string.Format(Strings.CustomNavigationPage_binding_context_type_error, nameof(IFormsViewModel));
        
        private readonly Type _lazyViewModelType;
        private readonly object _lazyParamsObj;
        
        private readonly Lazy<IPageBuilder> _pageBuilder = new Lazy<IPageBuilder>(() 
            => Ioc.Container.Resolve<IPageBuilder>());

        public CustomNavigationPage(Page root, Type lazyViewModelType = null, object lazyParamsObj = null)
            : base(root)
        {
            _lazyViewModelType = lazyViewModelType;
            _lazyParamsObj = lazyParamsObj;
            
            if (lazyViewModelType == null)
            {
                if (!(root.BindingContext is IFormsViewModel vm))
                    throw new TypeAccessException(BindingContextTypeErrorMessage);
            
                vm.Navigator = this;
            }
        }

        public async Task StartLazyInitialization()
        {
            if (_lazyViewModelType != null
                && Navigation.NavigationStack.Count == 1
                && Navigation.NavigationStack.First() is EmptyPage emptyPage)
            {
                var page = BuildPageFromViewModel(_lazyViewModelType, _lazyParamsObj);
                Navigation.InsertPageBefore(page, emptyPage);
                await Navigation.PopToRootAsync(false);
            }
        }

        public Task Push<TViewModel>(object paramsObj = null, bool animated = true) 
            where TViewModel : class, IFormsViewModel 
            => Push(typeof(TViewModel), paramsObj, animated);

        public Task Push(Type viewModelType, object paramsObj = null, bool animated = true)
        {
            var page = BuildPageFromViewModel(viewModelType, paramsObj);
            return Navigation.PushAsync(page, animated);
        }

        public Task PushModal<TViewModel>(object paramsObj = null, bool animated = true) 
            where TViewModel : class, IFormsViewModel =>
            PushModal(typeof(TViewModel), paramsObj, animated);

        public Task PushModal(Type viewModelType, object paramsObj = null, bool animated = true)
        {
            var page = BuildPageFromViewModel(viewModelType, paramsObj);
            var navigationPage = new CustomNavigationPage(page) { Title = page.Title };
            return Navigation.PushModalAsync(navigationPage, animated);
        }

        public Task ReplaceCurrentStackWith<TViewModel>(object paramsObj = null, bool animated = true) 
            where TViewModel : class, IFormsViewModel
        {
            var page = BuildPageFromViewModel(typeof(TViewModel), paramsObj);
            Navigation.InsertPageBefore(page, Navigation.NavigationStack.First());
            return Navigation.PopToRootAsync(animated);
        }

        private Page BuildPageFromViewModel(Type viewModelType, object paramsObj)
        {
            var page = _pageBuilder.Value.BuildPageFromViewModel(viewModelType, paramsObj);
            
            if (!(page.BindingContext is IFormsViewModel vm))
                throw new TypeAccessException(BindingContextTypeErrorMessage);
            
            vm.Navigator = this;
            
            SetBackButtonTitle(page, string.Empty);

            return page;
        }
    }
}