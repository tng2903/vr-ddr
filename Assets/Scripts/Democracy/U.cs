using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class U {
    public static int pmod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
