using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Helpers
{
    public static class ImageHelper
    {
        public static string UploadSinglePhoto(IFormFile image, IWebHostEnvironment env)
        {
            var path = "/uploads/" + Guid.NewGuid() + image.FileName;
            using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
            return path;
        }


    }
    // public static class FıleUpload
    // {
    //     public async static Task<string> UploadSinglePhotoAsync(this IFormFile file, IWebHostEnvironment env)
    //     {
    //         var path = "/uploads/" + Guid.NewGuid() + file.FileName;
    //         using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
    //         {
    //             file.CopyTo(fileStream);
    //         }
    //         return path;
    //     }
    // }
}