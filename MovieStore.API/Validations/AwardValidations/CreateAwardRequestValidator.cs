using FluentValidation;
using MovieStore.DTO.AwardDTO;

namespace MovieStore.API.Validations.AwardValidations
{
    public class CreateAwardRequestValidator : AbstractValidator<CreateAwardRequest>
    {
        public CreateAwardRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
        }
    }
}
