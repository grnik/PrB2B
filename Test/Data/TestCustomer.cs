using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;
using NUnit;
using NUnit.Framework;

namespace Test.Data
{
    [TestFixture]
    class TestCustomer
    {
        [TestCase("TEST_GRIGORYEV")]
        public void GetAll(string customerNo)
        {
            List<DCustomer> customers = DCustomer.GetAll();
            Assert.That(customers.Count, Is.GreaterThan(1));
            Assert.IsTrue(customers.Exists(c=>c.Id==customerNo));
        }

        [TestCase("TEST_GRIGORYEV")]
        public void GetById(string customerNo)
        {
            DCustomer customer = DCustomer.GetById(customerNo);
            Assert.IsNotNull(customer);

            List<DCustomer> customers = DCustomer.GetAll();
            DCustomer custFind = customers.Find(c => c.Id == customerNo);
            Compare(custFind, customer);
        }

        void Compare(DCustomer cust1, DCustomer cust2)
        {
            Assert.That(cust1.Id, Is.EqualTo(cust2.Id));
            Assert.That(cust1.CommonNo, Is.EqualTo(cust2.CommonNo));
            Assert.That(cust1.BalanceType, Is.EqualTo(cust2.BalanceType));
            Assert.That(cust1.Currency, Is.EqualTo(cust2.Currency));
            Assert.That(cust1.Email, Is.EqualTo(cust2.Email));
            Assert.That(cust1.Name, Is.EqualTo(cust2.Name));
            Assert.That(cust1.Phone, Is.EqualTo(cust2.Phone));
            Assert.That(cust1.PriceType, Is.EqualTo(cust2.PriceType));
        }
    }
}
