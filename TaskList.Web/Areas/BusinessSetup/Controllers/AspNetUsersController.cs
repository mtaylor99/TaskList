using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.Web.Attribute.RequiresRole;
using TaskList.Web.Models.ViewModels;

namespace TaskList.Web.Areas.BusinessSetup.Controllers
{
    [RequiresRole(Roles.Administrator)]
    public class AspNetUsersController : Controller
    {
        private ILogger<AspNetUsersController> logger;
        private IIdentityRepository repository;
        Identity identity;
        public int PageSize = 4;

        public AspNetUsersController(ILogger<AspNetUsersController> log, IIdentityRepository repo)
        {
            logger = log;
            repository = repo;
            identity = new Identity(logger, repository);

            logger.LogInformation("Asp Net Users Controller");
        }

        public ViewResult List(int page = 1)
        {
            var aspNetUsersListViewModel = new AspNetUsersListViewModel();

            aspNetUsersListViewModel.AspNetUsers = identity.GetAspNetUsers(page, PageSize);

            aspNetUsersListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = identity.GetAspNetUsersCount()
            };

            return View(aspNetUsersListViewModel);
        }

        public ViewResult Create()
        {
            var aspNetUsersViewModel = new AspNetUsersViewModel();

            foreach (var group in identity.GetAspNetGroups().ToList())
            {
                var listItem = new SelectListItem()
                {
                    Text = group.Name,
                    Value = group.GroupId.ToString(),
                    Selected = false
                };
                aspNetUsersViewModel.AspNetGroupsList.Add(listItem);
            }

            return View(aspNetUsersViewModel);
        }

        [HttpPost]
        public IActionResult Create(AspNetUsersViewModel aspNetUsersViewModel, params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var aspNetUser = new AspNetUser();
                aspNetUser.UserName = aspNetUsersViewModel.UserName;
                aspNetUser.FullName = aspNetUsersViewModel.FullName;
                aspNetUser.Email = aspNetUsersViewModel.Email;

                var userID = identity.AddAspNetUser(aspNetUser, aspNetUsersViewModel.Password, selectedGroups);

                if (userID != null)
                {
                    TempData["message"] = $"Asp Net User has been created.";
                }
                else
                {
                    TempData["message"] = $"Asp Net User was not created.";
                }

                return RedirectToAction("List");
            }
            else
            {
                // there is something wrong with the data values
                foreach (var group in identity.GetAspNetGroups().ToList())
                {
                    var listItem = new SelectListItem()
                    {
                        Text = group.Name,
                        Value = group.GroupId.ToString(),
                        Selected = selectedGroups.Contains(group.GroupId.ToString())
                    };
                    aspNetUsersViewModel.AspNetGroupsList.Add(listItem);
                }

                return View(aspNetUsersViewModel);
            }
        }

        public ViewResult Edit(string id)
        {
            var aspNetUsersViewModel = new AspNetUsersViewModel();

            var aspNetUser = identity.GetAspNetUser(id);

            if (aspNetUser != null)
            {
                aspNetUsersViewModel.Id = aspNetUser.Id;
                aspNetUsersViewModel.UserName = aspNetUser.UserName;
                aspNetUsersViewModel.FullName = aspNetUser.FullName;
                aspNetUsersViewModel.Email = aspNetUser.Email;
            }

            var userGroups = identity.GetUserGroups(id);

            foreach (var group in identity.GetAspNetGroups().ToList())
            {
                var listItem = new SelectListItem()
                {
                    Text = group.Name,
                    Value = group.GroupId.ToString(),
                    Selected = userGroups.Contains(group.GroupId.ToString())
                };
                aspNetUsersViewModel.AspNetGroupsList.Add(listItem);
            }

            return View(aspNetUsersViewModel);
        }

        [HttpPost]
        public IActionResult Edit(AspNetUsersViewModel aspNetUsersViewModel, params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var aspNetUser = new AspNetUser();
                aspNetUser.Id = aspNetUsersViewModel.Id;
                aspNetUser.UserName = aspNetUsersViewModel.UserName;
                aspNetUser.FullName = aspNetUsersViewModel.FullName;
                aspNetUser.Email = aspNetUsersViewModel.Email;

                var userID = identity.EditAspNetUser(aspNetUser, aspNetUsersViewModel.Password, selectedGroups);

                if (userID != null)
                {
                    TempData["message"] = $"Asp Net User has been edtied.";
                }
                else
                {
                    TempData["message"] = $"Asp Net User was not edited.";
                }

                return RedirectToAction("List");
            }
            else
            {
                // there is something wrong with the data values
                var userGroups = identity.GetUserGroups(aspNetUsersViewModel.Id);

                foreach (var group in identity.GetAspNetGroups().ToList())
                {
                    var listItem = new SelectListItem()
                    {
                        Text = group.Name,
                        Value = group.GroupId.ToString(),
                        Selected = userGroups.Contains(group.GroupId.ToString())
                    };
                    aspNetUsersViewModel.AspNetGroupsList.Add(listItem);
                }

                return View(aspNetUsersViewModel);
            }
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                identity.DeleteAspNetUser(id);

                TempData["message"] = $"Asp Net User has been deleted.";

                return RedirectToAction("List");
            }
            else
            {
                // there is something wrong with the data values
                return View();
            }
        }
    }
}