using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class Schedule
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("IsPublic")]
        public int IsPublic { get; set; }
        public DateTime createTime { get; set; }
        public string decription { get; set; }

        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("Status")]
        public int Status { get; set; }

        public Schedule()
        {
        }

        public Schedule(string id, int isPublic, DateTime createTime, string decription, string semesterId, int status)
        {
            Id = id;
            IsPublic = isPublic;
            this.createTime = createTime;
            this.decription = decription;
            SemesterId = semesterId;
            Status = status;
        }
    }
}
