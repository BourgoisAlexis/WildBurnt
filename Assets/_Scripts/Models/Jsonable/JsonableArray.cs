using System;

[Serializable]
public struct JsonableArray<T> {
    public T[] Values;


    public JsonableArray(T[] values) {
        Values = values;
    }
}