using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using minesweeper.UI;

namespace minesweeper.states;

public class PauseClass : StateClass
{
    StateClass prev_state;
    Texture2D fade_texture;
    SpriteFont font;
    ButtonControler buttons;
    public PauseClass(Game1 game, ContentManager content, GraphicsDevice graphics_device, StateClass prev_state) : base(game, content, graphics_device) {
        this.prev_state = prev_state;
        this.font = content.Load<SpriteFont>("font");

        // make fade texture
        fade_texture = new Texture2D(graphics_device, game.screen_width, game.screen_height);
        Color[] colors = new Color[game.screen_width * game.screen_height];
        for(int pixel = 0; pixel < colors.Count(); pixel++) {
            colors[pixel] = new Color(0, 0, 0, 100);
        }
        fade_texture.SetData(colors);

        // buttons
        ButtonClass[] Buttons_array = new ButtonClass[] {new ButtonClass("Resume", new Vector2(100, 150), font, resume_button),
                                                         new ButtonClass("Title", new Vector2(100, 180), font, title_button)};
        buttons = new ButtonControler(Buttons_array);
    }

    public override void draw(SpriteBatch sprite_batch) {
        prev_state.draw(sprite_batch);

        sprite_batch.Draw(fade_texture, new Vector2(0, 0), Color.White);
        sprite_batch.DrawString(font, "Pause", new Vector2(100, 100), Color.White);
        
        buttons.draw(sprite_batch);
    }

    public override void update(GameTime game_time) {
        if (game.Keyboard_pressed(Keys.Escape)) {
            game.change_state(prev_state);
        }

        buttons.update();
    }



    // button
    public void resume_button(object sender, EventArgs e) {
        game.change_state(prev_state);
    }
    public void title_button(object sender, EventArgs e) {
        game.change_state(new MenuClass(game, content, graphics_device));
    }
}