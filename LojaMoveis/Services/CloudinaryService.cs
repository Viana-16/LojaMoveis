//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;

//public class CloudinaryService
//{
//    private readonly Cloudinary _cloudinary;

//    public CloudinaryService(IConfiguration configuration)
//    {
//        var account = new Account(
//            configuration["Cloudinary:dg9ss8s58"],
//            configuration["Cloudinary:314866793874983"],
//            configuration["Cloudinary:ceNufRXWjV7pVQkJsZIRwwu--B8"]);

//        _cloudinary = new Cloudinary(account);
//    }

//    public async Task<string> UploadImagemAsync(IFormFile file)
//    {
//        using var stream = file.OpenReadStream();
//        var uploadParams = new ImageUploadParams
//        {
//            File = new FileDescription(file.FileName, stream),
//            Folder = "produtos"
//        };

//        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
//        return uploadResult.SecureUrl.ToString();
//    }
//}

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LojaMoveis.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LojaMoveis.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var settings = config.Value;
            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImagemAsync(IFormFile imagem)
        {
            if (imagem == null || imagem.Length == 0)
                return null;

            await using var stream = imagem.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imagem.FileName, stream),
                Folder = "produtos", // opcional: cria uma pasta no Cloudinary
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false,
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                return uploadResult.SecureUrl.ToString();

            return null;
        }
    }
}
