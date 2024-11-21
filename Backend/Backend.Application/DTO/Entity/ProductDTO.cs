using Backend.Domain.Entites;
using Microsoft.AspNetCore.Http;
using System;

namespace Backend.Application.DTO.Entity;
public partial class ProductDTO : Product
{
    public IFormFile? FileUrl { get; set; }

}