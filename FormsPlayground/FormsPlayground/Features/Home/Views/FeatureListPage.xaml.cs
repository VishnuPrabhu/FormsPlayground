using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Xamarin.Forms.Xaml;

namespace FormsPlayground.Features.Home.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeatureListPage
    {
        public FeatureListPage()
        {
            InitializeComponent();
            
            this.WhenActivated(disposables =>
            {
                this
                    .WhenAnyValue(x => x.ViewModel.LoadCommand, x => x.Loaded)
                    .Where(x => x.Item1 != null && !x.Item2)
                    .Do(x => Loaded = true) // prevent additional load when navigating back
                    .Select(x => Unit.Default)
                    .InvokeCommand(ViewModel.LoadCommand)
                    .DisposeWith(disposables);
            });
        }
    }
}
