using FluentValidation;
using IntegrateCRM.Abstractions.Services.CRMService.Models;

namespace IntegrateCRM.Abstractions.Validators
{
	public class RegisterCRMModelValidator : AbstractValidator<RegisterCRMModel> , IValidatorModel
	{
		public RegisterCRMModelValidator()
		{
			RuleFor(model => model.Company).NotNull().NotEmpty();
			RuleFor(model => model.Email).NotNull().NotEmpty();
			RuleFor(model => model.Country).NotNull().NotEmpty();
			RuleFor(model => model.Phone).NotNull().NotEmpty();
			RuleFor(model => model.LastName).NotNull().NotEmpty();
			RuleFor(model => model.FirstName).NotNull().NotEmpty();
			RuleFor(model => model.Phone).NotNull().NotEmpty();
			RuleFor(model => model.FreeText).NotNull().NotEmpty();
		}
	}
}
