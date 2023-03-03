using FPTULecturerScheduler.Entity;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.DAO
{
    public class CourseGroupItemDAO
    {
        public static List<CourseGroupItem> getListCourseGroupItemFromExcel(string fileName)
        {
            List<CourseGroupItem> CourseGroupItemList = new List<CourseGroupItem>();
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
                        string courseGroupItemID = workSheet.Cells[i, j++].Value.ToString();
                        string lecturerCourseGroupID = workSheet.Cells[i, j++].Value.ToString();
                        string courseID = workSheet.Cells[i, j++].Value.ToString();
                        string priority = workSheet.Cells[i, j++].Value.ToString();
                        string status = workSheet.Cells[i, j++].Value.ToString();

                        // lấy ra cột ngày sinh tương ứng giá trị tại vị trí [i, 2]. i lần đầu là 2
                        // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
                        // lấy ra giá trị ngày tháng và ép kiểu thành DateTime                      
                        //var birthdayTemp = workSheet.Cells[i, j++].Value;

                        CourseGroupItem CourseGroupItem = new CourseGroupItem(courseGroupItemID, lecturerCourseGroupID, courseID, Int32.Parse(priority), Int32.Parse(status));
                        /*                         
                        string slotTypeID, string timeStart, string timeEnd, int slotNumber, int status
                        Đừng lười biến mà dùng đoạn code này sẽ gây ra lỗi nếu giá trị value không thỏa kiểu DateTime

                        DateTime birthday = (DateTime)workSheet.Cells[i, j++].Value;

                         */




                        // add course vào danh sách courseList
                        CourseGroupItemList.Add(CourseGroupItem);

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

            return CourseGroupItemList;

        }

        public static List<CourseGroupItem> ReadDataJsonCourseGroupItem(string filePath)
        {
            List<CourseGroupItem> courseGroupItems = new List<CourseGroupItem>();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var obj = sr.ReadToEnd();
                courseGroupItems = JsonConvert.DeserializeObject<List<CourseGroupItem>>(obj);

            }

            return courseGroupItems;
        }

        //public void WriteDataJsonCourseGroupItem(List<CourseGroupItem> courseGroupItems)
        //{
        //    try
        //    {
        //        if (courseGroupItems != null)
        //        {
        //            using (StreamWriter sw = File.CreateText(filePathOut + fileNameCourseGroupItems))
        //            {
        //                var courseGroupItemsJson = JsonConvert.SerializeObject(courseGroupItems);
        //                sw.WriteLine(courseGroupItemsJson);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}
    }
}
