using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPBlockCall : MonoBehaviour
{
    public CPBlock c;
    public void Click()
    {
        c.Run();
    }
}
