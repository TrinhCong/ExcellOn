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
            if (Session["ShoppingCart"] == null)
                Session["ShoppingCart"] = new List<OrderDetail> { detail };
            else if (detail != null && detail.product_id > 0)
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
            return Json(new ResponseInfo(true, data: Session["ShoppingCart"]), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult updateShopingCartFromSession(int _userId)
        //{
        //    using (var session = GetSession())
        //    {
        //        var infos = (List<OrderDetail>)Session["ShoppingCart"];
        //        //Kiểm tra nếu user Chưa có giỏ hàng thì tạo mới 1 giỏ hàng
        //        var userId = _userId;
        //        var existItem = session
        //            .Find<Order>(stm => stm.Where($"{nameof(Order.user_id)}='{userId}'"))
        //            .FirstOrDefault();
        //        var cartId = 0;
        //        if (existItem == null)
        //        {
        //            var cart = new Order { user_id = userId };
        //            session.Insert(cart);
        //            cartId = cart.id;
        //        }
        //        else
        //            cartId = existItem.id;


        //        if (infos != null)
        //        {
        //            foreach (var info in infos)
        //            {
        //                //Trường hợp người dùng thêm sản phẩm vào giỏ hàng sau đó mới đăng nhập
        //                //Ta cần merge sản phẩm ở giỏ hàng vào sản phẩm trong database
        //                var existInfo = session.Find<OrderDetail>(stm =>
        //                        stm.Where(
        //                            $"{nameof(OrderDetail.order_id)}={info.order_id} AND {nameof(OrderDetail.product_id)}={info.product_id}"))
        //                    .FirstOrDefault();
        //                if (existInfo != null && info.id == 0)
        //                {
        //                    info.quantity += existInfo.quantity;
        //                    info.id = existInfo.id;
        //                }

        //                var item = info;
        //                if (item.id > 0)
        //                    session.Update(item);
        //                else
        //                {
        //                    session.Insert(item);
        //                    info.id = item.id;
        //                }
        //            }
        //            Session["ShoppingCart"] = null;
        //        }

        //        var orderDetails = session.Find<OrderDetail>(stm => stm
        //            .Where($"{Sql.Table<Order>()}.{nameof(Order.user_id)}='{userId}'")
        //            .Include<Order>()).ToList();

        //        BindProductToOrderDetails(ref orderDetails);
        //        return Json(new ResponseInfo(true, data: orderDetails), JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public ActionResult updateCartAnonymous(List<OrderDetail> infos)
        //{
        //    var cartInfos = new List<OrderDetail>();
        //    var cartBefores = (List<OrderDetail>)Session["ShoppingCart"];

        //    if (infos != null && infos.Count() > 0)
        //    {
        //        foreach (var info in infos)
        //        {
        //            if (cartBefores != null)
        //            {
        //                var exsist_items = cartBefores.Where(x => x.product.id == info.product.id);

        //                if (exsist_items.FirstOrDefault() != null)
        //                {
        //                    var item = exsist_items.FirstOrDefault();
        //                    item.quantity += info.quantity;
        //                }
        //                else
        //                    cartInfos.Add(info);
        //            }
        //            else
        //                cartInfos.Add(info);
        //        }
        //    }
        //    if (cartBefores != null)
        //    {
        //        BindProductToOrderDetails(ref cartBefores);
        //        cartInfos.AddRange(cartBefores);
        //    }

        //    Session["ShoppingCart"] = cartInfos;
        //    return Json(new ResponseInfo(true, data: cartInfos), JsonRequestBehavior.AllowGet);
        //}

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
        private void BindProductToOrderDetail(OrderDetail detail)
        {
            if (detail.product == null)
                detail.product = _productRepository.GetItem(detail.product_id);
        }
    }
}