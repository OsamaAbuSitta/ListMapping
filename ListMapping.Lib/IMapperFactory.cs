namespace ListMapping
{
    public interface IMapperFactory
    {
        Func<TSource, TDestination, TDestination> Create<TSource, TDestination>();
    }
}