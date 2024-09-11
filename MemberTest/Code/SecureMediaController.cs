using System.Security.Claims;
using MemberTest.Models;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;

[UmbracoMemberAuthorize]
public class SecureMediaController : SurfaceController
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMemberManager _memberManager;
    private readonly IMediaService _mediaService;

    public SecureMediaController(
        IWebHostEnvironment hostingEnvironment,
        IMemberManager memberManager,
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        IMediaService mediaService)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _hostingEnvironment = hostingEnvironment;
        _memberManager = memberManager;
        _mediaService = mediaService;
    }


    // Accept a media ID, load that media and stream it back to the client
    [HttpGet]
    public IActionResult GetMediaById(int id)
    {

        // Sample : PDF
        // URL : umbraco/surface/SecureMedia/getmediabyid?id=1166
        // Full Url : https://localhost:44389/umbraco/surface/SecureMedia/getmediabyid?id=1166

        // Sample : Image
        // URL : umbraco/surface/SecureMedia/getmediabyid?id=1139
        // Full Url : https://localhost:44389/umbraco/surface/SecureMedia/getmediabyid?id=1139

        var media = _mediaService.GetById(id);
        if (media == null)
        {
            return NotFound();
        }
        
        // Also check the member is in the right member group
        var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = userIdentity.Claims.FirstOrDefault(x => x.Type == "nameidentifier")?.Value;


        //
        var filePath = (string)media.Properties["umbracoFile"].GetValue();
        var mappedFilePath = _hostingEnvironment.MapPathWebRoot(filePath);

        // Load the file reference from the media item
        if (System.IO.File.Exists(mappedFilePath))
        {
            var mimeType = GetMimeType(mappedFilePath); // A method to get the file's MIME type
            var fileBytes = System.IO.File.ReadAllBytes(mappedFilePath); // Load the file bytes into memory
            return File(fileBytes, mimeType);
        }

        return NotFound();
    }

    // Example method to determine MIME type based on the file extension
    private string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".jpg" => "image/jpeg",
            ".png" => "image/png",
            ".mp4" => "video/mp4",
            ".mp3" => "audio/mpeg",
            ".pdf" => "application/pdf",
            // Add more MIME types as necessary
            _ => "application/octet-stream", // Default MIME type for binary data
        };
    }

}
