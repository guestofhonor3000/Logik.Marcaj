using Marcaj.Models.LocalDbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.CustomModels
{
    public partial class LDineInTableAndEmpModel
    {
        public LDineInTablesModel DineIn { get; set; }
        public string EmpName { get; set; }
        public bool Opened { get; set; }

    }
}
