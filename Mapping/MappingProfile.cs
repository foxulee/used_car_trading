using AutoMapper;
using project_vega.Controllers.Resources;
using project_vega.Core.Models;
using System.Linq;

namespace project_vega.Mapping
{
    //using Auto Mapper
    public class MappingProfile : Profile
    {
        //mapping API resources to domain resources
        public MappingProfile()
        {
            //Domain(at server) to API Resource(from client)
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValuePairResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(vr => vr.Contact,
                    opt => opt.MapFrom(v =>
                        new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Contact,
                    opt => opt.MapFrom(v =>
                        new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => new KeyValuePairResource { Id = vf.Feature.Id, Name = vf.Feature.Name })))
                .ForMember(vr => vr.Make, opt => opt.MapFrom(v => v.Model.Make));
            //for generic class:
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));
            CreateMap<Photo, PhotoResource>();


            //API Resource to Domain
            CreateMap<SaveVehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())    //don't map/change id(key) property
                .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
                .ForMember(v => v.Features, opt => opt.Ignore())  // Features cannot be automatically matched using MapFrom(), so for mapping Features, Ingore() first, then AfterMap() manually map it
                .AfterMap((vr, v) =>
                {
                    //remove unselected features
                    var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId));

                    foreach (var r in removedFeatures)
                    {
                        v.Features.Remove(r);
                    }
                    //add new features
                    var addedFeatures = vr.Features.Where(id => v.Features.All(f => f.FeatureId != id)).Select(id => new VehicleFeature { FeatureId = id });
                    foreach (var af in addedFeatures)
                    {
                        v.Features.Add(af);
                    }

                });
            CreateMap<VehicleQueryResource, VehicleQuery>();


        }
    }
}