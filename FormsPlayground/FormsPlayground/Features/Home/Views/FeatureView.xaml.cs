using System.Linq;
using System.Reactive.Disposables;
using ReactiveUI;
using Xamarin.Forms.Xaml;

namespace FormsPlayground.Features.Home.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeatureView
    {
        public FeatureView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this
                    .OneWayBind(ViewModel,
                        x => x.Children,
                        v => v.RightChevron.IsVisible,
                        children => children != null && children.Any())
                    .DisposeWith(disposables);
            });
        }
    }
}
