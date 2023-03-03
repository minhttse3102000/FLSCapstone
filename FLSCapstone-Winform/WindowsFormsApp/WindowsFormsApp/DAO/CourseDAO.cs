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
    public class CourseDAO
    {
        public static List<Course> getListCourseFromExcel(string fileName)
        {
            List<Course> courseList = new List<Course>();
            try
            {
                FileInfo file = new FileInfo(fileName);

                // mở file excel
                var package = new ExcelPackage(new FileInfo(fileName));
                // lấy ra sheet đầu tiên để thao tác
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];

                // duyệt tuần tự từ dòng thứ 2 đến dòng cuối cùng của file. lưu ý file excel bắt đầu từ số 1 không phải số 0
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    try
                    {
                        // biến j biểu thị cho một column trong file
                        int j = 1;

                        // lấy ra cột họ tên tương ứng giá trị tại vị trí [i, 1]. i lần đầu là 2
                        // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
                        string courseID = workSheet.Cells[i, j++].Value.ToString();
                        string subjectID = workSheet.Cells[i, j++].Value.ToString();
                        string semesterID = workSheet.Cells[i, j++].Value.ToString();
                        string slotTypeID = workSheet.Cells[i, j++].Value.ToString();
                        string description = workSheet.Cells[i, j++].Value.ToString();
                        string slotAmount = workSheet.Cells[i, j++].Value.ToString();
                        string status = workSheet.Cells[i, j++].Value.ToString();
                        // lấy ra cột ngày sinh tương ứng giá trị tại vị trí [i, 2]. i lần đầu là 2
                        // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
                        // lấy ra giá trị ngày tháng và ép kiểu thành DateTime                      
                        //var birthdayTemp = workSheet.Cells[i, j++].Value;

                        Course course = new Course(courseID, subjectID, semesterID, slotTypeID, description, Int32.Parse(slotAmount), Int32.Parse(status));
                        /*                         

                        Đừng lười biến mà dùng đoạn code này sẽ gây ra lỗi nếu giá trị value không thỏa kiểu DateTime

                        DateTime birthday = (DateTime)workSheet.Cells[i, j++].Value;

                         */



                        // add course vào danh sách courseList
                        courseList.Add(course);

                    }
                    catch (Exception exe)
                    {
                        Console.WriteLine(exe.Message);

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return courseList;

        }

        public static List<Course> ReadDataJsonCourse(string filePath)
        {
            List<Course> courses = new List<Course>();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var obj = sr.ReadToEnd();
                courses = JsonConvert.DeserializeObject<List<Course>>(obj);

            }


            return courses;
        }

        //public void WriteDataJsonCourse(List<Course> courses)
        //{
        //    try
        //    {
        //        if (courses != null)
        //        {
        //            using (StreamWriter sw = File.CreateText(filePathOut + fileNameCourse))
        //            {
        //                var coursesJson = JsonConvert.SerializeObject(courses);
        //                sw.WriteLine(coursesJson);
        //            }


        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        public static async Task<bool> UpdateCourseAsync(Course course)
        {
            try
            {
          
                var client = new HttpClient();
                var endpoint = new Uri("http://20.214.249.72/api/Course/" + course.ID);
                var newDepartmentJson = JsonConvert.SerializeObject(course);
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

        public static async Task<List<Course>> GetCourseAsync(string semesterId)
        {
            List<Course> courses;
            try
            {
                var client = new HttpClient();
                var endpoint = new Uri("http://20.214.249.72/api/Course?SemesterId=" + semesterId + "&Status=1&pageIndex=1&pageSize=10000");
                HttpResponseMessage response = await client.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Get success ");
                    //Console.WriteLine("Status Code: " + response.StatusCode);
                    //Console.WriteLine("Header: " + response.Headers);
                    //Console.WriteLine("Respone: " + response.Content);

                    //Console.WriteLine("Content Respone: " + responseContent);
                    courses = JsonConvert.DeserializeObject<List<Course>>(responseContent);
                    return courses;
                }
                Console.WriteLine("Get fail");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at Get: " + ex.Message);
                return null;
            }
        }

    }


}
