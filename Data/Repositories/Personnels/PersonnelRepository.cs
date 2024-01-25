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

        public Task<bool> CheckPersonnelExistWithNationalCode(string nationalCode)
        {
            return _context.Personnels.AnyAsync(x => x.National_Code.Equals(nationalCode));

        }

        public Task<bool> CheckPersonnelExistWithPersonnelCode(long personnelCode)
        {

            return _context.Personnels.AnyAsync(x => x.Personnel_Code.Equals(personnelCode));
        }

        public async Task<AcademicDegree> GetAcademicDegreeById(long id)
        {
            return await _context.AcademicDegrees.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<AcademicDegree>> GetAcademicDegreesByPersonnelId(long personnelId)
        {
            return await _context.AcademicDegrees.Where(x => x.PersonnelId == personnelId).ToListAsync();
        }

        public async Task<Personnel?> GetPersonneByIdAsync(long personnelId)
        {
           return await _context.Personnels.FirstOrDefaultAsync(x=>x.Personnel_Id.Equals(personnelId));
        }

        public async Task<Personnel?> GetPersonneByIdEagerLoadAsync(long personnelId)
        {
            return await _context.Personnels.Include(x => x.AcademicDegrees.Where(ad => !ad.IsDeleted)).FirstOrDefaultAsync(x => x.Personnel_Id.Equals(personnelId));
        }

        public async Task<Personnel?> GetPersonneByPersonnelCodeAsync(long personnelCode)
        {
            return await _context.Personnels.FirstOrDefaultAsync(x => x.Personnel_Code.Equals(personnelCode));
        }

        public async Task<List<Personnel>> GetPersonnelsListEagerLoadPaginatedAsync(int pageNum, int pageSize)
        {
            int skip = (pageNum - 1) * pageSize;

            return await _context.Personnels
           .Include(x => x.AcademicDegrees.Where(ad => !ad.IsDeleted)) 
           .Where(x => !x.IsDeleted).Skip(skip)
           .Take(pageSize)
           .ToListAsync();

        }
        public async Task<List<Personnel>> GetPersonnelsListAsync()
        {
            return await _context.Personnels
           .Where(x => !x.IsDeleted)
           .ToListAsync();
        }

        public async Task<int> PersonnelsCount()
        {
           return await _context.Personnels.Where(x=> !x.IsDeleted).CountAsync();
        }
    }
}
