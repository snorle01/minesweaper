using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace minesweeper.entity;

public class FieldClass {
    private SpriteFont font;
    private Dictionary<string, Texture2D> textures;

    private Color color;
    public bool is_reveled;
    public bool is_mine;
    public bool is_flaged;
    public int number;

    public FieldClass(Dictionary<string, Texture2D> textures) {
        this.textures = textures;
    }

    public void draw(SpriteBatch sprite_batch, Vector2 pos) {
        if (is_reveled) {
            if (is_mine) {
                sprite_batch.Draw(textures["dark"], pos, Color.White);
                sprite_batch.Draw(textures["mine"], pos, new Rectangle(0, 0, textures["dark"].Width, textures["dark"].Height), Color.White);
            }
            else {
                Vector2 string_vector = font.MeasureString(number.ToString()) / 2;
                Vector2 grid_vector = new Vector2(textures["dark"].Width, textures["dark"].Height) / 2;

                Vector2 number_pos = pos + grid_vector - string_vector;
                sprite_batch.Draw(textures["dark"], pos, Color.White);
                sprite_batch.DrawString(font, number.ToString(), number_pos, color);
            }
        } else if (is_flaged) {
            sprite_batch.Draw(textures["light"], pos, Color.White);
            sprite_batch.Draw(textures["flag"], pos, Color.White);
        } else {
            sprite_batch.Draw(textures["light"], pos, Color.White);
        }
    }

    public void set_number(int num, SpriteFont font) {
        this.font = font;
        this.number = num;
        switch (number) {
        case 1:
            this.color = Color.Black;
            break;
        case 2:
            this.color = Color.Purple;
            break;
        case 3:
            this.color = Color.Blue;
            break;
        case 4:
            this.color = Color.Cyan;
            break;
        case 5:
            this.color = Color.Green;
            break;
        case 6:
            this.color = Color.Yellow;
            break;
        case 7:
            this.color = Color.Orange;
            break;
        case 8:
            this.color = Color.Red;
            break;
        }
    }
}