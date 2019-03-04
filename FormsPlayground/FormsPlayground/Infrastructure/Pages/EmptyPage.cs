using Xamarin.Forms;

namespace FormsPlayground.Infrastructure.Pages
{
    internal class EmptyPage : ContentPage
    {
        public EmptyPage(bool hideNavigationBar = false)
        {
            BackgroundColor = Color.White;
            
            NavigationPage.SetBackButtonTitle(this, string.Empty);

            if (hideNavigationBar)
                NavigationPage.SetHasNavigationBar(this, false); 
        }
    }
}