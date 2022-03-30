namespace DataBaseTest.Repos
{
    public interface INationality
    {
        public IName CountryName { get; }
        public string Language { get; }
        public IName State { get; }
        public string LGA { get; }
        public IName CityTown { get; }
    }
}
