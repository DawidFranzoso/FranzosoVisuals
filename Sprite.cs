using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace FranzosoVisuals
{
    class FSprite : Primitive
    {
        IValue<Vec2f> position;
        Texture tex;
        Sprite sprite;

        public FSprite(byte[] bytes, IValue<Vec2f> position_a)
        {
            position = position_a;

            tex = new Texture(bytes);
            sprite = new Sprite(tex);
            sprite.Origin = new Vector2f(0, sprite.TextureRect.Height);
        }

        public override void draw(FVWindow target_window, Vec2f offset, Matrix2X2F pos_scale)
        {
            sprite.Position = position.get();
            target_window.window.Draw(convSFML_SPR(sprite, target_window, offset, pos_scale));
        }
    }
}
