using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public struct JsonableArray<T> {
    public T[] Values;
    public int[] Chunks;


    public JsonableArray(T[] values) {
        Values = values;
        Chunks = new[] { values.Length };
    }

    public JsonableArray(T[][] values) {
        Values = values.SelectMany(x => x).ToArray();
        Chunks = values.Select(x => x.Length).ToArray();
    }

    public T[][] GetValues() {
        List<T[]> result = new List<T[]>();
        int offset = 0;

        foreach (int x in Chunks) {
            result.Add(Values.Skip(offset).Take(x).ToArray());
            offset += x;
        }

        return result.ToArray();
    }
}
