using FluentValidation;
using MovieStore.DTO.MovieDTO;

namespace MovieStore.API.Validations.MovieValidations
{
    public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
    {
        public UpdateMovieRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Budget).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Rating).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Month).NotEmpty();
            RuleFor(x => x.Year).GreaterThan(1900);
            RuleFor(x => x.Storyline).NotEmpty();
        }
    }
}
