namespace PaymentContext.Tests.ValueObjects;

[TestClass]
public class DocumentTests
{
    //Red (deixar o codigo com falha), green(deixa o codigo certo), refactor
    [TestMethod]
    public void ShouldReturnErrorWhenCPNJIsInvalid()
    {
        var doc = new Document("123", EDocumentType.CNPJ);
        Assert.IsTrue(doc.Invalid);
    }

    [TestMethod]
    public void ShouldReturnSuccessWhenCPNJIsValid()
    {
        var doc = new Document("30118969000122", EDocumentType.CNPJ);
        Assert.IsTrue(doc.Valid);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenCPFIsInvalid()
    {
        var doc = new Document("123", EDocumentType.CPF);
        Assert.IsTrue(doc.Invalid);
    }

    [TestMethod]
    [DataTestMethod]
    [DataRow("34225545806")]
    [DataRow("54139739347")]
    [DataRow("01077284608")]
    public void ShouldReturnSuccessWhenCPFIsValid(string cpf)
    {
        var doc = new Document(cpf, EDocumentType.CPF);
        Assert.IsTrue(doc.Valid);
    }
}
