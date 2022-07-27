using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.ANT.Keyboard.Models;

namespace UWP.ANT.Keyboard.ViewModels
{
    public class KeyViewModel : ObservableObject
    {
        private string keyLower;
        private string keyUpper;

        private string key;
        public string Key
        {
            get { return key; }
            set { SetProperty(ref key, value); }
        }

        public int Width { get; set; }

        public KeyViewModel(KeyModel model)
        {
            this.keyLower = model.Key;
            this.keyUpper = model.KeyUpper;

            this.Width = model.Width;

            this.Key = this.keyLower;
        }

        public void ToUpper()
        {
            this.Key = keyUpper;
        }
        public void ToLower()
        {
            this.Key = keyLower;
        }
    }
}
