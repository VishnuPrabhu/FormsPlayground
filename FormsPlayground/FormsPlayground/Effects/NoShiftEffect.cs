using Xamarin.Forms;

namespace FormsPlayground.Effects
{
    public class NoShiftEffect : RoutingEffect
    {
        public NoShiftEffect() 
            : base($"{nameof(FormsPlayground)}.{nameof(NoShiftEffect)}")
        { }
    }
}