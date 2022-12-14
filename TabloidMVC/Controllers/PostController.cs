using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        public IActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            return View(post);
        }

        [HttpPost]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(post);
            }
        }


        public IActionResult UserIndex()
        {
            var id = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllPostsByUserId(id);
            return View(posts);
        }

            public IActionResult Edit(int id)
            {
                var viewModel = new PostCreateViewModel();
                viewModel.Post = _postRepository.GetPublishedPostById(id);
                viewModel.CategoryOptions = _categoryRepository.GetAll().ToList();

                if (viewModel.Post == null)
                {
                    return NotFound();
                }
                return View(viewModel);
            }

            [HttpPost]
            public IActionResult Edit(PostCreateViewModel viewModel, int id)
            {
                try
                {
                    viewModel.Post.Id = id;
                    viewModel.Post.CreateDateTime = DateAndTime.Now;
                    viewModel.Post.IsApproved = true;
                    viewModel.Post.UserProfileId = GetCurrentUserProfileId();

                    _postRepository.UpdatePost(viewModel.Post);
                    return RedirectToAction("Details", new { id = viewModel.Post.Id });
                }
                catch
                {
                    viewModel.CategoryOptions = _categoryRepository.GetAll();
                    return View(viewModel);
                }
            }
        
    }
}
