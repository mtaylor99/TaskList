using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using TaskList.DAL.Models;

namespace TaskList.BL.Interaces
{
    public interface IImages
    {
        List<Guid> GetImages(int taskId);

        Image ViewImage(Guid id);

        string UploadImage(IList<IFormFile> file, int taskId);
    }
}
