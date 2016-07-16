using UnityEngine;
using System.Collections;
using LitJson;

public class GetItems {

    public long getRandom()
    {
        long val = Random.Range(1, 10000);
        return val;
    }
}
