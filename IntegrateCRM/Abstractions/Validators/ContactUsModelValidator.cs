using FluentValidation;
using IntegrateCRM.Abstractions.Models;

namespace IntegrateCRM.Abstractions.Validators
{
	public class ContactUsModelValidator : AbstractValidator<ContactUsModel> , IValidatorModel
	{
		public ContactUsModelValidator()
		{
			RuleFor(model => model.Company).NotNull().NotEmpty();
			RuleFor(model => model.Email).NotNull().NotEmpty();
			RuleFor(model => model.Country).NotNull().NotEmpty();
			RuleFor(model => model.Message).NotNull().NotEmpty();
			RuleFor(model => model.Name).NotNull().NotEmpty();
			RuleFor(model => model.PhoneNumber).NotNull().NotEmpty();
		}
	}
}
