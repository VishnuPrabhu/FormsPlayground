using System;
using Xamarin.Forms;

namespace FormsPlayground.Framework.Mvvm
{
    public interface IPageBuilder
    {
        Page BuildPage<TViewModel>(object paramsObj = null, string pageKey = null)
            where TViewModel : class, IFormsViewModel;
        
        Page BuildPageFromViewModel(Type viewModelType, object paramsObj = null, string pageKey = null);
    }
}