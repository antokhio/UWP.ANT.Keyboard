using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.ANT.Keyboard.Models
{
    public class LayoutModel
    {
        public string Name { get; set; }
        public List<List<KeyModel>> Layout { get; set; }
        public Dictionary<string, string[]> Candidtaes { get; set; }
    }
}
