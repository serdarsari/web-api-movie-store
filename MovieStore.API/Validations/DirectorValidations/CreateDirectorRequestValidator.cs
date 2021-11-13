using FluentValidation;
using MovieStore.DTO.DirectorDTO;
using System;

namespace MovieStore.API.Validations.DirectorValidations
{
    public class CreateDirectorRequestValidator : AbstractValidator<CreateDirectorRequest>
    {
        public CreateDirectorRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.PlaceOfBirth).NotEmpty();
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today);
        }
    }
}
