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

    public class ActionPanelUI : UI
    {
        private List<ButtonUI> buttons;

        private Rectangle dest;

        private ButtonUI selectedButton;

        public ActionPanelUI()
        {
            buttons = new List<ButtonUI>();

            dest = new Rectangle();

            AddButton(ResourceBank.Sprites["harvest_icon"], JobType.HARVEST);
            AddButton(ResourceBank.Sprites["chop_icon"], JobType.CHOP);
            AddButton(ResourceBank.Sprites["mine_icon"], JobType.MINE);
            AddButton(ResourceBank.Sprites["haul_icon"], JobType.HAUL);
            AddButton(ResourceBank.Sprites["stoсkpile_icon"], JobType.STOCKPILE);
            AddButton(ResourceBank.Sprites["fish_icon"], JobType.FISH);
            AddButton(ResourceBank.Sprites["build_icon"], JobType.BUILD);
            AddButton(ResourceBank.Sprites["plant_icon"], JobType.PLANT);

            UpdatePositions();
        }

        public override void Update(GameTime gameTime)
        {
            // По нажатию правой кнопки все выбранные действия отменяются
            if(InputManager.GetMouseButtonDown(MouseInput.RightButton))
            {
                UnselectLastButton();
            }

            // Кнопки на главной панели
            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                if (dest.Contains(new Point(InputManager.GetX(), InputManager.GetY())))
                {
                    GUIManager.MouseOnUI = true;

                    for (int i = 0; i < buttons.Count; i++)
                        if (buttons[i].GetButtonDown())
                            break;
                }
            }
        }

        private void AddButton(Texture2D texture, JobType jobType)
        {
            ButtonUI button = new ButtonUI(ResourceBank.Sprites["button"], texture);
            button.OnButtonDownCallback(delegate
            {
                UnselectLastButton();

                GamePlayState.WorldManager.SetJobType(jobType);

                selectedButton = button;
                button.Selected = true;
            });
            buttons.Add(button);
        }

        public void UnselectLastButton()
        {
            if (selectedButton != null)
            {
                selectedButton.Selected = false;
                selectedButton = null;

                GamePlayState.WorldManager.SetJobType(JobType.NONE);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if(buttons[i].Visible)
                    buttons[i].Draw(spriteBatch);
            }
        }

        public override bool Intersects(int x, int y)
        {
            throw new NotImplementedException();
        }

        private void UpdatePositions()
        {
            int width = (buttons.Count * GUIManager.BUTTON_SIZE + (buttons.Count - 1) * GUIManager.OFFSET);
            int height = GUIManager.BUTTON_SIZE;

            int xStart = (Game1.ScreenRectangle.Width / 2) - width / 2;
            int yStart = Game1.ScreenRectangle.Height - (height + GUIManager.OFFSET);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].X = xStart + i * GUIManager.BUTTON_SIZE + i * GUIManager.OFFSET;
                buttons[i].Y = yStart;
                buttons[i].Width = GUIManager.BUTTON_SIZE;
                buttons[i].Height = GUIManager.BUTTON_SIZE;
            }

            dest.X = xStart;
            dest.Y = yStart;
            dest.Width = width;
            dest.Height = height;
        }
    }
}
