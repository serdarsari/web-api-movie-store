using FluentValidation;
using MovieStore.DTO.MovieDTO;

namespace MovieStore.API.Validations.MovieValidations
{
    public class GetMoviesRequestValidator : AbstractValidator<GetMoviesRequest>
    {
        public GetMoviesRequestValidator()
        {
            RuleFor(x => x.PageNumber).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).NotEmpty().GreaterThanOrEqualTo(5).LessThanOrEqualTo(20);
        }
    }
}
