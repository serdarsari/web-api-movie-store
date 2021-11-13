using FluentValidation;
using MovieStore.DTO.DirectorDTO;

namespace MovieStore.API.Validations.DirectorValidations
{
    public class UpdateDirectorRequestValidator : AbstractValidator<UpdateDirectorRequest>
    {
        public UpdateDirectorRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }
}
