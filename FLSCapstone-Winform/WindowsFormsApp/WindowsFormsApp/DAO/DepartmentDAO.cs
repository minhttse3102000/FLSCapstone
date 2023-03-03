using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPTULecturerScheduler.Entity;
using Newtonsoft.Json;

namespace WindowsFormsApp.DAO
{
    public class DepartmentDAO
    {
        public static List<Department> getListDepartmentFromExcel(string fileName)
        {
            List<Department> departmentList = new List<Department>();
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
                        string ID = workSheet.Cells[i, j++].Value.ToString();
                        string DepartmentName = workSheet.Cells[i, j++].Value.ToString();
                        string status = workSheet.Cells[i, j++].Value.ToString();


                        // lấy ra cột ngày sinh tương ứng giá trị tại vị trí [i, 2]. i lần đầu là 2
                        // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
                        // lấy ra giá trị ngày tháng và ép kiểu thành DateTime                      
                        //var birthdayTemp = workSheet.Cells[i, j++].Value;

                        Department department = new Department(ID, DepartmentName, Int32.Parse(status));
                        /*                         
                        Department(string Id, string departmentName, int status)

                        Đừng lười biến mà dùng đoạn code này sẽ gây ra lỗi nếu giá trị value không thỏa kiểu DateTime

                        DateTime birthday = (DateTime)workSheet.Cells[i, j++].Value;

                         */

                        // add course vào danh sách courseList
                        departmentList.Add(department);

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

            return departmentList;

        }

        public static List<Department> ReadDataJsonDepartment(string filePath)
        {
            List<Department> departments = new List<Department>();
            //using (StreamReader sr = File.OpenText(@"C:\Users\Admin\Desktop\dataFromFap\driver\Department.json"))
            using (StreamReader sr = File.OpenText(filePath))
            {
                var obj = sr.ReadToEnd();
                departments = JsonConvert.DeserializeObject<List<Department>>(obj);

            }

            return departments;
        }

        //public void WriteDataJsonDepartment(List<Department> departments)
        //{
        //    try
        //    {
        //        if (departments != null)
        //        {
        //            using (StreamWriter sw = File.CreateText(filePathOut + fileNameDepartment))
        //            {
        //                var departmentsJson = JsonConvert.SerializeObject(departments);
        //                sw.WriteLine(departmentsJson);
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
