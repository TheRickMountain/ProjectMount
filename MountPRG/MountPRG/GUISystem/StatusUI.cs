using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class StatusUI
    {
        private PanelUI panel;

        private SettlerControllerCmp settler;

        private SpriteUI avatarSprite;
        private TextUI nameText;

        private TextUI enduranceText;
        private SliderUI enduranceSlider;

        private TextUI satietyText;
        private SliderUI satietySlider;

        public bool Active
        {
            get; set;
        }

        public StatusUI()
        {
            panel = new PanelUI();
            panel.InnerWidth = 100 + 5 + 50;
            panel.InnerHeight = 150;

            panel.X = Game1.ScreenRectangle.Width - panel.Width;
            panel.Y = Game1.ScreenRectangle.Height - panel.Height;

            avatarSprite = new SpriteUI(ResourceBank.Sprites["avatar"], 32, 32);
            avatarSprite.X = panel.InnerX;
            avatarSprite.Y = panel.InnerY;

            nameText = new TextUI(ResourceBank.Fonts["mountFont"], "");
            nameText.X = avatarSprite.X + avatarSprite.Width + GUIManager.OFFSET;
            nameText.Y = panel.InnerY;

            enduranceText = new TextUI(ResourceBank.Fonts["mountFont"], "Endurance");
            enduranceText.X = panel.InnerX;
            enduranceText.Y = avatarSprite.Y + avatarSprite.Height + GUIManager.OFFSET;

            enduranceSlider = new SliderUI(50, 8, Color.DarkGray, Color.Green);
            enduranceSlider.X = panel.InnerX + panel.InnerWidth - enduranceSlider.Width;
            enduranceSlider.Y = enduranceText.Y;

            satietyText = new TextUI(ResourceBank.Fonts["mountFont"], "Satiety");
            satietyText.X = panel.InnerX;
            satietyText.Y = enduranceText.Y + enduranceText.Height + GUIManager.OFFSET;


            satietySlider = new SliderUI(50, 8, Color.DarkGray, Color.Orange);
            satietySlider.X = panel.InnerX + panel.InnerWidth - satietySlider.Width;
            satietySlider.Y = satietyText.Y;
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                enduranceSlider.SetValue(settler.Endurance, settler.MaxEndurance);
                satietySlider.SetValue(settler.Satiety, settler.MaxSatiety);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                panel.Draw(spriteBatch);

                avatarSprite.Draw(spriteBatch);
                nameText.Draw(spriteBatch);

                enduranceText.Draw(spriteBatch);
                enduranceSlider.Draw(spriteBatch);
                satietyText.Draw(spriteBatch);
                satietySlider.Draw(spriteBatch);
            }
        }

        public void Open(SettlerControllerCmp settler)
        {
            this.settler = settler;

            nameText.Text = settler.Name;
            enduranceSlider.SetValue(settler.Endurance, settler.MaxEndurance);
            satietySlider.SetValue(settler.Satiety, settler.MaxSatiety);

            Active = true;
        }

        public void Close()
        {
            Active = false;
        }

    }
}
