using Backend.Application.Interface.DomainRepository;
using Backend.Application.Services;
using Backend.Domain.Entites;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        private readonly IProductRepository _iProductRepository;
        public ProductValidator(IProductRepository iProductRepository)
        {
            _iProductRepository = iProductRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(" name is required.")
                .MaximumLength(5).WithMessage("First name must not exceed 5 characters.");

            RuleFor(x => x.Price).LessThan(10)
             .WithMessage("price can not mess than 10 taka");

            RuleFor(x => x.Name).CustomAsync(async (name, context, cancellationToken) =>
            {
                if (!string.IsNullOrEmpty(name))
                {
                    //var exists = await _iProductRepository.CheckduplicateProductByName(name);
                    //if (exists)
                    //{
                    //    context.AddFailure("Name", "This Name is already in use.Please use different Name");
                    //}
                }
            });

        }
    }
}
