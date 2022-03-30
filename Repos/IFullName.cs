using System.Collections.Generic;

namespace DataBaseTest.Repos
{
    /// <summary>
    /// Will be adding maiden name
    /// </summary>
    public interface IFullName
    {
        IName Name { get; }

        IName Surname { get; }
        string Title { get; }
        IName Nickname { get; }
        IName MaidenName { get; }
        IEnumerable<IName> OtherNames { get; }
    }
}
