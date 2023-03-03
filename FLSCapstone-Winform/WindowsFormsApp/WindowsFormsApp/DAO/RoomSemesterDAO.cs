using FPTULecturerScheduler.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.DAO
{
    public class RoomSemesterDAO
    {
        public static async Task<bool> UpdateCourseAsync(RoomSemester roomSemester)
        {
            try
            {
                var client = new HttpClient();
                var endpoint = new Uri("http://20.214.249.72/api/Course/" + roomSemester.ID);
                var newDepartmentJson = JsonConvert.SerializeObject(roomSemester);
                var payload = new StringContent(newDepartmentJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(endpoint, payload);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("PUT success ");
                    //Console.WriteLine("Status Code: " + response.StatusCode);
                    //Console.WriteLine("Header: " + response.Headers);
                    //Console.WriteLine("Respone: " + response.Content);

                    //Console.WriteLine("Content Respone: " + responseContent);
                    return true;
                }
                //Console.WriteLine("PUT fail");
                return false;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error at PUT: " + ex.Message);
                return false;
            }
        }
    }
}
