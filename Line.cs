using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;

namespace FranzosoVisuals
{
    abstract class Line : Primitive
    {
        Rf<float> max_length = 9999;
        protected Rf<float> default_thickness = 1.1f;
        static protected Rf<float> default_length = 0;

        protected IValue<Vec2f> anchor_coord;
        IValue<float> thickness;
        IValue<bool> origin_centered;
        IValue<Color> color;

        abstract protected float getLength();
        abstract protected float getFiRad();
        abstract public (IValue<Vec2f>, IValue<Vec2f>) getPoints();
        float getFiDeg() { return MathExt.RadToDeg(getFiRad()); }

        RectangleShape correspondingRect()
        {
            RectangleShape r = new RectangleShape();

            r.Size = new Vec2f(getLength()!=0? getLength():max_length.get(), thickness.get());

            r.Origin = new Vec2f(getLength() == 0||origin_centered.get()==true? r.Size.X / 2:0, r.Size.Y / 2);

            r.Position = anchor_coord.get();
            r.Rotation = getFiDeg();
            r.FillColor = color.get();

            return r;
        }

        public Line(IValue<Vec2f> anchor_coord_a, IValue<bool> origin_centered_a, IValue<float> thickness_a = null, IValue<Color> color_a = null) // length = 0 -> inifitely long line
        { // change all the Rf<> arguments to functions
            anchor_coord = anchor_coord_a;
            thickness = MathExt.defIfNull(thickness_a, default_thickness);
            color = MathExt.defIfNull(color_a, new Rf<Color>(Color.White));
            origin_centered = origin_centered_a;
        }

        public override void draw(FVWindow target_window)
        {
            target_window.window.Draw(convSFML_S(correspondingRect(),target_window));
        }
    }
}
