using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments
{
    public class CollectionConstructorProviderOptions
    {
        public ICollectionConstructor ArrayConstructor { get; set; }
        public Dictionary<Type, ICollectionConstructor> CollectionConstructors { get; set; }

        public CollectionConstructorProviderOptions(bool defaults = true)
        {
            if (defaults)
            {
                ArrayConstructor = new ArrayConstructor();
                var listCtor = new ListConstructor();
                CollectionConstructors = new Dictionary<Type, ICollectionConstructor>
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
