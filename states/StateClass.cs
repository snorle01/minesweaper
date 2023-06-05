using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace minesweeper;

public abstract class StateClass {
    protected Game1 game;
    protected ContentManager content;
    protected GraphicsDevice graphics_device;

    public StateClass(Game1 game, ContentManager content, GraphicsDevice graphics_device) {
        this.game = game;
        this.content = content;
        this.graphics_device = graphics_device;
    }

    public abstract void draw(SpriteBatch sprite_batch);

    public abstract void update(GameTime game_time);
}