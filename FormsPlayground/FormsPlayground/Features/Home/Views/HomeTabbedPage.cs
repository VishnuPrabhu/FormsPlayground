using System;
using FormsPlayground.Features.Home.ViewModels;
using FormsPlayground.Framework.Pages;
using FormsPlayground.Resources;

namespace FormsPlayground.Features.Home.Views
{
    public sealed class HomeTabbedPage : CustomTabbedPage
    {
        public HomeTabbedPage()
        {
            AddTab<FeatureListViewModel>("icon_forms", Strings.Menu_Forms, false);
            AddTab<FeatureListViewModel>("icon_design", Strings.Menu_Design);
            AddTab<FeatureListViewModel>("icon_libs", Strings.Menu_Libs);
            AddTab<FeatureListViewModel>("icon_skia", Strings.Menu_Skia);
            AddTab<MoreViewModel>("icon_more", Strings.Menu_More);
        }

        protected override object CreateParametersObject(Type viewModelType, string title, bool lazyLoad) 
            => new {resourceKey = $"Index{title}"};
    }
}