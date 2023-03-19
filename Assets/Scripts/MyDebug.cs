using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectHex
{
    public class MyDebug : MonoBehaviour
    {
        void Start()
        {
            Hex my = ExtensionMethods.OffsetToCube(new Vector3Int(2,2,0));
            Debug.Log($"Q: {my.q}  | R: {my.r}  | S: {my.s}");

            Vector3Int coord = ExtensionMethods.CubeToOffset(new Hex(-1, -2, 3));
            Debug.Log($"X: {coord.x}  | Y: {coord.y}");

        }

    }
}
