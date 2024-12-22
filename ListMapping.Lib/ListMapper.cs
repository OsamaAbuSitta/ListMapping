using Mapster;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace ListMapping
{
    public class Mapper
    {
        // Static cache for compiled property accessors
        private readonly ConcurrentDictionary<(Type, string), object> _idAccessorCache
            = new ConcurrentDictionary<(Type, string), object>();

        /// <summary>
        /// MapList with string property names.
        /// </summary>
        public void MapList<TSource, TDestination>(
            IList<TSource> sourceList,
            IList<TDestination> destinationList,
            string sourceIdProperty = "Id",
            string destinationIdProperty = "Id")
        {
            if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));
            if (destinationList == null) throw new ArgumentNullException(nameof(destinationList));

            PropertyInfo sourceIdpropertyInfo = typeof(TSource).GetProperty(sourceIdProperty,
             BindingFlags.Public | BindingFlags.Instance);

            if (sourceIdpropertyInfo == null)
            {
                throw new ArgumentException($"Property '{sourceIdProperty}' not found on type '{typeof(TSource).Name}'");
            }

            PropertyInfo destinationIdPropertyInfo = typeof(TDestination).GetProperty(destinationIdProperty,
                BindingFlags.Public | BindingFlags.Instance);

            if (destinationIdPropertyInfo == null)
            {
                throw new ArgumentException($"Property '{destinationIdProperty}' not found on type '{typeof(TDestination).Name}'");
            }


            var sourceIdAccessor =  GetIdAccessor<TSource, object>(sourceIdpropertyInfo);
            var destinationIdAccessor = GetIdAccessor<TDestination, object>(destinationIdPropertyInfo);

            PerformMapping(sourceList, destinationList, sourceIdAccessor, destinationIdAccessor);
        }

        /// <summary>
        /// MapList with lambda selectors.
        /// </summary>
        public void MapList<TSource, TDestination, IdType>(
            IList<TSource> sourceList,
            IList<TDestination> destinationList,
            Expression<Func<TSource, IdType>> sourceIdSelector,
            Expression<Func<TDestination, IdType>> destinationIdSelector)
        {
            if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));
            if (destinationList == null) throw new ArgumentNullException(nameof(destinationList));
            if (sourceIdSelector == null) throw new ArgumentNullException(nameof(sourceIdSelector));
            if (destinationIdSelector == null) throw new ArgumentNullException(nameof(destinationIdSelector));

            var sourceIdProperty = GetPropertyName(sourceIdSelector);
            var destinationIdProperty = GetPropertyName(destinationIdSelector);


            PropertyInfo sourceIdpropertyInfo = typeof(TSource).GetProperty(sourceIdProperty,
             BindingFlags.Public | BindingFlags.Instance);

            if (sourceIdpropertyInfo == null)
            {
                throw new ArgumentException($"Property '{sourceIdProperty}' not found on type '{typeof(TSource).Name}'");
            }

            PropertyInfo destinationIdPropertyInfo = typeof(TDestination).GetProperty(destinationIdProperty,
                BindingFlags.Public | BindingFlags.Instance);

            if (destinationIdPropertyInfo == null)
            {
                throw new ArgumentException($"Property '{destinationIdProperty}' not found on type '{typeof(TDestination).Name}'");
            }



            var sourceIdAccessor = GetIdAccessor<TSource, IdType>(sourceIdpropertyInfo);
            var destinationIdAccessor = GetIdAccessor<TDestination, IdType>(destinationIdPropertyInfo);

            PerformMapping(sourceList, destinationList, sourceIdAccessor, destinationIdAccessor);
        }

        /// <summary>
        /// Core mapping logic.
        /// </summary>
        private void PerformMapping<TSource, TDestination, IdType>(
            IList<TSource> sourceList,
            IList<TDestination> destinationList,
            Func<TSource, IdType> sourceIdAccessor,
            Func<TDestination, IdType> destinationIdAccessor)
        {
            // Remove items not in the source list
            var itemsToRemove = destinationList
                .Where(d => !sourceList.Any(s =>
                    sourceIdAccessor(s) != null &&
                    sourceIdAccessor(s).Equals(destinationIdAccessor(d))))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                destinationList.Remove(item);
            }

            // Update existing items or add new ones
            foreach (var source in sourceList)
            {
                var sourceId = sourceIdAccessor(source);
                var destination = destinationList
                    .FirstOrDefault(d => destinationIdAccessor(d) != null &&
                                         destinationIdAccessor(d).Equals(sourceId));

                if (destination != null)
                {
                    source.Adapt(destination);
                }
                else
                {
                    destinationList.Add(source.Adapt<TDestination>());
                }
            }
        }

        /// <summary>
        /// Build an ID accessor with caching.
        /// </summary>
        private Func<T, TId> GetIdAccessor<T, TId>(PropertyInfo propertyInfo)
        {
            var key = (typeof(T), propertyInfo.Name);

            var result = (Func<T, TId>)_idAccessorCache.GetOrAdd(key, _ => GetIdAccessorInte<T, TId>(propertyInfo));

            return result ?? throw new NullReferenceException();
        }

        private Func<T, TId> GetIdAccessorInte<T, TId>(PropertyInfo propertyInfo)
        {
            var parameter = Expression.Parameter(typeof(T), "obj");
            Expression body;
            if (typeof(TId).FullName == propertyInfo.PropertyType.FullName)
            {
                body = Expression.MakeMemberAccess(parameter, propertyInfo);
            }
            else { 
                body = Expression.Convert(Expression.MakeMemberAccess(parameter, propertyInfo), typeof(TId));
            }
            
            var lambda = Expression.Lambda<Func<T, TId>>(body, parameter);

            // Compile the lambda into a delegate
            return lambda.Compile();
        }

        /// <summary>
        /// Extract property name from lambda expression.
        /// </summary>
        private string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member.Name;

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberExpr)
                return memberExpr.Member.Name;

            throw new ArgumentException("Invalid property expression");
        }
    }
}