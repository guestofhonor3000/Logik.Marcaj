using System;
using System.Collections.Generic;
using System.Text;

namespace Marcaj.Models.DbModels.LGK
{
    public partial class LGKMClientsModel
    {
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string ClientDbCode
        {
            get; set;
        }
    }
}
