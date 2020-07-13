using DotEngine.Fonts;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Text))]
    [ExecuteInEditMode]
    public class BitmapFontUIText : BitmapFontText
    {
        public Text uiText = null;

        protected override void OnTextChanged(string mappedText)
        {
            if(uiText == null)
            {
                uiText = GetComponent<Text>();
            }

            if(uiText!=null)
            {
                if(uiText.font != FontData.bmFont)
                {
                    uiText.font = FontData.bmFont;
                }
                uiText.text = mappedText;
            }
        }
    }
}


