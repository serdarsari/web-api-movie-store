using FluentValidation;
using MovieStore.DTO.AwardDTO;

namespace MovieStore.API.Validations.AwardValidations
{
    public class GetAwardsRequestValidator : AbstractValidator<GetAwardsRequest>
    {
        public GetAwardsRequestValidator()
        {
            RuleFor(x => x.PageNumber).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).NotEmpty().GreaterThanOrEqualTo(5).LessThanOrEqualTo(20);
        }
    }
}
