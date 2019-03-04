using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FormsPlayground.Infrastructure.Mvvm
{
    public class BaseViewModel : ReactiveObject, IFormsViewModel
    {
        public IFormsNavigation Navigator { get; set; }
        
        [Reactive] public string Title { get; protected set; }
    }
}