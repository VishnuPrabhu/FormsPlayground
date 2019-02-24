using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DryIoc;
using FormsPlayground.Effects;
using FormsPlayground.Features.Home.Tabs.Design;
using FormsPlayground.Features.Home.Tabs.Forms;
using FormsPlayground.Features.Home.Tabs.Libs;
using FormsPlayground.Features.Home.Tabs.More;
using FormsPlayground.Features.Home.Tabs.Skia;
using FormsPlayground.Infrastructure;
using FormsPlayground.Resources;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace FormsPlayground.Features.Home
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            On<Xamarin.Forms.PlatformConfiguration.Android>()
                .SetToolbarPlacement(ToolbarPlacement.Bottom)
                .SetIsSwipePagingEnabled(false)
                .SetIsSmoothScrollEnabled(false)
                .SetBarItemColor(Color.FromHex("#9B9B9B"))
                .SetBarSelectedItemColor(Color.FromHex("#00A4FF"));
            
            Effects.Add(new NoShiftEffect());
            
            BarBackgroundColor = Color.White;
            
            Children.Add(GetTab<FormsViewModel>("icon_skia", Strings.Menu_Forms, false));
            Children.Add(GetTab<DesignViewModel>("icon_skia", Strings.Menu_Design));
            Children.Add(GetTab<LibsViewModel>("icon_skia", Strings.Menu_Libs));
            Children.Add(GetTab<SkiaViewModel>("icon_skia", Strings.Menu_Skia));
            Children.Add(GetTab<MoreViewModel>("icon_more", Strings.Menu_More));
            
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
                        ex => Console.WriteLine(ex.StackTrace));
            }
        }

        private CustomNavigationPage GetTab<TViewModel>(string icon, string title, bool lazyLoad = true)
            where TViewModel : class, IFormsViewModel
        {
            try
            {
                CustomNavigationPage holderPage;
                
                if (lazyLoad) 
                {
                    // To improve load performance: start with an empty page,
                    // CustomNavigationPage will then create the actual page when navigating to it
                    holderPage = new CustomNavigationPage(new EmptyPage {Title = title}, typeof(TViewModel));
                }
                else
                {
                    var page = Ioc.Container
                        .Resolve<IPageBuilder>()
                        .BuildPage<TViewModel>();
                    
                    holderPage = new CustomNavigationPage(page);
                }

                holderPage.Icon = icon;
                holderPage.Title = title;
                holderPage.AutomationId = title;

                return holderPage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debugger.Break();
                throw;
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
    }
}