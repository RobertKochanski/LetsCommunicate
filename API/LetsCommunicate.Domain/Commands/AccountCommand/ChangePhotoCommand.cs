using LetsCommunicate.Domain.Results;
using LetsCommunicate.Infrastructure.Entities;
using LetsCommunicate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using LetsCommunicate.Domain.Authentication;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace LetsCommunicate.Domain.Commands.AccountCommand
{
    public class ChangePhotoCommand : IRequest<Result<string>>
    {
        [JsonIgnore]
        public string? UserEmail { get; set; }
        public IFormFile File { get; set; }

        public ChangePhotoCommand(IFormFile file)
        {
            File = file;
        }
    }

    public class ChangePhotoCommandHandler : IRequestHandler<ChangePhotoCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ChangePhotoCommandHandler> _logger;

        private readonly Cloudinary _cloudinary;

        public ChangePhotoCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<ChangePhotoCommandHandler> logger, IOptions<CloudinarySettings> config)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;

            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<Result<string>> Handle(ChangePhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(x => x.Photo).FirstOrDefaultAsync(x => x.Email == request.UserEmail);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now}] Problem with find user");
                return Result.BadRequest<string>("Problem with find user");
            }

            var uploadResult = new ImageUploadResult();

            if (request.File.Length > 0)
            {
                using var stream = request.File.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(request.File.FileName, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null) return Result.BadRequest<string>(uploadResult.Error.Message);

            if (user.Photo != null)
            {
                var deleteResult = await _cloudinary.DestroyAsync(new DeletionParams(user.Photo.PublicId));

                if (deleteResult.Error != null) return Result.BadRequest<string>(deleteResult.Error.Message);

                _dbContext.Photos.Remove(user.Photo);
                _dbContext.SaveChanges();
            }
            
            var photo = new Photo
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };

            user.Photo = photo;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Result.Ok(photo.Url);
            }

            return Result.BadRequest<string>("Problem adding photo");
        }
    }
}
