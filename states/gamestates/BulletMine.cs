using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace minesweeper.states.gamestates;

public class BulletMine : GameClass {
    public BulletMine(Game1 game, ContentManager content, GraphicsDevice graphics_device, int num_of_mines, int texture_size) : base(game, content, graphics_device, num_of_mines, texture_size) {
    }

    public override void lose() {
    }

    public override void win() {
    }
}