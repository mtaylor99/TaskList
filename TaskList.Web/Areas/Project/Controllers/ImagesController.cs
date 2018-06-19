using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.Web.Models.ViewModels;

namespace TaskList.Web.Areas.Project.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private ILogger<TasksController> logger;
        private IImageRepository imageRepository;
        Images images;

        public ImagesController(ILogger<TasksController> log, IImageRepository imageRepo)
        {
            logger = log;
            imageRepository = imageRepo;
            images = new Images(logger, imageRepository);

            logger.LogInformation("Images Controller");
        }

        [HttpGet]
        public IActionResult List(int id)
        {
            var imagesListViewModel = new ImagesListViewModel();

            imagesListViewModel.TaskId = id;
            imagesListViewModel.ImageIds = images.GetImages(id);

            return View (imagesListViewModel);
        }

        [HttpPost]
        public IActionResult UploadImage(IList<IFormFile> file, int taskId)
        {
            if (file.Count > 0)
            {
                var imageId = images.UploadImage(file, taskId);
            }

            return RedirectToAction("List", "Tasks");
        }

        [HttpGet]
        public FileStreamResult ViewImage(Guid id)
        {
            Image image = images.ViewImage(id);

            MemoryStream ms = new MemoryStream(image.Data);

            return new FileStreamResult(ms, image.ContentType);
        }
    }
}
