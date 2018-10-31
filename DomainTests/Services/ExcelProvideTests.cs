using System;
using System.Collections.Generic;
using System.IO;
using Armin.Suitsupply.Domain.Entities;
using Armin.Suitsupply.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClosedXML.Excel;

namespace DomainTests.Services
{
    [TestClass]
    public class ExcelProvideTests
    {
        [TestMethod]
        public void TestCreateExcel()
        {
            ExcelProvider<Product> excel = new ExcelProvider<Product>();

            // Empty list test
            var mem = excel.CreateExcel(new List<Product>());
            ValidateExcel(mem, true);

            // Normal list
            var products2 = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Test",
                    LastUpdate = DateTime.Now,
                    Photo = "test.photo",
                    Price = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Test2",
                    LastUpdate = DateTime.Now,
                    Photo = "test2.photo",
                    Price = 2
                }
            };
            var mem2 = excel.CreateExcel(products2);

            // Different list
            var sheet = ValidateExcel(mem2);
            Assert.AreEqual(sheet.Cell(2, 1).Value , products2[0].Name );

            var testList = new List<TestClass>
            {
                new TestClass
                {
                    Byte = 2,
                    Int = 3,
                    Long = 4
                }
            };
            ExcelProvider<TestClass> excelTestClass = new ExcelProvider<TestClass>();
            var mem3 = excelTestClass.CreateExcel(testList);
            var sheet3 = ValidateExcel(mem3);
            Assert.AreEqual(Convert.ToInt32(sheet3.Cell(2, 1).Value), testList[0].Int);

        }

        private IXLWorksheet ValidateExcel(MemoryStream stream, bool emptySheet = false)
        {
            XLWorkbook workbook;
            try
            {
                workbook = new XLWorkbook(stream);

            }
            catch (Exception ex)
            {
                Assert.Fail("Reading stream problem: " + ex.Message);
                return null;
            }

            if (emptySheet) return null;

            try
            {
                var sheet = workbook.Worksheets.Worksheet(1);
                Assert.IsNotNull(sheet.Row(1).Cell(1).Value);
                return sheet;

            }
            catch (Exception ex)
            {
                Assert.Fail("Reading worksheet problem: " + ex.Message);
            }

            return null;
        }

        public class TestClass
        {
            public int Int { get; set; }
            public long Long { get; set; }
            public byte Byte { get; set; }

        }
    }
}
