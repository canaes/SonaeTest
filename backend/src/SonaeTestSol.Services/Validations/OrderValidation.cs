using FluentValidation;
using SonaeTestSol.Domain.Entities;
using SonaeTestSol.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonaeTestSol.Services.Validations
{
    public class OrderValidation : AbstractValidator<Order>
    {
        public OrderValidation(int currStock)
        {
            RuleFor(prop => prop.Quantity).GreaterThanOrEqualTo(1)
                .NotNull().WithMessage("Quantity greater than or equal to 1");

            RuleFor(prop => prop.Quantity).LessThanOrEqualTo(currStock)
                .NotNull().WithMessage("Quantity of products unavailable");
        }
    }
}
