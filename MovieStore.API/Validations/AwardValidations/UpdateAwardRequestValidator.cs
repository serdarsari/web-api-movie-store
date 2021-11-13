using FluentValidation;
using MovieStore.DTO.AwardDTO;

namespace MovieStore.API.Validations.AwardValidations
{
    public class UpdateAwardRequestValidator : AbstractValidator<UpdateAwardRequest>
    {
        public UpdateAwardRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
        }
    }
}
