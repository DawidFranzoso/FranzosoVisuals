using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using MathNet.Numerics.LinearAlgebra;

namespace FranzosoVisuals
{
    public interface IVec2<T>
    {
        T X { get; set; }
        T Y { get; set; }
    }

    public struct Vec2f : IVec2<float>, ITransformable<Vec2f>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vec2f(float x, float y) { (X, Y) = (x, y); }
        public Vec2f((float, float) t) { (X, Y) = t; }
        public Vec2f(Vector2f v) { (X, Y) = (v.X, v.Y); }
        public static implicit operator Vector2f(Vec2f v) => new Vector2f(v.X, v.Y);
        public static implicit operator Vec2f(Vector2f v) => new Vec2f(v.X, v.Y);
        public static implicit operator Vec2f((float, float) v) => new Vector2f(v.Item1, v.Item2);
        public static implicit operator Vec2f(Vector<float> v) => (v[0], v[1]);
        public static implicit operator Vector<float>(Vec2f v) => Vector<float>.Build.DenseOfArray(new float[]{v.X, v.Y});
        public static Vec2f operator +(Vec2f a, IVec2<float> b) => ((dynamic)a.X + b.X, (dynamic)a.Y + b.Y);
        public static Vec2f operator -(Vec2f a, IVec2<float> b) => ((dynamic)a.X - b.X, (dynamic)a.Y - b.Y);
        public static Vec2f operator *(Vec2f a, float b) => (a.X * b, a.Y * b);
        public static Vec2f operator *(float a, Vec2f b) => b * a;
        public static Vec2f operator /(Vec2f a, float b) => (a.X / b, a.Y / b);

        public static Vec2f operator *(Vec2f a, decimal b) => a*(float)b;
        public static Vec2f operator *(decimal a, Vec2f b) => b * a;
        public static Vec2f operator /(Vec2f a, decimal b) => a/(float)b;

        public static Vec2f operator *(Vec2f a, int b) => (a.X * b, a.Y * b);
        public static Vec2f operator *(int a, Vec2f b) => b * a;
        public static Vec2f operator /(Vec2f a, int b) => (a.X / b, a.Y / b);

        public Vec2f get() => new Vec2f(X, Y);

        public void setValue(Vec2f x) { (X,Y) = (x.X, x.Y); }

        public Vec2f addReturn(Vec2f x) => this + x;

        public void addValue(Vec2f x) { setValue(addReturn(x)); }

        public Vec2f scaleReturn(float x) => this * x;

        public void scaleValue(float x) { setValue(scaleReturn(x)); }
    }
}
