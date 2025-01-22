using FluentValidation;
using SonaeTestSol.Domain.Interfaces;
using SonaeTestSol.Domain.Interfaces.Service;
using SonaeTestSol.Domain.Models;


namespace SonaeTestSol.Services.Base
{
    public abstract class BaseService
    {
        protected readonly IErrorService ErrorService;

        protected BaseService(IErrorService errorService)
        {
            ErrorService = errorService;
        }

        protected bool ValidOperation()
        {
            return !ErrorService.Exists();
        }

        public bool RunValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : IEntity
        {
            FluentValidation.Results.ValidationResult validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            foreach (FluentValidation.Results.ValidationFailure error in validator.Errors)
            {
                ErrorService.Add(new Error(error.ErrorMessage));
            }
            return false;
        }

        public bool RunValidation<TV, TE>(TV validation, IEnumerable<TE> entities) where TV : AbstractValidator<TE> where TE : IEntity
        {
            foreach (TE entity in entities)
            {
                FluentValidation.Results.ValidationResult validator = validation.Validate(entity);
                if (!validator.IsValid)
                {
                    foreach (FluentValidation.Results.ValidationFailure error in validator.Errors)
                    {
                        ErrorService.Add(new Error(error.ErrorMessage));
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
