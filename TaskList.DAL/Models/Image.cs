using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.DAL.Models
{
    public class Image
    {
        public Guid ImageId { get; set; }

        public int TaskId { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string ContentType { get; set; }
    }
}
