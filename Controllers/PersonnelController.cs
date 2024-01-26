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
using AbidiCompanySenario.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using OfficeOpenXml;
using System.Reflection.Metadata;
using System.Xml;
using IronPdf;


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
        private const int PAGESIZE = 5;

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
        public async Task<IActionResult> Index(int pageNum = 1)
        {
            try
            {
                var personnels = await _Repository.GetPersonnelsListEagerLoadPaginatedAsync(pageNum, PAGESIZE);
                var listCount = await _Repository.PersonnelsCount();
                var personnelViewModel = _mapper.Map<List<PersonnelViewModel>>(personnels);
                PaginationMetadata paginationMetadata = new PaginationMetadata
                {
                    TotalCount = listCount,
                    PageSize = PAGESIZE,
                    CurrentPage = pageNum
                };
                var viewModel = new PaginatedPersonnelListViewModel
                {
                    Personnels = personnelViewModel,
                    Metadata = paginationMetadata,
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Oops! Something went wrong. Please try again later.", 5);
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
                    if (!Utilites.NationalCodeIsValid(model.National_Code))
                    {
                        _toastNotification.Error("Invalid National Code. Please try another code.", 5);
                        return RedirectToAction("Index");
                    }
                    if (await _Repository.CheckPersonnelExistWithNationalCode(model.National_Code))
                    {
                        _toastNotification.Error("A person with this National code already exists. Please try another code.", 5);
                        return RedirectToAction("Index");
                    }
                    if (await _Repository.CheckPersonnelExistWithPersonnelCode(model.Personnel_Code))
                    {
                        _toastNotification.Error("A person with this personnel code already exists. Please try another code.", 5);
                        return RedirectToAction("Index");
                    }
                    var personnel = _mapper.Map<Personnel>(model);
                    await _personnelGenericRepository.AddAsync(personnel);
                    await _personnelGenericRepository.SaveAsync();
                    if (model.AcademicDegrees != null)
                    {
                        foreach (var item in model.AcademicDegrees)
                        {
                            if (Utilites.CheckFileType(item.FileName))
                            {
                                AcademicDegree degree = new AcademicDegree
                                {
                                    FileName = await FileHandler.FileUploadAsync(item, _webHostEnvironment),
                                    FileType = item.ContentType,
                                    PersonnelId = personnel.Personnel_Id
                                };
                                await _academicDegreeGenericRepository.AddAsync(degree);
                                await _academicDegreeGenericRepository.SaveAsync();
                            }
                        }
                    }
                    _toastNotification.Success("Personnel Added Successfully", 5);
                    return RedirectToAction("Index");
                }
                _toastNotification.Error("Please Fill Form Correctly", 5);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Oops! Something went wrong. Please try again later.", 5);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long personnelId)
        {
            try
            {
                var personnel = await _Repository.GetPersonneByIdEagerLoadAsync(personnelId);
                return Ok(personnel);
            }
            catch (Exception ex)
            {
                return Ok("Oops! Something went wrong. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPersonnelViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var personnelResult = await _Repository.GetPersonneByIdAsync(model.Personnel_Id);
                    personnelResult.First_Name = model.First_Name;
                    personnelResult.Last_Name = model.Last_Name;
                    _personnelGenericRepository.Update(personnelResult);
                    await _personnelGenericRepository.SaveAsync();
                    if (model.AcademicDegrees != null)
                    {
                        foreach (var item in model.AcademicDegrees)
                        {
                            AcademicDegree degree = new AcademicDegree
                            {
                                FileName = await FileHandler.FileUploadAsync(item, _webHostEnvironment),
                                FileType = item.ContentType,
                                PersonnelId = personnelResult.Personnel_Id
                            };
                            await _academicDegreeGenericRepository.AddAsync(degree);
                            await _academicDegreeGenericRepository.SaveAsync();
                        }
                    }
                    _toastNotification.Success("Personnel Updated Successfully", 5);
                    return RedirectToAction("Index");
                }
                _toastNotification.Error("Please Fill Form Correctly", 5);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Oops! Something went wrong. Please try again later.", 5);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long personnelId)
        {
            try
            {
                var personnel = await _Repository.GetPersonneByIdAsync(personnelId);
                if (personnel != null)
                {
                    var degrees = await _Repository.GetAcademicDegreesByPersonnelId(personnelId);
                    if (degrees != null)
                    {
                        foreach (var item in degrees)
                        {
                            item.IsDeleted = true;
                            FileHandler.DeleteFile(item.FileName, _webHostEnvironment);
                            _academicDegreeGenericRepository.Update(item);
                        }
                        await _academicDegreeGenericRepository.SaveAsync();
                    }
                    personnel.IsDeleted = true;
                    _personnelGenericRepository.Update(personnel);
                    await _personnelGenericRepository.SaveAsync();
                    _toastNotification.Success("Personnel Deleted Successfully", 5);
                    return RedirectToAction("Index");
                }

                _toastNotification.Error("Person Not Found", 5);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Oops! Something went wrong. Please try again later", 5);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAcademicDegree(long id)
        {
            try
            {
                var degrees = await _Repository.GetAcademicDegreeById(id);
                if (degrees != null)
                {
                    degrees.IsDeleted = true;
                    FileHandler.DeleteFile(degrees.FileName, _webHostEnvironment);
                    _academicDegreeGenericRepository.Update(degrees);
                    await _academicDegreeGenericRepository.SaveAsync();
                    return Ok(StatusCodes.Status200OK);
                }
                return Ok(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return Ok(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var personnels = await _Repository.GetPersonnelsListAsync();
                var personnelViewModel = _mapper.Map<List<PersonnelViewModel>>(personnels);
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Personnels");
                    worksheet.Cells.LoadFromCollection(personnelViewModel, true);
                    using (var memoryStream = new MemoryStream())
                    {
                        package.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Personnels.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Oops! Something went wrong. Please try again later.", 5);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToPdf()
        {
            try
            {
                var personnels = await _Repository.GetPersonnelsListAsync();
                var personnelViewModel = _mapper.Map<List<PersonnelViewModel>>(personnels);
                IronPdf.License.LicenseKey = "IRONSUITE.ARIYAN137923.YAHOO.COM.15552-918E3B7A36-G4R3SVMDU6COG6-T6IZMKFZDCDK-IMMMF42S5AQ6-D3QJPWCE4GYL-5GW4DXW4ZLK4-TC6UXVGTCFUN-UANOGP-TKJFJQH46WGLUA-DEPLOYMENT.TRIAL-LYFR2M.TRIAL.EXPIRES.25.FEB.2024";
                var pdfDocument = new IronPdf.HtmlToPdf();
                var htmlContent = Utilites.GetHtmlContent(personnelViewModel);
                var pdfBytes = pdfDocument.RenderHtmlAsPdf(htmlContent).BinaryData;
                return File(pdfBytes, "application/pdf", "Personnels.pdf");
            }
            catch (Exception ex)
            {
                _toastNotification.Error("Oops! Something went wrong. Please try again later.", 5);
                return RedirectToAction("Index");
            }
        }
    }
}
