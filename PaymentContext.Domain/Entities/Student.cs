namespace PaymentContext.Domain.Entities;

public class Student : Entity
{
    private IList<Subscription> _subscriptions;
    public Student
    (
        Name name,
        Document document,
        Email email
    )
    {
        Name = name;
        Document = document;
        Email = email;

        _subscriptions = new List<Subscription>();

        AddNotifications(name, document, email);
    }

    public Name Name { get; private set; }
    public Document Document { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }

    public IReadOnlyCollection<Subscription> Subscriptions
    {
        get
        {
            return _subscriptions.ToArray();
        }
    }

    public void AddSubscription(Subscription subscription)
    {
        if(subscription.Payments.Count == 0) { }
        var hasSubscriptionActive = false;

        foreach (var sub in _subscriptions)
        {
            if (sub.Active)
                hasSubscriptionActive = true;
        }

        AddNotifications(new Flunt.Validations.Contract()
            .Requires()
            .IsFalse(hasSubscriptionActive, "Student.Subscriptions", "You already have an active subscription.")
            .AreEquals(0, subscription.Payments.Count, "Student.Subscription.Payments", "This subscription has no payments.")
            );

        // Alternativa
        // if (hasSubscriptionActive)
        //     AddNotification("Student.Subscriptions", "Você já tem uma assinatura ativa");
    }
}