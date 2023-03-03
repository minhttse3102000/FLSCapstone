using FPTULecturerScheduler.Entity;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.DAO
{
    public class LecturerDAO
    {
        //public static List<Lecturer> getListLecturerFromExcel(string fileName)
        //{
        //    List<Lecturer> lecturerList = new List<Lecturer>();
        //    try
        //    {
        //        FileInfo file = new FileInfo(fileName);

        //        // mở file excel
        //        var package = new ExcelPackage(new FileInfo(fileName));
        //        // lấy ra sheet đầu tiên để thao tác
        //        ExcelWorksheet workSheet = package.Workbook.Worksheets[0];

        //        // duyệt tuần tự từ dòng thứ 2 đến dòng cuối cùng của file. lưu ý file excel bắt đầu từ số 1 không phải số 0
        //        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
        //        {
        //            try
        //            {
        //                // biến j biểu thị cho một column trong file
        //                int j = 1;

        //                // lấy ra cột họ tên tương ứng giá trị tại vị trí [i, 1]. i lần đầu là 2
        //                // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
        //                string LecturerID = workSheet.Cells[i, j++].Value.ToString();
        //                string DepartmentID = workSheet.Cells[i, j++].Value.ToString();
        //                string LecturerName = workSheet.Cells[i, j++].Value.ToString();
        //                string Email = workSheet.Cells[i, j++].Value.ToString();
        //                string DOB = workSheet.Cells[i, j++].Value.ToString();
        //                string Gender = workSheet.Cells[i, j++].Value.ToString();
        //                string IDCard = workSheet.Cells[i, j++].Value.ToString();
        //                string Address = workSheet.Cells[i, j++].Value.ToString();
        //                string Phone = workSheet.Cells[i, j++].Value.ToString();
        //                string PriorityLecturer = workSheet.Cells[i, j++].Value.ToString();
        //                int IsFullTime = workSheet.Cells[i, j++].Value.ToString();
        //                string status = workSheet.Cells[i, j++].Value.ToString();


        //                // lấy ra cột ngày sinh tương ứng giá trị tại vị trí [i, 2]. i lần đầu là 2
        //                // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
        //                // lấy ra giá trị ngày tháng và ép kiểu thành DateTime                      
        //                //var birthdayTemp = workSheet.Cells[i, j++].Value;

        //                Lecturer lecturer = new Lecturer(LecturerID, LecturerName, Email, DOB, Gender, IDCard, Address, Phone, Int32.Parse(status), Int32.Parse(PriorityLecturer), IsFullTime, DepartmentID);
        //                /*                         

        //                Đừng lười biến mà dùng đoạn code này sẽ gây ra lỗi nếu giá trị value không thỏa kiểu DateTime

        //                DateTime birthday = (DateTime)workSheet.Cells[i, j++].Value;

        //                 */

        //                // add course vào danh sách courseList
        //                lecturerList.Add(lecturer);

        //            }
        //            catch (Exception exe)
        //            {
        //                Console.WriteLine(exe.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return lecturerList;

        //}

        public static List<Lecturer> ReadDataJsonLecturer(string filePath)
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var obj = sr.ReadToEnd();
                lecturers = JsonConvert.DeserializeObject<List<Lecturer>>(obj);

            }

            return lecturers;
        }

        //public void WriteDataJsonLecturer(List<Lecturer> lecturers)
        //{
        //    try
        //    {
        //        if (lecturers != null)
        //        {
        //            using (StreamWriter sw = File.CreateText(filePathOut + fileNameLecturer))
        //            {
        //                var lecturersJson = JsonConvert.SerializeObject(lecturers);
        //                sw.WriteLine(lecturersJson);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        public static async Task<List<Lecturer>> GetLecturerAsync()
        {
            List<Lecturer> lecturers;
            try
            {
                var client = new HttpClient();

                //string accessToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik5nbyBEYW5nIEhhIEFuIiwiZW1haWwiOiJtaW5odmlwcHJvbGsxMjNAZ21haWwuY29tIiwic3ViIjoibWluaHZpcHByb2xrMTIzQGdtYWlsLmNvbSIsImp0aSI6ImdyYnRsNmpPblJCeHZuNElJc1VaU2lpQ3JzUEJXcCIsIlVzZXJOYW1lIjoiTmdvIERhbmcgSGEgQW4iLCJJZCI6IkFuTkRIMiIsIm5iZiI6MTY2OTE5MDMwNiwiZXhwIjoxNjY5MTk3NTA2LCJpYXQiOjE2NjkxOTAzMDZ9.XBGvZYjWpGzS1pfRXjnDyVvfyeKAwcdJMnF5dX3jR8gai88K_zqgtF-JIvcpHLGs9fFXVbK9kdcyeN1FeKvMkw";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                //var endpoint = new Uri("http://20.214.249.72/api/UserAuthen?pageIndex=1&pageSize=1000000");

                var endpoint = new Uri("http://20.214.249.72/api/User?RoleIDs=LC&Status=1&pageIndex=1&pageSize=10000");


                HttpResponseMessage response = await client.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Get success ");
                    //Console.WriteLine("Status Code: " + response.StatusCode);
                    //Console.WriteLine("Header: " + response.Headers);
                    //Console.WriteLine("Respone: " + response.Content);

                    //Console.WriteLine("Content Respone: " + responseContent);
                    lecturers = JsonConvert.DeserializeObject<List<Lecturer>>(responseContent);
                    return lecturers;
                }
                //Console.WriteLine("Get fail");
                return null;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error at Get: " + ex.Message);
                return null;
            }
        }

        public static async Task<Lecturer> GetUserByKeyAsync(string key)
        {
            Lecturer lecturer;
            try
            {
                var client = new HttpClient();

                //string accessToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik5nbyBEYW5nIEhhIEFuIiwiZW1haWwiOiJtaW5odmlwcHJvbGsxMjNAZ21haWwuY29tIiwic3ViIjoibWluaHZpcHByb2xrMTIzQGdtYWlsLmNvbSIsImp0aSI6ImdyYnRsNmpPblJCeHZuNElJc1VaU2lpQ3JzUEJXcCIsIlVzZXJOYW1lIjoiTmdvIERhbmcgSGEgQW4iLCJJZCI6IkFuTkRIMiIsIm5iZiI6MTY2OTE5MDMwNiwiZXhwIjoxNjY5MTk3NTA2LCJpYXQiOjE2NjkxOTAzMDZ9.XBGvZYjWpGzS1pfRXjnDyVvfyeKAwcdJMnF5dX3jR8gai88K_zqgtF-JIvcpHLGs9fFXVbK9kdcyeN1FeKvMkw";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                //var endpoint = new Uri("http://20.214.249.72/api/UserAuthen?pageIndex=1&pageSize=1000000");

                var endpoint = new Uri("http://20.214.249.72/api/Token/GetUserByRefreshToken/" + key);

                HttpResponseMessage response = await client.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Get success ");
                    //Console.WriteLine("Status Code: " + response.StatusCode);
                    //Console.WriteLine("Header: " + response.Headers);
                    //Console.WriteLine("Respone: " + response.Content);

                    //Console.WriteLine("Content Respone: " + responseContent);
                    lecturer = JsonConvert.DeserializeObject<Lecturer>(responseContent);
                    return lecturer;
                }
                //Console.WriteLine("Get fail");
                return null;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error at Get: " + ex.Message);
                return null;
            }
        }
    }
}
