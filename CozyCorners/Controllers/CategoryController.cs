using AutoMapper;
using CozyCorners.Core;
using CozyCorners.Core.Models;
using CozyCorners.Extentions;
using CozyCorners.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCorners.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CategoryServices _categoryServices;

        public CategoryController(IUnitOfWork unitOfWork,IMapper mapper ,CategoryServices categoryServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
           _categoryServices = categoryServices;
        }
        public async  Task<IActionResult> Index()
        {
            var categories=await _unitOfWork.Repository<Category>().GetAllAsync();
           
            var mappedCategories = _mapper.Map<IEnumerable<Category>,IEnumerable<CategoryVM>>(categories);
            return View(mappedCategories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var PhotoPath = await _categoryServices.GetPhotoPath(categoryVM);
                    categoryVM.photo=PhotoPath;
                   var CategoryMapped = _mapper.Map<CategoryVM, Category>(categoryVM);
                    //var CategoryMapped = new Category()
                    //{
                    //    Name=categoryVM.Name,
                    //    Description=categoryVM.Description,
                    //    PhotoPath = categoryVM.photo = PhotoPath
                    //};
                    await _unitOfWork.Repository<Category>().Add(CategoryMapped);
                    var RowsAdded=await _unitOfWork.Complet();
                    if (RowsAdded != null)
                    {
                        TempData["Message"] = $"{CategoryMapped.Name} Category Added Successfully!";
                        //ViewBag.Message =$"{CategoryMapped.Name} Category Added Successfully!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }
               

            }
            return View(categoryVM);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id,string? viewname)
        {
            var category =await  _unitOfWork.Repository<Category>().GetById(id);
            var CategoryMapped = _mapper.Map<Category, CategoryVM>(category);
            return View(viewname,CategoryMapped);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
           
            return await Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    if (categoryVM.FilePath !=null)
                    { 
                        var PhotoPath = await _categoryServices.GetPhotoPath(categoryVM);
                        categoryVM.photo = PhotoPath;

                    }
                  
                    var CategoryMapped = _mapper.Map<CategoryVM, Category>(categoryVM);
                  
                    _unitOfWork.Repository<Category>().update(CategoryMapped);
                    var RowsAdded =await _unitOfWork.Complet();
                    if (RowsAdded != null)
                    {
                        TempData["Message"] = $"Updated Category Done Successfully!";
                        //ViewBag.Message =$"{CategoryMapped.Name} Category Added Successfully!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }


            }
            return View(categoryVM);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {  try
                {


                    var category = await _unitOfWork.Repository<Category>().GetById(id);
                   _unitOfWork.Repository<Category>().delete(category);
                    var RowsAdded =await _unitOfWork.Complet();
                    if (RowsAdded != null)
                    {
                        TempData["Message"] = $"Category Deleted Successfully!";
                        //ViewBag.Message =$"{CategoryMapped.Name} Category Added Successfully!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }


            
          
        }


    }
}
