
using System.Collections.Generic;

namespace DataBaseTest.Repos
{
    public interface IEducation
    {
        IQualification Primary
        {
            get;
        }
        IQualification Secondary
        {
            get;
        }

        IQualification PrimaryTertiary
        {
            get;
        }

        IEnumerable<IQualification> Others
        {
            get;
        }
    }
}
