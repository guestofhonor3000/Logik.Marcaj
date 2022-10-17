using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.LocalDbModels
{
    public class LDineInTablesModel
    {
        [PrimaryKey]
        public int DineInTableID { get; set; }
        public string DineInTableText { get; set; }
        public int TableGroupID { get; set; }
        public bool DineInTableInActive { get; set; }
    }
}
