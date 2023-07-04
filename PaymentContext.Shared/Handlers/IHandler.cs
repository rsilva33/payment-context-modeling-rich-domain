namespace PaymentContext.Shared.Handlers;

public interface IHandler<T> where T : Commands.ICommand
{
    ICommandResult Handle(T command);
}
