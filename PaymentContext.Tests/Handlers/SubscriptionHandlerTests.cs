namespace PaymentContext.Tests.Handlers;

[TestClass]
public class SubscriptionHandlerTests
{
    [TestMethod]
    public void ShouldReturnErrorWhenDocumentExists()
    {
        var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
        var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Teste";
            command.LastName = "Sobreteste";
            command.Document = "00099900099";
            command.Email = "hello@balta.io";
            command.BarCode = "123456789";
            command.BoletoNumber = "123456789";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now;
            command.Total = 110;
            command.TotalPaid = 120;
            command.Payer = "Wayne Corp";
            command.PayerDocument = "123456789";
            command.PaymentNumber = "234234343";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "payer@email.com";
            command.Street = "Diadema";
            command.Number = "123456789";
            command.Neighborhood = "Liuzi esquina";
            command.City = "Rio";
            command.State = "Janeiro";
            command.Country = "123456789";
            command.ZipCode = "0999210";

        handler.Handle(command);
        Assert.AreEqual(false, handler.Valid);
}
}
