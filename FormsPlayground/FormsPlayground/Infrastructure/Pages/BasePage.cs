using ReactiveUI.XamForms;

namespace FormsPlayground.Infrastructure.Pages
{
    public abstract class BasePage<TViewModel> : ReactiveContentPage<TViewModel>
        where TViewModel : class
    {
        public bool Loaded { get; protected set; }
    }
}