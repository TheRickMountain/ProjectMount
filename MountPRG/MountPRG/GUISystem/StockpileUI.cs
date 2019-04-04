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
    public class StockpileUI : UI
    {
        private List<Tile> tiles;

        private PanelUI panel;

        private List<ElementUI> elements = new List<ElementUI>();

        private const int ELEMENTS_COUNT = 10;

        public StockpileUI()
        {
            panel = new PanelUI();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: update content 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            panel.Draw(spriteBatch);

            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Draw(spriteBatch);
            }

        }

        public void Open(Stockpile stockpile)
        {
            Active = true;

            tiles = stockpile.GetTiles();

            panel.InnerWidth = ElementUI.ICON_SIZE + GUIManager.OFFSET + ElementUI.TEXT_WIDTH;
            panel.InnerHeight = ELEMENTS_COUNT * ElementUI.ICON_SIZE + (ELEMENTS_COUNT - 1) * GUIManager.OFFSET;

            panel.X = Game1.ScreenRectangle.Width - panel.Width;
            panel.Y = Game1.ScreenRectangle.Height - panel.Height;


            for(int i = 0, count = 0; i < tiles.Count; i++)
            {
                Tile tile = tiles[i];
                if(tile.Item != null)
                {  
                    ElementUI element = new ElementUI(tile.Item.Icon, tile.Item.Name, tile.ItemCount);
                    element.X = panel.InnerX;
                    element.Y = panel.InnerY + count * ElementUI.ICON_SIZE + count * GUIManager.OFFSET;
                    elements.Add(element);
                    count++;
                }
            }
        }

        public void Close()
        {
            Active = false;

            elements.Clear();
            tiles = null;
        }

        public override bool Intersects(int x, int y)
        {
            throw new NotImplementedException();
        }

        class ElementUI
        {
            private Rectangle dest;

            private SpriteUI sprite;
            private TextUI text;

            public int X
            {
                get { return dest.X; }
                set
                {
                    if(dest.X != value)
                    {
                        dest.X = value;
                        sprite.X = dest.X;
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
                    }
                }
            }

            public const int TEXT_WIDTH = 200;
            public const int ICON_SIZE = 24;

            public ElementUI(MyTexture icon, string name, int count)
            {
                sprite = new SpriteUI(icon.Texture, icon.ClipRect, ICON_SIZE, ICON_SIZE);
                text = new TextUI(ResourceBank.Fonts["mountFont"], name + " " + count);

                dest = new Rectangle(0, 0, ICON_SIZE + GUIManager.OFFSET + TEXT_WIDTH, ICON_SIZE);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                sprite.Draw(spriteBatch);
                text.Draw(spriteBatch);
            }
        }
    }
}
