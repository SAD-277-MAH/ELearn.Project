using ELearn.Common.Helpers.Interface;
using ELearn.Data.Context;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Site.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.FileActions
{
    public class FileActionsController : Controller
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IUtilities _utilities;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IOrderService _orderService;

        public FileActionsController(IUnitOfWork<DatabaseContext> db, IUtilities utilities, IWebHostEnvironment hostingEnvironment, IOrderService orderService)
        {
            _db = db;
            _utilities = utilities;
            _hostingEnvironment = hostingEnvironment;
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("StreamFile/{id}")]
        public async Task<IActionResult> StreamFile(string id)
        {
            var session = await _db.SessionRepository.GetAsync(s => s.Id == id, "Course");
            if (session == null)
            {
                return NotFound();
            }

            if (!(User.HasClaim(ClaimTypes.Role, "Admin")
                || (User.HasClaim(ClaimTypes.Role, "Student") && await _orderService.IsUserInCourseAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, session.CourseId))
                || (User.HasClaim(ClaimTypes.Role, "Teacher") && User.FindFirst(ClaimTypes.NameIdentifier).Value == session.Course.TeacherId)))
            {
                return NotFound();
            }

            string filePath = _utilities.FindLocalPathFromUrl(session.VideoUrl);
            string path = Path.Combine(_hostingEnvironment.WebRootPath, filePath);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            return PhysicalFile(path, "application/octet-stream", enableRangeProcessing: true);
        }

        [Authorize]
        [HttpGet("DownloadFile/{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var session = await _db.SessionRepository.GetAsync(s => s.Id == id, "Course");
            if (session == null)
            {
                return NotFound();
            }

            if (!(User.HasClaim(ClaimTypes.Role, "Admin")
                || (User.HasClaim(ClaimTypes.Role, "Student") && await _orderService.IsUserInCourseAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), session.CourseId))
                || (User.HasClaim(ClaimTypes.Role, "Teacher") && User.FindFirstValue(ClaimTypes.NameIdentifier) == session.Course.TeacherId)))
            {
                return NotFound();
            }

            string filePath = _utilities.FindLocalPathFromUrl(session.VideoUrl);
            string path = Path.Combine(_hostingEnvironment.WebRootPath, filePath);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var str = Path.GetExtension(path);
            return PhysicalFile(path, "application/force-download", $"{session.SessionNumber}-{session.Course.Title}--{session.Title}{Path.GetExtension(path)}", true);
        }
    }
}
