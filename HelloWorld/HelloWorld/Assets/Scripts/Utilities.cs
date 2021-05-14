using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static int PossibleNegative()
    {
        return Random.Range(0, 2) == 0 ? 1 : -1;
    }
}
