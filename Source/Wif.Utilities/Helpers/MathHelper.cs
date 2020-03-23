using System;
using System.Windows;

namespace Frontier.Wif.Utilities.Helpers
{
    /// <summary>
    /// 数学帮助类。
    /// </summary>
    public class MathHelper
    {
        #region Methods

        /// <summary>
        /// 两点计算角度
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public static double CalulateRotateAngle(double startX, double startY, double endX, double endY)
        {
            double angle;
            var tan = Math.Atan2(Math.Abs(startX - endX), Math.Abs(startY - endY));
            if (endX >= startX && endY >= startY) //第一象限
                angle = tan;
            else if (endX >= startX && endY < startY) //第四象限
                angle = Math.PI - tan;
            else if (endX < startX && endY >= startY) //第二象限
                angle = 2 * Math.PI - tan;
            else //第三象限
                angle = Math.PI + tan;
            angle = angle * 180 / Math.PI;
            return angle;
        }

        /// <summary>
        /// 两点计算角度
        /// </summary>
        /// <param name="startx"></param>
        /// <param name="starty"></param>
        /// <param name="endx"></param>
        /// <param name="endy"></param>
        /// <returns></returns>
        public static double CalulateXYAnagle(double startx, double starty, double endx, double endy)
        {
            //除数不能为0
            var tan = Math.Atan(Math.Abs((endy - starty) / (endx - startx))) * 180 / Math.PI;
            if (endx > startx && endy > starty) //第一象限
                return -tan;
            if (endx > startx && endy < starty) //第二象限
                return tan;
            if (endx < startx && endy > starty) //第三象限
                return tan - 180;
            return 180 - tan;
        }

        /// <summary>
        /// 获取两个点之间的距离
        /// </summary>
        /// <param name="mp1"></param>
        /// <param name="mp2"></param>
        /// <returns></returns>
        public static double PolyLineDistance(Point mp1, Point mp2)
        {
            return Math.Sqrt(Math.Pow(mp1.X - mp2.X, 2) + Math.Pow(mp1.Y - mp2.Y, 2));
        }

        #endregion
    }
}