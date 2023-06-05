using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace minesweeper.UI;

public class WriteControler {
    private MouseState prev_mouse;
    private MouseState current_mouse;
    private KeyboardState prev_keyboard;
    private KeyboardState current_keyboard;
    private int? selected_index = null;
    public WriteClass[] writes;

    public WriteControler(WriteClass[] writes) {
        this.writes = writes;
    }

    public void draw(SpriteBatch sprite_batch) {
        int index = 0;
        foreach (WriteClass write in writes) {
            bool draw_higlighted = false;
            if (index == selected_index) {
                draw_higlighted = true;
            }

            write.draw(sprite_batch, draw_higlighted);
            index++;
        }
    }

    public void update() {
        prev_mouse = current_mouse;
        current_mouse = Mouse.GetState();
        prev_keyboard = current_keyboard;
        current_keyboard = Keyboard.GetState();

        if (current_mouse.LeftButton == ButtonState.Released && prev_mouse.LeftButton == ButtonState.Pressed) {
            int index = 0;
            foreach (WriteClass write in writes) {
                if (write.rect.Contains(new Vector2(Mouse.GetState().X, Mouse.GetState().Y))) {
                    selected_index = index;
                    break;
                }
                index++;
            }
            
            if (index == writes.Count()) {
                selected_index = null;
            }
        }

        if (selected_index != null) {
            if (Keyboard_pressed(Keys.Back) && writes[(int)selected_index].text.Length != 0) {
                writes[(int)selected_index].text = writes[(int)selected_index].text.Remove(writes[(int)selected_index].text.Length - 1);
            }
            if (Keyboard_pressed(Keys.D0)) {
                writes[(int)selected_index].text += "0";
            } 
            else if (Keyboard_pressed(Keys.D1)) {
                writes[(int)selected_index].text += "1";
            }
            else if (Keyboard_pressed(Keys.D2)) {
                writes[(int)selected_index].text += "2";
            }
            else if (Keyboard_pressed(Keys.D3)) {
                writes[(int)selected_index].text += "3";
            }
            else if (Keyboard_pressed(Keys.D4)) {
                writes[(int)selected_index].text += "4";
            }
            else if (Keyboard_pressed(Keys.D5)) {
                writes[(int)selected_index].text += "5";
            }
            else if (Keyboard_pressed(Keys.D6)) {
                writes[(int)selected_index].text += "6";
            }
            else if (Keyboard_pressed(Keys.D7)) {
                writes[(int)selected_index].text += "7";
            }
            else if (Keyboard_pressed(Keys.D8)) {
                writes[(int)selected_index].text += "8";
            }
            else if (Keyboard_pressed(Keys.D9)) {
                writes[(int)selected_index].text += "9";
            }
        }
    }

    private bool Keyboard_pressed(Keys key) {
        return current_keyboard.IsKeyDown(key) && prev_keyboard.IsKeyUp(key);
    }
}

public class WriteClass {
    private SpriteFont font;
    private Vector2 pos;
    public string text;
    public Rectangle rect;

    public WriteClass(SpriteFont font, Vector2 pos, string start_string = "") {
        this.font = font;
        this.pos = pos;
        this.text = start_string;

        // makes rect
        string temp_string;
        if (start_string == "") {
            temp_string = "_";
        } else {
            temp_string = text;
        }

        Vector2 string_size = font.MeasureString(temp_string);
        this.rect = new Rectangle((int)pos.X, (int)pos.Y, (int)string_size.X, (int)string_size.Y);
    }

    public void draw(SpriteBatch sprite_batch, bool highlighted) {

        Color color;
        if (highlighted) {
            color = Color.White;
        } else {
            color = Color.Gray;
        }

        string write_text;
        if (text == "") {
            write_text = "_";
        } else {
            write_text = text;
        }

        sprite_batch.DrawString(font, write_text, pos, color);
    }
}