using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class FarmUI
    {

        private PanelUI panel;

        private const int ELEMENTS_COUNT = 5;

        private List<TextUI> names = new List<TextUI>();
        private List<CheckboxUI> checkboxes = new List<CheckboxUI>();

        private const int TEXT_WIDTH = 80;

        private int farmNum;

        public bool Active
        {
            get; set;
        }

        public FarmUI()
        {
            panel = new PanelUI();

            panel.InnerWidth = TEXT_WIDTH + GUIManager.CHECKBOX_SIZE;
            panel.InnerHeight = GUIManager.CHECKBOX_SIZE * ELEMENTS_COUNT + GUIManager.OFFSET * (ELEMENTS_COUNT - 1);

            panel.X = Game1.ScreenRectangle.Width - panel.Width;
            panel.Y = Game1.ScreenRectangle.Height - panel.Height;

            AddElement("Wheat");
            AddElement("Barley");
        }

        public void Update(GameTime gameTime)
        {
            if(Active)
            {
                if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
                {
                    if (panel.Intersects(InputManager.GetX(), InputManager.GetY()))
                    {
                        GUIManager.MouseOnUI = true;

                        for (int i = 0; i < checkboxes.Count; i++)
                        {
                            checkboxes[i].GetCheckboxDown();
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

                for (int i = 0; i < names.Count; i++)
                {
                    names[i].Draw(spriteBatch);
                    checkboxes[i].Draw(spriteBatch);
                }
            }
        }

      
        public void Open(int num)
        {
            farmNum = num;
            Active = true;
        }

        public void Close()
        {
            Active = false;
        }

        private void AddElement(string name)
        {
            TextUI text = new TextUI(ResourceBank.Fonts["mountFont"], name);
            CheckboxUI checkbox = new CheckboxUI();


            checkbox.X = panel.InnerX + TEXT_WIDTH;
            checkbox.Y = panel.InnerY + (checkboxes.Count * GUIManager.CHECKBOX_SIZE) + (checkboxes.Count * GUIManager.OFFSET);

            checkbox.OnCheckboxDownCallback(delegate
            {
                UnmarkAllCheckboxes();

                Tile[,] tiles = GamePlayState.FarmList.Get(farmNum);
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    for(int j = 0; j < tiles.GetLength(1); j++)
                    {
                        GamePlayState.JobList.Add(new Job(tiles[i, j], JobType.PLANT));
                    } 
                }
            });

            text.X = panel.InnerX;
            text.Y = checkbox.Y;

            names.Add(text);
            checkboxes.Add(checkbox);
        }

        private void UnmarkAllCheckboxes()
        {
            for (int i = 0; i < checkboxes.Count; i++)
                checkboxes[i].Marked = false;
        }

    }
}