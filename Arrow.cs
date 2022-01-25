using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace FranzosoVisuals
{
    class Arrow : Primitive
    {
        const float propTipWidthToLineWidth = 5;
        const float propTipLengthToTipWidth = 1.5f;
        Rf<float> default_thickness = 2;

        IValue<Vec2f> pos_vector;
        IValue<Vec2f> dir_vector;
        IValue<float> thickness;
        IValue<float> override_length = null;
        IValue<Color> color = null;

        public Arrow(IValue<Vec2f> pos_vector_a, IValue<Vec2f> dir_vector_a, IValue<float> thickness_a = null, IValue<float> override_length_a = null, IValue<Color> color_a = null)
        {
            pos_vector = pos_vector_a;
            dir_vector = dir_vector_a;
            thickness = MathExt.defIfNull(thickness_a,default_thickness);
            override_length = override_length_a;
            color = MathExt.defIfNull(color_a,new Rf<Color>(Color.White));
        }

        public override void draw(FVWindow target_window, Vec2f offset, Matrix2X2F pos_scale)
        {
            target_window.window.Draw(convSFML_V(getShape(offset, pos_scale), target_window));
        }

        VertexArray getShape(Vec2f offset, Matrix2X2F pos_scale)
        {
            VertexArray t = new VertexArray(PrimitiveType.TriangleStrip, 7);

            Vector2f position_transformed = pos_scale.returnTransformed(pos_vector.get()) + offset;
            Vector2f dir_vector_transformed = pos_scale.returnTransformed(dir_vector.get());

            float length_tip = override_length!=null?override_length.get():MathExt.magnitude2(dir_vector_transformed);
            float length_line = length_tip - thickness.get() * propTipWidthToLineWidth * propTipLengthToTipWidth;
            float thickness_diff = thickness.get() * ((propTipWidthToLineWidth - 1) / 2);
            Vec2f dir_left = MathExt.rotVecNeg(dir_vector_transformed);
            Vec2f dir_right = MathExt.rotVecPos(dir_vector_transformed);
            
            Vec2f pos_lower_left = MathExt.moveByLength(position_transformed, dir_left, thickness.get() / 2);
            Vec2f pos_lower_right = MathExt.moveByLength(position_transformed, dir_right, thickness.get() / 2);
            Vec2f pos_upper_left = pos_lower_left + MathExt.V2ofLength(dir_vector_transformed, length_line);
            Vec2f pos_upper_right = pos_lower_right + MathExt.V2ofLength(dir_vector_transformed, length_line);
            Vec2f pos_left_tip = pos_upper_left + MathExt.V2ofLength(dir_left, thickness_diff);
            Vec2f pos_right_tip = pos_upper_left + MathExt.V2ofLength(dir_right, thickness_diff);
            Vec2f pos_tip = position_transformed + MathExt.V2ofLength(dir_vector_transformed, length_tip);

            t[0] = new Vertex(pos_lower_left,color.get());
            t[1] = new Vertex(pos_lower_right, color.get());
            t[2] = new Vertex(pos_upper_left, color.get());
            t[3] = new Vertex(pos_upper_right, color.get());
            t[4] = new Vertex(pos_left_tip, color.get());
            t[5] = new Vertex(pos_right_tip, color.get());
            t[6] = new Vertex(pos_tip, color.get());

            return t;
        }
    }
}
