﻿namespace PaymentContext.Tests.Commands;

[TestClass]
public class CreateBoletoSubscriptionCommandTests
{
    [TestMethod]
    public void ShouldReturnErrorWhenNameIsInvalid()
    {
        var command = new CreateBoletoSubscriptionCommand();

        command.FirstName = "Test";
        command.Validate();

        Assert.AreEqual(false, command.Valid);
    }
}
