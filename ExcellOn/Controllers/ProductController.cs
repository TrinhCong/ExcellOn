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
        private readonly ProductRepository  _productRepository;
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
    
        public ActionResult GetAllProducts()
        {
            var product = _productRepository.GetAllProducts();
            return Json(new ResponseInfo(success: true, data: product), JsonRequestBehavior.AllowGet);
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
            ViewBag.ProductCategories = _categoryRepository.GetAllProductCategories();
            ViewBag.Product = _productRepository.GetAllProducts();
            return View("Index");
        }
        public ActionResult Create()
        {
            return View();
        }

        //Creat or Update Product table
        [HttpPost]
        public ActionResult CreateOrUpdateProduct(Product entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_productRepository.IsProductExist(entity))
                        {
                     
                            _productRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update product successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update product fail!"), JsonRequestBehavior.AllowGet);
                    }
                }
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
        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            using(var session = GetSession())
            {
                try
                {
                    session.Query("Delete from products where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete product fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }

    }
}