using AbidiCompanySenario.Models.Entities;
using AbidiCompanySenario.ViewModels.Common;

namespace AbidiCompanySenario.ViewModels.Personnels
{
    public class PaginatedPersonnelListViewModel
    {
        public List<PersonnelViewModel> Personnels { get; set; }
        public PaginationMetadata Metadata { get; set; }
    }
}
