using ChatApp.Domain.Models.Messages;
using FluentValidation;

namespace ChatApp.Domain.Validators.MessageValidator;

public class MessageValidator : AbstractValidator<MessageInputModel>
{
    public MessageValidator()
    {
        RuleFor(message => message).NotNull().WithMessage("The input cannot be null, please provide a valid message.");
        //RuleFor(message => message.UserId).NotNull().WithMessage("The message must be associated to a User.");
        RuleFor(message => message.Content).NotNull().NotEmpty().WithMessage("The content of the message cannot be null, please provide a valid message.");
        RuleFor(message => message.Content).MaximumLength(1000).WithMessage("The content of the message exceeds the max of 1000 characters.");
    }
}