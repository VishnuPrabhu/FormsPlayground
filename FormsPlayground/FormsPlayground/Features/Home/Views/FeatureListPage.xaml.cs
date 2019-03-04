using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FormsPlayground.Features.Home.Model;
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
                
                this
                    .WhenAnyValue(x => x.FeatureListView.SelectedItem)
                    .Where(x => x != null)
                    .Cast<FeatureNode>()
                    .Do(x => FeatureListView.SelectedItem = null) // without this, it will automatically execute when navigating back
                    .InvokeCommand(ViewModel.OpenFeatureCommand)
                    .DisposeWith(disposables);
            });
        }
    }
}
