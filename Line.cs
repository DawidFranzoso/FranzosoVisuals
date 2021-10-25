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
        protected IValue<bool> isInfinite;

        protected float getLength() => MathExt.magnitude2(getPoints().Item2.get() - getPoints().Item1.get());
        protected float getLengthTransformed(Matrix2X2F pos_scalar)
            => MathExt.magnitude2(getPointsPositionsTransformed(pos_scalar).Item2.get() - getPointsPositionsTransformed(pos_scalar).Item1.get());

        protected float getFiRad(Matrix2X2F pos_scalar)
        {
            float deltaX = getPointsPositionsTransformed(pos_scalar).Item2.X - getPointsPositionsTransformed(pos_scalar).Item1.X;
            float deltaY = getPointsPositionsTransformed(pos_scalar).Item2.Y - getPointsPositionsTransformed(pos_scalar).Item1.Y;

            return (float)Math.Atan2(deltaY, deltaX);
        }

        abstract public (IValue<Vec2f>, IValue<Vec2f>) getPoints();

        protected (Vec2f, Vec2f) getPointsPositionsTransformed(Matrix2X2F pos_scalar)
            => (pos_scalar.returnTransformed(getPoints().Item1.get()), pos_scalar.returnTransformed(getPoints().Item2.get()));

        float getFiDeg(Matrix2X2F pos_scalar) { return MathExt.RadToDeg(getFiRad(pos_scalar)); }

        RectangleShape correspondingRect(Matrix2X2F pos_scalar)
        {
            RectangleShape r = new RectangleShape();

            r.Size = new Vec2f(isInfinite.get()==false? getLengthTransformed(pos_scalar): max_length.get(), thickness.get());

            r.Origin = new Vec2f(isInfinite.get() == true || origin_centered.get()==true? r.Size.X / 2:0, r.Size.Y / 2);

            r.Position = getPointsPositionsTransformed(pos_scalar).Item1;
            r.Rotation = getFiDeg(pos_scalar);
            r.FillColor = color.get();

            return r;
        }

        public Line(IValue<Vec2f> anchor_coord_a, IValue<bool> isInfinite_a, IValue<bool> origin_centered_a, IValue<float> thickness_a = null, IValue<Color> color_a = null) // length = 0 -> inifitely long line
        {
            anchor_coord = anchor_coord_a;
            thickness = MathExt.defIfNull(thickness_a, default_thickness);
            color = MathExt.defIfNull(color_a, new Rf<Color>(Color.White));
            isInfinite = isInfinite_a;
            origin_centered = origin_centered_a;
        }

        public override void draw(FVWindow target_window, Vec2f offset, Matrix2X2F pos_scalar)
        {
            target_window.window.Draw(convSFML_S(correspondingRect(pos_scalar),target_window, offset, Matrix2X2F.unit));
        }
    }
}
