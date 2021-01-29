using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaveImageToFolderAPI.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string ImageID { get; set; }
        public string UserName { get; set; }
        public bool Resized { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
    }
}
