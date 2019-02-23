using System;
using System.Threading.Tasks;

namespace FormsPlayground.Infrastructure
{
    public interface IFormsNavigation
    {
        Task Push<TViewModel>(object paramsObj = null, bool animated = true) 
            where TViewModel : class, IFormsViewModel;
        
        Task Push(Type viewModelType, object paramsObj = null, bool animated = true);
        
        Task PushModal<TViewModel>(object paramsObj = null, bool animated = true)
            where TViewModel : class, IFormsViewModel;

        Task PushModal(Type viewModelType, object paramsObj = null, bool animated = true);
        
        Task ReplaceCurrentStackWith<TViewModel>(object paramsObj = null, bool animated = true)
            where TViewModel : class, IFormsViewModel;
    }
}