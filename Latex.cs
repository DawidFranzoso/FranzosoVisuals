using System;
using System.Collections.Generic;
using System.Text;
using WpfMath;
using SFML.Graphics;
using SFML.System;

namespace FranzosoVisuals
{
    class LatexSprite : Primitive
    {
        IValue<float> size;
        IValue<Func<string, string>> format;
        IValue<Vec2f> position;
        IValue<string> latex;
        TexFormulaParser parser = new TexFormulaParser();

        public LatexSprite(IValue<string> latex_a, IValue<Vec2f> position_a, IValue<float> size_a = null, IValue<Func<string, string>> format_a = null)
        {
            format = MathExt.defIfNull(format_a, new Rf<Func<string,string>>(s => @"\color{white}{\colorbox[RGBA]{0,0,0,192}{" + s + "}}"));
            size = MathExt.defIfNull(size_a, new Rf<float>(50.0f));
            position = position_a;
            latex = latex_a;
        }

        public FSprite createSprite()
        {
            var str = latex.get();
            TexFormula formula = parser.Parse(format.get().Invoke(str));
            return new FSprite(formula.RenderToPng(size.get(), 0.0, 0.0, "Arial"), position);
        }

        public override void draw(FVWindow target_window, Vec2f offset, Matrix2X2F pos_scale)
        {
            createSprite().draw(target_window,offset,pos_scale);
        }
    }
}
