using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranzosoVisuals
{
    public static class Curve
    {
        public static float linear(float x) => x;
        public static float hyperbolic(float x) => (float)(1 /(1+Math.Exp(-13   *(x-0.5))));
    }

    public interface ITransformation
    {
        public bool transform(); // returns false when is supposed to be deleted
    }

    public struct TransformationProperties<T>
    {
        public bool reset;
        public bool repeat;
        public Func<float, float> curve;
        public IValue<T> value;
        public IValue<T> end_value;
        public IValue<float> duration_milliseconds;

        public TransformationProperties(IValue<T> value_a, IValue<T> end_value_a, IValue<float> duration_milliseconds_a, Func<float, float> curve_a = null, bool reset_a = false, bool repeat_a = false)
        {
            reset = reset_a;
            repeat = repeat_a;
            curve = MathExt.defIfNull(curve_a,Curve.hyperbolic);
            value = value_a;
            end_value = end_value_a;
            duration_milliseconds = duration_milliseconds_a;
        }
    }

    public class TransformationObj<T> : ITransformation where T : ITransformable<T>
    {
        bool reset;
        bool repeat;
        Func<float, float> curve;

        IValue<T> value;
        IValue<float> time;
        IValue<float> duration_milliseconds;

        float start_time;
        T start_value;
        IValue<T> end_value;

        public TransformationObj(TransformationProperties<T> p, IValue<float> time_a)
        {
            reset = p.reset;
            repeat = p.repeat;
            curve = p.curve;
            value = p.value;
            end_value = p.end_value;
            duration_milliseconds = p.duration_milliseconds;
            time = time_a;

            start_time = time.get();
            start_value = value.get();
        }

        public bool transform()
        {
            float t = time.get() - start_time;
            float relative_t = t / duration_milliseconds.get();
            if (relative_t > 1)
            {
                value.setValue(reset?start_value:end_value.get());
                if (repeat) start_time = time.get();
                return repeat;
            }
            T delta = end_value.get().addReturn(start_value.scaleReturn(-1)); // end_value - start_value
            value.setValue(start_value.addReturn(delta.scaleReturn(curve(relative_t)))); // start_value + f(relative_t) * delta
            return true;
        }
    }

    public class Transformation<T> : ITransformation
    {
        bool reset;
        bool repeat;
        Func<float, float> curve;

        IValue<T> value;
        IValue<float> time;
        IValue<float> duration_milliseconds;

        float start_time;
        T start_value;
        IValue<T> end_value;

        public Transformation(TransformationProperties<T> p, IValue<float> time_a)
        {
            reset = p.reset;
            repeat = p.repeat;
            curve = p.curve;
            value = p.value;
            end_value = p.end_value;
            duration_milliseconds = p.duration_milliseconds;
            time = time_a;

            start_time = time.get();
            start_value = value.get();
        }

        public bool transform()
        {
            float t = time.get() - start_time;
            float relative_t = t / duration_milliseconds.get();
            if (relative_t > 1)
            {
                value.setValue(reset ? start_value : end_value.get());
                if (repeat) start_time = time.get();
                return repeat;
            }

            T delta = (dynamic) end_value.get() - start_value;
            value.setValue(start_value + (dynamic)delta * curve(relative_t)); // start_value + f(relative_t) * delta
            return true;
        }
    }

    public class TransformationHandler
    {
        List<ITransformation> transformations = new List<ITransformation>();
        Rf<float> time;

        public TransformationHandler(Rf<float> time_a)
        {
            time = time_a;
        }

        public void applyTransform()
        {
            List<ITransformation> complete = new List<ITransformation>();
            foreach (ITransformation t in transformations)
            {
                if (!t.transform()) complete.Add(t);
            }
            transformations = transformations.Except(complete).ToList();
        }

        public void addTransform<T>(Transformation<T> t) { transformations.Add(t); }
        public void addTransform<T>(TransformationObj<T> t) where T : ITransformable<T> { transformations.Add(t); }

        public void addTransform<T>(TransformationProperties<T> p, IValue<float> time_a)
        { addTransform(new Transformation<T>(p, time)); }
    }
}
