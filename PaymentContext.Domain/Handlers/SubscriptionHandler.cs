using PaymentContext.Domain.Services;

namespace PaymentContext.Domain.Handlers;

public class SubscriptionHandler :
    Notifiable,
    IHandler<CreateBoletoSubscriptionCommand>,
    IHandler<CreatePayPalSubscriptionCommand>
{
    private readonly IStudentRepository _repository;
    private readonly IEmailService _emailService;

    public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
    {
        // Fail Fast Validations
        command.Validate();

        if (command.Invalid)
        {
            AddNotifications(command);
            return new CommandResult(false, "Unable to complete your subscription.");
        }

        // Check if Document is already registered
        if (_repository.DocumentExists(command.Document))
            AddNotification("Document", "This CPF is already in use.");

        // Check if E-mail is already registered
        if (_repository.EmailExists(command.Email))
            AddNotification("Email", "This email is already in use.");

        // Generate the VOs
        var name = new Name(command.FirstName, command.LastName);
        var document = new Document(command.Document, EDocumentType.CPF);
        var email = new Email(command.Email);
        var address = new Address(
            command.Street,
            command.Number,
            command.Neighborhood,
            command.City,
            command.State,
            command.Country,
            command.ZipCode);

        // Generate Entities
        var student = new Student(name, document, email);
        var subscription = new Subscription(DateTime.Now.AddMonths(1));
        var payment = new BoletoPayment(
            command.BarCode,
            command.BoletoNumber,
            command.PaidDate,
            command.ExpireDate,
            command.Total,
            command.TotalPaid,
            command.Payer,
            new Document(command.PayerDocument, command.PayerDocumentType),
            address,
            email
        );

        // Relationships
        subscription.AddPayment(payment);
        student.AddSubscription(subscription);

        // Group the Validations
        AddNotifications(name, document, email, address, student, subscription, payment);

        // Check notifications
        if (Invalid)
            return new CommandResult(false, "Unable to complete your subscription.");

        // Save the information
        _repository.CreateSubscription(student);

        // Send Welcome Email
        _emailService.Send(student.Name.ToString(), student.Email.Address, "Welcome", "Your signature has been created.");

        // Return information
        return new CommandResult(true, "Successful subscription.");
    }

    public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
    {
        // Check if Document is already registered
        if (_repository.DocumentExists(command.Document))
            AddNotification("Document", "This CPF is already in use.");

        // Check if E-mail is already registered
        if (_repository.EmailExists(command.Email))
            AddNotification("Email", "This email is already in use.");

        // Generate the VOs
        var name = new Name(command.FirstName, command.LastName);
        var document = new Document(command.Document, EDocumentType.CPF);
        var email = new Email(command.Email);
        var address = new Address(
            command.Street,
            command.Number,
            command.Neighborhood,
            command.City,
            command.State,
            command.Country,
            command.ZipCode
        );

        // Generate Entities
        var student = new Student(name, document, email);
        var subscription = new Subscription(DateTime.Now.AddMonths(1));

        // It only changes the Payment implementation
        var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );

        // Relationships
        subscription.AddPayment(payment);
        student.AddSubscription(subscription);

        // Group the Validations
        AddNotifications(name, document, email, address, student, subscription, payment);


        // Check notifications
        _repository.CreateSubscription(student);

        // Send Welcome Email
        _emailService.Send(student.Name.ToString(), student.Email.Address, "Welcome", "Your signature has been created.");

        // Return information
        return new CommandResult(true, "Successful subscription.");
    }
}
