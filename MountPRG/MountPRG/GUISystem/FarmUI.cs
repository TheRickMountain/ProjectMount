using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class FarmUI
    {

        private PanelUI panel;

        private Farm farm;

        private const int ELEMENTS_COUNT = 10;

        private List<ElementUI> elements = new List<ElementUI>();

        public bool Active
        {
            get; set;
        }

        public FarmUI()
        {
            panel = new PanelUI();

            panel.InnerWidth = GUIManager.CHECKBOX_SIZE + GUIManager.OFFSET + ElementUI.ICON_SIZE + GUIManager.OFFSET + ElementUI.TEXT_WIDTH;
            panel.InnerHeight = GUIManager.CHECKBOX_SIZE * ELEMENTS_COUNT + GUIManager.OFFSET * (ELEMENTS_COUNT - 1);

            panel.X = Game1.ScreenRectangle.Width - panel.Width;
            panel.Y = Game1.ScreenRectangle.Height - panel.Height;

            AddElement(GamePlayState.ItemDatabase[Item.WHEAT], Plant.WHEAT);
            AddElement(GamePlayState.ItemDatabase[Item.BARLEY], Plant.BARLEY);
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

                        for (int i = 0; i < elements.Count; i++)
                        {
                            elements[i].Checkbox.GetCheckboxDown();
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

                for (int i = 0; i < elements.Count; i++)
                {
                    elements[i].Draw(spriteBatch);
                }
            }
        }

      
        public void Open(Farm farm)
        {
            this.farm = farm;

            UnmarkAllCheckboxes();

            switch(farm.TargetPlant)
            {
                case Plant.WHEAT:
                    elements[0].Checkbox.Marked = true;
                    break;
                case Plant.BARLEY:
                    elements[1].Checkbox.Marked = true;
                    break;
            }

            Active = true;
        }

        public void Close()
        {
            Active = false;
        }

        private void AddElement(Item item, Plant targetPlant)
        {
            ElementUI element = new ElementUI(item.Icon, item.Name);
            element.X = panel.InnerX;
            element.Y = panel.InnerY + (elements.Count * GUIManager.CHECKBOX_SIZE + elements.Count * GUIManager.OFFSET);

            element.Checkbox.OnCheckboxDownCallback(delegate
            {
                UnmarkAllCheckboxes();

                farm.SetTargetPlant(targetPlant);
            });

            elements.Add(element);
        }

        private void UnmarkAllCheckboxes()
        {
            for (int i = 0; i < elements.Count; i++)
                elements[i].Checkbox.Marked = false;
        }

        class ElementUI
        {
            private Rectangle dest;

            public CheckboxUI Checkbox;
            private SpriteUI sprite;
            private TextUI text;

            public int X
            {
                get { return dest.X; }
                set
                {
                    if (dest.X != value)
                    {
                        dest.X = value;
                        Checkbox.X = dest.X;
                        sprite.X = Checkbox.X + Checkbox.Width + GUIManager.OFFSET;
                        text.X = sprite.X + sprite.Width + GUIManager.OFFSET;
                    }
                }
            }

            public int Y
            {
                get { return dest.Y; }
                set
                {
                    if (dest.Y != value)
                    {
                        dest.Y = value;
                        sprite.Y = dest.Y;
                        text.Y = dest.Y;
                        Checkbox.Y = dest.Y;
                    }
                }
            }

            public const int TEXT_WIDTH = 200;
            public const int ICON_SIZE = 16;

            public ElementUI(MyTexture icon, string name)
            {
                Checkbox = new CheckboxUI();
                sprite = new SpriteUI(icon.Texture, icon.ClipRect, ICON_SIZE, ICON_SIZE);
                text = new TextUI(ResourceBank.Fonts["mountFont"], name);

                dest = new Rectangle(0, 0, GUIManager.CHECKBOX_SIZE +  GUIManager.OFFSET + ICON_SIZE + GUIManager.OFFSET + TEXT_WIDTH, ICON_SIZE);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Checkbox.Draw(spriteBatch);
                sprite.Draw(spriteBatch);
                text.Draw(spriteBatch);  
            }
        }
    }
}