using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using ClosedXML.Excel;


namespace Armin.Suitsupply.Domain.Services
{
    /**
     * Generic excel maker from a list of data by given type. In this file column titles will be properties name.
     * TODO: Read properties description and put them for colum names
     */
    public class ExcelProvider<T>
    {
        public static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new[]
                {
                    typeof(Enum),
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                IsSimpleType(type.GetGenericArguments()[0]);
        }

        public MemoryStream CreateExcel(List<T> data)
        {
            XLWorkbook workbook = new XLWorkbook();

            var sheet = workbook.Worksheets.Add("Data");

            var type = typeof(T);

            var properties = type.GetProperties().Where(p => IsSimpleType(p.PropertyType));

            int colIndex = 1;
            foreach (var prop in properties)
            {
                sheet.Cell(1, colIndex++).Value = prop.Name;
            }


            for (int i = 0; i < data.Count; i++)
            {
                colIndex = 1;
                foreach (var prop in properties)
                {
                    sheet.Cell(i + 2, colIndex++).Value = prop.GetValue(data[i]);
                }
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }
    }
}