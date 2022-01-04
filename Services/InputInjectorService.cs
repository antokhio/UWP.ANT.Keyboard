using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Input.Preview.Injection;

namespace UWP.ANT.Keyboard.Services
{
    public class InputInjectorService
    {
        protected internal InputInjector injector;

        public InputInjectorService()
        {
            injector = InputInjector.TryCreate();
        }

        
        public void Inject(string input)
        {
            switch (input)
            {

                case "esc":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Escape });
                    break;
                case "tab":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Tab });
                    break;
                case "back":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Back });
                    break;
                case "del":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Delete });
                    break;
                case "enter":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Enter });
                    break;
                case "space":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Space });
                    break;
                case "shift":
                case "caps":
                    break;
                default:
                    var key = new InjectedInputKeyboardInfo {
                        ScanCode = (ushort)input.ToCharArray().FirstOrDefault(),
                        KeyOptions = InjectedInputKeyOptions.Unicode
                    };
                    injector.InjectKeyboardInput(new[] { key });
                    break;
            }
            OnInputInjected();
        }

        private void InjectTwice(InjectedInputKeyboardInfo key)
        {
            injector.InjectKeyboardInput(new[] { key });
            key.KeyOptions = InjectedInputKeyOptions.KeyUp;
            injector.InjectKeyboardInput(new[] { key });
        }

        public event EventHandler InputInjected;
        protected virtual void OnInputInjected()
        {
            EventHandler handler = InputInjected;
            handler?.Invoke(this, null);
        }
    }
}
