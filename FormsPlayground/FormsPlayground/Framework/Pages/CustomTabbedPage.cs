using System;
using System.Diagnostics;
using DryIoc;
using FormsPlayground.Framework.Effects;
using FormsPlayground.Framework.Extensions;
using FormsPlayground.Framework.InversionOfControl;
using FormsPlayground.Framework.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace FormsPlayground.Framework.Pages
{
    public class CustomTabbedPage : TabbedPage
    {
        public CustomTabbedPage()
        {
            On<Xamarin.Forms.PlatformConfiguration.Android>()
                .SetToolbarPlacement(ToolbarPlacement.Bottom)
                .SetIsSwipePagingEnabled(false)
                .SetIsSmoothScrollEnabled(false)
                .SetBarItemColor(Color.FromHex("#9B9B9B"))
                .SetBarSelectedItemColor(Color.FromHex("#00A4FF"));
            
            Effects.Add(new NoShiftEffect());
            
            BarBackgroundColor = Color.White;
            
            CurrentPageChanged += Handle_CurrentPageChanged;
        }

        private void Handle_CurrentPageChanged(object sender, EventArgs e)
        {
            if (CurrentPage != null && CurrentPage is CustomNavigationPage cnp)
            {
                cnp
                    .StartLazyInitialization()
                    .SafeFireAndForget(
                        true, 
                        ex => Console.WriteLine((string) ex.StackTrace));
            }
        }
        
        protected override bool OnBackButtonPressed()
        {
            // when pressing android hardware back button, show first tab if currently in another tab
            if (CurrentPage != Children[0]
                && CurrentPage is CustomNavigationPage cnp 
                && cnp.CurrentPage == cnp.RootPage) // ignore nested navigations
            {
                CurrentPage = Children[0];
                return true; // returning true will prevent to bubble up the event
            }
            
            return base.OnBackButtonPressed();
        }
        
        protected CustomNavigationPage AddTab<TViewModel>(string icon, string title, bool lazyLoad = true)
            where TViewModel : class, IFormsViewModel
        {
            try
            {
                CustomNavigationPage holderPage;
                var type = typeof(TViewModel);
                var paramsObj = CreateParametersObject(type, title, lazyLoad);
                if (lazyLoad) 
                {
                    // To improve load performance: start with an empty page,
                    // Handle_CurrentPageChanged will then create the actual page when selected
                    holderPage = new CustomNavigationPage(
                        new EmptyPage(), 
                        title,
                        typeof(TViewModel), 
                        paramsObj);
                }
                else
                {
                    var page = Ioc.Container
                        .Resolve<IPageBuilder>()
                        .BuildPage<TViewModel>(paramsObj);
                    
                    holderPage = new CustomNavigationPage(page, title);
                }

                holderPage.Icon = icon;
                holderPage.Title = title;
                holderPage.AutomationId = title;
                
                Children.Add(holderPage);

                return holderPage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debugger.Break();
                throw;
            }
        }

        protected virtual object CreateParametersObject(Type viewModelType, string title, bool lazyLoad)
            => null;
    }
}