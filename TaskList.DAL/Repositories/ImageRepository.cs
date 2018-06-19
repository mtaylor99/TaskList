using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.DbContexts;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private ApplicationDbContext context;

        public ImageRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public List<Guid> GetImages(int taskId)
        {
            List<Guid> result = new List<Guid>();

            var query = (
                from images in context.Image.Where(i => i.TaskId == taskId)

                select new
                {
                    images.ImageId
                }).ToList();

            foreach (var image in query)
            {
                result.Add(image.ImageId);
            }

            return result;
        }

        public Image ViewImage(Guid imageId)
        {
            return context.Image.FirstOrDefault(i => i.ImageId == imageId);
        }

        public Guid AddImage(Image image)
        {
            context.Image.Add(image);
            context.SaveChanges();

            return image.ImageId;
        }
    }
}
