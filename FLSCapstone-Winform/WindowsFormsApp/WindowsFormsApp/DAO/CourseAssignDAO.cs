using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPTULecturerScheduler.Entity;
using Newtonsoft.Json;
using System.Net.Http;

namespace WindowsFormsApp.DAO
{
    internal class CourseAssignDAO
    {
        //public static List<CourseAssign> getCourseAssignFromExcel(string fileName)
        //{
        //    List<CourseAssign> courseAssignList = new List<CourseAssign>();
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
        //                string ID = workSheet.Cells[i, j++].Value.ToString();
        //                string lecturerID = workSheet.Cells[i, j++].Value.ToString();
        //                string courseID = workSheet.Cells[i, j++].Value.ToString();
        //                string slotTypeID = workSheet.Cells[i, j++].Value.ToString();

        //                // lấy ra cột ngày sinh tương ứng giá trị tại vị trí [i, 2]. i lần đầu là 2
        //                // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
        //                // lấy ra giá trị ngày tháng và ép kiểu thành DateTime                      
        //                //var birthdayTemp = workSheet.Cells[i, j++].Value;

        //                CourseAssign courseAssign = new CourseAssign(ID, lecturerID, courseID, slotTypeID, 1);
        //                /*                         

        //                Đừng lười biến mà dùng đoạn code này sẽ gây ra lỗi nếu giá trị value không thỏa kiểu DateTime

        //                DateTime birthday = (DateTime)workSheet.Cells[i, j++].Value;

        //                 */




        //                // add course vào danh sách courseList
        //                courseAssignList.Add(courseAssign);

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

        //    return courseAssignList;

        //}

        public static List<CourseAssign> ReadDataJsonCourseAssign(string filePath)
        {
            List<CourseAssign> courseAssigns = new List<CourseAssign>();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var obj = sr.ReadToEnd();
                courseAssigns = JsonConvert.DeserializeObject<List<CourseAssign>>(obj);

            }
            return courseAssigns;
        }

        //public void WriteDataJsonCourseAssign(List<CourseAssign> courseAssigns)
        //{
        //    try
        //    {
        //        if (courseAssigns != null)
        //        {
        //            using (StreamWriter sw = File.CreateText(filePathOut + fileNameCourseAssign))
        //            {
        //                var courseAssignsJson = JsonConvert.SerializeObject(courseAssigns);
        //                sw.WriteLine(courseAssignsJson);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        public static async Task<bool> CreateCourseAssignAsync(CourseAssign courseAssign)
        {
            try
            {
                var client = new HttpClient();
                var endpoint = new Uri("http://20.214.249.72/api/CourseAssign");
                var newDepartmentJson = JsonConvert.SerializeObject(courseAssign);
                var payload = new StringContent(newDepartmentJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, payload);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("POST success ");
                    //Console.WriteLine("Status Code: " + response.StatusCode);
                    //Console.WriteLine("Header: " + response.Headers);
                    //Console.WriteLine("Respone: " + response.Content);
                    //Console.WriteLine("Content Respone: " + responseContent);
                    return true;
                }
                //Console.WriteLine("POST fail");
                return false;
            }
            catch (Exception ex)
            {

                //Console.WriteLine("Error at POST: " + ex.Message);
                return false;
            }
        }

        public static async Task<bool> CreateListCourseAssignAsync(string scheduleId, List<CourseAssign> courseAssigns)
        {
            try
            {
                var client = new HttpClient();
                var endpoint = new Uri("http://20.214.249.72/api/CourseAssign/AddListCourseAssign/"+ scheduleId);
                var newDepartmentJson = JsonConvert.SerializeObject(courseAssigns);
                var payload = new StringContent(newDepartmentJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, payload);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("POST success ");
                    //Console.WriteLine("Status Code: " + response.StatusCode);
                    //Console.WriteLine("Header: " + response.Headers);
                    //Console.WriteLine("Respone: " + response.Content);
                    //Console.WriteLine("Content Respone: " + responseContent);
                    return true;
                }
                //Console.WriteLine("POST fail");
                return false;
            }
            catch (Exception ex)
            {

                //Console.WriteLine("Error at POST: " + ex.Message);
                return false;
            }
        }
    }
}
