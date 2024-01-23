using AbidiCompanySenario.Data.Interfaces.Personnels;
using AbidiCompanySenario.Models.Entities;
using AbidiCompanySenario.ViewModels.Personnels;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AbidiCompanySenario.Utilities;
using ChristianBeauty.Utilities;
using Microsoft.AspNetCore.Hosting;
using AbidiCompanySenario.Data.Interfaces;

namespace AbidiCompanySenario.Controllers
{
    public class PersonnelController : Controller
    {

        private readonly IPersonnelRepository _Repository;
        private readonly IGenericRepository<Personnel> _personnelGenericRepository;
        private readonly IGenericRepository<AcademicDegree> _academicDegreeGenericRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _toastNotification;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public PersonnelController(IPersonnelRepository personnelRepository, IGenericRepository<AcademicDegree> academicDegreeGenericRepository, IGenericRepository<Personnel> personnelGenericRepository, IWebHostEnvironment webHostEnvironment, INotyfService notyfService, IMapper mapper)
        {
            _Repository = personnelRepository;
            _mapper = mapper;
            _toastNotification = notyfService;
            _webHostEnvironment = webHostEnvironment;
            _academicDegreeGenericRepository = academicDegreeGenericRepository;
            _personnelGenericRepository = personnelGenericRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
              var personnels = await _Repository.GetPersonnelsListEagerLoadAsync();
              var personnelViewModel = _mapper.Map<List<PersonnelViewModel>>(personnels);
                return View(personnelViewModel);
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Oops! Something went wrong. Please try again later.", 10);
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddPersonnelViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _Repository.CheckPersonnelExistWithPersonnelCode(model.Personnel_Code))
                    {
                        _toastNotification.Error("A person with this personnel code already exists. Please try another code.", 10);
                        return View("Index");
                    }
                    if (!Utilites.NationalCodeIsValid(model.National_Code))
                    {
                        _toastNotification.Error("Invalid National Code. Please try another code.", 10);
                        return View("Index");
                    }
                    var personnel = _mapper.Map<Personnel>(model);
                    await _personnelGenericRepository.AddAsync(personnel);
                    await _personnelGenericRepository.SaveAsync();
                    if (model.AcademicDegrees != null)
                    {
                        foreach (var item in model.AcademicDegrees)
                        {
                            AcademicDegree degree = new AcademicDegree
                            {
                                FileName = await FileHandler.ImageUploadAsync(item, _webHostEnvironment),
                                FileType = item.ContentType,
                                PersonnelId = personnel.Personnel_Id
                            };
                            await _academicDegreeGenericRepository.AddAsync(degree);
                            await _academicDegreeGenericRepository.SaveAsync();
                        }
                    }
                    _toastNotification.Success("Personnel Added Successfully", 10);
                    return View("Index");
                }
                _toastNotification.Error("Please Fill Form Correctly", 10);
                return View("Index");
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Oops! Something went wrong. Please try again later.", 10);
                return View("Index");
            }
        }





    }
}
