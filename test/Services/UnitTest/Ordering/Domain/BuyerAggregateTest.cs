using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.Exceptions;
using System;
using Xunit;

public class BuyerAggregateTest
{
    public BuyerAggregateTest()
    { }

    [Fact]
    public void Create_buyer_item_success()
    {
		//Arrange    
		string identity = new Guid().ToString();

		//Act 
		Buyer fakeBuyerItem = new Buyer(identity);

        //Assert
        Assert.NotNull(fakeBuyerItem);
    }

    [Fact]
    public void Create_buyer_item_fail()
    {
		//Arrange    
		string identity = string.Empty;

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => new Buyer(identity));
    }

    [Fact]
    public void Add_payment_success()
    {
		//Arrange    
		int cardTypeId = 1;
		string alias = "fakeAlias";
		string cardNumber = "124";
		string securityNumber = "1234";
		string cardHolderName = "FakeHolderNAme";
		DateTime expiration = DateTime.Now.AddYears(1);
		int orderId = 1;
		string identity = new Guid().ToString();
		Buyer fakeBuyerItem = new Buyer(identity);

		//Act
		PaymentMethod result = fakeBuyerItem.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration, orderId);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreatePaymentMethodSuccess()
    {
		//Arrange    
		int cardTypeId = 1;
		string alias = "fakeAlias";
		string cardNumber = "124";
		string securityNumber = "1234";
		string cardHolderName = "FakeHolderNAme";
		DateTime expiration = DateTime.Now.AddYears(1);
		PaymentMethod fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

		//Act
		PaymentMethod result = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreatePaymentMethodExpirationFail()
    {
		//Arrange    
		int cardTypeId = 1;
		string alias = "fakeAlias";
		string cardNumber = "124";
		string securityNumber = "1234";
		string cardHolderName = "FakeHolderNAme";
		DateTime expiration = DateTime.Now.AddYears(-1);        

        //Act - Assert
        Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration));
    }

    [Fact]
    public void PaymentMethod_isEqualTo()
    {
		//Arrange    
		int cardTypeId = 1;
		string alias = "fakeAlias";
		string cardNumber = "124";
		string securityNumber = "1234";
		string cardHolderName = "FakeHolderNAme";
		DateTime expiration = DateTime.Now.AddYears(1);

		//Act
		PaymentMethod fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);
		bool result = fakePaymentMethod.IsEqualTo(cardTypeId, cardNumber, expiration);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Add_new_PaymentMethod_raises_new_event()
    {
		//Arrange    
		string alias = "fakeAlias";
		int orderId = 1;
		int cardTypeId = 5;
		string cardNumber = "12";
		string cardSecurityNumber = "123";
		string cardHolderName = "FakeName";
		DateTime cardExpiration = DateTime.Now.AddYears(1);
		int expectedResult = 1;

		//Act 
		Buyer fakeBuyer = new Buyer(Guid.NewGuid().ToString());
        fakeBuyer.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, orderId);

        //Assert
        Assert.Equal(fakeBuyer.DomainEvents.Count, expectedResult);
    }
}