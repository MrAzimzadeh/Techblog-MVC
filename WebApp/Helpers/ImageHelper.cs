using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Helpers
{
    public static class ImageHelper
    {
        public static string UploadSinglePhoto(IFormFile file, IWebHostEnvironment env)
        {
            string folderName = "";

            if (file.ContentType.Contains("video"))
            {
                folderName = "uploads/videos";
            }
            else if (file.ContentType.Contains("image"))
            {
                folderName = "uploads";
            }


            var path = "/" + folderName + "/" + Guid.NewGuid() + Path.GetExtension(file.FileName);

            using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return path;

        }


    }
}