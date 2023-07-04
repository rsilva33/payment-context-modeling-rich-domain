namespace PaymentContext.Tests.Entities;

[TestClass]
public class StudentTests
{
    private readonly Name _name;
    private readonly Email _email;
    private readonly Address _address;
    private readonly Document _document;
    private readonly Student _student;
    private readonly Subscription _subscription;
    private readonly PayPalPayment _payPalPayment;

    public StudentTests()
    {
        _name = new Name("Bruce", "Wayne");
        _document = new Document("36645378076", EDocumentType.CPF);
        _email = new Email("biru@liru.com");
        _address = new Address("Rua 1", "1234", "Parque real", "Diadema", "SP", "BR", "12345643");
        _payPalPayment = new PayPalPayment("12345673", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "Wayne corp", _document, _address, _email);


        _student = new Student(_name, _document, _email);
        _subscription = new Subscription(null);

    }

    [TestMethod]
    public void ShouldReturnErrorWhenHadActiveSubcription()
    {
        _subscription.AddPayment(_payPalPayment);
        _student.AddSubscription(_subscription);
        _student.AddSubscription(_subscription);

        Assert.IsTrue(_student.Invalid);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenSubcriptionHasNoPayment()
    {
        _student.AddSubscription(_subscription);

        Assert.IsTrue(_student.Valid);
    }

    [TestMethod]
    public void ShouldReturnSuccessWhenAddSubcription()
    {
        _subscription.AddPayment(_payPalPayment);
        _student.AddSubscription(_subscription);

        Assert.IsTrue(_student.Invalid);
    }
}