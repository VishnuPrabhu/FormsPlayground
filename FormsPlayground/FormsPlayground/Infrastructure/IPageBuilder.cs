using System;
using Xamarin.Forms;

namespace FormsPlayground.Infrastructure
{
    public interface IPageBuilder
    {
        Page BuildPage<TViewModel>(object paramsObj = null)
            where TViewModel : class, IFormsViewModel;
        
        Page BuildPageFromViewModel(Type viewModelType, object paramsObj = null);
    }
}