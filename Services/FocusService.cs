using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UWP.ANT.Keyboard.Services
{
    public class FocusService
    {
        public Control PreviousFocus { get; set; }
        public async Task Focus()
        {
            var elem = FocusManager.GetFocusedElement();

            if (elem is TextBox)
                PreviousFocus = elem as TextBox;
            if (elem is RichEditBox)
                PreviousFocus = elem as RichEditBox;
            if (elem is AutoSuggestBox)
                PreviousFocus = elem as AutoSuggestBox;
            else
            {
                if (PreviousFocus != null)
                    await FocusManager.TryFocusAsync(PreviousFocus, FocusState.Programmatic);
            }
        }
    }
}
