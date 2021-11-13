using FluentValidation;
using MovieStore.DTO.ActorDTO;

namespace MovieStore.API.Validations.ActorValidations
{
    public class UpdateActorRequestValidator : AbstractValidator<UpdateActorRequest>
    {
        public UpdateActorRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }
}
