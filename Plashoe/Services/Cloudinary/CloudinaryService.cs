

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Plashoe.Services.Cloudinary;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {

        var cloudName = configuration["CloudinarySettings:CloudName"];
        var apiKey = configuration["CloudinarySettings:ApiKey"];
        var apiSecret = configuration["CloudinarySettings:ApiSecret"];


        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
        
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file.Length <= 0) return null;

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Crop("fill").Gravity("auto"),
            //Folder = "plashoe/assets/products"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.AbsoluteUri;
    }
}
