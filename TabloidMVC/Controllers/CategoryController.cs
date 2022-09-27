using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;



namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            var cat = new Category();
            var Categories = _categoryRepository.GetAll();
            return View(cat);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {

                category.Name = category.Name;
                _categoryRepository.AddCategory(category);

                return RedirectToAction("Index", new { id = category.Id });
            }
            catch
            {
                return View();
            }
        }
    }
}

  
        