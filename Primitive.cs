using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace FranzosoVisuals
{
    abstract class Primitive
    {
        public void addPosition<T>(ref T t, Vec2f v) where T : Transformable { t.Position = new Vector2f(t.Position.X + v.X, t.Position.Y + v.Y); }
        public void addPosition(ref Vertex t, Vec2f v) { t.Position = new Vec2f(t.Position.X + v.X, t.Position.Y + v.Y); }

        public void scalePosition<T>(ref T t, Matrix2X2F m) where T : Transformable { t.Position = t.Position.X * m.span.Item1.get() + t.Position.Y * m.span.Item2.get(); }
        public void scalePosition(ref Vertex t, Matrix2X2F m) { t.Position = t.Position.X * m.span.Item1.get() + t.Position.Y * m.span.Item2.get(); }

        public Shape convSFML_S(Shape d, FVWindow w, Vec2f offset, Matrix2X2F pos_scale)
        {
            scalePosition(ref d, pos_scale); //scale
            addPosition(ref d, offset);  // add offset

            // flip
            scalePosition(ref d, new Matrix2X2F((1,0),(0,-1)));
            addPosition(ref d, new Vec2f(0, w.window.Size.Y));

            d.Rotation = 360 - d.Rotation;
            d.Scale = new Vector2f(d.Scale.X, d.Scale.Y * -1);

            return d;
        }

        public VertexArray convSFML_V(VertexArray v, FVWindow w, Vector2f offset, Matrix2X2F pos_scale)
        {
            VertexArray v_transformed = new VertexArray(v);
            v_transformed.Clear();

            for (uint i = 0; i < v.VertexCount; i++)
            {
                Vertex v_single = v[i];

                scalePosition(ref v_single, pos_scale); // scale
                addPosition(ref v_single, offset);  // add offset

                // flip
                scalePosition(ref v_single, new Matrix2X2F((1, 0), (0, -1)));
                addPosition(ref v_single, new Vector2f(0, w.window.Size.Y));

                //v_single.Position = new Vector2f(v_single.Position.X, w.window.Size.Y - v_single.Position.Y); // flipY
                v_transformed.Append(v_single);
            }

            return v_transformed;
        }

        public Sprite convSFML_SPR(Sprite s, FVWindow w, Vec2f offset, Matrix2X2F pos_scale)
        {
            Sprite temp = new Sprite(s);

            scalePosition(ref temp, pos_scale); // scale
            addPosition(ref temp, offset);  // add offset

            // flip
            scalePosition(ref temp, new Matrix2X2F((1, 0), (0, -1)));
            addPosition(ref temp, new Vector2f(0, w.window.Size.Y));

            temp.Rotation = 360 - temp.Rotation;

            return temp;
        }

        public abstract void draw(FVWindow target_window, Vec2f offset, Matrix2X2F pos_scale);
    }
}
