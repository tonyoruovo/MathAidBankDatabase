
namespace DataBaseTest.Repos
{
    public interface IEntity
    {
        /// <summary>
        /// This is also the relationship of an IEmployee to its next of kin property
        /// or an ICustomer to its next of kin property.
        /// </summary>
        public IName Name { get; }
        public IContact Contact { get; }
    }
}
