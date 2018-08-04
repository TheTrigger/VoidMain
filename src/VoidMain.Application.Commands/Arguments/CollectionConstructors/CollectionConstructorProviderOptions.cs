using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors
{
    public class CollectionConstructorProviderOptions
    {
        public TypeOrInstance<ICollectionConstructor> ArrayConstructor { get; set; }
        public Dictionary<Type, TypeOrInstance<ICollectionConstructor>> CollectionConstructors { get; set; }

        public CollectionConstructorProviderOptions(bool defaults = true)
        {
            if (defaults)
            {
                var arrayCtor = typeof(ArrayConstructor);
                var listCtor = typeof(ListConstructor);
                var dictCtor = typeof(DictionaryConstructor);

                ArrayConstructor = arrayCtor;
                CollectionConstructors = new Dictionary<Type, TypeOrInstance<ICollectionConstructor>>
                {
                    [typeof(IEnumerable<>)] = arrayCtor,
                    [typeof(ICollection<>)] = arrayCtor,
                    [typeof(IReadOnlyCollection<>)] = arrayCtor,
                    [typeof(IReadOnlyList<>)] = arrayCtor,

                    [typeof(IList<>)] = listCtor,
                    [typeof(List<>)] = listCtor,

                    [typeof(IDictionary<,>)] = dictCtor,
                    [typeof(IReadOnlyDictionary<,>)] = dictCtor,
                    [typeof(Dictionary<,>)] = dictCtor,
                };
            }
        }

        public void Validate()
        {
            if (CollectionConstructors == null)
            {
                throw new ArgumentNullException(nameof(CollectionConstructors));
            }
        }
    }
}
