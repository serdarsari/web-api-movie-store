using FluentValidation;
using MovieStore.DTO.ActorDTO;

namespace MovieStore.API.Validations.ActorValidations
{
    public class GetActorsRequestValidator : AbstractValidator<GetActorsRequest>
    {
        public GetActorsRequestValidator()
        {
            RuleFor(x => x.PageNumber).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).NotEmpty().GreaterThanOrEqualTo(5).LessThanOrEqualTo(20);
        }
    }
}
