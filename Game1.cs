using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using minesweeper.states;

namespace minesweeper;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private StateClass current_state;
    public int screen_width;
    public int screen_height;

    // input
    private MouseState prev_mouse;
    private MouseState current_mouse;
    private KeyboardState prev_keyboard;
    private KeyboardState current_keyboard;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {

        current_state = new MenuClass(this, Content, GraphicsDevice);
        
        screen_height = _graphics.PreferredBackBufferHeight;
        screen_width = _graphics.PreferredBackBufferWidth;

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }



    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();

        // update mouse
        prev_mouse = current_mouse;
        current_mouse = Mouse.GetState();
        // update keyboard
        prev_keyboard = current_keyboard;
        current_keyboard = Keyboard.GetState();

        current_state.update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {

        _spriteBatch.Begin();
        current_state.draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void change_state(StateClass new_state) {
        current_state = new_state;
    }

    public void quit() {
        Exit();
    }

    // input
    public bool mouse_clicked_left() {
        return current_mouse.LeftButton == ButtonState.Released && prev_mouse.LeftButton == ButtonState.Pressed;
    }
    public bool mouse_clicked_right() {
        return current_mouse.RightButton == ButtonState.Released && prev_mouse.RightButton == ButtonState.Pressed;
    }
    public bool Keyboard_pressed(Keys key) {
        return current_keyboard.IsKeyDown(key) && prev_keyboard.IsKeyUp(key);
    }
}
