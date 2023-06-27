﻿namespace PaymentContext.Domain.Entities;

public class Subscription : Entity
{
    private IList<Payment> _payments;
    public Subscription(DateTime? expireDate)
    {
        ExpireDate = expireDate;
        CreateDate = DateTime.Now;
        LastUpdateDate = DateTime.Now;
        Active = true;
        _payments = new List<Payment>();
    }

    public bool Active { get; private set; }
    public DateTime CreateDate { get; private set; }
    public DateTime LastUpdateDate { get; private set; }
    public DateTime? ExpireDate { get; private set; }
    public IReadOnlyCollection<Payment> Payments { get { return _payments.ToArray(); } }

    public void AddPayment(Payment payment)
    {
        AddNotifications(new Flunt.Validations.Contract()
           .Requires()
           .IsGreaterThan(DateTime.Now, payment.PaidDate, "Subscription.Payments", "Invalid date.")
        );

        _payments.Add(payment);
    }

    public void Activate()
    {
        Active = true;
        LastUpdateDate = DateTime.Now;
    }

    public void Inactivate()
    {
        Active = false;
        LastUpdateDate = DateTime.Now;
    }
}