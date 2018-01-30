using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using System;
using System.Linq;
using UnitTest.Ordering;
using Xunit;

public class OrderAggregateTest
{
    public OrderAggregateTest()
    { }

    [Fact]
    public void Create_order_item_success()
    {
		//Arrange    
		int productId = 1;
		string productName = "FakeProductName";
		int unitPrice = 12;
		int discount = 15;
		string pictureUrl = "FakeUrl";
		int units = 5;

		//Act 
		OrderItem fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.NotNull(fakeOrderItem);
    }

    [Fact]
    public void Invalid_number_of_units()
    {
		//Arrange    
		int productId = 1;
		string productName = "FakeProductName";
		int unitPrice = 12;
		int discount = 15;
		string pictureUrl = "FakeUrl";
		int units = -1;

        //Act - Assert
        Assert.Throws<OrderingDomainException>(() => new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units));
    }

    [Fact]
    public void Invalid_total_of_order_item_lower_than_discount_applied()
    {
		//Arrange    
		int productId = 1;
		string productName = "FakeProductName";
		int unitPrice = 12;
		int discount = 15;
		string pictureUrl = "FakeUrl";
		int units = 1;

        //Act - Assert
        Assert.Throws<OrderingDomainException>(() => new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units));
    }

    [Fact]
    public void Invalid_discount_setting()
    {
		//Arrange    
		int productId = 1;
		string productName = "FakeProductName";
		int unitPrice = 12;
		int discount = 15;
		string pictureUrl = "FakeUrl";
		int units = 5;

		//Act 
		OrderItem fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.Throws<OrderingDomainException>(() => fakeOrderItem.SetNewDiscount(-1));
    }

    [Fact]
    public void Invalid_units_setting()
    {
		//Arrange    
		int productId = 1;
		string productName = "FakeProductName";
		int unitPrice = 12;
		int discount = 15;
		string pictureUrl = "FakeUrl";
		int units = 5;

		//Act 
		OrderItem fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.Throws<OrderingDomainException>(() => fakeOrderItem.AddUnits(-1));
    }

    [Fact]
    public void WhenAddTwoTimesOnTheSameItemThenTheTotalOfOrderShouldBeTheSumOfTheTwoItems()
    {
		Address address = new AddressBuilder().Build();
		Order order = new OrderBuilder(address)
            .AddOne(1,"cup",10.0m,0,string.Empty)
            .AddOne(1,"cup",10.0m,0,string.Empty)
            .Build();

        Assert.Equal(20.0m, order.GetTotal());
    }

    [Fact]
    public void Add_new_Order_raises_new_event()
    {
		//Arrange
		string street = "fakeStreet";
		string city = "FakeCity";
		string state = "fakeState";
		string country = "fakeCountry";
		string zipcode = "FakeZipCode";
		int cardTypeId = 5;
		string cardNumber = "12";
		string cardSecurityNumber = "123";
		string cardHolderName = "FakeName";
		DateTime cardExpiration = DateTime.Now.AddYears(1);
		int expectedResult = 1;

		//Act 
		Order fakeOrder = new Order("1", new Address(street, city, state, country, zipcode), cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        //Assert
        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }

    [Fact]
    public void Add_event_Order_explicitly_raises_new_event()
    {
		//Arrange   
		string street = "fakeStreet";
		string city = "FakeCity";
		string state = "fakeState";
		string country = "fakeCountry";
		string zipcode = "FakeZipCode";
		int cardTypeId = 5;
		string cardNumber = "12";
		string cardSecurityNumber = "123";
		string cardHolderName = "FakeName";
		DateTime cardExpiration = DateTime.Now.AddYears(1);
		int expectedResult = 2;

		//Act 
		Order fakeOrder = new Order("1", new Address(street, city, state, country, zipcode), cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
        fakeOrder.AddDomainEvent(new OrderStartedDomainEvent(fakeOrder, "1", cardTypeId,cardNumber,cardSecurityNumber,cardHolderName,cardExpiration));
        //Assert
        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }

    [Fact]
    public void Remove_event_Order_explicitly()
    {
		//Arrange    
		string street = "fakeStreet";
		string city = "FakeCity";
		string state = "fakeState";
		string country = "fakeCountry";
		string zipcode = "FakeZipCode";
		int cardTypeId = 5;
		string cardNumber = "12";
		string cardSecurityNumber = "123";
		string cardHolderName = "FakeName";
		DateTime cardExpiration = DateTime.Now.AddYears(1);
		Order fakeOrder = new Order("1", new Address(street, city, state, country, zipcode), cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
		OrderStartedDomainEvent @fakeEvent = new OrderStartedDomainEvent(fakeOrder, "1", cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
		int expectedResult = 1;

        //Act         
        fakeOrder.AddDomainEvent(@fakeEvent);
        fakeOrder.RemoveDomainEvent(@fakeEvent);
        //Assert
        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }
}