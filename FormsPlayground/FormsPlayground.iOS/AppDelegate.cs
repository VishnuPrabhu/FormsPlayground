using FormsPlayground.Core;
using FormsPlayground.Infrastructure;
using Foundation;
using UIKit;

namespace FormsPlayground.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Ioc.Bootstrapper.Bootstrap("iOSApplication", container =>
            {
                // register iOS platform services
            });
            
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
