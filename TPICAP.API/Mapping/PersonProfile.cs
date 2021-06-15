using AutoMapper;
using TPICAP.API.Models;
using TPICAP.Data.Models;

namespace TPICAP.API.Mapping
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            this.CreateMap<Person, PersonResponseModel>()
                .ForMember(
                    destination => destination.Id,
                    options => options.MapFrom(source => source.Id)
                )
                .ForMember(
                    destination => destination.FirstName,
                    options => options.MapFrom(source => source.FirstName)
                )
                .ForMember(
                    destination => destination.LastName,
                    options => options.MapFrom(source => source.LastName)
                )
                .ForMember(
                    destination => destination.Dob,
                    options => options.MapFrom(source => source.Dob)
                )
                .ForMember(
                    destination => destination.Salutation,
                    options => options.MapFrom(source => source.Salutation)
                );
            this.CreateMap<PersonCreationModel, Person>()
                .ForMember(
                    destination => destination.FirstName,
                    options => options.MapFrom(source => source.FirstName)
                )
                .ForMember(
                    destination => destination.LastName,
                    options => options.MapFrom(source => source.LastName)
                )
                .ForMember(
                    destination => destination.Dob,
                    options => options.MapFrom(source => source.Dob)
                )
                .ForMember(
                    destination => destination.Salutation,
                    options => options.MapFrom(source => source.Salutation)
                );
            this.CreateMap<PersonModificationModel, Person>()
                .ForMember(
                    destination => destination.Id,
                    options => options.MapFrom(source => source.Id)
                )
                .ForMember(
                    destination => destination.FirstName,
                    options => options.MapFrom(source => source.FirstName)
                )
                .ForMember(
                    destination => destination.LastName,
                    options => options.MapFrom(source => source.LastName)
                )
                .ForMember(
                    destination => destination.Dob,
                    options => options.MapFrom(source => source.Dob)
                )
                .ForMember(
                    destination => destination.Salutation,
                    options => options.MapFrom(source => source.Salutation)
                );
        }
    }
}
