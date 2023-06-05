using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace minesweeper.UI;

public class ButtonControler {
    private MouseState prev_mouse;
    private MouseState current_mouse;
    private ButtonClass[] buttons;

    public ButtonControler(ButtonClass[] buttons) {
        this.buttons = buttons;
    }

    public void update() {
        prev_mouse = current_mouse;
        current_mouse = Mouse.GetState();

        if (current_mouse.LeftButton == ButtonState.Released && prev_mouse.LeftButton == ButtonState.Pressed) {
            foreach (ButtonClass button in buttons) {
                button.update();
            }
        }
    }

    public void draw(SpriteBatch sprite_batch) {
        foreach (ButtonClass button in buttons) {
            button.draw(sprite_batch);
        }
    }
}



public class ButtonClass {
    private SpriteFont font;
    private string text;
    public Rectangle rect;
    public event EventHandler click_event;

    public ButtonClass(string text, Vector2 pos, SpriteFont font, EventHandler click_event) {
        this.click_event = click_event;
        this.font = font;
        this.text = text;
        this.rect = new Rectangle((int)pos.X, (int)pos.Y, (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y);
    }

    public void update() {
        if (rect.Contains(Mouse.GetState().X, Mouse.GetState().Y)) {
            click_event.Invoke(this, new EventArgs());
        }
    }

    public void draw(SpriteBatch sprite_batch) {
        sprite_batch.DrawString(font, text, new Vector2(rect.X, rect.Y), Color.White);
    }

    public bool pos_on_button(Vector2 pos) {
        return rect.Contains(pos);
    }
}