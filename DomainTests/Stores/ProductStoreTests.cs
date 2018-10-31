using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Armin.Suitsupply.Domain.Data;
using Armin.Suitsupply.Domain.Entities;
using Armin.Suitsupply.Domain.Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DomainTests.Stores
{
    [TestClass]
    public class ProductStoreTests : BaseTestUnit
    {
        private DomainDbContext _db;
        private ProductStore _store;

        [TestInitialize]
        public void Init()
        {
            _db = BuildDataContext();
            _store = new ProductStore(_db);
        }

        [TestMethod]
        public void CreateTest()
        {
            var prd = new Product
            {
                Name = "TestProductName-Long"
            };
            var preCount = _db.Products.Count();
            var prdAfter = _store.Create(prd);
            var postCount = _db.Products.Count();

            Assert.AreEqual(prd.Name, prdAfter.Name);
            Assert.AreEqual(preCount + 1, postCount);

            var prd1 = new Product
            {
                Name = "TestProductName-Short"
            };
            var prd2 = new Product
            {
                Name = "ProductName-Short"
            };
            _store.Create(prd1);
            _store.Create(prd2);
            var lastCount = _db.Products.Count();
            Assert.AreEqual(postCount + 2, lastCount);
        }

        [TestMethod]
        public void SearchTest()
        {
            var all = _db.Products.ToList();
            _db.RemoveRange(all);
            _db.SaveChanges();

            CreateTest();
            var task = _store.Search("TestProductName-Long");
            var result = task.Result;
            Assert.AreEqual(result.Count(), 1);

            task = _store.Search("TestProductName");
            result = task.Result;

            Assert.AreEqual(result.Count(), 2);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var firstProduct = _db.Products.First();
            firstProduct.Price += 100;
            DateTime now = DateTime.Now;
            _store.Update(firstProduct);

            var changedProduct = _db.Products.FirstOrDefault(p => p.Id == firstProduct.Id);
            Assert.IsNotNull(changedProduct);
            Assert.AreEqual(firstProduct.Price, changedProduct.Price);
            Assert.IsTrue(changedProduct.LastUpdate >= now);
        }

        [TestMethod]
        public void DeleteTest()
        {
            CreateTest();
            var preCount = _db.Products.Count();
            var firstProduct = _db.Products.First();
            _store.Delete(firstProduct);
            var postCount = _db.Products.Count();

            Assert.AreEqual(preCount - 1, postCount);

            var found = _db.Products.FirstOrDefault(p => p.Id == firstProduct.Id);
            Assert.IsNull(found);
        }
    }
}