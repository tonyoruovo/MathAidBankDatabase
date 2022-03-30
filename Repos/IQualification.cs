using System.Collections.Generic;

namespace DataBaseTest.Repos
{
    public interface IQualification
    {
        IDictionary<string, string> Grades { get; }
        string Certification { get; }
        IAddress Academy { get; }
    }
}