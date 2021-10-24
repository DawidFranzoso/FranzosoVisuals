using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;


namespace FranzosoVisuals
{
    static class Screen
    {
        public static Vec2f getCenter(RenderWindow w)
        {
            return getCoordsOfProportions(w, 0.5f, 0.5f);
        }

        public static Vec2f getCoordsOfProportions(RenderWindow w, float propX, float propY)
        {
            return ((float)(w.Size.X) * propX, (float)(w.Size.Y) * propY);
        }
    }
}
