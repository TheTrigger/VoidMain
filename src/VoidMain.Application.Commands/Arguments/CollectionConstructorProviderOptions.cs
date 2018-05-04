using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments
{
    public class CollectionConstructorProviderOptions
    {
        public TypeOrInstance<ICollectionConstructor> ArrayConstructor { get; set; }
        public Dictionary<Type, TypeOrInstance<ICollectionConstructor>> CollectionConstructors { get; set; }

        public CollectionConstructorProviderOptions(bool defaults = true)
        {
            if (defaults)
            {
                ArrayConstructor = typeof(ArrayConstructor);
                var listCtor = typeof(ListConstructor);
                CollectionConstructors = new Dictionary<Type, TypeOrInstance<ICollectionConstructor>>
                {
                    [typeof(IEnumerable<>)] = ArrayConstructor,
                    [typeof(ICollection<>)] = ArrayConstructor,
                    [typeof(IReadOnlyCollection<>)] = ArrayConstructor,
                    [typeof(IReadOnlyList<>)] = ArrayConstructor,
                    [typeof(IList<>)] = listCtor,
                    [typeof(List<>)] = listCtor
                };
            }
        }

        public void Validate()
        {
            if (ArrayConstructor == null)
            {
                throw new ArgumentNullException(nameof(ArrayConstructor));
            }
            if (CollectionConstructors == null)
            {
                throw new ArgumentNullException(nameof(CollectionConstructors));
            }
        }
    }
}
