using System.Collections.Generic;

namespace DataBaseTest.Repos
{
    public interface IContact
    {
        IEnumerable<string> Emails { get; }
        IEnumerable<string> SocialMedia { get; }
        IEnumerable<string> Mobiles { get; }
        IEnumerable<string> Websites { get; }
        IAddress Address { get; }
    }
}
