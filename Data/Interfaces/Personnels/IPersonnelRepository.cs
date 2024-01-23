using AbidiCompanySenario.Models.Entities;

namespace AbidiCompanySenario.Data.Interfaces.Personnels
{
    public interface IPersonnelRepository
    {
        Task<Personnel?> GetPersonneByIdAsync(long personnelId);
        Task<List<Personnel>> GetPersonnelsListEagerLoadAsync();
        Task<Personnel?> GetPersonneByNationalCodeAsync(string nationalCode);
        Task<bool> CheckPersonnelExistWithPersonnelCode(long personnelCode);
    }
}
