using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowParameters
{
    public ArrowSide arrowSide;
    public Vector2 startPos;
    public Vector2 endPos;
    public double startTime;
    public double endTime;
    public Arrow arrow;
    public int road;
    public uint distanceCount;

    public ArrowParameters(Vector2 startPos, Vector2 endPos, double startTime,double endTime,uint distanceCount, ArrowSide arrowSide, Arrow arrow,int road)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.startTime = startTime;
        this.endTime = endTime;
        this.arrowSide = arrowSide;
        this.arrow = arrow;
        this.road = road;
        this.distanceCount = distanceCount;
    }
}
