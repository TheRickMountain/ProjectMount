using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MountPRG
{
    public class HutUI : IGUI
    {
        private UI background;

        private SpriteFont font;

        private List<Slot> avatars;
        private List<string> names;
        private List<Button> checkboxes;

        public HutUI(bool active) : base(active)
        {
            background = new UI();

            font = ResourceBank.Fonts["mountFont"];
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetKeyDown(Keys.E) || InputManager.GetMouseButtonDown(MouseInput.RightButton))
            {
                Active = false;

                avatars.Clear();
                names.Clear();
                checkboxes.Clear();
            }

            if(InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                for (int i = 0; i < checkboxes.Count; i++)
                {
                    if(checkboxes[i].Intersects(InputManager.GetX(), InputManager.GetY()))
                    {
                        UnselectAll();
                        checkboxes[i].Icon = ResourceBank.Sprites["mark"];
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            for(int i = 0; i < avatars.Count; i++)
            {
                avatars[i].Draw(spriteBatch);
            }

            for(int i = 0; i < names.Count; i++)
            {
                spriteBatch.DrawString(font, names[i],
                    new Vector2(avatars[i].X + GUIManager.SLOT_SIZE + GUIManager.OFFSET, avatars[i].Y), Color.White);
            }

            for(int i = 0; i < checkboxes.Count; i++)
            {
                checkboxes[i].Draw(spriteBatch);
            }
        }

        private void UnselectAll()
        {
            for (int i = 0; i < checkboxes.Count; i++)
            {
                checkboxes[i].Icon = null;
            }
        }
    

        public void Open(Hut hut)
        {
            Active = true;

            avatars = new List<Slot>();
            names = new List<string>();
            checkboxes = new List<Button>();

            List<Settler> settlers = GamePlayState.Settlers;

            for (int i = 0; i < settlers.Count; i++)
            {
                avatars.Add(new Slot(ResourceBank.Sprites["avatar"], GUIManager.SLOT_SIZE, GUIManager.SLOT_SIZE, true));
                names.Add(settlers[i].Get<SettlerController>().Name);
                checkboxes.Add(new Button(ResourceBank.Sprites["checkbox"], "", true));
            }

            background.InnerWidth = GUIManager.SLOT_SIZE + GUIManager.OFFSET + 60 + GUIManager.OFFSET + 16;
            background.InnerHeight = GUIManager.SLOT_SIZE * avatars.Count + GUIManager.OFFSET * (avatars.Count - 1);

            background.X = Game1.ScreenRectangle.Width - background.Width;
            background.Y = Game1.ScreenRectangle.Height - background.Height;

            for (int i = 0; i < settlers.Count; i++)
            {
                avatars[i].X = background.InnerX;
                avatars[i].Y = background.InnerY + GUIManager.SLOT_SIZE * i + GUIManager.OFFSET * i;

                checkboxes[i].X = avatars[i].X + GUIManager.SLOT_SIZE + GUIManager.OFFSET + 50 + GUIManager.OFFSET;
                checkboxes[i].Y = avatars[i].Y;
                checkboxes[i].Width = 16;
                checkboxes[i].Height = 16;
            }
        }
    }

}
