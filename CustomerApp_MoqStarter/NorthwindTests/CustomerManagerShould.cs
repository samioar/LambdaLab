using Moq;
using NorthwindBusiness;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;

namespace NorthwindTests
{
    public class CustomerManagerShould
    {
        private CustomerManager _sut;

        [Test]
        [Category("Happy Path")]
        public void BeAbleToBeConstructed()
        {
            //arrange
            var mockCustomerService = new Mock<ICustomerService>();
            //act
            _sut = new CustomerManager(mockCustomerService.Object);
            //assert
            Assert.That(_sut, Is.InstanceOf<CustomerManager>());
        }

        //stubs

        [Test]
        [Category("Happy Path")]
        public void ReturnTrue_WhenUpdateIsCalledWithValidId()
        {
            //arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer() { CustomerId = "MANDAL" };
            //act
            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns(originalCustomer);
            _sut = new CustomerManager(mockCustomerService.Object);
            var result = _sut.Update("MANDAL", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //assert
            Assert.That(result);
        }

        [Test]
        [Category("Happy Path")]
        public void UpdateSelectedCustomer_WhenUpdateIsCalledWithValidId()
        {
            //arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer() { CustomerId = "MANDAL", ContactName = "Nish Mandal", CompanyName = "Sparta Global", City = "Birmingham" };
            //act
            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns(originalCustomer);
            _sut = new CustomerManager(mockCustomerService.Object);
            var result = _sut.Update("MANDAL", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //assert
            Assert.That(result);
        }

        [Test]
        [Category("Happy Path")]
        public void UpdateSelectedCustomer_WhenUpdateIsCalled_WithValidId()
        { // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer { CustomerId = "MANDAL", ContactName = "Nish Mandal", CompanyName = "Sparta Global", City = "Birmingham" };
            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns(originalCustomer);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Update("MANDAL", "Nish Kumar", "UK", "London", null);
            // Assert
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Nish Kumar"));
            Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Sparta Global"));
            Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo("UK"));
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("London"));
        }

        [Test]
        [Category("Sad Path")]
        public void ReturnFalse_WhenUpdateIsCalled_WithInvalidId()
        { // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Update("MANDAL", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            // Assert

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Happy Path")]
        public void NotChangeTheSelectedCustomer_WhenUpdateIsCalledWithValidId()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer() { CustomerId = "MANDAL", ContactName = "Nish Mandal", CompanyName = "Sparta Global", City = "London", Country = "UK" };

            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);
            _sut.SelectedCustomer = originalCustomer;

            _sut.Update("KUMAR", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Nish Mandal"));
            Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Sparta Global"));
            Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo("UK"));
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("London"));
        }

        [Test]
        [Category("Happy Path")]
        public void DeleteCustomer_ReturnsTrue()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer() { CustomerId = "MANDAL" };
            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns(originalCustomer);
            _sut = new CustomerManager(mockCustomerService.Object);
            //Act]
            var results = _sut.Delete("MANDAL");
            //Assert
            _sut.SelectedCustomer = originalCustomer;
            Assert.IsTrue(results);
            Assert.That(_sut.SelectedCustomer, Is.Not.Null);
        }

        [Test]
        [Category("Sad Path")]
        public void DeleteCustomer_ReturnsFalse()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            var originalCustomer = new Customer
            {
                CustomerId = "MANDAL"
            };
            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);
            //Act]
            _sut.SelectedCustomer = originalCustomer;
            var results = _sut.Delete("MANDAL");
            //Assert
            Assert.IsFalse(results);
            Assert.That(_sut.SelectedCustomer.CustomerId, Is.EqualTo("MANDAL"));
        }

        [Test]
        [Category("Sad Path")]
        public void ReturnsFalse_WhenUpdateIsCalled_AndDataBaseExceptionIsThrowed()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
            mockCustomerService.Setup(cs => cs.SaveCustomerChanges()).Throws<DataException>();
            _sut = new CustomerManager(mockCustomerService.Object);
            var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(result, Is.False);
        }

        [Test]
        public void CallSaveCustomerChanges_WhenUpdateIsCalled_WithValidId()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns(new Customer());
            _sut = new CustomerManager(mockCustomerService.Object);
            var result = _sut.Update("MANDAL", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.Exactly(1));
        }

        [Test]
        public void LetsSeeWhatHappends_WhenUpdateIsCalled_IfAllMethodCallAreNotSetup()
        {
            var mockCustomerService = new Mock<ICustomerService>(MockBehavior.Strict);
            mockCustomerService.Setup(cs => cs.GetCustomerById("MANDAL")).Returns(new Customer());
            mockCustomerService.Setup(cs => cs.SaveCustomerChanges());
            _sut = new CustomerManager(mockCustomerService.Object);
            var result = _sut.Update("MANDAL", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.Exactly(1));
            Assert.That(result);
        }

        //----------------------------------------------------------------------------------------------------------------------------//
        [Test]
        [Category("Happy Path")]
        public void TestingRetrieveAll_ReturnsCorrectList()
        {
            // Arrange
            var originalCustomers = new List<Customer>
    {
        new Customer
        {
            CustomerId = "MANDAL",
            ContactName = "Nish Mandal",
            CompanyName = "Sparta Global",
            City = "London",
            Country = "UK"
        },
        new Customer
        {
            CustomerId = "DOE",
            ContactName = "John Doe",
            CompanyName = "Tencent",
            City = "Shenzhen",
            Country = "China"
        }
    };

            var expectedCustomerList = originalCustomers;

            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(cs => cs.GetCustomerList()).Returns(expectedCustomerList);

            var customerManager = new CustomerManager(mockCustomerService.Object);

            var customerList = customerManager.RetrieveAll();

            mockCustomerService.Verify(cs => cs.GetCustomerList(), Times.Exactly(1));
            Assert.That(customerList, Is.EqualTo(expectedCustomerList));
            CollectionAssert.Contains(expectedCustomerList, originalCustomers[0]); //tests for mandal ???
            CollectionAssert.Contains(expectedCustomerList, originalCustomers[1]);// tests for john
        }

        [Test]
        public void TestingCreateCustomer()
        {
            var mockCustomerService = new Mock<ICustomerService>();

            var originalCustomer = new Customer
            {
                CustomerId = "MANDAL",
                ContactName = "Nish Mandal",
                CompanyName = "Sparta Global"
            };

            mockCustomerService.Setup(cs => cs.CreateCustomer(originalCustomer));
            CustomerManager _sut = new CustomerManager(mockCustomerService.Object);
            _sut.Create("MANDAL", "Nish Mandal", "Sparta Global");

            mockCustomerService.Verify(cs => cs.CreateCustomer(originalCustomer), Times.Exactly(1)); //It.IsAny<Customer>?

        }


    }
}