using System;
using UnityEngine;

namespace Some1.User.ViewModel
{
    public static class IndicatorColorExtensions
    {
        public static Color ToUnityColor(this IndicatorColor that) => that.ToUnityColor(0);

        public static Color ToUnityColor(this IndicatorColor that, float a)
        {
            var result = that switch
            {
                IndicatorColor.White => Color.white,
                IndicatorColor.Yellow => Color.yellow,
                IndicatorColor.Red => Color.red,
                _ => throw new NotImplementedException()
            };
            result.a = a;
            return result;
        }
    }
}
