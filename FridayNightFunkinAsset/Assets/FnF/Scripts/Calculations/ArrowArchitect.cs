//using UnityEngine;
//namespace FridayNightFunkin.Calculations
//{
//    public class ArrowArchitect
//    {
//        private double timelineTime;

//        public ArrowArchitect(double timelineTime)
//        {
//            this.timelineTime = timelineTime;
//        }

//        public Vector2 CalculateArrowPos(Vector2 startPos, Vector2 endPos, double startTime, double endTime)
//        {
//            if (endTime == 0 && startTime == 0)
//            {
//                Debug.LogError($"endTime or startTime equals to zero");
//                return Vector2.zero;
//            }
//            double speed = (endPos.y - startPos.y) / (endTime - startTime);

//            Vector2 arrowPos = new Vector2(startPos.x, startPos.y + (float)(speed * (timelineTime - startTime)));

//            return arrowPos;
//        }
//    }
//}