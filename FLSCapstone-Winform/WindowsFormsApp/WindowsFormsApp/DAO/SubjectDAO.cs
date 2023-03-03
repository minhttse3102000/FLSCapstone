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
    public class SubjectDAO
    {
        public static List<Subject> getListSubjectFromExcel(string fileName)
        {
            List<Subject> subjectList = new List<Subject>();
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
                        string subjectID = workSheet.Cells[i, j++].Value.ToString();
                        string subjectName = workSheet.Cells[i, j++].Value.ToString();
                        string departmentID = workSheet.Cells[i, j++].Value.ToString();
                        string description = workSheet.Cells[i, j++].Value.ToString();
                        string status = workSheet.Cells[i, j++].Value.ToString();
                        // lấy ra cột ngày sinh tương ứng giá trị tại vị trí [i, 2]. i lần đầu là 2
                        // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
                        // lấy ra giá trị ngày tháng và ép kiểu thành DateTime                      
                        //var birthdayTemp = workSheet.Cells[i, j++].Value;

                        Subject subject = new Subject(subjectID, subjectName, description, Int32.Parse(status), departmentID);
                        /*                         

                        Đừng lười biến mà dùng đoạn code này sẽ gây ra lỗi nếu giá trị value không thỏa kiểu DateTime

                        DateTime birthday = (DateTime)workSheet.Cells[i, j++].Value;

                         */



                        // add course vào danh sách courseList
                        subjectList.Add(subject);

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

            return subjectList;

        }

        public static List<Subject> ReadDataJsonSubject(string filePath)
        {
            List<Subject> subjects = new List<Subject>();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var obj = sr.ReadToEnd();
                subjects = JsonConvert.DeserializeObject<List<Subject>>(obj);

            }

            return subjects;
        }

        public void WriteData(List<Subject> subjects)
        {
            try
            {
                if (subjects != null)
                {
                    using (StreamWriter sw = File.CreateText(@"D:\SubjectJS.json"))
                    {
                        var subjectsJson = JsonConvert.SerializeObject(subjects);
                        sw.WriteLine(subjectsJson);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }


}
