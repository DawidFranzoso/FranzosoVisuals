using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using MathNet.Numerics.LinearAlgebra;

namespace FranzosoVisuals
{
    public static class MathExt
    {
        public static T defIfNull<T>(T value, T default_value) { return (dynamic)value != null ? value : default_value; }
        public static FuncA<(IValue<A1>, IValue<A2>), TResult> combine<A1, A2, TResult>(IValue<A1> f1, IValue<A2> f2, Func<(IValue<A1>, IValue<A2>), TResult> f_combined)
        {
            return new FuncA<(IValue<A1>, IValue<A2>), TResult>(f_combined, (f1, f2));
        }

        public static Matrix<float> getRotationMatrix2(float fi_rad)
        {
            return Matrix<float>.Build.DenseOfArray(new float[,] {
                { (float)Math.Cos(fi_rad), (float)(-Math.Sin(fi_rad)) },
                { (float)Math.Sin(fi_rad), (float)Math.Cos(fi_rad) } });
        }

        public static Vec2f rotVec(Vec2f v, float fi_rad) => getRotationMatrix2(fi_rad) * v;
        public static Vec2f rotVecPos(Vec2f v) => rotVec(v, (float)(Math.PI / 2));
        public static Vec2f rotVecNeg(Vec2f v) => rotVec(v, (float)(-Math.PI / 2));

        public static float magnitude2(Vec2f v) => (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
        public static Vec2f unitVector2(Vec2f v) => v / magnitude2(v);
        public static Vec2f V2ofLength(Vec2f v, float length) => unitVector2(v) * length;
        public static Vec2f moveByLength(Vec2f pos, Vec2f dir, float length) => pos + V2ofLength(dir, length); 

        public static Vec2f VFmTF(Tuple<float, float> tuple) => new Vec2f(tuple.Item1, tuple.Item2);
        public static Tuple<float, float> FFmVF(Vec2f f) => new Tuple<float, float>(f.X,f.Y);

        public static float RadToDeg(float rad) { return (float)(rad / Math.PI) * 180; }
        public static float DegToRad(float deg) { return (float)(deg * Math.PI) / 180; }
    }
}
