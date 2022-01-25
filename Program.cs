using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace FranzosoVisuals
{
    class Program
    {
        static void test_program()
        {
            FVWindow Fwindow = new FVWindow(true);
            FuncA<RenderWindow, Vec2f> screen_center = new FuncA<RenderWindow, Vec2f>(w => Screen.getCenter(w), Fwindow.window);
            FuncA<RenderWindow, Vec2f> mouse_position = new FuncA<RenderWindow, Vec2f>(w => new Vec2f(Mouse.GetPosition(w).X, w.Size.Y - Mouse.GetPosition(w).Y), Fwindow.window);

            FuncA<IValue<Vec2f>, Vec2f> v1 = new FuncA<IValue<Vec2f>, Vec2f>(t => ((float)Math.Cos(t.get().X / Fwindow.window.Size.X * 2 * Math.PI) * 100, (float)-Math.Sin(t.get().X / Fwindow.window.Size.X * 2 * Math.PI) * 100), mouse_position);
            FuncA<IValue<Vec2f>, Vec2f> v2 = new FuncA<IValue<Vec2f>, Vec2f>(t => ((float)-Math.Sin(-t.get().Y / Fwindow.window.Size.Y * 2 * Math.PI) * 100, (float)Math.Cos(-t.get().Y / Fwindow.window.Size.Y * 2 * Math.PI) * 100), mouse_position);

            Rf<Vec2f> v3 = new Rf<Vec2f>((100f, 0f));
            Rf<Vec2f> v4 = v3.get();

            Matrix2X2F m1 = new Matrix2X2F(v1, v2);
            Matrix2X2F m2 = new Matrix2X2F(v3, v4);
            Matrix2X2F mgrid = m1;

            Grid grid = new Grid(mgrid, new Rf<Vec2f>((200, 200)), (Rf<bool>)true);

            FuncA<IValue<Vec2f>, string> f = new FuncA<IValue<Vec2f>, string>(v => $"{v.get().X}/{v.get().Y}", mouse_position);
            FuncA<Grid, string> fv1 = new FuncA<Grid, string>(g => $"{g.spanX.get().X}/{g.spanX.get().Y}", grid);
            FuncA<Grid, string> fv2 = new FuncA<Grid, string>(g => $"{g.spanY.get().X}/{g.spanY.get().Y}", grid);
            FuncA<(IValue<string>, IValue<string>), string> fvcombined = MathExt.combine(fv1, fv2, f => f.Item1.get() + @"\\" + f.Item2.get());

            FuncA<IValue<Vec2f>, Func<decimal, decimal>> linear_var = new FuncA<IValue<Vec2f>, Func<decimal, decimal>>(m=>(x => (decimal)m.get().Y/300 * x), mouse_position);

            Fwindow.Add(grid);
            grid.Graph(linear_var, color: new Rf<Color>(Color.Green));
            Fwindow.Add(new LatexSprite(f, mouse_position));
            Fwindow.Add(new LatexSprite(fvcombined, new Rf<Vec2f>((100, 100))));
            Fwindow.Add(() => { grid.span = m2; Fwindow.Add(new TransformationProperties<Vec2f>(v4, new Rf<Vec2f>((0f, 100f)), new Rf<float>(1000))); });
            Fwindow.Add(() => Fwindow.Add(new TransformationProperties<Vec2f>(v4, new Rf<Vec2f>((100f, 100f)), new Rf<float>(1000))));
            Fwindow.Add(() => Fwindow.Add(new TransformationProperties<Vec2f>(v4, new Rf<Vec2f>((100f, 0f)), new Rf<float>(1000))));
            //Fwindow.Add(() => grid.span = m1);

            while (Fwindow.window.IsOpen)
                Fwindow.loop();
        }

        static void test_program2()
        {
            FVWindow Fwindow = new FVWindow(true);

            FuncA<RenderWindow, Vec2f> screen_center = new FuncA<RenderWindow, Vec2f>(w => Screen.getCenter(w), Fwindow.window);
            Matrix2X2F base_system = new Matrix2X2F(new Rf<Vec2f>((250, 0)), new Rf<Vec2f>((0, 250)));

            Rf<float> radius = new Rf<float>(1);
            FuncA<IValue<float>, float> alpha = new FuncA<IValue<float>, float>(t => t.get() / 1000 * 2 * (float)Math.PI / 5, Fwindow.time);
            FuncA<IValue<float>, Func<decimal, decimal>> upper_circle = new FuncA<IValue<float>, Func<decimal, decimal>>(r => (x => Math.Abs(x) > (decimal)r.get() ? 0 : (decimal)Math.Sqrt(((double)((decimal)(r.get() * r.get()) - (x * x))))), radius);
            FuncA<IValue<Func<decimal, decimal>>, Func<decimal, decimal>> lower_circle = new FuncA<IValue<Func<decimal, decimal>>, Func<decimal, decimal>>(f => (x => -f.get()(x)), upper_circle);

            Grid grid = new Grid(base_system, screen_center, new Rf<bool>(true));
            grid.Graph(lower_circle, new Rf<decimal>(0.01M), new Rf<(decimal, decimal)>((-3M, 3M)));
            grid.Graph(upper_circle, new Rf<decimal>(0.01M), new Rf<(decimal, decimal)>((-3M, 3M)));
            Fwindow.Add(grid);

            Line_fi hypo = new Line_fi(new Rf<Vec2f>((1, 1)), alpha, radius, color_a: new Rf<Color>(Color.Red));

            grid.Graph(hypo);

            Fwindow.Add(new LatexSprite(new FuncA<(IValue<float>, (IValue<Vec2f>, IValue<Vec2f>)), string>(
                v => $"sin({v.Item1.get():f2})={(v.Item2.Item2.get() - v.Item2.Item1.get()).Y / 250:f2}\\\\" +
                $"cos({v.Item1.get():f2})={(v.Item2.Item2.get() - v.Item2.Item1.get()).X / 250:f2}",
                (alpha, hypo.getPoints())), new Rf<Vec2f>((50, 50))));

            Fwindow.Add(new LatexSprite(new Rf<string>(@"\pi\(\int_{0}^{1} e^{\pi*i*x}\,dx\)"), new Rf<Vec2f>((1500, 900))));

            Fwindow.Add(() => Fwindow.Add(new TransformationProperties<float>(radius, new Rf<float>(2f), new Rf<float>(1000))));
            Fwindow.Add(() => Fwindow.Add(new TransformationProperties<float>(radius, new Rf<float>(0.5f), new Rf<float>(1000))));
            Fwindow.Add(() => Fwindow.Add(new TransformationProperties<float>(radius, new Rf<float>(1f), new Rf<float>(1000))));

            while (Fwindow.window.IsOpen)
                Fwindow.loop();
        }

        static void test_program3()
        {
            FVWindow Fwindow = new FVWindow(true);

            FuncA<RenderWindow, Vec2f> screen_center = new FuncA<RenderWindow, Vec2f>(w => Screen.getCenter(w), Fwindow.window);
            FuncA<RenderWindow, Vec2f> mouse_position = new FuncA<RenderWindow, Vec2f>(w => new Vec2f(Mouse.GetPosition(w).X, w.Size.Y - Mouse.GetPosition(w).Y), Fwindow.window);

            FuncA<IValue<Vec2f>, Vec2f> v1 = new FuncA<IValue<Vec2f>, Vec2f>(t => ((float)Math.Cos(t.get().X / Fwindow.window.Size.X * 2 * Math.PI) * 100, (float)-Math.Sin(t.get().X / Fwindow.window.Size.X * 2 * Math.PI) * 100), mouse_position);
            FuncA<IValue<Vec2f>, Vec2f> v2 = new FuncA<IValue<Vec2f>, Vec2f>(t => ((float)-Math.Sin(-t.get().Y / Fwindow.window.Size.Y * 2 * Math.PI) * 100, (float)Math.Cos(-t.get().Y / Fwindow.window.Size.Y * 2 * Math.PI) * 100), mouse_position);

            Matrix2X2F base_system = new Matrix2X2F(v1,v2);

            Rf<float> radius = new Rf<float>(1);
            FuncA<IValue<float>, float> alpha = new FuncA<IValue<float>, float>(t => t.get() / 1000 * 2 * (float)Math.PI / 5, Fwindow.time);
            FuncA<IValue<float>, Func<decimal, decimal>> upper_circle = new FuncA<IValue<float>, Func<decimal, decimal>>(r => (x => Math.Abs(x) > (decimal)r.get() ? 0 : (decimal)Math.Sqrt(((double)((decimal)(r.get() * r.get()) - (x * x))))), radius);
            FuncA<IValue<Func<decimal, decimal>>, Func<decimal, decimal>> lower_circle = new FuncA<IValue<Func<decimal, decimal>>, Func<decimal, decimal>>(f => (x => -f.get()(x)), upper_circle);

            Grid base_grid = new Grid(base_system, new Rf<Vec2f>((200, 200)), new Rf<bool>(true));
            Grid inner_grid = new Grid(new Matrix2X2F(2,0,0,2), new Rf<Vec2f>((0,0)), new Rf<bool>(false));
            inner_grid.Graph(lower_circle, new Rf<decimal>(0.01M), new Rf<(decimal, decimal)>((-3M, 3M)));
            inner_grid.Graph(upper_circle, new Rf<decimal>(0.01M), new Rf<(decimal, decimal)>((-3M, 3M)));
            base_grid.Graph(inner_grid);
            Fwindow.Add(base_grid);


            while (Fwindow.window.IsOpen)
                Fwindow.loop();
        }


        static void Main(string[] args)
        {
            test_program3();

        }
    }
}
