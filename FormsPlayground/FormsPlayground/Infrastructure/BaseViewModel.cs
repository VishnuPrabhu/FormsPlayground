using ReactiveUI;

namespace FormsPlayground.Infrastructure
{
    public class BaseViewModel : ReactiveObject, IFormsViewModel
    {
        public IFormsNavigation Navigator { get; set; }
    }
}