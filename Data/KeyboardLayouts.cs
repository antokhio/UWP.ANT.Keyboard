using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.ANT.Keyboard.Data
{
    public static class KeyboardLayouts
    {
        public static string[] LowerRussian = new string[]
        {
              "ё 1 2 3 4 5 6 7 8 9 0 - = back esc",
              "tab й ц у к е н г ш щ з х ъ \\ del",
              "caps ф ы в а п р о л д ж э enter",
              "shift я ч с м и т ь б ю . , shift",
              "lng space  "
        };

        public static string[] UpperRussian = new string[]
        {
              "Ё ! \" № ; % : ? * ( ) _ + back esc",
              "tab Й Ц У К Е Н Г Ш Щ З Х Ъ / del",
              "caps Ф Ы В А П Р О Л Д Ж Э enter",
              "shift Я Ч С М И Т Ь Б Ю . , shift",
              "lng space  "
        };

        public static string[] LowerEnglish = new string[]
        {
              "` 1 2 3 4 5 6 7 8 9 0 - = back esc",
              "tab q w e r t y u i o p [ ] del",
              "caps a s d f g h j k l ; ' enter",
              "shift z x c v b n m , . / shift",
              "lng space  "
        };

        public static string[] UpperEnglish = new string[]
        {
              "~ ! @ # $ % ^ & * ( ) _ + back esc",
              "tab Q W E R T Y U I O P { } del",
              "caps A S D F G H J K L : \" enter",
              "shift Z X C V B N M < > ? shift",
              "lng space  "
        };
    }
}
