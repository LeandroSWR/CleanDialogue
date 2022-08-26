using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleanDialogue.Utilities
{
    public static class CollectionUtility
    {
        public static void AddItem<K, V>(this SerializableDictionary<K, List<V>> serializableDictionary, K key, V value)
        {
            if (serializableDictionary.ContainsKey(key))
            {
                serializableDictionary[key].Add(value);
            }
            else
            {
                serializableDictionary.Add(key, new List<V>() { value });
            }
        }
    }
}
