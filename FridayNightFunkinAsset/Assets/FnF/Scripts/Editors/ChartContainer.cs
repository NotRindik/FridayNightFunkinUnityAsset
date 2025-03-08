using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using FridayNightFunkin.GamePlay;
using UnityEngine;

public class ChartContainer : MonoBehaviour
{
    [SerializedDictionary] public SerializedDictionary<RoadSide, List<Arrow>> arrowsList = new SerializedDictionary<RoadSide, List<Arrow>>();
}
