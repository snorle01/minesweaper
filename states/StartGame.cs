using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using minesweeper.UI;
using minesweeper.states.gamestates;

namespace minesweeper.states;
public class StartGame : StateClass
{
    private WriteControler write_controler;
    private ButtonControler buttons;
    private SpriteFont font;
    private string warning_text = null;
    private int warning_timer = 0;

    public StartGame(Game1 game, ContentManager content, GraphicsDevice graphics_device) : base(game, content, graphics_device) {
        this.font = content.Load<SpriteFont>("font");
        WriteClass[] writes = new WriteClass[] {new WriteClass(font, new Vector2(font.MeasureString("number of mines").X + 120, 100), "50"),
                                                new WriteClass(font, new Vector2(font.MeasureString("size of grid").X + 120, 130), "30")};
        ButtonClass[] button = new ButtonClass[] {new ButtonClass("Start game", new Vector2(100, 160), font, start_game)};
        write_controler = new WriteControler(writes);
        buttons = new ButtonControler(button);
    }

    public override void draw(SpriteBatch sprite_batch) {
        graphics_device.Clear(Color.CornflowerBlue);

        write_controler.draw(sprite_batch);

        sprite_batch.DrawString(font, "number of mines", new Vector2(100, 100), Color.White);
        sprite_batch.DrawString(font, "size of grid", new Vector2(100, 130), Color.White);
        if (warning_text != null) {
            sprite_batch.DrawString(font, warning_text, new Vector2(100, 190), Color.Red);
        }

        buttons.draw(sprite_batch);
    }

    public override void update(GameTime game_time) {
        write_controler.update();
        buttons.update();

        if (warning_timer > 0) {
            warning_timer--;
            if (warning_timer == 0) {
                warning_text = null;
            }
        }
    }

    // button
    private void start_game(object sender, EventArgs e) {
        int grid_width = game.screen_width / int.Parse(write_controler.writes[1].text);
        int grid_height = (game.screen_height - 40) / int.Parse(write_controler.writes[1].text);
        int grid_size = grid_width * grid_height;

        bool there_are_mines = false;
        bool not_to_many_mines = false;

        if (int.Parse(write_controler.writes[0].text) != 0) {
            there_are_mines = true;
        } else {
            warning_text = "no mines!";
            warning_timer = 180;
        }

        if (int.Parse(write_controler.writes[0].text) < grid_size - 8) {
            not_to_many_mines = true;
        } else {
            warning_text = "to many  mines!";
            warning_timer = 180;
        }

        if (there_are_mines && not_to_many_mines) {
            game.change_state(new NormalMine(game, content, graphics_device, int.Parse(write_controler.writes[0].text), int.Parse(write_controler.writes[1].text)));
        }
    }
}