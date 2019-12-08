using ExcellOn.Models;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.UnitOfWork.Interfaces;
using System.Web.Mvc;
using PagedList;

namespace ExcellOn.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly CategoryRepository<CategoryService> _categoryServiceRepository;

        public CategoryController(
                                    IDbFactory dbFactory,
                                    CategoryRepository<CategoryService> categoryServiceRepository
                                ) : base(dbFactory)
        {
            _categoryServiceRepository = categoryServiceRepository;

        }

        public ActionResult Index(string Search, int? page)

        {

            using (var session = _dbFactory.Create<IAppSession>())
            {
                return View();
            }
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category newItem)
        {

            return View(newItem);
        }
        public ActionResult Get(int id)
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Category category)
        {


            return View();
        }
        public ActionResult Delete(int id)
        {
            return View();
        }


    }
}