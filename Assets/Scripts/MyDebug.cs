using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectHex
{
    public class MyDebug : MonoBehaviour
    {
        void Start()
        {
            foreach(var my in ExtensionMethods.GetNeighbors(new QRS(-2, 0, 2)))
            {
                Debug.Log($"Q: {my.q}  | R: {my.r}  | S: {my.s}");
            }
        }

    }
}
