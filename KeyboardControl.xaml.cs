using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.ANT.Keyboard.Data;
using UWP.ANT.Keyboard.Services;
using UWP.ANT.Keyboard.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWP.ANT.Keyboard
{
    public sealed partial class KeyboardControl : UserControl
    {
        public KeyboardViewModel ViewModel { get; set; }
        InputInjectorService InjectorService { get; set; }
        FocusService FocusService { get; set; }
        //List<List<KeyModel>> Layout { get; set;  }
        FontFamily GlyphFont = new FontFamily("Segoe MDL2 Assets");



        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(UserControl), new PropertyMetadata(null));



        public Style ButtonActiveStyle
        {
            get { return (Style)GetValue(ButtonActiveStyleProperty); }
            set { SetValue(ButtonActiveStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonActiveStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonActiveStyleProperty =
            DependencyProperty.Register("ButtonActiveStyle", typeof(Style), typeof(UserControl), new PropertyMetadata(null));

        private Grid MainKeyboard;
        private Grid AltKeyboard;

        private bool KeyLng;


        public KeyboardControl()
        {
            this.InitializeComponent();
            this.InjectorService = new InputInjectorService();
            this.FocusService = new FocusService();


            var lng = ApplicationLanguages.PrimaryLanguageOverride;
            switch (lng)
            {
                default:
                case "ru-RU":
                    this.ViewModel = new KeyboardViewModel(this.InjectorService, this.FocusService, LayoutService.GetLayout("RU", KeyboardLayouts.LowerRussian, KeyboardLayouts.UpperRussian), LayoutService.GetLayout("EN", KeyboardLayouts.LowerEnglish, KeyboardLayouts.UpperEnglish));
                    break;
                case "en-US":
                    this.ViewModel = new KeyboardViewModel(this.InjectorService, this.FocusService, LayoutService.GetLayout("EN", KeyboardLayouts.LowerEnglish, KeyboardLayouts.UpperEnglish), LayoutService.GetLayout("RU", KeyboardLayouts.LowerRussian, KeyboardLayouts.UpperRussian));
                    break;
                case "zh-Hans-CN":
                    this.ViewModel = new KeyboardViewModel(this.InjectorService, this.FocusService, LayoutService.GetLayout("ZH-CN", KeyboardLayouts.LowerChinese, KeyboardLayouts.UpperChinese, KeyboardLayouts.ChiniseCandidates), LayoutService.GetLayout("EN", KeyboardLayouts.LowerEnglish, KeyboardLayouts.UpperEnglish));
                    break;
            }
            this.keyboardControl.Loaded += KeyboardControl_Loaded;
            this.candidateList.Loaded += CandidateList_Loaded;
        }

        private void CandidateList_Loaded(object sender, RoutedEventArgs e)
        {
            //BindingOperations.SetBinding(candidateList, ListView.ItemsSourceProperty, new Binding { Source = ViewModel.Candidates, Mode = BindingMode.OneWay });
            candidateList.ItemClick += (s, a) => ViewModel.CandidateCommand.Execute(a.ClickedItem);
        }

        private void KeyboardControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainKeyboard = Keyboard(ViewModel.Keys, ViewModel.KeysName);
            AltKeyboard = Keyboard(ViewModel.AltKeys, ViewModel.AltKeysName);

            (sender as UserControl).Content = MainKeyboard;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            KeyLng = !KeyLng;

            if (!KeyLng)
                this.keyboardControl.Content = MainKeyboard;
            else
                this.keyboardControl.Content = AltKeyboard;

        }

        private Grid Keyboard(List<List<KeyViewModel>> keys, string name)
        {
            var grid = new Grid();
            var layout = keys;

            var rowsCount = layout.Count();

            for (int i = 0; i < rowsCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });

                var gridRow = new Grid();
                var colsCount = layout[i].Count();

                for (int j = 0; j < colsCount; j++)
                {
                    var item = layout[i][j];

                    gridRow.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(item.Width, GridUnitType.Star)
                    });

                    if (String.Equals(item.Key, String.Empty))
                    {
                        var grd = new Grid();
                        grd.HorizontalAlignment = HorizontalAlignment.Stretch;
                        grd.VerticalAlignment = VerticalAlignment.Stretch;

                        Grid.SetColumn(grd, j);
                        gridRow.Children.Add(grd);
                    }
                    else
                    {
                        var btn = new Button();
                        btn.Name = item.Key;
                        btn.AllowFocusOnInteraction = false;
                        btn.Style = this.ButtonStyle;
                        btn.IsTabStop = false;

                        switch (item.Key)
                        {
                            case "lng":
                                //btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uE774" };
                                btn.Content = name;
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.LngCommand });
                                btn.Click += Btn_Click;
                                break;
                            case "esc":
                                btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uE106" };
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.EscCommand });
                                break;
                            case "back":
                                btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uEB96" };
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.BackCommand });
                                break;
                            case "tab":
                                btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uE7FD" };
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.TabCommand });
                                break;
                            case "del":
                                btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uED60" };
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.DeleteCommand });
                                break;
                            case "caps":
                                btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uE8E9" };
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.CapsCommand });
                                BindingOperations.SetBinding(btn, Button.StyleProperty, new Binding { Source = ViewModel, Mode = BindingMode.OneWay, Path = new PropertyPath("IsCaps"), Converter = this.Resources["BooleanToStyle"] as IValueConverter, UpdateSourceTrigger = UpdateSourceTrigger.Default, ConverterParameter = new[] { ButtonStyle, ButtonActiveStyle } });
                                break;
                            case "shift":
                                btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uE8E8" };
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.ShiftCommand });
                                BindingOperations.SetBinding(btn, Button.StyleProperty, new Binding { Source = ViewModel, Mode = BindingMode.OneWay, Path = new PropertyPath("IsShift"), Converter = this.Resources["BooleanToStyle"] as IValueConverter, UpdateSourceTrigger = UpdateSourceTrigger.Default, ConverterParameter = new[] { ButtonStyle, ButtonActiveStyle } });
                                break;
                            case "space":
                                btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uE75D" };
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.SpaceCommand });
                                break;
                            case "enter":
                                btn.Content = new FontIcon { FontFamily = GlyphFont, Glyph = "\uEB97" };
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.ReturnCommand });
                                break;
                            default:
                                //btn.Content = item.Key;
                                BindingOperations.SetBinding(btn, Button.ContentProperty, new Binding { Source = keys[i][j], Mode = BindingMode.OneWay, Path = new PropertyPath("Key"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
                                BindingOperations.SetBinding(btn, Button.CommandProperty, new Binding { Source = ViewModel.KeyCommand });
                                BindingOperations.SetBinding(btn, Button.CommandParameterProperty, new Binding { Source = item });
                                break;
                        };

                        btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                        btn.VerticalAlignment = VerticalAlignment.Stretch;

                        Grid.SetColumn(btn, j);
                        gridRow.Children.Add(btn);
                    }
                }

                Grid.SetRow(gridRow, i);
                grid.Children.Add(gridRow);
            }
            return grid;
        }
    }
}
