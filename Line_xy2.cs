using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;

namespace FranzosoVisuals
{
    class Line_xy2 : Line
    {
        IValue<Vec2f> point2_coord;
        IValue<bool> isInfinite;

        public Line_xy2(IValue<Vec2f> anchor_coord_a, IValue<Vec2f> point2_coord_a, IValue<bool> isInfinite_a = null, IValue<float> thickness_a = null, IValue<Color> color_a = null)
            : base(anchor_coord_a, new Rf<bool>(false), thickness_a, color_a)
        {
            point2_coord = point2_coord_a;
            isInfinite = MathExt.defIfNull<IValue<bool>>(isInfinite_a, new Rf<bool>(false));
        }

        public override (IValue<Vec2f>, IValue<Vec2f>) getPoints() => (anchor_coord, point2_coord);

        protected override float getFiRad()
        {
            float deltaX = point2_coord.get().X - anchor_coord.get().X;
            float deltaY = point2_coord.get().Y - anchor_coord.get().Y;

            return (float)Math.Atan2(deltaY, deltaX);
        }

        protected override float getLength()
        {
            if (isInfinite.get()) return 0;

            float deltaX = point2_coord.get().X - anchor_coord.get().X;
            float deltaY = point2_coord.get().Y - anchor_coord.get().Y;

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
