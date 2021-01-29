using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaveImageToFolderAPI.DTOs
{
    public class NewImage
    {
        public string UserName { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public string ImageData { get; set; } // Base64 
    }
}
