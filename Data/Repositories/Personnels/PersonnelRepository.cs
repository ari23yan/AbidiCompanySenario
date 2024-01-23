using AbidiCompanySenario.Data.Context;
using AbidiCompanySenario.Data.Interfaces.Personnels;
using AbidiCompanySenario.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbidiCompanySenario.Data.Repositories.Personnels
{
    public class PersonnelRepository : IPersonnelRepository
    {

        private readonly ProjectDbContext _context;
        public PersonnelRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public Task<bool> CheckPersonnelExistWithPersonnelCode(long personnelCode)
        {

            return _context.Personnels.AnyAsync(x => x.Personnel_Code.Equals(personnelCode));
        }

        public Task<Personnel?> GetPersonneByIdAsync(long personnelId)
        {
           return _context.Personnels.FirstOrDefaultAsync(x=>x.Personnel_Id.Equals(personnelId));
        }

        public Task<Personnel?> GetPersonneByNationalCodeAsync(string nationalCode)
        {
            return _context.Personnels.FirstOrDefaultAsync(x => x.National_Code.Equals(nationalCode));
        }

        public async Task<List<Personnel>> GetPersonnelsListEagerLoadAsync()
        {
            return await _context.Personnels
                    .Include(x => x.AcademicDegrees)
                    .Where(x => !x.IsDeleted)
                    .ToListAsync();
        }
    }
}
