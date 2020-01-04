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
using System.IO;
using Dapper.FastCrud;
using System.Threading.Tasks;

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


        public ActionResult Info()
        {
            return View();
        }
        public ActionResult Cart()
        {
            setReturnUrlSession("/Product/Cart");
            return View();
        }

        public ActionResult Get(int id)
        {
            return Json(new ResponseInfo(success: true, data: _productRepository.GetItem(id)), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllCategories()
        {
            var items = _categoryRepository.GetAllProductCategories();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllProducts()
        {
            var product = _productRepository.GetItems();
            return Json(new ResponseInfo(success: true, data: product), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductsByCategoryId(int catId = 0)
        {
            var condtion = "(1=1)";
            if (catId > 0)
                condtion = $"{Sql.Table<Product>()}.{nameof(Product.cat_id)}={catId}";
            return Json(new ResponseInfo(success: true, data: _productRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelatedProducts(int productId, int catId = 0)
        {
            var condtion = "(1=1)";
            if (catId > 0)
                condtion = $"{Sql.Table<Product>()}.{nameof(Product.cat_id)}={catId} AND {Sql.Table<Product>()}.{nameof(Product.id)}<>{productId}";
            return Json(new ResponseInfo(success: true, data: _productRepository.GetItems(condtion).Take(3)), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCategory(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    session.Query("Delete from cat_products where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete category fail!"), JsonRequestBehavior.AllowGet);
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
            ViewBag.Product = _productRepository.GetItems();
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateShoppingCart(OrderDetail detail)
        {
            if (detail != null && detail.product_id > 0)
            {

                if (Session["ShoppingCart"] == null)
                {
                    Session["ShoppingCart"] = new List<OrderDetail> { BindProductToOrderDetail(detail) };
                }
                else
                {
                    var existItems = (List<OrderDetail>)Session["ShoppingCart"];
                    var isExist = false;
                    foreach (var item in existItems)
                    {
                        if (item.product_id == detail.product_id)
                        {
                            item.quantity += detail.quantity;
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                        existItems.Add(detail);
                    BindProductToOrderDetails(ref existItems);
                    Session["ShoppingCart"] = existItems;
                }

            }
            return Json(new ResponseInfo(true, data: Session["ShoppingCart"]), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> saveOrderDetails(List<OrderDetail> orderDetails, int userId, string message, string ship_address, int typeId = 1)
        {
            using (var session = GetSession())
            {
                try
                {

                    var order = new Order
                    {
                        user_id = userId,
                        message = message,
                        order_date = DateTime.Now,
                        ship_address = ship_address,
                        required_date = DateTime.Now.AddDays(7),
                    };
                    await session.InsertAsync(order);
                    foreach (var orderDetail in orderDetails)
                    {
                        var item = new OrderDetail
                        {
                            order_id = order.id,
                            product_id = orderDetail.product.id,
                            quantity = orderDetail.quantity,
                            discount = orderDetail.discount
                        };
                        await session.InsertAsync(item);
                    }
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception err)
                {
                    return Json(new ResponseInfo(false), JsonRequestBehavior.AllowGet);
                }

            }
        }

        public ActionResult RemoveProductFromCart(int id)
        {
            if (id > 0)
            {
                var items = (List<OrderDetail>)Session["ShoppingCart"];
                Session["ShoppingCart"] = items.Where(x => x.product_id != id).ToList();
            }
            return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
        }
        //Creat or Update Product table
        [HttpPost]
        public ActionResult CreateOrUpdateProduct(Product entity, IEnumerable<HttpPostedFileBase> files)
        {
            using (var session = GetSession())
            {
                try
                {
                    if (!_productRepository.IsProductExist(entity))
                    {
                        using (var uow = session.UnitOfWork())
                        { _productRepository.SaveOrUpdate(entity, uow); }


                        if (files != null && files.Count() > 0)
                        {
                            DeleteFiles(entity.id);
                            var directory = Server.MapPath("~/Content/uploads/products");
                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);
                            foreach (var file in files)
                            {
                                string fileName = Path.GetFileName(string.Format("{0}{1}", DateTime.Now.Ticks.GetHashCode().ToString("x"), Path.GetExtension(file.FileName)));
                                file.SaveAs(directory + "/" + fileName);
                                session.Insert(new ProductImage
                                {
                                    original_name = file.FileName,
                                    path = "/Content/uploads/products/" + fileName,
                                    product_id = entity.id
                                });
                            }

                        }
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
            using (var session = GetSession())
            {
                try
                {
                    DeleteFiles(id);
                    session.Query("Delete from products where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete product fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }
        private void DeleteFiles(int productId)
        {
            using (var session = GetSession())
            {
                var existImages = session.Find<ProductImage>(stm => stm.Where($"{nameof(ProductImage.product_id)}={productId}"));
                if (existImages.Count() > 0)
                {
                    foreach (var image in existImages)
                    {
                        var file = Server.MapPath(image.path);
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
                session.Query("Delete from product_images where product_id=" + productId);
            }
        }


        private void BindProductToOrderDetails(ref List<OrderDetail> details)
        {
            foreach (var item in details)
                BindProductToOrderDetail(item);
        }
        private OrderDetail BindProductToOrderDetail(OrderDetail detail)
        {
            if (detail.product == null)
                detail.product = _productRepository.GetItem(detail.product_id);
            return detail;
        }
    }
}