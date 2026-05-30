using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliklininka.Entities
{
    public class AllergyPatient
    {
        public int MedCardId { get; set; }
        public int AllergyId { get; set; }

        public MedCard MedCard { get; set; } = null!;
        public Allergy Allergy { get; set; } =null!;

    }
}
