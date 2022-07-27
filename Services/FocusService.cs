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


            if (elem is TextBox || elem is RichEditBox || elem is AutoSuggestBox)
            {
                if (PreviousFocus != null)
                {
                    PreviousFocus.LostFocus -= PreviousFocus_LostFocus;
                }

                PreviousFocus = (Control)elem;
                PreviousFocus.LostFocus += PreviousFocus_LostFocus;
            }
            else
            {
                if (PreviousFocus != null)
                    await FocusManager.TryFocusAsync(PreviousFocus, FocusState.Programmatic);
            }

        }

        private void PreviousFocus_LostFocus(object sender, RoutedEventArgs e)
        {
            OnFocusLost();
        }


        public EventHandler FocusLost;
        protected virtual void OnFocusLost()
        {
            EventHandler handler = FocusLost;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}

