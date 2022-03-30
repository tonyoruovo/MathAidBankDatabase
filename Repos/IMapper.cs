namespace DataBaseTest.Repos
{
    public interface IMapper<TFrom, TTo>
    {
        TTo Map(TFrom from);

        TFrom ReverseMap(TTo to);
    }
}
