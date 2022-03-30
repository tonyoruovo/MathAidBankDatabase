namespace DataBaseTest.Repos
{
    public interface IBuilder<TModel, TBuilder>
    {
        TModel Build();

        void Clear();

        TBuilder ReBuild(TModel model);
    }
}
