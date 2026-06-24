using AutoMapper;
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
            CreateMap<Document, DocumentDto>();

        }
    }
}
