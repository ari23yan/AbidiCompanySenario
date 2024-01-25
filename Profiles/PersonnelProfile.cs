using AbidiCompanySenario.Models.Entities;
using AbidiCompanySenario.ViewModels.Personnels;
using AutoMapper;
using System.Reflection.Metadata;

namespace AbidiCompanySenario.Profiles
{
    public class PersonnelProfile: Profile
    {
        public PersonnelProfile()
        {
            CreateMap<AddPersonnelViewModel, Personnel >()
                    .ForMember(dest => dest.AcademicDegrees, opt => opt.Ignore())
                    .ReverseMap();

            CreateMap<PersonnelViewModel, Personnel>()
               .ReverseMap();




            CreateMap<EditPersonnelViewModel, Personnel>()
                    .ForMember(dest => dest.AcademicDegrees, opt => opt.Ignore())
                .ReverseMap();
       



        }
    }
}
