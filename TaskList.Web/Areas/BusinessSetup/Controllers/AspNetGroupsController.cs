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
    public class AspNetGroupsController : Controller
    {
        private ILogger<AspNetGroupsController> logger;
        private IIdentityRepository repository;
        Identity identity;
        public int PageSize = 4;

        public AspNetGroupsController(ILogger<AspNetGroupsController> log, IIdentityRepository repo)
        {
            logger = log;
            repository = repo;
            identity = new Identity(logger, repository);

            logger.LogInformation("Asp Net Groups Controller");
        }

        public ViewResult List(int page = 1)
        {
            var aspNetGroupsListViewModel = new AspNetGroupsListViewModel();

            aspNetGroupsListViewModel.AspNetGroups = identity.GetAspNetGroups(page, PageSize);

            aspNetGroupsListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = identity.GetAspNetGroupsCount()
            };

            return View(aspNetGroupsListViewModel);
        }

        public ViewResult Create()
        {
            var aspNetGroupsViewModel = new AspNetGroupsViewModel();

            foreach (var role in identity.GetAspNetRoles().ToList())
            {
                var listItem = new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Id,
                    Selected = false
                };
                aspNetGroupsViewModel.AspNetRolesList.Add(listItem);
            }

            return View(aspNetGroupsViewModel);
        }

        [HttpPost]
        public IActionResult Create(AspNetGroupsViewModel aspNetGroupsViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var groupID = identity.AddGroup(aspNetGroupsViewModel.AspNetGroup, selectedRoles);

                if (groupID > 0)
                {
                    TempData["message"] = $"Asp Net Group has been created.";
                }
                else
                {
                    TempData["message"] = $"Asp Net Group was not created.";
                }

                return RedirectToAction("List");
            }
            else
            {
                foreach (var role in identity.GetAspNetRoles().ToList())
                {
                    var listItem = new SelectListItem()
                    {
                        Text = role.Name,
                        Value = role.Id,
                        Selected = selectedRoles.Contains(role.Id)
                    };
                    aspNetGroupsViewModel.AspNetRolesList.Add(listItem);
                }

                return View(aspNetGroupsViewModel);
            }
        }

        public ViewResult Edit(int id)
        {
            var aspNetGroupsViewModel = new AspNetGroupsViewModel();

            var groupRoles = identity.GetRolesInGroup(id);

            aspNetGroupsViewModel.AspNetGroup = identity.GetAspNetGroup(id);

            foreach (var role in identity.GetAspNetRoles().ToList())
            {
                var listItem = new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Id,
                    Selected = groupRoles.Contains(role.Id)
                };
                aspNetGroupsViewModel.AspNetRolesList.Add(listItem);
            }

            return View(aspNetGroupsViewModel);
        }

        [HttpPost]
        public IActionResult Edit(AspNetGroupsViewModel aspNetGroupsViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var groupID = identity.EditGroup(aspNetGroupsViewModel.AspNetGroup, selectedRoles);

                if (groupID > 0)
                {
                    TempData["message"] = $"Asp Net Group has been edtied.";
                }
                else
                {
                    TempData["message"] = $"Asp Net Group was not edited.";
                }

                return RedirectToAction("List");
            }
            else
            {
                var groupRoles = identity.GetRolesInGroup(aspNetGroupsViewModel.AspNetGroup.GroupId);

                foreach (var role in identity.GetAspNetRoles().ToList())
                {
                    var listItem = new SelectListItem()
                    {
                        Text = role.Name,
                        Value = role.Id,
                        Selected = groupRoles.Contains(role.Id)
                    };
                    aspNetGroupsViewModel.AspNetRolesList.Add(listItem);
                }

                return View(aspNetGroupsViewModel);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                identity.DeleteGroup(id);

                TempData["message"] = $"Asp Net Group has been deleted.";

                return RedirectToAction("List");
            }
            else
            {
                return View();
            }
        }
    }
}