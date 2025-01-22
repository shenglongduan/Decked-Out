using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SerializableDictionary<string, float>))]
public class StringFloatDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
