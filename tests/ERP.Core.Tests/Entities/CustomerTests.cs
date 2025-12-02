using ERP.Core.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace ERP.Core.Tests.Entities;

public class CustomerTests
{
    [Fact]
    public void Customer_Properties_ShouldSetCorrectly()
    {
        // Arrange: Create customer with specific sample data
        var customer = new Customer
        {
            Name = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        // Assert: Verify properties map correctly to the entity
        Assert.Equal("John", customer.Name);
        Assert.Equal("Doe", customer.LastName);
        Assert.Equal("john@example.com", customer.Email);
    }

    [Fact]
    public void Customer_IsActive_DefaultsToTrue()
    {
        // Arrange: Create a new customer
        var customer = new Customer();
        
        // Assert: Verify IsActive is true by default
        Assert.True(customer.IsActive);
    }

    [Fact]
    public void Customer_Address_CanBeNull()
    {
        // Arrange: Create a new customer without setting the address
        var customer = new Customer();
        
        // Assert: Verify Address is null (since it is an optional field)
        Assert.Null(customer.Address);
    }
}
