using System.Reactive.Linq;
using FormsPlayground.Infrastructure.Mvvm;
using ReactiveUI;
using ReactiveUI.XamForms;

namespace FormsPlayground.Infrastructure.Pages
{
    public abstract class BasePage<TViewModel> : ReactiveContentPage<TViewModel>
        where TViewModel : BaseViewModel
    {
        public bool Loaded { get; protected set; }

        public BasePage()
        {
            this.WhenActivated(disposables =>
            {
                this
                    .WhenAnyValue(x => x.ViewModel.Title)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .BindTo(this, x => x.Title);
            });
        }
    }
}