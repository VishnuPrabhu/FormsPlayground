using Xamarin.Forms;

namespace FormsPlayground.Framework.Effects
{
    public class NoShiftEffect : RoutingEffect
    {
        public NoShiftEffect() 
            : base($"{nameof(FormsPlayground)}.{nameof(NoShiftEffect)}")
        { }
    }
}