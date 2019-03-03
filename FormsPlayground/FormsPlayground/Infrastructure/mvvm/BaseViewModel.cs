using ReactiveUI;

namespace FormsPlayground.Infrastructure.Mvvm
{
    public class BaseViewModel : ReactiveObject, IFormsViewModel
    {
        public IFormsNavigation Navigator { get; set; }
    }
}