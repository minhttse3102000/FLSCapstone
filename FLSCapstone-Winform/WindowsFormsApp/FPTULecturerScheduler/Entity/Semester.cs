using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class Semester
    {
        [JsonProperty("Id")]
        public string ID { get; set; }
        [JsonProperty("Term")]
        public string Term { get; set; }
        [JsonProperty("DateStartFormat")]
        public string DateStart { get; set; }
        [JsonProperty("DateEndFormat")]
        public string DateEnd { get; set; }
        [JsonProperty("Status")]
        public int status { get; set; }

        public Semester()
        {
        }
        public Semester(string Id, string term, string dateStart, string dateEnd, int status)
        {
            ID = Id;
            Term = term;
            DateStart = dateStart;
            DateEnd = dateEnd;
            this.status = status;
        }
    }
}
