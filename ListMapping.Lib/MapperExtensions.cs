using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ListMapping
{
    public static class MapperExtensions
    {
        private static readonly Mapper _mapper = new Mapper();

        // Overload for String Properties
        public static void MapList<TSource, TDestination>(
            this IList<TDestination> destinationList,
            IList<TSource> sourceList,
            string sourceIdProperty = "Id",
            string destinationIdProperty = "Id")
        {
            _mapper.MapList(sourceList, destinationList, sourceIdProperty, destinationIdProperty);
        }

        // Overload for Lambda Expressions
        public static void MapList<TSource, TDestination, IdType>(
            this IList<TDestination> destinationList,
            IList<TSource> sourceList,
            Expression<Func<TSource, IdType>> sourceIdSelector,
            Expression<Func<TDestination, IdType>> destinationIdSelector)
        {
            _mapper.MapList(sourceList, destinationList, sourceIdSelector, destinationIdSelector);
        }
    }
}
