using System;
using System.Collections.Generic;

using TaskList.DAL.Models;

namespace TaskList.DAL.Interfaces
{
    public interface IImageRepository
    {
        List<Guid> GetImages(int taskId);

        Image ViewImage(Guid ImageId);

        Guid AddImage(Image image);
    }
}
