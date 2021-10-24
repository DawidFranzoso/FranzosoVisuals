using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace FranzosoVisuals
{
    abstract class Primitive
    {
        public Shape convSFML_S(Shape d, FVWindow w)
        {
            d.Position = new Vector2f(d.Position.X, w.window.Size.Y - d.Position.Y);
            d.Rotation = 360 - d.Rotation;
            d.Scale = new Vector2f(d.Scale.X, d.Scale.Y * -1);

            return d;
        }

        public VertexArray convSFML_V(VertexArray v, FVWindow w)
        {
            VertexArray v_transformed = new VertexArray(v);
            v_transformed.Clear();

            for (uint i = 0; i < v.VertexCount; i++)
            {
                Vertex v_single = v[i];
                v_single.Position = new Vector2f(v_single.Position.X, w.window.Size.Y - v_single.Position.Y);
                v_transformed.Append(v_single);
            }

            return v_transformed;
        }

        public Sprite convSFML_SPR(Sprite s, FVWindow w)
        {
            Sprite temp = new Sprite(s);

            temp.Position = new Vector2f(temp.Position.X, w.window.Size.Y - temp.Position.Y);
            temp.Rotation = 360 - temp.Rotation;

            return temp;
        }

        public abstract void draw(FVWindow target_window);
    }
}
