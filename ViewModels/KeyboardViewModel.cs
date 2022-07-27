using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.ANT.Keyboard.Helpers;
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

        public RelayCommand<string> CandidateCommand;

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

        public string KeysName;
        public string AltKeysName;

        //public Dictionary<string, string[]> MainCandidates;
        //public Dictionary<string, string[]> AltCandidates;


        public List<Dictionary<string, string[]>> LayoutCandidates;
        public string SearchCandidate = "";
        public ObservableCollection<string> Candidates;
        public string CandidateFound;

        private bool hasCandidates;

        public bool HasCandidtaes
        {
            get => LayoutCandidates.Count > 0;
        }

        public RelayCommand CandidateCloseCommand { get; set; }

        private FocusService _focusService;

        private int Lang;

        public KeyboardViewModel(InputInjectorService injectorService, FocusService focusService, LayoutModel keys, LayoutModel altKeys)
        {
            _focusService = focusService;

            EscCommand = new RelayCommand(() =>
            {
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

            ShiftCommand = new RelayCommand(() =>
            {
                IsShift = !IsShift;
                IsCaps = false;
                //injectorService.Inject("shift"); 
                SwitchLayout();
            });
            CapsCommand = new RelayCommand(() =>
            {
                IsCaps = !IsCaps;
                IsShift = false;
                //injectorService.Inject("caps"); 
                SwitchLayout();
            });

            LngCommand = new RelayCommand(() =>
            {
                Lang = (Lang + 1) % 2;
            });

            injectorService.InputInjected += OnInputInjected;

            // DEBATABLE
            // TODO MAKE Keys List

            KeysName = keys.Name;
            Keys = new List<List<KeyViewModel>>();
            foreach (var rows in keys.Layout)
            {
                var row = new List<KeyViewModel>();
                foreach (var key in rows)
                {
                    row.Add(new KeyViewModel(key));
                }
                Keys.Add(row);
            }

            AltKeysName = altKeys.Name;
            LayoutCandidates = new List<Dictionary<string, string[]>>();
            LayoutCandidates.Add(keys.Candidtaes);


            AltKeys = new List<List<KeyViewModel>>();
            foreach (var rows in altKeys.Layout)
            {
                var row = new List<KeyViewModel>();
                foreach (var key in rows)
                {
                    row.Add(new KeyViewModel(key));
                }
                AltKeys.Add(row);
            }
            LayoutCandidates.Add(altKeys.Candidtaes);



            Candidates = new ObservableCollection<string>();

            CandidateCommand = new RelayCommand<string>((s) =>
            {
                var elem = _focusService.PreviousFocus;
                if (elem != null)
                {
                    var box = elem as TextBox;
                    var text = box.Text;

                    var res = Utils.ReplaceLastOccurrence(text, SearchCandidate, s);
                    //box.SetValue(TextBox.TextProperty, res.Item1);


                    //await focusService.Focus();
                    //box.SelectionStart = box.Text.LastIndexOf(s) + 1;
                    //box.SelectionStart = res.Item2 + 1;

                    //int start = text.LastIndexOf(SearchCandidate);

                    //if (start != -1)
                    //{
                    //    box.Select(start, SearchCandidate.Length);
                    //    box.SelectedText = s;
                    //    box.SelectionStart = start + 1;
                    //}

                    List<int> indexes = text.AllIndexesOf(SearchCandidate);
                    if (indexes.Count > 0)
                    {
                        int position = box.SelectionStart - SearchCandidate.Length;
                        int nearest = indexes.OrderBy(x => Math.Abs(x - position)).First();

                        box.Select(nearest, SearchCandidate.Length);
                        box.SelectedText = s;
                        box.SelectionStart = nearest + 1;
                        box.SelectionLength = 0;
                    }


                    Candidates.Clear();
                    SearchCandidate = "";
                }
            });

            CandidateCloseCommand = new RelayCommand(() =>
            {
                Candidates.Clear();
                SearchCandidate = "";
            });

            _focusService.FocusLost += (s, a) =>
            {
                SearchCandidate = "";
                Candidates.Clear();
            };
        }

        private void UpdateCandidates(InjectedKey injectedKey)
        {
            if (injectedKey.Type == InjectorServiceInjectedType.Key)
                SearchCandidate += injectedKey.Key;
            else if (injectedKey.Key == "back" && SearchCandidate.Length > 0)
                SearchCandidate.Remove(SearchCandidate.Length - 1);
            else
                SearchCandidate = "";

            if (LayoutCandidates != null)
                if (LayoutCandidates[Lang] != null && LayoutCandidates[Lang].Count > 0)
                    if (LayoutCandidates[Lang].ContainsKey(SearchCandidate))
                    {
                        Candidates.Clear();
                        foreach (var candidate in LayoutCandidates[Lang][SearchCandidate])
                            Candidates.Add(candidate);
                    }
                    else
                        Candidates.Clear();
            if (!LayoutCandidates[Lang].Keys.Any(k => k.StartsWith(SearchCandidate)))
                SearchCandidate = "";



        }


        private void OnInputInjected(object e, InjectedKey a)
        {
            IsShift = false;
            SwitchLayout();

            if (LayoutCandidates != null)
                if (LayoutCandidates[Lang] != null && LayoutCandidates[Lang].Count > 0)
                    UpdateCandidates(a);
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
