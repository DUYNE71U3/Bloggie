using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };
            
            await _tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery)
        {
            //for view search bar
            ViewBag.SearchQuery = searchQuery;

            var tags = await _tagRepository.GetAllAsync(searchQuery);
            return View(tags);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var tag = await _tagRepository.GetAsync(Id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName                    
                };
                return View(editTagRequest);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            
            var updatedTag = await _tagRepository.UpdateAsync(tag);

            if(updatedTag != null)
            {
                //Show success notification
            }
            else
            {
                //Show Error Notification
            }
            
            return RedirectToAction("List", new { id = editTagRequest.Id });   
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await _tagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                //Show Success Notification
                return RedirectToAction("List");
            }
            else
            {
                //Show Error Notification
            }
            //Show Error Notification
            return RedirectToAction("List", new { id = editTagRequest.Id });
        }
    }
}
