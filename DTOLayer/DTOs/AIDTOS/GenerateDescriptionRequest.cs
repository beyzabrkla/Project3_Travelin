using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOs.AIDTOS
{
    public class GenerateDescriptionRequest
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Duration { get; set; }
        public string Price { get; set; }
        public string Tone { get; set; }
    }
}
