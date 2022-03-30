using DataBaseTest.Repos;
using System;
using System.Collections.Generic;

namespace DataBaseTest.Models
{
    public class Bank : Company<ITransaction>
    {
        public Bank(IEmployee head, IName name, IContactInfo contactInfo) : base(head, name, contactInfo)
        {
            customers = new List<ICustomer>();
        }

        public void AddEmail(string email)
        {
            ContactInfo.Emails.Add(email);
        }

        public void AddWebsite(string website)
        {
            ContactInfo.Websites.Add(website);
        }

        public void AddSocialMedia(string scm)
        {
            ContactInfo.SocialMedia.Add(scm);
        }

        public override List<IObserver<ITransaction>> GetClients()
        {
            return (List<IObserver<ITransaction>>)customers;
        }

        public override void Register(IObserver<ITransaction> o)
        {
            customers.Add((ICustomer)o);
        }

        public override void UnRegister(IObserver<ITransaction> o)
        {
            customers.Remove((ICustomer)o);
        }

        public override void Update()
        {
            //foreach(IObserver<ITransaction> o in GetClients())
            //  o.Inform()
        }

        protected bool GenerateAccountNumber()
        {
            IEmployee employee = Head;
            if (employee.Level >= 3)
            {
                _GenerateAccountNumber(employee.UniqueTag);
                return true;
            }
            return CannotReport(employee);
        }

        private bool CannotReport(IEmployee employee)
        {
            if (employee.Superior == null)
                return true;
            errorFrom.Add(employee);
            generationError = true;
            return !generationError;
        }

        private void _GenerateAccountNumber(Guid uniqueTag)
        {
            string s = numbers((uint)uniqueTag.GetHashCode()).ToString();
            try
            {
                s = s.Substring(0, 10);
            }
            catch (ArgumentOutOfRangeException)
            {
                s = s.PadLeft(10, '0');
            }
            availableAccountNumber = UInt32.Parse(s);
        }

        private IEnumerator<uint> numbers(uint seed)
        {
            uint t = seed += 0x6D2B79F5;
            while (true)
            {
                t = (uint)Math.BigMul((int)(t ^ t >> 15), (int)(t | 1));
                t ^= t + (uint)Math.BigMul((int)(t ^ t >> 7), (int)(t | 61));
                yield return (uint)(((t ^ t >> 14) >> 0) / 4294967296);
            }
        }

        private IList<ICustomer> customers;
        private bool generationError = false;
        private IList<IEmployee> errorFrom = new List<IEmployee>();
        private uint availableAccountNumber;
    }

    /*
	 * There should be an account status property that is an enum of one of the following values:
	 * INACTIVE, FROZEN, BLOCKED, LIMITED, ACTIVE. Each of these values will have a field called
	 * message of type string that will contain a descriptive comment stating the reason(s) for
	 * the current status.
	 */
}