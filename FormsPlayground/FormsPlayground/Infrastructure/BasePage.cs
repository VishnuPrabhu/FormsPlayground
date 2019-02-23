using ReactiveUI.XamForms;

namespace FormsPlayground.Infrastructure
{
    public abstract class BasePage<TViewModel> : ReactiveContentPage<TViewModel>
        where TViewModel : class
    {
        
    }
}