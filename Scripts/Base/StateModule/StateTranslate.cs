using System;
using System.Text;
using JetBrains.Annotations;

namespace Base.StateModule
{
    public class StateTranslate
    {
        public string Translate([NotNull] State state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            var effectCaption = new StringBuilder();
            if (state.OtherEffect.Equals(""))
            {
                if (state.Logic != 0)
                {
                    effectCaption.Append("逻辑+" + state.Logic + " ");
                }
                if (state.Talk != 0)
                {
                    effectCaption.Append("言语+" + state.Talk + " ");
                }
                if (state.Athletics != 0)
                {
                    effectCaption.Append("体能+" + state.Athletics + " ");
                }
                if (state.Creativity != 0)
                {
                    effectCaption.Append("灵感+" + state.Creativity + " ");
                }
            }
            else
            {
                effectCaption.Append(state.OtherEffect);
            }
            return effectCaption.ToString();
        }
    }
}