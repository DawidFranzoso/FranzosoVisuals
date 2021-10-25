using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace FranzosoVisuals
{
    class Line_fi : Line
    {
        IValue<float> fi_rad, length;

        public override (IValue<Vec2f>, IValue<Vec2f>) getPoints()
        => (anchor_coord, new FuncA<Line_fi, Vec2f>(line => line.anchor_coord.get() + MathExt.rotVec((line.length.get(), 0), fi_rad.get()), this));
        
        public Line_fi(IValue<Vec2f> anchor_coord_a, IValue<float> fi_rad_a, IValue<float> length_a = null, IValue<bool> isInfinite_a = null, IValue<Color> color_a = null, IValue<bool> origin_centered_a = null, IValue<float> thickness_a = null)
            : base(anchor_coord_a, MathExt.defIfNull(isInfinite_a,new Rf<bool>(false)), MathExt.defIfNull(origin_centered_a, MathExt.defIfNull(length_a, default_length).get()==0?new Rf<bool>(true):new Rf<bool>(false)), thickness_a, color_a)
        {
            fi_rad = fi_rad_a;
            length = MathExt.defIfNull(length_a, default_length);
        }
    }
}
