using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomVal : MonoBehaviour
{
    public static int GetRandmVal(List<WeightedVal> valList)
    {
        int output = 0;

        int totalweight = 0;

        foreach (var item in valList)
        {
            totalweight += item.weight;
        }

        int rndVal = UnityEngine.Random.Range(1, totalweight + 1);

        int processedVal = 0;

        foreach (var item in valList)
        {
            processedVal += item.weight;

            if (rndVal <= processedVal)
            {
                output = item.type;
                break;
            }
        }

        return output;
    }
}

[Serializable]
public class WeightedVal
{
    public int type;
    public int weight;

    public WeightedVal(int type1, int weight1)
    {
        type = type1;
        weight = weight1;
    }
}
