using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SaveImageToFolderAPI.Data;
using SaveImageToFolderAPI.DTOs;
using SaveImageToFolderAPI.Models;

namespace SaveImageToFolderAPI
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly SaveImageToFolderAPIContext _context;

        private readonly IOptions<ImageServiceConfiguration> _imageServiceConfig;


        public ImagesController(SaveImageToFolderAPIContext context, IOptions<ImageServiceConfiguration> imageServiceConfig)
        {
            _context = context;
            _imageServiceConfig = imageServiceConfig;

       
        }

       

        // GET: /Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImage(string userName)
        {

            return await _context.Image.Where(x => x.UserName == userName).ToListAsync();
        }

        // GET: /Images/Statistic
        [Route("statistics/")]
        [HttpGet]
        public async Task<ActionResult<Object>> GetInfo()
        {
            int notResized = _context.Image.Count(x => x.Resized == false);
            int Resized = _context.Image.Count(x => x.Resized == true);
            return new { notResizedImages = notResized, ResizedImages = Resized };
        }

        // GET: /Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            var image = await _context.Image.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // PUT: api/Images/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, Image image)
        {
            if (id != image.ID)
            {
                return BadRequest();
            }

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Images
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Image>> PostImage( NewImage newImage)
        {

            // Create list of folders
            List<string> folders = new List<string>() { _imageServiceConfig.Value.MainFolderName,
              _imageServiceConfig.Value.ImageUploadDirectory,_imageServiceConfig.Value.ImageResizedDirectory};

            folders.ForEach((folder) =>
            {
                //ckeck if folder exist
                if (System.IO.Directory.Exists(System.IO.Path.Combine(folder)) == false)
                {
                    // Create new folder
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(folder));
                }
            });

            // Convert string to byte array
            byte[] imageDataByteArray = Convert.FromBase64String(newImage.ImageData);

            // received images will be saved to C:/Original_Images
            string imagesFolder = _imageServiceConfig.Value.ImageUploadDirectory;
    

               // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-create-a-file-or-folder


            // images new name with Global 
            string fileName = Guid.NewGuid().ToString() + ".png";

            // Use Combine to add the current directory, folder name and file name to the path.
            string pathString = System.IO.Path.Combine( imagesFolder, fileName);

            // Check that the file doesn't already exist. If it doesn't exist, create
            if (!System.IO.File.Exists(pathString))
            {
                using (System.IO.FileStream newImageFile = System.IO.File.Create(pathString))
                {
                    // write byte array to created file
                    newImageFile.Write(imageDataByteArray);                 
                }
            }
            else
            {
                Console.WriteLine("File already exists.", fileName);         
            }

            // received image was saved to folder now all the details will be saved to database

            Image image = new Image();
            image.ImageID = fileName;
            image.UserName = newImage.UserName;
            image.Resized = false;
            image.ImageHeight = newImage.ImageHeight;
            image.ImageWidth = newImage.ImageWidth;


            _context.Image.Add(image);
            await _context.SaveChangesAsync();


            return image;
        }

        //// DELETE: api/Images/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Image>> DeleteImage(int id)
        //{
        //    var image = await _context.Image.FindAsync(id);
        //    if (image == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Image.Remove(image);
        //    await _context.SaveChangesAsync();

        //    return image;
        //}

        private bool ImageExists(int id)
        {
            return _context.Image.Any(e => e.ID == id);
        }
    }
}
