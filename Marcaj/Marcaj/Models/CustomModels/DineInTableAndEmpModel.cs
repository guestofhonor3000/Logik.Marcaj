using System;
using System.Collections.Generic;
using System.Text;
using Marcaj.Models.DbModels;

namespace Marcaj.Models.CustomModels
{
    public partial class DineInTableAndEmpModel
    {
        public DineInTableModel DineIn { get; set; }
        public string EmpName { get; set; }
    }
}
