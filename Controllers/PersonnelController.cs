using AbidiCompanySenario.Data.Interfaces.Personnels;
using AbidiCompanySenario.ViewModels.Personnels;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AbidiCompanySenario.Controllers
{
    public class PersonnelController : Controller
    {

        private readonly IPersonnelRepository _Repository;
        private readonly IMapper _mapper;
        private readonly INotyfService _toastNotification;

        public PersonnelController(IPersonnelRepository personnelRepository, INotyfService notyfService, IMapper mapper)
        {
            _Repository = personnelRepository;  
            _mapper = mapper;
            _toastNotification = notyfService;  
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Add(AddPersonnelViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   if(await _Repository.CheckPersonnelExistWithPersonnelCode(model.Personnel_Code))
                    {
                        _toastNotification.Error("A person with this personnel code already exists. Please try another code.", 10);
                        return View();
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
