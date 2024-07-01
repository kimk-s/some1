namespace UnityEngine
{
    internal static class SystemExtensions
    {
        public static Vector3 ToUnityVector3(this System.Numerics.Vector2 vector) => new(vector.X, 0, vector.Y);

        public static Vector3 ToUnityVector3(this System.Numerics.Vector2 vector, float y) => new(vector.X, y, vector.Y);

        public static Vector2 ToUnityVector2(this System.Numerics.Vector2 vector) => new(vector.X, vector.Y);

        public static Color ToUnityColor(this System.Drawing.Color x) => x.ToUnityColor(x.A * ColorMul);

        public static Color ToUnityColor(this System.Drawing.Color x, float a) => new(x.R * ColorMul, x.G * ColorMul, x.B * ColorMul, a);

        public static Quaternion ToUnityQuaternion(this float x) => Quaternion.AngleAxis(360 - x, Vector3.up);

        private const float ColorMul = 1f / 255;
    }
}
