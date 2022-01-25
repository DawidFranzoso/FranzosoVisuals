using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace FranzosoVisuals
{
    class Grid : Primitive
    {
        public Grid(Matrix2X2F span_a, IValue<Vec2f> anchor_a = null,
            IValue<bool> showVectors_a = null, IValue<float> interval_a = null,
            Func<float, float> thickness_function_x_a = null, Func<float, float> thickness_function_y_a = null,
            Func<float, Color> color_function_x_a = null, Func<float, Color> color_function_y_a = null,
            IValue<Color> color_vec_X_a = null, IValue<Color> color_vec_Y_a = null)
        {
            span = span_a;

            anchor = MathExt.defIfNull(anchor_a,new Rf<Vec2f>((0,0)));
            showVectors = MathExt.defIfNull(showVectors_a, new Rf<bool>(false));
            interval = MathExt.defIfNull(interval_a,new Rf<float>(1));
            thickness_function_x = MathExt.defIfNull(thickness_function_x_a, x => ((int)(x)*interval.get() % 4 == 0 ? 1f : 0.5f));
            thickness_function_y = MathExt.defIfNull(thickness_function_y_a, y => ((int)(y)*interval.get() % 4 == 0 ? 1f : 0.5f));
            color_function_x = MathExt.defIfNull(color_function_x_a, x => ((int)(x) * interval.get() % 2 == 0 ? Color.Yellow:Color.White));
            color_function_y = MathExt.defIfNull(color_function_y_a, x => ((int)(x) * interval.get() % 2 == 0 ? Color.Yellow:Color.White));

            vector_colors = (MathExt.defIfNull(color_vec_X_a, new Rf<Color>(Color.Red)), MathExt.defIfNull(color_vec_Y_a, new Rf<Color>(Color.Green)));

            spanX = new FuncA<Grid, Vec2f>((t) => t.span.span.Item1.get(), this);
            spanY = new FuncA<Grid, Vec2f>((t) => t.span.span.Item2.get(), this);

            arrowVecX = new Arrow(anchor, spanX, color_a: vector_colors.Item1);
            arrowVecY = new Arrow(anchor, spanY, color_a: vector_colors.Item2);
        }

        List<Line> generateLines()
        {
            List<Line> lines_t = new List<Line>();
            float linestart = (-number_lines.get() + 1) * interval.get() / 2;
            Rf<float> nlinex = linestart;
            Rf<float> nliney = linestart;

            while (nlinex.get() <= number_lines.get()/2)
            {
                FuncA<(IValue<Vec2f>,float, IValue<Vec2f>), Vec2f> anchor_coord_func = new FuncA<(IValue<Vec2f>, float, IValue<Vec2f>),Vec2f>((t) => t.Item1.get() * t.Item2 + t.Item3.get(), (spanY,nlinex.get(), anchor));
                FuncA<IValue<Vec2f>, Vec2f> point2_coord_func = new FuncA<IValue<Vec2f>, Vec2f>((t) => anchor_coord_func.get() + t.get(), spanX);

                FuncA<float, float> thickness_function_x_t = new FuncA<float, float>(thickness_function_x, nlinex.get());
                FuncA<float, Color> color_function_x_t = new FuncA<float, Color>(color_function_x, nlinex.get());
                Line_xy2 line = new Line_xy2(anchor_coord_func, point2_coord_func, (Rf<bool>)true, thickness_function_x_t, color_function_x_t);
                lines_t.Add(line);
                nlinex.value += 1/ interval.get();
            }

            while (nliney.get() <= number_lines.get() / 2)
            {
                FuncA<(IValue<Vec2f>, float, IValue<Vec2f>), Vec2f> anchor_coord_func = new FuncA<(IValue<Vec2f>, float, IValue<Vec2f>), Vec2f>((t) => t.Item1.get() * t.Item2 + t.Item3.get(), (spanX, nliney.get(), anchor));
                FuncA<IValue<Vec2f>, Vec2f> point2_coord_func = new FuncA<IValue<Vec2f>, Vec2f>((t) => anchor_coord_func.get() + t.get(), spanY);

                FuncA<float, float> thickness_function_y_t = new FuncA<float, float>(thickness_function_y, nliney.get());
                FuncA<float, Color> color_function_y_t = new FuncA<float, Color>(color_function_y, nliney.get());
                Line_xy2 line = new Line_xy2(anchor_coord_func, point2_coord_func, (Rf<bool>)true, thickness_function_y_t, color_function_y_t);
                lines_t.Add(line);
                nliney.value += 1 / interval.get();
            }

            return lines_t;
        }

        List<Line> lines = new List<Line>();

        Arrow arrowVecX;
        Arrow arrowVecY;

        public IValue<int> number_lines = new Rf<int>(501);

        IValue<float> interval;
        Func<float, float> thickness_function_x = null;
        Func<float, float> thickness_function_y = null;
        Func<float, Color> color_function_x = null;
        Func<float, Color> color_function_y = null;
        public Matrix2X2F span;
        public IValue<Vec2f> spanX;
        public IValue<Vec2f> spanY;
        public IValue<Vec2f> anchor;
        public IValue<bool> showVectors;
        public (IValue<Color>, IValue<Color>) vector_colors;
        public List<(IValue<Func<decimal, decimal>>, IValue<decimal>, IValue<(decimal, decimal)>, IValue<Color>)> function_graphs = new List<(IValue<Func<decimal, decimal>>, IValue<decimal>, IValue<(decimal, decimal)>, IValue<Color>)>();
        // function, interval, mapping_range, color

        public List<Primitive> primitives = new List<Primitive>();

        public void Graph(Primitive p) { primitives.Add(p); }

        public void Graph(IValue<Func<decimal, decimal>> f, IValue<decimal> interval = null, IValue<(decimal, decimal)> range = null, IValue<Color> color = null)
        {
            interval = MathExt.defIfNull(interval, new Rf<decimal>(0.02M));
            range = MathExt.defIfNull(range, new Rf<(decimal, decimal)>((-10M, 10M)));
            color = MathExt.defIfNull(color, new Rf<Color>(Color.Green));
            function_graphs.Add((f,interval,range,color));
        }

        /*public IValue<Vec2f> projectionX(IValue<Vec2f> )
        {

        }*/

        public override void draw(FVWindow target_window, Vec2f offset, Matrix2X2F pos_scale)
        {
            lines = generateLines();
            foreach (Line l in lines)
                l.draw(target_window, offset, pos_scale.returnTransformed(span));

            foreach((IValue<Func<decimal, decimal>>, IValue<decimal>, IValue<(decimal,decimal)>, IValue<Color>) args in function_graphs)
            {
                Func<decimal, decimal> function = args.Item1.get();
                decimal interval = args.Item2.get();
                (decimal, decimal) range = args.Item3.get();
                Color color = args.Item4.get();

                VertexArray graph = new VertexArray(PrimitiveType.LineStrip);
                decimal last_y = 0;
                decimal y = 0;
                for(decimal x = range.Item1; x <= range.Item2; x += interval)
                {
                    y = function(x);
                    if(last_y != y || y != function(x + interval)) // append if not between two of the same values
                        graph.Append(new Vertex(x * spanX.get() + y * spanY.get() + anchor.get(),color));
                    last_y = y;
                }
                target_window.window.Draw(convSFML_V(getTransformedVertexArray(graph, offset, pos_scale),target_window));
            } // TODO: Make an extra class for graphs

            foreach (Primitive p in primitives)
            {
                // if there is any mistakes on chaining transformations, switch a and b. Couldnt wrap my head around it
                p.draw(target_window, offset + anchor.get(), span.returnTransformed(pos_scale));
            }

            if (showVectors.get())
            {
                arrowVecX.draw(target_window,offset, span.returnTransformed(pos_scale));
                arrowVecY.draw(target_window,offset, span.returnTransformed(pos_scale));
            }
        }
    }
}
