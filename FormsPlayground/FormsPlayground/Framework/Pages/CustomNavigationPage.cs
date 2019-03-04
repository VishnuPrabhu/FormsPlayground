using System;
using System.Linq;
using System.Threading.Tasks;
using DryIoc;
using FormsPlayground.Framework.InversionOfControl;
using FormsPlayground.Framework.Mvvm;
using FormsPlayground.Resources;
using Xamarin.Forms;

namespace FormsPlayground.Framework.Pages
{
    public class CustomNavigationPage : NavigationPage, IFormsNavigation
    {
        private static readonly string BindingContextTypeErrorMessage 
            = string.Format(Strings.CustomNavigationPage_binding_context_type_error, nameof(IFormsViewModel));
        
        private readonly Type _lazyViewModelType;
        private readonly object _lazyParamsObj;
        private readonly string _lazyPageKey;
        private readonly string _title;

        private readonly Lazy<IPageBuilder> _pageBuilder = new Lazy<IPageBuilder>(() 
            => Ioc.Container.Resolve<IPageBuilder>());

        public CustomNavigationPage(Page root, string title, Type lazyViewModelType = null, object lazyParamsObj = null, string lazyPageKey = null)
            : base(root)
        {
            _lazyViewModelType = lazyViewModelType;
            _lazyParamsObj = lazyParamsObj;
            _lazyPageKey = lazyPageKey;
            _title = title;

            root.Title = Title = title;
            
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
                var page = BuildPageFromViewModel(_lazyViewModelType, _lazyParamsObj, _lazyPageKey);
                page.Title = _title;
                
                Navigation.InsertPageBefore(page, emptyPage);
                await Navigation.PopToRootAsync(false);
            }
        }

        public Task Push<TViewModel>(object paramsObj = null, string pageKey = null, bool animated = true) 
            where TViewModel : class, IFormsViewModel 
            => Push(typeof(TViewModel), paramsObj, pageKey, animated);

        public Task Push(Type viewModelType, object paramsObj = null, string pageKey = null, bool animated = true)
        {
            var page = BuildPageFromViewModel(viewModelType, paramsObj, pageKey);
            return Navigation.PushAsync(page, animated);
        }

        public Task PushModal<TViewModel>(object paramsObj = null, string pageKey = null, bool animated = true) 
            where TViewModel : class, IFormsViewModel =>
            PushModal(typeof(TViewModel), paramsObj, pageKey, animated);

        public Task PushModal(Type viewModelType, object paramsObj = null, string pageKey = null, bool animated = true)
        {
            var page = BuildPageFromViewModel(viewModelType, paramsObj, pageKey);
            var navigationPage = new CustomNavigationPage(page, _title);
            return Navigation.PushModalAsync(navigationPage, animated);
        }

        public Task ReplaceCurrentStackWith<TViewModel>(object paramsObj = null, string pageKey = null, bool animated = true) 
            where TViewModel : class, IFormsViewModel
        {
            var page = BuildPageFromViewModel(typeof(TViewModel), paramsObj, pageKey);
            Navigation.InsertPageBefore(page, Navigation.NavigationStack.First());
            return Navigation.PopToRootAsync(animated);
        }

        private Page BuildPageFromViewModel(Type viewModelType, object paramsObj, string pageKey)
        {
            var page = _pageBuilder.Value.BuildPageFromViewModel(viewModelType, paramsObj, pageKey);
            
            if (!(page.BindingContext is IFormsViewModel vm))
                throw new TypeAccessException(BindingContextTypeErrorMessage);
            
            vm.Navigator = this;
            
            SetBackButtonTitle(page, string.Empty);

            return page;
        }
    }
}