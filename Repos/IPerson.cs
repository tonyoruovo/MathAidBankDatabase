
using DataBaseTest.Models;
using System;
using System.Collections.Generic;

namespace DataBaseTest.Repos
{
    public interface IPerson
    {
        public IEntity Entity { get; }
        public IEntity OfficialEntity { get; }
        public MaritalStatus MaritalStatus { get; }
        public IPerson NextOfKin { get; }
        public INationality CountryOfOrigin { get; }
        public IFullName FullName { get; }
        public DateTime BirthDate { get; }
        public Guid UniqueTag { get; }
        public bool IsMale { get; }

        public IDictionary<string, string> Identification { get; }

        public byte Age();
        public string JobType { get; }
        public byte[] Passport { get; }
        public byte[] Signature { get; }
        public byte[] FingerPrint { get; }
    }
}
