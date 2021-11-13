using FluentValidation;
using MovieStore.DTO.DirectorDTO;

namespace MovieStore.API.Validations.DirectorValidations
{
    public class GetDirectorsRequestValidator : AbstractValidator<GetDirectorsRequest>
    {
        public GetDirectorsRequestValidator()
        {
            RuleFor(x => x.PageNumber).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).NotEmpty().GreaterThanOrEqualTo(5).LessThanOrEqualTo(20);
        }
    }
}
