using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.LocalDbModels
{
    public class LMenuGroupsModel
    {
        public int MenuGroupID { get; set; }
        public string MenuGroupText { get; set; }
        public int DisplayIndex { get; set; }
        public bool MenuGroupInActive { get; set; }
        public bool ShowCaption { get; set; }
        public string RowGUID { get; set; }
    }
}
