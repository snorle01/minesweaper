using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using minesweeper.UI;

namespace minesweeper.states;
public class MenuClass : StateClass
{
    private ButtonControler buttons;
    public MenuClass(Game1 game, ContentManager content, GraphicsDevice graphics_device) : base(game, content, graphics_device) {
        ButtonClass[] button = new ButtonClass[] {new ButtonClass("Start game", new Vector2(100, 100), content.Load<SpriteFont>("font"), start_game),
                                                  new ButtonClass("Exit", new Vector2(100, 130), content.Load<SpriteFont>("font"), exit_game)};
        buttons = new ButtonControler(button);
    }

    public override void draw(SpriteBatch sprite_batch) {
        graphics_device.Clear(Color.CornflowerBlue);

        buttons.draw(sprite_batch);
    }

    public override void update(GameTime game_time) {
        buttons.update();
    }

    // button
    private void start_game(object sender, EventArgs e) {
        game.change_state(new StartGame(game, content, graphics_device));
    }
    private void exit_game(object sender, EventArgs e) {
        game.quit();
    }
}