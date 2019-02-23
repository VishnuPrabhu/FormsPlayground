using System;
using System.Diagnostics;
using DryIoc;
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
            
            Children.Add(GetTab<SkiaViewModel>("icon_skia", Strings.Menu_Skia, false));
            Children.Add(GetTab<MoreViewModel>("icon_more", Strings.Menu_More));
            
            CurrentPageChanged += Handle_CurrentPageChanged;
        }
        
        private void Handle_CurrentPageChanged(object sender, EventArgs e)
        {
            if (CurrentPage != null && CurrentPage is CustomNavigationPage cnp)
            {
                cnp
                    .StartLazyInitialisation()
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
                    holderPage = new CustomNavigationPage(new EmptyPage(), typeof(TViewModel));
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
    }
}