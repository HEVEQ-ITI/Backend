using AutoMapper;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents
{
    public class DocumentProfile:Profile
    {
        public DocumentProfile()
        {
            CreateMap<Document, DocumentDto>()
                .ForMember(d => d.DocumentType, opt => opt.MapFrom(s => s.DocumentType.ToString()))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.StatusAr, opt => opt.MapFrom(s => s.Status.ToArabic()));


        }

    }
}
