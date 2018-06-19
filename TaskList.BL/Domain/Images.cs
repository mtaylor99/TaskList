using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TaskList.BL.Interaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BL.Domain
{
    public class Images : IImages
    {
        private ILogger logger;
        private IImageRepository imageRepository;

        public Images(ILogger log, IImageRepository imageRepo)
        {
            logger = log;
            imageRepository = imageRepo;

            logger.LogInformation("Images Business Logic - Constructor");
        }

        public List<Guid> GetImages(int taskId)
        {
            return imageRepository.GetImages(taskId);
        }

        public Image ViewImage(Guid id)
        {
            return imageRepository.ViewImage(id);
        }

        public string UploadImage(IList<IFormFile> file, int taskId)
        {
            IFormFile uploadedImage = file.FirstOrDefault();
            if (uploadedImage == null || uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream ms = new MemoryStream();
                uploadedImage.OpenReadStream().CopyTo(ms);

                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                Image imageEntity = new Image()
                {
                    ImageId = Guid.NewGuid(),
                    TaskId = taskId,
                    Name = uploadedImage.Name,
                    Data = ms.ToArray(),
                    Width = image.Width,
                    Height = image.Height,
                    ContentType = uploadedImage.ContentType
                };

                return imageRepository.AddImage(imageEntity).ToString();
            }

            return null;
        }
    }
}
