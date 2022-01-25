using System;
using System.Collections.Generic;
using System.Text;

namespace FranzosoVisuals
{
    public interface ITransformable<T>
    {
        //public abstract T get();
        //public abstract void setValue(T x);
        public abstract T addReturn(T x);
        public abstract void addValue(T x);
        public abstract T scaleReturn(float x);
        public abstract void scaleValue(float x);
    }

    public interface IValue<T>
    {
        public abstract T get();
        public abstract void setValue(T x);
    }

    /*public struct Tr<T> : ITransformable<T>
    {
        public T value;

        public T addReturn(T x) => (dynamic)value + x;
        public void addValue(T x) { value = addReturn(x); }

        public T scaleReturn(float x) => (dynamic)value * x;
        public void scaleValue(float x) { value = scaleReturn(x); }

        public void setValue(T x) { value = x; }

        public Tr(T x) { value = x; }

        public static implicit operator T(Tr<T> x) => x.value;
        public static implicit operator Tr<T>(T x) => new Tr<T>(x);
    }*/

    public class Rf<T> : IValue<T>
    {
        public T value;
        public T get() => value;
        public void setValue(T x) { value = x; }

        public Rf(T v) { value = v; }
        public static implicit operator Rf<T>(T v) => new Rf<T>(v);
    }

    public class FuncA<TArgs, TResult> : IValue<TResult>
    {
        public TResult get() => function(args);

        public void setValue(TResult x) { function = (t => x); }

        public Func<TArgs, TResult> function;
        public TArgs args;
        public FuncA(Func<TArgs, TResult> function_a, TArgs args_a) { function = function_a; args = args_a; }
    }

    public class Matrix2X2F : ITransformable<Matrix2X2F>
    {
        public (IValue<Vec2f>, IValue<Vec2f>) span;
        public Matrix2X2F((Vec2f, Vec2f) x) { span = (new Rf<Vec2f>(x.Item1), new Rf<Vec2f>(x.Item2)); }
        public Matrix2X2F((IValue<Vec2f>, IValue<Vec2f>) x) { span = x; }

        public Matrix2X2F(Vec2f x, Vec2f y) : this((x, y)) { }
        public Matrix2X2F(IValue<Vec2f> x, IValue<Vec2f> y) : this((x, y)) { }

        public Matrix2X2F(float x1, float x2, float y1, float y2) : this((x1, x2), (y1, y2)) { }

        public Matrix2X2F get() => this;
        public void setValue(Matrix2X2F x) { span.Item1.setValue(x.span.Item1.get()); span.Item2.setValue(x.span.Item2.get()); }

        public Matrix2X2F addReturn(Matrix2X2F x) => new Matrix2X2F(span.Item1.get() + x.span.Item1.get(), span.Item2.get() + x.span.Item2.get());
        public void addValue(Matrix2X2F x) { span.Item1.setValue(addReturn(x).span.Item1.get()); span.Item2.setValue(addReturn(x).span.Item2.get()); }

        public Matrix2X2F scaleReturn(float x) => new Matrix2X2F(span.Item1.get() * x, span.Item2.get() * x);
        public void scaleValue(float x) { span.Item1.setValue(scaleReturn(x).span.Item1.get()); span.Item2.setValue(scaleReturn(x).span.Item2.get()); }

        public Matrix2X2F returnTransformed(Matrix2X2F a) // returns B * A
        {
            Vec2f a1, a2, b1, b2;
            Vec2f n1, n2;
            (a1, a2, b1, b2) = (a.span.Item1.get(), a.span.Item2.get(), this.span.Item1.get(), this.span.Item2.get());
            n1 = new Vec2f(b1.X * a1.X + b2.X * a1.Y, b1.Y * a1.X + b2.Y * a1.Y);
            n2 = new Vec2f(b1.X * a2.X + b2.X * a2.Y, b1.Y * a2.X + b2.Y * a2.Y);

            return new Matrix2X2F(n1,n2);
        }

        public Vec2f returnTransformed(IValue<Vec2f> v)
        {
            Matrix2X2F a = new Matrix2X2F(v.get(), new Vec2f(0, 0));
            returnTransformed(a);

            return a.span.Item1.get();
        }

        public Vec2f returnTransformed(Vec2f v) => returnTransformed(new Rf<Vec2f>(v)).get();

        public static readonly Matrix2X2F unit = new Matrix2X2F((1,0),(0,1));
    }
}
