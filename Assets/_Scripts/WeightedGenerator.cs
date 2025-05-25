using System.Collections.Generic;
using UnityEngine;

public class WeightedGenerator {
    private int _maxResult;
    private float _weightStep;

    private float[] _weights;
    private float[] _totals;


    public WeightedGenerator(int maxResult, int weightStep) {
        _maxResult = maxResult;
        _weightStep = weightStep;

        _weights = new float[maxResult];

        for (int i = 0; i < _weights.Length; i++)
            _weights[i] = 100 / (float)_maxResult;
    }


    public int[] GenerateArray(int size) {
        List<int> result = new List<int>();

        _totals = new float[_maxResult];
        float total = 0;

        for (int i = 0; i < _maxResult; i++) {
            total += _weights[i];
            _totals[i] = total;
        }

        for (int i = 0; i < size; i++)
            result.Add(PickARandom());

        return result.ToArray();
    }

    public int PickARandom() {
        int result = 0;

        float r = Random.Range(0, GetTotal());
        for (int j = 0; j < _maxResult; j++)
            if (r <= _totals[j]) {
                result = j;
                break;
            }

        for (int i = 0; i < _maxResult; i++)
            _weights[i] += i == result ? -_weightStep : _weightStep;

        float total = GetTotal();

        for (int i = 0; i < _maxResult; i++) {
            float percent = _weights[i] / total;
            _weights[i] = percent * 100;
        }

        return result;
    }

    private float GetTotal() {
        float total = 0;

        foreach (float f in _weights)
            total += f;

        return total;
    }
}
