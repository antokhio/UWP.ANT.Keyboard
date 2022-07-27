using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.ANT.Keyboard.Models;
using Windows.UI.Input.Preview.Injection;

namespace UWP.ANT.Keyboard.Services
{
    public static class LayoutService
    {
        public static LayoutModel GetLayout(string name, string[] layoutLower, string[] layoutUpper, Dictionary<string, string[]> candidates = null)
        {
            var layout = new List<List<KeyModel>>();

            var rowsCount = layoutLower.Length;
            for (int i = 0; i < rowsCount; i++)
            {
                var row = new List<KeyModel>();
                var keysLower = layoutLower[i].Split(" ");
                var keysUpper = layoutUpper[i].Split(" ");

                for (int j = 0; j < keysLower.Length; j++)
                {
                    int width = 1;
                    switch (keysLower[j])
                    {
                        case ("lng"):
                        case ("esc"):
                        case ("back"):
                        case ("tab"):
                        case ("del"):
                        case ("caps"):
                        case ("shift"):
                            width = 2;
                            break;
                        case ("space"):
                            width = 8;
                            break;
                        default:
                            width = 1;
                            break;
                    }

                    row.Add(new KeyModel
                    {
                        Key = keysLower[j],
                        KeyUpper = keysUpper[j],
                        Width = width
                    });
                }
                layout.Add(row);
            };

            return new LayoutModel { Name = name, Layout = layout, Candidtaes = candidates };
        }
    }
}
