using System;

[Serializable]
public class JSONableArray<T> {
    public T[] Array;

    public JSONableArray(T[] array) {
        Array = array;
    }
}