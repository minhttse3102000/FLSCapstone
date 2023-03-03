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
    public class LecturerSlotConfigDAO
    {
        public static List<LecturerSlotConfig> getSlotConfigFromExcel(string fileName)
        {
            List<LecturerSlotConfig> slotConfigList = new List<LecturerSlotConfig>();
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
                        string lecturerSlotConfigID = workSheet.Cells[i, j++].Value.ToString();
                        string slotTypeID = workSheet.Cells[i, j++].Value.ToString();
                        string lecturerID = workSheet.Cells[i, j++].Value.ToString();
                        string PreferenceLevel = workSheet.Cells[i, j++].Value.ToString();
                        string IsEnable = workSheet.Cells[i, j++].Value.ToString();
                        string semesterID = workSheet.Cells[i, j++].Value.ToString();
                        string status = workSheet.Cells[i, j++].Value.ToString();
                        // lấy ra cột ngày sinh tương ứng giá trị tại vị trí [i, 2]. i lần đầu là 2
                        // tăng j lên 1 đơn vị sau khi thực hiện xong câu lệnh
                        // lấy ra giá trị ngày tháng và ép kiểu thành DateTime                      
                        //var birthdayTemp = workSheet.Cells[i, j++].Value;

                        LecturerSlotConfig slotConfig = new LecturerSlotConfig(lecturerSlotConfigID, semesterID, lecturerID, slotTypeID, Int32.Parse(PreferenceLevel), Int32.Parse(IsEnable), Int32.Parse(status));
                        /*                         
                        LecturerSlotConfig(string Id, string semesterID, string lecturerID, string slotTypeID, int preferenceLevel, int isEnable, int status)
                        Đừng lười biến mà dùng đoạn code này sẽ gây ra lỗi nếu giá trị value không thỏa kiểu DateTime

                        DateTime birthday = (DateTime)workSheet.Cells[i, j++].Value;

                         */



                        // add course vào danh sách courseList
                        slotConfigList.Add(slotConfig);

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

            return slotConfigList;

        }

        public static List<LecturerSlotConfig> ReadDataJsonLecturerSlotConfig(string filePath)
        {
            List<LecturerSlotConfig> lecturerSlotConfigs = new List<LecturerSlotConfig>();
            using (StreamReader sr = File.OpenText(filePath))
            {
                var obj = sr.ReadToEnd();
                lecturerSlotConfigs = JsonConvert.DeserializeObject<List<LecturerSlotConfig>>(obj);

            }


            return lecturerSlotConfigs;
        }

        public static void WriteData(List<LecturerSlotConfig> lecturerSlotConfigs)
        {
            try
            {
                if (lecturerSlotConfigs != null)
                {
                    using (StreamWriter sw = File.CreateText(@"D:\LecturerSlotConfigJS.json"))
                    {
                        var lecturerSlotConfigsJson = JsonConvert.SerializeObject(lecturerSlotConfigs);
                        sw.WriteLine(lecturerSlotConfigsJson);
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
