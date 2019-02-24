using Android.Support.Design.BottomNavigation;
using Android.Support.Design.Widget;
using Android.Views;
using FormsPlayground.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportEffect (typeof(NoShiftEffect), nameof(FormsPlayground.Effects.NoShiftEffect))]
namespace FormsPlayground.Droid.Effects
{
    // Thanks to: https://montemagno.com/xamarin-forms-fully-customize-bottom-tabs-on-android-turn-off-shifting/
    public class NoShiftEffect : PlatformEffect
    {
        protected override void OnAttached ()
        {
            if (!(Container.GetChildAt(0) is ViewGroup layout))
                return;

            if (!(layout.GetChildAt(1) is BottomNavigationView bottomNavigationView))
                return;

            // This is what we set to adjust if the shifting happens
            bottomNavigationView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityLabeled;
        }

        protected override void OnDetached ()
        {
        }
    }
}