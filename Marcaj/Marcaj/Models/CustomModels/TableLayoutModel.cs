using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Marcaj.Models.CustomModels
{
    public class TableLayoutModel
    {
        public string Position { get; set; }
        public string Text { get; set; }
        public bool Visible { get; set; }
        public string TableText { get; set; }
        public string EmpName { get; set; }
        public bool Fumatori { get; set; }
        public bool Cabina { get; set; }
        public bool Fereastra { get; set; }
    }
}
