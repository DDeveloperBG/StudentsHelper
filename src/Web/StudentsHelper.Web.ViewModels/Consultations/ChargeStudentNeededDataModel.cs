﻿namespace StudentsHelper.Web.ViewModels.Consultations
{
    using System;

    using AutoMapper;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class ChargeStudentNeededDataModel : IHaveCustomMappings
    {
        public string ConsultationId { get; set; }

        public decimal Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meeting, ChargeStudentNeededDataModel>()
                .ForMember(
                     x => x.ConsultationId,
                     opt => opt.MapFrom(src => src.Consultation.Id))
                 .ForMember(
                     x => x.Price,
                     opt => opt.MapFrom(src => Math.Round(src.Consultation.HourWage / 60 * src.DurationInMinutes, 2)));
        }
    }
}
