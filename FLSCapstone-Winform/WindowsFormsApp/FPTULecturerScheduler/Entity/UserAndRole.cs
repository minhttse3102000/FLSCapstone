using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class UserAndRole
    {
        [JsonProperty("Id")]
        public string ID { set; get; }
        [JsonProperty("UserId")]
        public string UserID{ set; get; }
        [JsonProperty("RoleId")]
        public string RoleID { set; get; }
        [JsonProperty("Status")]
        public int status { set; get; }

        public UserAndRole()
        {
        }

        public UserAndRole(string iD, string userID, string roleID, int status)
        {
            ID = iD;
            UserID = userID;
            RoleID = roleID;
            this.status = status;
        }
    }
}
