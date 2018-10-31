using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Suitsupply.Domain.Data;
using Armin.Suitsupply.Domain.Entities;
using Armin.Suitsupply.Domain.Services;
using Armin.Suitsupply.Domain.Stores;
using Armin.Suitsupply.Web.Filters;
using Armin.Suitsupply.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Armin.Suitsupply.Web.Controllers
{
    // https://www.c-sharpcorner.com/article/creating-crud-api-in-asp-net-core-2-0/

    [Route("api/v1/products")]
    [ApiController]
    [ValidateModelState]
    public class ProductsController : ControllerBase
    {
        private readonly IProductStore _store;
        readonly FileSystem _fs = new FileSystem();

        public ProductsController(IProductStore store)
        {
            _store = store;
        }

        // GET api/v1/products/search/{name?}
        [HttpGet("search/{name?}", Name = "SearchProduct")]
        public async Task<ActionResult<IEnumerable<ProductOutputModel>>> Search(string name)
        {
            return await SearchProducts(name);
            ;
        }

        // GET api/v1/products/excel/{name?}
        [HttpGet("excel/{name?}", Name = "ExcelProduct")]
        public async Task<IActionResult> SearchExcel(string name)
        {
            var products = await SearchProducts(name);
            ExcelProvider<ProductOutputModel> excel = new ExcelProvider<ProductOutputModel>();
            var productsExcelStream = excel.CreateExcel(products);
            return File(productsExcelStream, "application/ms-excel", "Catalog.xlsx");
        }

        private async Task<List<ProductOutputModel>> SearchProducts(string name)
        {
            var products = await _store.Search(name);
            List<ProductOutputModel> productsOutput = products.Select(ProductOutputModel.FromEntity).ToList();
            return productsOutput;
        }


        // GET api/v1/products/{id}
        /// <summary>
        /// Retreive full data and model of given product
        /// </summary>
        /// <param name="id">Product Id as it appear on data signiture</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ViewProduct")]
        public ActionResult<ProductOutputModel> Get(long id)
        {
            Product product = _store.GetById(id);
            if (null == product) return NoContent();

            return ProductOutputModel.FromEntity(product);
        }

        // POST api/v1/products
        [HttpPost(Name = "CreateProduct")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromForm] ProductInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            var product = inputModel.ToEntity();

            if (null != inputModel.Photo)
            {
                string imagePath = await _fs.Create(inputModel.Photo.OpenReadStream(),
                    FileTypes.ProductPhoto,
                    _fs.CreateDefaultFileName(inputModel.Photo.FileName));

                product.Photo = imagePath;
            }

            product = _store.Create(product);

            var outputModel = ProductOutputModel.FromEntity(product);
            return CreatedAtRoute("ViewProduct",
                new {id = outputModel.Id}, outputModel);
        }

        // PUT api/v1/products
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(long id, [FromForm] ProductInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            var product = _store.GetById(id);
            if (null == product) return NotFound();

            product.Name = inputModel.Name;
            product.Price = inputModel.Price;

            if (null != inputModel.Photo)
            {
                if (_fs.Exists(product.Photo))
                {
                    _fs.Delete(product.Photo);
                }

                string imagePath = await _fs.Create(inputModel.Photo.OpenReadStream(),
                    FileTypes.ProductPhoto,
                    _fs.CreateDefaultFileName(inputModel.Photo.FileName));

                product.Photo = imagePath;
            }


            _store.Update(product);

            return NoContent();
        }


        // DELETE api/v1/products/{id}
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(204)]
        public IActionResult Delete(int id)
        {
            var product = _store.GetById(id);
            if (null == product) return NotFound();

            _store.Delete(product);

            return NoContent();
        }
    }
}