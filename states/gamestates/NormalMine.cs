using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace minesweeper.states.gamestates;

public class NormalMine : GameClass {
    public NormalMine(Game1 game, ContentManager content, GraphicsDevice graphics_device, int num_of_mines, int grid_size) : base(game, content, graphics_device, num_of_mines, grid_size) {
    }

    public override void lose() {
        foreach (Vector2 mine_pos in mine_locations) {
            int index = pos_to_index(mine_pos);
            fields[index].is_reveled = true;
        }
        game.change_state(new GameOver(game, content, graphics_device, this));
    }

    public override void win() {
        game.change_state(new WinClass(game, content, graphics_device, this));
    }
}