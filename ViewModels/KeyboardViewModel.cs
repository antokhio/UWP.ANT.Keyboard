using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.ANT.Keyboard.Models;
using UWP.ANT.Keyboard.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UWP.ANT.Keyboard.ViewModels
{
    public class KeyboardViewModel : ObservableObject
    {
        public RelayCommand EscCommand;
        public RelayCommand TabCommand;
        public RelayCommand BackCommand;
        public RelayCommand DeleteCommand;
        public RelayCommand CapsCommand;
        public RelayCommand ShiftCommand;
        public RelayCommand ReturnCommand;
        public RelayCommand SpaceCommand;
        public RelayCommand<KeyViewModel> KeyCommand;

        public RelayCommand LngCommand;

        private bool isShift;
        public bool IsShift
        {
            get => isShift;
            set => SetProperty(ref isShift, value);
        }

        private bool isCaps;
        public bool IsCaps
        {
            get => isCaps;
            set => SetProperty(ref isCaps, value);
        }

        public List<List<KeyViewModel>> Keys;
        public List<List<KeyViewModel>> AltKeys;

        public KeyboardViewModel(InputInjectorService injectorService, FocusService focusService, List<List<KeyModel>> keys, List<List<KeyModel>> altKeys)
        {
            EscCommand = new RelayCommand(() => {
                var elem = FocusManager.GetFocusedElement();
                if (elem is TextBox)
                    (elem as TextBox).Text = String.Empty;
                if (elem is AutoSuggestBox)
                    (elem as AutoSuggestBox).Text = String.Empty;
                if (elem is PasswordBox)
                    (elem as PasswordBox).Password = String.Empty;
                // 2 DO if (elem is RichEditBox)

                injectorService.Inject("esc");
            });

            TabCommand = new RelayCommand(() => injectorService.Inject("tab"));
            BackCommand = new RelayCommand(() => injectorService.Inject("back"));
            DeleteCommand = new RelayCommand(() => injectorService.Inject("del"));
            ReturnCommand = new RelayCommand(() => injectorService.Inject("enter"));
            SpaceCommand = new RelayCommand(() => injectorService.Inject("space"));

            KeyCommand = new RelayCommand<KeyViewModel>(async (i) =>
            {
                await focusService.Focus();
                injectorService.Inject(i.Key);
            });

            ShiftCommand = new RelayCommand(() => {
                IsShift = !IsShift;
                IsCaps = false;
                //injectorService.Inject("shift"); 
                SwitchLayout();
            });
            CapsCommand = new RelayCommand(() => {
                IsCaps = !IsCaps;
                IsShift = false;
                //injectorService.Inject("caps"); 
                SwitchLayout();
            });

            LngCommand = new RelayCommand(() =>
            {

            });

            injectorService.InputInjected += OnInputInjected;

            // DEBATABLE
            Keys = new List<List<KeyViewModel>>();
            foreach (var rows in keys)
            {
                var row = new List<KeyViewModel>();
                foreach (var key in rows)
                {
                    row.Add(new KeyViewModel(key));
                }
                Keys.Add(row);
            }

            AltKeys = new List<List<KeyViewModel>>();
            foreach (var rows in altKeys)
            {
                var row = new List<KeyViewModel>();
                foreach (var key in rows)
                {
                    row.Add(new KeyViewModel(key));
                }
                AltKeys.Add(row);
            }
        }

        private void OnInputInjected(object e, EventArgs a)
        {
            IsShift = false;
            SwitchLayout();
        }

        private void SwitchLayout()
        {
            if (isShift || isCaps)
            {
                Keys.ForEach(r => r.ForEach(k => k.ToUpper()));
                AltKeys.ForEach(r => r.ForEach(k => k.ToUpper()));
            }
            else
            {
                Keys.ForEach(r => r.ForEach(k => k.ToLower()));
                AltKeys.ForEach(r => r.ForEach(k => k.ToLower()));
            }
                
        }
    }
}
