using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class BuildUI
    {
        private PanelUI panel;

        private List<ButtonUI> buttons;

        public bool Active
        {
            get; set;
        }

        public BuildUI()
        {
            panel = new PanelUI();

            buttons = new List<ButtonUI>();

            panel.InnerWidth = 150;
            panel.InnerHeight = 150;

            panel.X = Game1.ScreenRectangle.Width - panel.Width;
            panel.Y = Game1.ScreenRectangle.Height - panel.Height;

            AddElement("Hut", new Hut());
            AddElement("Workbench", new Workbench());
        }

        public void Update(GameTime gameTime)
        {
            if(Active)
            {
                if(InputManager.GetMouseButtonDown(MouseInput.RightButton))
                {
                    Close();
                }

                if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
                {
                    if (panel.Intersects(InputManager.GetX(), InputManager.GetY()))
                    {
                        GUIManager.MouseOnUI = true;

                        for (int i = 0; i < buttons.Count; i++)
                        {
                            buttons[i].GetButtonDown();
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                panel.Draw(spriteBatch);

                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Draw(spriteBatch);
                }
            }
        }

        public void Open()
        {
            Active = true;
        }

        public void Close()
        {
            Active = false;
        }

        private void AddElement(string name, Entity entity)
        {
            ButtonUI button = new ButtonUI(ResourceBank.Sprites["button"], new TextUI(ResourceBank.Fonts["mountFont"], name));

            button.X = panel.InnerX;
            button.Y = panel.InnerY + (buttons.Count * button.Height) + (buttons.Count * GUIManager.OFFSET);

            button.OnButtonDownCallback(delegate
            {
                GamePlayState.WorldManager.CurrentBuilding = entity.Clone();
                GamePlayState.Entities.Add(GamePlayState.WorldManager.CurrentBuilding);
                Close();
            });

            buttons.Add(button);
        }

    }
}
