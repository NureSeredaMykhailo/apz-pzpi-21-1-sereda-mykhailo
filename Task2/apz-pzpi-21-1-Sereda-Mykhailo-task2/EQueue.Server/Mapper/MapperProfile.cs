using AutoMapper;
using EQueue.Db.Models;
using EQueue.Predictor.Models;
using EQueue.Shared.Dto;

namespace EQueue.Server.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<TraumaType, TraumaTypeDto>().ReverseMap();
            CreateMap<TraumaPlace, TraumaPlaceDto>().ReverseMap();
            CreateMap<Clinic, ClinicDto>().ReverseMap();
            CreateMap<Trauma, TraumaDto>()
                .ForMember(m => m.TraumaPlaceTitle, otp => otp.MapFrom(s => s.TraumaPlace.Title))
                .ForMember(m => m.TraumaTypeTitle, otp => otp.MapFrom(s => s.TraumaType.Title));
            CreateMap<TraumaDto, Trauma>();
            CreateMap<Case, CaseDto>()
                .ForMember(m => m.TraumaDtos, otp => otp.MapFrom(s => s.Traumas))
                .ReverseMap();

            CreateMap<Case, DamagePredictInput>()
                .ForMember(dest => dest.ClinicId, opt => opt.MapFrom(src => src.ClinicId ?? 0))
                .ForMember(dest => dest.Trauma0TypeId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(0) != null ? src.Traumas[0].TraumaTypeId : 0))
                .ForMember(dest => dest.Trauma0PlaceId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(0) != null ? src.Traumas[0].TraumaPlaceId : 0))
                .ForMember(dest => dest.Trauma1TypeId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(1) != null ? src.Traumas[1].TraumaTypeId : 0))
                .ForMember(dest => dest.Trauma1PlaceId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(1) != null ? src.Traumas[1].TraumaPlaceId : 0))
                .ForMember(dest => dest.Trauma2TypeId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(2) != null ? src.Traumas[2].TraumaTypeId : 0))
                .ForMember(dest => dest.Trauma2PlaceId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(2) != null ? src.Traumas[2].TraumaPlaceId : 0))
                .ForMember(dest => dest.Trauma3TypeId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(3) != null ? src.Traumas[3].TraumaTypeId : 0))
                .ForMember(dest => dest.Trauma3PlaceId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(3) != null ? src.Traumas[3].TraumaPlaceId : 0))
                .ForMember(dest => dest.TreatmentStartDelayMinutes, opt => opt.MapFrom(src => CalculateTreatmentStartDelayMinutes(src.TraumasRegisteredUnixTime, src.StartedTreatmentUnixTime)))
                .ForMember(dest => dest.GotIncurableDamage, opt => opt.MapFrom(src => src.GotIncurableDamage));

            CreateMap<Case, DeathPredictInput>()
                .ForMember(dest => dest.ClinicId, opt => opt.MapFrom(src => src.ClinicId ?? 0))
                .ForMember(dest => dest.Trauma0TypeId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(0) != null ? src.Traumas[0].TraumaTypeId : 0))
                .ForMember(dest => dest.Trauma0PlaceId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(0) != null ? src.Traumas[0].TraumaPlaceId : 0))
                .ForMember(dest => dest.Trauma1TypeId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(1) != null ? src.Traumas[1].TraumaTypeId : 0))
                .ForMember(dest => dest.Trauma1PlaceId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(1) != null ? src.Traumas[1].TraumaPlaceId : 0))
                .ForMember(dest => dest.Trauma2TypeId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(2) != null ? src.Traumas[2].TraumaTypeId : 0))
                .ForMember(dest => dest.Trauma2PlaceId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(2) != null ? src.Traumas[2].TraumaPlaceId : 0))
                .ForMember(dest => dest.Trauma3TypeId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(3) != null ? src.Traumas[3].TraumaTypeId : 0))
                .ForMember(dest => dest.Trauma3PlaceId, opt => opt.MapFrom(src => src.Traumas.ElementAtOrDefault(3) != null ? src.Traumas[3].TraumaPlaceId : 0))
                .ForMember(dest => dest.TreatmentStartDelayMinutes, opt => opt.MapFrom(src => CalculateTreatmentStartDelayMinutes(src.TraumasRegisteredUnixTime, src.StartedTreatmentUnixTime)))
                .ForMember(dest => dest.Survived, opt => opt.MapFrom(src => src.Survived));

            CreateMap<CasePriority, CaseQueueDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Case.Name))
                .ForMember(dest => dest.CaseId, opt => opt.MapFrom(src => src.CaseId))
                .ForMember(dest => dest.TraumasRegisteredUnixTime, opt => opt.MapFrom(src => src.Case.TraumasRegisteredUnixTime))
                .ForMember(dest => dest.TraumaDtos, opt => opt.MapFrom(src => src.Case.Traumas))
                .ForMember(dest => dest.PriorityPeriodStartUnix, opt => opt.MapFrom(src => src.PriorityPeriodStartUnix))
                .ForMember(dest => dest.PriorityPeriodEndUnix, opt => opt.MapFrom(src => src.PriorityPeriodEndUnix))
                .ForMember(dest => dest.CombinedPriority, opt => opt.MapFrom(src => src.CombinedPriority));

        }

        private int CalculateTreatmentStartDelayMinutes(long registeredUnixTime, long startedTreatmentUnixTime)
        {
            DateTime registeredTime = DateTimeOffset.FromUnixTimeSeconds(registeredUnixTime).UtcDateTime;
            DateTime startedTime = DateTimeOffset.FromUnixTimeSeconds(startedTreatmentUnixTime).UtcDateTime;

            return (int)(startedTime - registeredTime).TotalMinutes;
        }
    }
}
