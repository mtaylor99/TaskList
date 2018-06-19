using System;
using System.Collections.Generic;

namespace TaskList.Web.Models.ViewModels
{
    public class ImagesListViewModel
    {
        public int TaskId { get; set; }

        public IList<Guid> ImageIds { get; set; }
    }
}
