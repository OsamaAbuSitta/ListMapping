using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListMapping.Tests
{
    public class DefaultMapperFactory : IMapperFactory
    {
        public Func<TSource, TDestination, TDestination> Create<TSource, TDestination>()
        {
            return (TSource source, TDestination destination) =>
            {
                if (destination != null)
                {
                    source.Adapt(destination);
                    return destination;
                }
                return source.Adapt<TDestination>();
            };
        }
    }
}
