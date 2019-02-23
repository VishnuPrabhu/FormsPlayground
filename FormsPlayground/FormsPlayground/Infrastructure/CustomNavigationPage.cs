using System;
using Xamarin.Forms;

namespace FormsPlayground.Infrastructure
{
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage(Page root, Type lazyViewModelType = null, object paramsObj = null)
            : base(root)
        {
            
        }
    }
}