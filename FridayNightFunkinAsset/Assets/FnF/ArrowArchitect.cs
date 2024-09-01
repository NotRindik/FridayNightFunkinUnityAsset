using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum functionType
{
    liner,
    parabola
}

public class ArrowArchitect
{
    private ArrowSide arrowSide;
    private double timelineTime;
    private uint distanceCount = 0;
    private int road = 0;

    public ArrowArchitect(ArrowSide arrowSide, double timelineTime, uint distanceCount = 0, int road = 0)
    {
        this.arrowSide = arrowSide;
        this.timelineTime = timelineTime;
        this.distanceCount = distanceCount;
        this.road = road;
    }

    public Vector2 CalculateArrowPos(Vector2 startPos, Vector2 endPos,double startTime, double endTime)
    {
        if (endTime == 0 && startTime == 0) 
        {
            Debug.LogError($"endTime or startTime equals to zero");
            return Vector2.zero;
        }
        double speed = (endPos.y - startPos.y) / (endTime - startTime);

        Vector2 arrowPos = new Vector2(startPos.x, startPos.y + (float)(speed*(timelineTime - startTime)));

        return arrowPos;
    }
}
