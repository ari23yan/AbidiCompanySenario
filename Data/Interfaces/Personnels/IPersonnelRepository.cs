using AbidiCompanySenario.Models.Entities;

namespace AbidiCompanySenario.Data.Interfaces.Personnels
{
    public interface IPersonnelRepository
    {
        Task<Personnel?> GetPersonneByIdAsync(long personnelId);
        Task<Personnel?> GetPersonneByIdEagerLoadAsync(long personnelId);
        Task<List<AcademicDegree>> GetAcademicDegreesByPersonnelId(long personnelId);
        Task<AcademicDegree> GetAcademicDegreeById(long id);
        Task<List<Personnel>> GetPersonnelsListAsync();
        Task<List<Personnel>> GetPersonnelsListEagerLoadPaginatedAsync(int pageNum,int pageSize);
        Task<Personnel?> GetPersonneByPersonnelCodeAsync(long personnelCode);
        Task<bool> CheckPersonnelExistWithPersonnelCode(long personnelCode);
        Task<bool> CheckPersonnelExistWithNationalCode(string nationalCode);
        Task<int> PersonnelsCount();
    }
}
