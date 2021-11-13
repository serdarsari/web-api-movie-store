using FluentValidation;
using MovieStore.DTO.ActorDTO;
using System;

namespace MovieStore.API.Validations.ActorValidations
{
    public class CreateActorRequestValidator : AbstractValidator<CreateActorRequest>
    {
        public CreateActorRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Biography).NotEmpty();
            RuleFor(x => x.PlaceOfBirth).NotEmpty();
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today);
        }
    }
}
