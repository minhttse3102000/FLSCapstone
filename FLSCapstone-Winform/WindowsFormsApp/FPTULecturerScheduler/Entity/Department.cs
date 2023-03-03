using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class Department
    {
        [JsonProperty("Id")]
        public string ID { set; get; }
        [JsonProperty("DepartmentName")]
        public string DepartmentName { set; get; }
        [JsonProperty("Status")]
        public int status { set; get; }

        public Department()
        {
        }

        public Department(string Id, string departmentName, int status)
        {
            ID = Id;
            DepartmentName = departmentName;
            this.status = status;
        }
    }
}
