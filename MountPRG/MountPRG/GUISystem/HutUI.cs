using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class HutUI
    {

        private PanelUI panel;

        private List<SpriteUI> avatars = new List<SpriteUI>();
        private List<TextUI> names = new List<TextUI>();
        private List<CheckboxUI> checkboxes = new List<CheckboxUI>();

        private List<SettlerControllerCmp> settlers;

        private const int AVATAR_SIZE = 48;
        private const int TEXT_WIDTH = 80;

        private const int ELEMENTS_COUNT = 3;

        private int scrollPos;

        public bool Active
        {
            get; set;
        }

        public HutUI()
        {
            panel = new PanelUI();

            panel.InnerWidth = AVATAR_SIZE + GUIManager.OFFSET + TEXT_WIDTH + GUIManager.OFFSET + GUIManager.CHECKBOX_SIZE;

            panel.InnerHeight = ELEMENTS_COUNT * AVATAR_SIZE + (ELEMENTS_COUNT - 1) * GUIManager.OFFSET;

            panel.X = Game1.ScreenRectangle.Width - panel.Width;
            panel.Y = Game1.ScreenRectangle.Height - panel.Height;

            settlers = new List<SettlerControllerCmp>();

            for(int i = 0; i < ELEMENTS_COUNT; i++)
            {
                avatars.Add(new SpriteUI(ResourceBank.Sprites["avatar"], AVATAR_SIZE, AVATAR_SIZE));
                avatars[i].X = panel.InnerX;
                avatars[i].Y = panel.InnerY + i * AVATAR_SIZE + i * GUIManager.OFFSET;

                names.Add(new TextUI(ResourceBank.Fonts["mountFont"], "Name"));
                names[i].X = avatars[i].X + AVATAR_SIZE + GUIManager.OFFSET;
                names[i].Y = avatars[i].Y;

                checkboxes.Add(new CheckboxUI());
                checkboxes[i].X = names[i].X + TEXT_WIDTH + GUIManager.OFFSET;
                checkboxes[i].Y = avatars[i].Y;
            }
        }

        public void Update(GameTime gameTime)
        {
            if(Active)
            {
                if (panel.Intersects(InputManager.GetX(), InputManager.GetY()))
                {
                    GUIManager.MouseOnUI = true;

                    scrollPos = MathHelper.Clamp(scrollPos - InputManager.Scroll, 0, settlers.Count - ELEMENTS_COUNT);


                    if(InputManager.GetMouseButtonDown(MouseInput.LeftButton))
                    {
                        for(int i = 0; i < ELEMENTS_COUNT; i++)
                        {
                            if(checkboxes[i].Intersects(InputManager.GetX(), InputManager.GetY()))
                            {
                                if (!checkboxes[i].Marked)
                                {
                                    //hut.SetOwner((Settler)settlers[i + scrollPos].Parent);
                                }
                                else
                                {
                                    //hut.SetOwner(null);
                                }
                            }
}
                    }
                }

                for (int i = 0; i < ELEMENTS_COUNT; i++)
                {
                    // avatars[i].Texture = settlers[i + scrollPos].Avatar;
                    names[i].Text = settlers[i + scrollPos].Name;

                    checkboxes[i].Marked = false;
                    //if (settlers[i + scrollPos].Parent.Equals(hut.Owner))
                        //checkboxes[i].Marked = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                panel.Draw(spriteBatch);

                for(int i = 0; i < ELEMENTS_COUNT; i++)
                {
                    avatars[i].Draw(spriteBatch);
                    names[i].Draw(spriteBatch);
                    checkboxes[i].Draw(spriteBatch);
                }
            }
        }

      
        public void Open(List<Settler> stl)
        {
            Active = true;

            //this.hut = hut;

            for (int i = 0; i < stl.Count; i++)
                settlers.Add(stl[i].Get<SettlerControllerCmp>());
        }

        public void Close()
        {
            Active = false;

            //hut = null;

            settlers.Clear();
        }

    }
}