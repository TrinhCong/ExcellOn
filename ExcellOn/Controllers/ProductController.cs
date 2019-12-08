using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.UnitOfWork.Interfaces;
using ExcellOn.Models;
using PagedList;

namespace ExcellOn.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ProductRepository _productRepository;

        public ProductController(
                                IDbFactory dbFactory, 
                                ProductRepository productRepository
                                ) : base(dbFactory)
        {
            _productRepository = productRepository;

        }
        // GET: Product

        public ActionResult Index(string Search, Product product, string Discontinued, int? page , int SupplierID =0, int CategoryId = 0)
        {
                return View("Index");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                
                return View();
            }

        }
        public ActionResult Details(int id)
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                return View();
            }

        }
        public ActionResult Edit(int id)
        {
            using (var session = _dbFactory.Create<IAppSession>())

            {
                var editItem = this._productRepository.GetKey(id, session);

                if (editItem != null)
                {
                   return View(editItem);
                }
                else
                    return HttpNotFound();

            }

        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                return View();
            }

        }
        public ActionResult Delete(int id)
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}