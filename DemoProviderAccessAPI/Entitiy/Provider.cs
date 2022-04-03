using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProviderAccessAPI.Entitiy
{
    public class Provider
    {
        public string ProviderFirstName { get; set; }

        public string ProviderLastName { get; set; }

        public string Address { get; set; }

        public string NPI { get; set; }

        public string ExternalID { get; set; }

        public string FeeScheduleID { get; set; }

        public string EffectiveStartDate { get; set; }

        public string EffectiveEndDate { get; set; }

        public string CarrierID { get; set; }

        public string AccountID { get; set; }

        public string GroupID { get; set; }
    }
}
