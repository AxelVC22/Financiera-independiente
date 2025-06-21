using Independiente.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Repository
{
    [TestClass]
    public class FieldValidatorTest
    {
        [TestMethod]
        public void TestValidPasswordSuccess()
        {
            Assert.IsTrue(FieldValidator.IsValidPassword("Password1!"));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidPasswordFail()
        {
            FieldValidator.IsValidPassword("pass");
        }

        [TestMethod]
        public void TestValidEmailSuccess()
        {
            Assert.IsTrue(FieldValidator.IsValidEmail("correo@ejemplo.com"));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidEmailFail()
        {
            FieldValidator.IsValidEmail("correo@com");
        }

        [TestMethod]
        public void TestValidNameSuccess()
        {
            Assert.IsTrue(FieldValidator.IsValidName("Juan Pérez"));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidNameFail()
        {
            FieldValidator.IsValidName("J1");
        }

        [TestMethod]
        public void TestValidRFC()
        {
            Assert.IsTrue(FieldValidator.IsValidRFC("ARPS920101ABC"));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidRFC()
        {
            FieldValidator.IsValidRFC("1234");
        }

        [TestMethod]
        public void TestValidCURP()
        {
            Assert.IsTrue(FieldValidator.IsValidCURP("MOPG910101HMCLRL01"));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidCURP()
        {
            FieldValidator.IsValidCURP("MOPG123");
        }

        [TestMethod]
        public void TestValidPhoneNumber()
        {
            Assert.IsTrue(FieldValidator.IsValidPhoneNumber("5512345678"));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidPhoneNumber()
        {
            FieldValidator.IsValidPhoneNumber("12345");
        }

        [TestMethod]
        public void TestValidMoney()
        {
            Assert.IsTrue(FieldValidator.IsValidMoney(1000.50m));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidMoney()
        {
            FieldValidator.IsValidMoney(-100);
        }

        [TestMethod]
        public void TestValidCLABE()
        {
            Assert.IsTrue(FieldValidator.IsValidCLABE("123456789012345678"));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidCLABE()
        {
            FieldValidator.IsValidCLABE("12345");
        }

        [TestMethod]
        public void TestValidDate()
        {
            Assert.IsTrue(FieldValidator.IsValidDate(DateTime.Now.AddDays(-1)));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidDate()
        {
            FieldValidator.IsValidDate(DateTime.Now.AddDays(1));
        }

        [TestMethod]
        public void TestValidLoanTerm()
        {
            Assert.IsTrue(FieldValidator.IsValidLoanTerm(12));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidLoanTerm()
        {
            FieldValidator.IsValidLoanTerm(0);
        }

        [TestMethod]
        public void TestValidInterestRate()
        {
            Assert.IsTrue(FieldValidator.IsValidInterestRate(10.5m));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidInterestRate()
        {
            FieldValidator.IsValidInterestRate(-1);
        }

        [TestMethod]
        public void TestValidDescription()
        {
            Assert.IsTrue(FieldValidator.IsValidDescription("Préstamo personal mensual, cliente confiable."));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestInvalidDescription()
        {
            FieldValidator.IsValidDescription("a");
        }
    }
}
