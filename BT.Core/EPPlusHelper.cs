using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BT.Core
{
    public static class EPPlusHelper
    {

        /// <summary>
        /// 导出下载
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="path">要保存的路径（Web）</param>
        /// <param name="list">数据源</param>
        /// <returns></returns>
        public static Task<PhysicalFileResult> DownExcel<T>(string path, IEnumerable<T> list)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                fileInfo = new FileInfo(path);
            }
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("信息");
                worksheet.Cells.LoadFromCollection(list, true);
                package.Save();
            }
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
            //application/vnd.ms-excel
            return Task.FromResult(new PhysicalFileResult(fileInfo.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
        }


        /// <summary>
        /// 上传excel返回list
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="formFile">上传的文件</param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> UploadExcel<T>(IFormFile formFile)
        {
            List<T> list = new List<T>();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", formFile.FileName);

            using (var stream = File.Create(path))
            {
                await formFile.CopyToAsync(stream);
            }
            FileInfo fileInfo = new FileInfo(path);
            Type type = typeof(T);
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                var sheeet = excelPackage.Workbook.Worksheets[1];
                var rowcount = sheeet.Dimension.Rows;
                var colcount = sheeet.Dimension.Columns;
                if (type.GetProperties().Length != colcount)
                {
                    return list;
                }
                for (int i = 2; i <= rowcount; i++)
                {
                    T t = (T)Activator.CreateInstance(type);
                    int j = 1;
                    foreach (var item in type.GetProperties())
                    {
                        if (j> colcount)
                        {
                            break;
                        }          
                        var excelValueType = sheeet.GetValue(i, j).GetType();
                        if (excelValueType==null)
                        {
                            item.SetValue(t, null);
                        }
                        else if (excelValueType == typeof(System.Int32))
                        {
                            item.SetValue(t, sheeet.GetValue<int>(i, j));
                        }
                        else if (excelValueType == typeof(System.String))
                        {
                            item.SetValue(t, sheeet.GetValue<string>(i, j));
                        }
                        else if (excelValueType == typeof(System.DateTime))
                        {
                            item.SetValue(t, sheeet.GetValue<DateTime>(i, j));
                        }
                        else if (excelValueType == typeof(System.Boolean))
                        {
                            item.SetValue(t, sheeet.GetValue<bool>(i, j));
                        }
                        else if (excelValueType == typeof(System.Double))
                        {
                            var value = sheeet.GetValue<double>(i, j);
                            int intValue=0;
                            try
                            {
                                if (int.TryParse(value.ToString(), out intValue))
                                {
                                    item.SetValue(t, intValue);
                                }
                                else
                                {
                                    item.SetValue(t, value);
                                }
                            }
                            catch (Exception)
                            {
                                DateTime dateTime;
                                if (DateTime.TryParse(DateTime.FromOADate(value).ToString(), out dateTime))
                                {
                                   
                                    try
                                    {
                                        item.SetValue(t, dateTime);
                                    }
                                    catch (Exception)
                                    {

                                        item.SetValue(t, dateTime.ToString());
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        item.SetValue(t, value.ToString());
                                    }
                                    catch (Exception)
                                    {

                                        item.SetValue(t, dateTime.ToString());
                                    }
                                }
                                
                            }
                           
                        }
                        else
                        {
                            item.SetValue(t, sheeet.GetValue(i, j));
                        }

                        j++;
                    }
                    list.Add(t);
                }
            }
            return list;

        }
    }
}
