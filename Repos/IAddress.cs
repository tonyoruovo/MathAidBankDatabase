namespace DataBaseTest.Repos
{
    public interface IAddress
    {
        INationality CountryOfResidence
        {
            get;
        }
        public string StreetAddress { get; }
        public string ZIPCode { get; }
        public string PMB { get; }
    }
}
