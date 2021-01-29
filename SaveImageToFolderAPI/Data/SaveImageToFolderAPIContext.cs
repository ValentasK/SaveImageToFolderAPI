using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaveImageToFolderAPI.Models;

namespace SaveImageToFolderAPI.Data
{
    public class SaveImageToFolderAPIContext : DbContext
    {
        public SaveImageToFolderAPIContext (DbContextOptions<SaveImageToFolderAPIContext> options)
            : base(options)
        {
        }

        public DbSet<SaveImageToFolderAPI.Models.Image> Image { get; set; }
    }
}
