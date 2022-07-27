using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Input.Preview.Injection;

namespace UWP.ANT.Keyboard.Services
{
    public enum InjectorServiceInjectedType
    {
        Key,
        Special
    }
    public class InjectedKey
    {
        public string Key { get; set; }
        public InjectorServiceInjectedType Type { get; set; }
    }
    public class InputInjectorService
    {
        protected internal InputInjector injector;

        public InputInjectorService()
        {
            injector = InputInjector.TryCreate();
        }

        private InjectedKey _injectedKey;

        public void Inject(string input)
        {
            switch (input)
            {
                case "esc":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Escape });
                    _injectedKey = new InjectedKey { Key = input, Type = InjectorServiceInjectedType.Special };
                    break;
                case "tab":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Tab });
                    _injectedKey = new InjectedKey { Key = input, Type = InjectorServiceInjectedType.Special };
                    break;
                case "back":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Back });
                    _injectedKey = new InjectedKey { Key = input, Type = InjectorServiceInjectedType.Special };
                    break;
                case "del":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Delete });
                    _injectedKey = new InjectedKey { Key = input, Type = InjectorServiceInjectedType.Special };
                    break;
                case "enter":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Enter });
                    _injectedKey = new InjectedKey { Key = input, Type = InjectorServiceInjectedType.Special };
                    break;
                case "space":
                    InjectTwice(new InjectedInputKeyboardInfo { VirtualKey = (ushort)VirtualKey.Space });
                    _injectedKey = new InjectedKey { Key = input, Type = InjectorServiceInjectedType.Special };
                    break;
                case "shift":
                case "caps":
                    _injectedKey = new InjectedKey { Key = input, Type = InjectorServiceInjectedType.Special };
                    break;
                default:
                    var key = new InjectedInputKeyboardInfo
                    {
                        ScanCode = (ushort)input.ToCharArray().FirstOrDefault(),
                        KeyOptions = InjectedInputKeyOptions.Unicode
                    };
                    injector.InjectKeyboardInput(new[] { key });
                    _injectedKey = new InjectedKey { Key = input, Type = InjectorServiceInjectedType.Key };
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

        public event EventHandler<InjectedKey> InputInjected;
        protected virtual void OnInputInjected()
        {
            EventHandler<InjectedKey> handler = InputInjected;
            handler?.Invoke(this, _injectedKey);
        }
    }
}
