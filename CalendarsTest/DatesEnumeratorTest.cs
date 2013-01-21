using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CalendarsLib;
using System.Reflection;


namespace CalendarsTest
{
    [TestClass]
    public class DatesEnumeratorTest
    {

        private DateTime _today;
        private DateTime _minDate;
        private DateTime _maxDate;

        [TestInitialize]
        public void Init()
        {
            _today = DateTime.Today;
            _minDate = _today.AddDays(-10);
            _maxDate = _today.AddDays(10);
        }


        [TestMethod]
        public void EmptyConstructorTest()
        {
            var mock = new Mock<DatesEnumerator>() { CallBase = true };
            DatesEnumerator enumerator = mock.Object;
            Assert.AreEqual(DateTime.MinValue, enumerator.MinValue, "Minimum date not initialized with expected default value.");
            Assert.AreEqual(DateTime.MaxValue, enumerator.MaxValue, "Maximum date not initialized with expected default value.");
            Assert.IsTrue(DateTime.Now.Subtract(enumerator.Current).TotalSeconds <= 1.0, "Initial date not initialized as Now.");
        }

        [TestMethod]
        public void InitialDateConstructorTest()
        {
            var mock = new Mock<DatesEnumerator>(_today) { CallBase = true };
            DatesEnumerator enumerator = mock.Object;
            Assert.AreEqual(DateTime.MinValue, enumerator.MinValue, "Minimum date not initialized with expected default value.");
            Assert.AreEqual(DateTime.MaxValue, enumerator.MaxValue, "Maximum date not initialized with expected default value.");
            Assert.AreEqual(_today,enumerator.Current, "Initial date not initialized as Now.");
        }

        [TestMethod]
        public void RangeConstructorTest()
        {
            var mock = new Mock<DatesEnumerator>(_minDate,_maxDate) { CallBase = true };
            DatesEnumerator enumerator = mock.Object;
            Assert.AreEqual(_minDate, enumerator.MinValue, "Minimum date not initialized as expected.");
            Assert.AreEqual(_maxDate, enumerator.MaxValue, "Maximum date not initialized as expected.");
            Assert.AreEqual(_minDate, enumerator.Current, "Initial date not initialized as the minimum value.");
        }

        [TestMethod]
        public void CorrectConstructorTest()
        {
            var mock = new Mock<DatesEnumerator>(_today,_minDate,_maxDate) { CallBase = true };
            DatesEnumerator enumerator = mock.Object;
            mock.Verify(x=>x.IsPossible(_today),Times.Exactly(1),"IsPossible method is not called in constructor to check validity of initial date.");
            Assert.AreEqual(_minDate, enumerator.MinValue,"Minimum date not initialized as expected.");
            Assert.AreEqual(_maxDate,enumerator.MaxValue,"Maximum date not initialized as expected.");
            Assert.AreEqual(_today,enumerator.Current, "Initial date not initilialized as expected.");
        }

        [TestMethod]
        public void IsPossibleTest()
        {
            var mock = new Mock<DatesEnumerator>(_minDate, _maxDate) { CallBase = true };
            DatesEnumerator enumerator = mock.Object;
            Assert.IsTrue(enumerator.IsPossible(_today),"Valid date detected as impossible.");
            Assert.IsFalse(enumerator.IsPossible(_minDate.AddDays(-1.0)), "Invalid date detected as possible.");
            Assert.IsFalse(enumerator.IsPossible(_maxDate.AddDays(1.0)), "Invalid date detected as possible.");
        }
    }
}
