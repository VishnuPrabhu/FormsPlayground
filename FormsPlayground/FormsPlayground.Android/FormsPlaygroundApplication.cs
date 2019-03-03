using System;
using Android.App;
using Android.Content;
using FormsPlayground.Core;
using FormsPlayground.Infrastructure;
using FormsPlayground.Infrastructure.InversionOfControl;

namespace FormsPlayground.Droid
{
#if DEBUG
    [Application(Debuggable = true)]
#else
	[Application(Debuggable = false)]
#endif
    
    public class FormsPlaygroundApplication : Application
    {
        public FormsPlaygroundApplication()
        { }

        public FormsPlaygroundApplication(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        { }

        public override void OnTrimMemory(TrimMemory level)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            base.OnTrimMemory(level);
        }

        public override void OnLowMemory()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            base.OnLowMemory();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            
            Ioc.Bootstrapper.Bootstrap("AndroidApplication", container =>
            {
                // register android platform services
            });
        }
    }
}