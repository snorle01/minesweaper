using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using minesweeper.entity;

namespace minesweeper.states.gamestates;

public abstract class GameClass : StateClass
{
    private int grid_width;
    private int grid_height;
    private int grid_offset_x;
    private int grid_offset_y;
    private int grid_size;
    private int total_mines_flaged = 0;
    private int total_flags_left;
    private Random random = new Random();
    private bool started = false;
    public int total_num_mines;
    public int texture_size;
    public Vector2[] mine_locations;
    public FieldClass[] fields;

    // top menu
    private int top_space_height = 40;
    private Texture2D flag_texture;
    private SpriteFont font;

    public GameClass(Game1 game, ContentManager content, GraphicsDevice graphics_device, int num_of_mines, int texture_size) : base(game, content, graphics_device) {
        this.total_num_mines = num_of_mines;
        this.texture_size = texture_size;
        this.total_flags_left = total_num_mines;
        this.flag_texture = content.Load<Texture2D>("flag");
        this.font = content.Load<SpriteFont>("font");

        // grid variables
        this.grid_width = game.screen_width / texture_size;
        this.grid_height = (game.screen_height - top_space_height) / texture_size;
        this.grid_size = grid_width * grid_height;
        this.grid_offset_x = (game.screen_width % texture_size) / 2;
        this.grid_offset_y = ((game.screen_height - top_space_height) % texture_size) / 2;

        // textures for field
        Texture2D grid_texture_light = make_texture(new Color(150, 150, 150), new Color(250, 250, 250), new Color(200, 200, 200));
        Texture2D grid_texture_dark = make_texture(new Color(120, 120, 120), new Color(220, 220, 220), new Color(170, 170, 170));
        Dictionary<string, Texture2D> field_textures = new Dictionary<string, Texture2D>() {{"light", grid_texture_light}, 
                                                                                            {"dark", grid_texture_dark}, 
                                                                                            {"mine", content.Load<Texture2D>("mine")}, 
                                                                                            {"flag", content.Load<Texture2D>("flag")}};

        fields = new FieldClass[this.grid_size];
        for (int index = 0; index < fields.Count(); index++) {
            fields[index] = new FieldClass(field_textures);
        }
    }

    public override void draw(SpriteBatch sprite_batch) {
        graphics_device.Clear(new Color(100, 100, 100));

        int x = 0;
        int y = 0;
        Vector2 pos;
        foreach(FieldClass field in fields) {
            pos = new Vector2(x * texture_size + grid_offset_x, (y * texture_size + grid_offset_y) + top_space_height);
            field.draw(sprite_batch, pos);

            x++;
            if (x == grid_width) {
                x = 0;
                y++;
            }
        }

        sprite_batch.Draw(flag_texture, new Vector2(10, 10), Color.White);
        sprite_batch.DrawString(font, "Flags left " + total_flags_left, new Vector2(20 + flag_texture.Width, 10), Color.White);
    }

    public override void update(GameTime game_time) {
        if (game.Keyboard_pressed(Keys.Escape)) {
            game.change_state(new PauseClass(game, content, graphics_device, this));
        }

        if (game.mouse_clicked_left()) {
            Vector2 mouse_pos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

            if (pos_on_grid(mouse_pos)) {
                Vector2 grid_pos = screen_to_grid(mouse_pos);
                int index = pos_to_index(grid_pos);

                // once cliked on the grid make the grid
                if (started == false) {
                    make_grid(screen_to_grid(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)));
                    started = true;
                }

                // reveal field
                if (fields[index].is_flaged == false) {
                    fields[index].is_reveled = true;

                    // game over
                    if (fields[index].is_mine) {
                        lose();

                    // if field is 0 reveal other tiles
                    } else if (fields[index].number == 0 && fields[index].is_mine == false) {
                        List<Vector2> done_list = new List<Vector2>();
                        clear_empty_area(grid_pos, done_list);
                    }
                }
            }
        }
        if (game.mouse_clicked_right()) {
            Vector2 mouse_pos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

            if (pos_on_grid(mouse_pos)) {
                Vector2 grid_pos = screen_to_grid(mouse_pos);
                int index = pos_to_index(grid_pos);

                // flag field
                if (fields[index].is_reveled == false) {
                    if (fields[index].is_flaged) {
                        fields[index].is_flaged = false;
                        total_flags_left++;
                        if (fields[index].is_mine) {
                            total_mines_flaged--;
                        }
                    } else if (total_flags_left > 0) {
                        fields[index].is_flaged = true;
                        total_flags_left--;
                        if (fields[index].is_mine) {
                            total_mines_flaged++;

                            // win
                            if (total_mines_flaged == total_num_mines) {
                                win();
                            }
                        }
                    }
                }
            }
        }
    }



    private void make_grid(Vector2 mouse_pos) {
        // makes all the mine locations
        List<Vector2> no_mine = get_neighbours_sqare(mouse_pos);
        no_mine.Add(mouse_pos);
        
        mine_locations = new Vector2[total_num_mines];
        for (int index = 0; index < total_num_mines; index++) {
            Vector2 new_mine = new Vector2(random.Next(0, grid_width), random.Next(0, grid_height));

            // check if mine is valid
            bool valid = true;

            foreach (Vector2 no_mine_pos in no_mine) {
                if (no_mine_pos == new_mine) {
                    valid = false;
                    break;
                }
            }

            if (valid) {
                foreach (Vector2 old_mine in mine_locations) {
                    if (old_mine == new_mine) {
                        valid = false;
                        break;
                    }
                }
            }

            if (valid) {
                mine_locations[index] = new_mine;
            } else {
                index--;
            }
        }

        // makes the numbers on the tiles
        List<int> int_list = Enumerable.Repeat(0, this.grid_size).ToList();

        foreach (Vector2 mine in mine_locations) {
            List<Vector2> neighbours = get_neighbours_sqare(mine);
            foreach (Vector2 pos in neighbours) {
                int_list[pos_to_index(pos)]++;
            }
        }

        // makes all the fields
        List<FieldClass> field_list = new List<FieldClass>();
        int x = 0;
        int y = 0;
        for (int index = 0; index < this.grid_size; index++) {

            foreach(Vector2 mine in mine_locations) {
                if (new Vector2(x, y) == mine) {
                    fields[index].is_mine = true;
                    break;
                }
            }

            fields[index].set_number(int_list[index], font);

            x++;
            if (x == grid_width) {
                x = 0;
                y++;
            }
        }
    }

    private Texture2D make_texture(Color standard_color, Color highlight_color, Color dark_color) {
        int x = 0;
        int y = 0;
        Texture2D texture = new Texture2D(graphics_device, texture_size, texture_size);
        Color[] colors = new Color[texture_size * texture_size];
        for(int pixel = 0; pixel < colors.Count(); pixel++) {

            if (x == 0 || y == 0) {
                colors[pixel] = highlight_color;
            } else if (x == texture_size - 1 || y == texture_size - 1) {
                colors[pixel] = standard_color;
            } else {
                colors[pixel] = dark_color;
            }

            x++;
            if (x == texture_size) {
                x = 0;
                y++;
            }
        }
        texture.SetData(colors);
        return texture;
    }

    private Vector2 screen_to_grid(Vector2 pos) {
        return new Vector2((int)((pos.X - grid_offset_x) / texture_size), (int)((pos.Y - grid_offset_y - top_space_height) / texture_size));
    }
    public int pos_to_index(Vector2 pos) {
        return (int)pos.X + (int)(grid_width * pos.Y);
    }

    private List<Vector2> get_neighbours_star(Vector2 pos) {
        List<Vector2> list = new List<Vector2>();

        // top
        if (pos.Y != 0) {
            list.Add(pos + new Vector2(0, -1));
        }
        // bottom
        if (pos.Y != grid_height - 1) {
            list.Add(pos + new Vector2(0, 1));
        }
        // left
        if (pos.X != 0) {
            list.Add(pos + new Vector2(-1, 0));
        }
        // right
        if (pos.X != grid_width - 1) {
            list.Add(pos + new Vector2(1, 0));
        }
        return list;
    }

    private List<Vector2> get_neighbours_sqare(Vector2 pos) {
        List<Vector2> list = new List<Vector2>();

        // top
        if (pos.Y != 0) {
            list.Add(pos + new Vector2(0, -1));
            // top left
            if (pos.X != 0) {
                list.Add(pos + new Vector2(-1, -1));
            }
            // top right
            if (pos.X != grid_width - 1) {
                list.Add(pos + new Vector2(1, -1));
            }
        }

        // bottom
        if (pos.Y != grid_height - 1) {
            list.Add(pos + new Vector2(0, 1));
            // left
            if (pos.X != 0) {
                list.Add(pos + new Vector2(-1, 1));
            }
            // right
            if (pos.X != grid_width - 1) {
                list.Add(pos + new Vector2(1, 1));
            }
        }

        // left
        if (pos.X != 0) {
            list.Add(pos + new Vector2(-1, 0));
        }
        // right
        if (pos.X != grid_width - 1) {
            list.Add(pos + new Vector2(1, 0));
        }
        return list;
    }

    private void clear_empty_area(Vector2 pos, List<Vector2> done_list) {
        done_list.Add(pos);
        List<Vector2> neighbours = get_neighbours_sqare(pos);

        // gets neighbouring tiles
        foreach (Vector2 neighbour in neighbours) {
            int index = pos_to_index(neighbour);

            // if tile is not reveled continue
            if (fields[index].is_reveled == false) {
                // checks if tile has been checked before
                bool new_pos = true;
                for (int i = 0; i < done_list.Count(); i++) {
                    if (neighbour == done_list[i]) {
                        new_pos = false;
                        break;
                    }
                }
                if (new_pos && fields[index].number == 0) {
                    clear_empty_area(neighbour, done_list);
                }
                fields[index].is_reveled = true;
            }
        }
    }

    private bool pos_on_grid(Vector2 pos) {
        return  pos.X >= 0 + grid_offset_x && pos.X <= game.screen_width - grid_offset_x &&
                pos.Y >= 0 + grid_offset_y + top_space_height && pos.Y <= game.screen_height - grid_offset_y;
    }



    public abstract void lose();
    public abstract void win();
}