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
using ExcellOn.ViewModels;
using Dapper;

namespace ExcellOn.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository<CategoryProduct> _categoryRepository;

        public ProductController(
                                IDbFactory dbFactory, 
                                ProductRepository productRepository,
                                CategoryRepository<CategoryProduct> categoryRepository
                                ) : base(dbFactory)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }



        public ActionResult GetAllCategories()
        {
            var items = _categoryRepository.GetAllProductCategories();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteCategory(int id)
        { 
            using (var session = GetSession())
            {
                try
                {
                    session.Query("Delete from cat_products where id="+id);
                    return Json(new ResponseInfo(true),JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete user fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }


        [HttpPost]
        public ActionResult CreateOrUpdateCategory(CategoryProduct entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_categoryRepository.IsCategoryProductExist(entity))
                        {
                            _categoryRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update category successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update category fail!"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        // GET: Product

        public ActionResult Index()
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
                var editItem = _productRepository.GetKey(id, session);

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