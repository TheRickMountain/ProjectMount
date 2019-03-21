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

    public enum BuildingType
    {
        STRAW_HUT,
        TOOLS_WORKBENCH,
    }

    public class ActionPanelUI : UI
    {
        private List<ButtonUI> buttons;

        private List<ButtonUI> subButtons;

        private Rectangle dest;

        private Rectangle subDest;

        public JobType CurrentJobType
        {
            get; set;
        }

        public BuildingType CurrentBuildingType
        {
            get; private set;
        }

        public Entity CurrentBuilding
        {
            get; set;
        }

        public ActionPanelUI()
        {
            Texture2D buttonBackground = ResourceBank.Sprites["button"];

            buttons = new List<ButtonUI>();
            subButtons = new List<ButtonUI>();

            dest = new Rectangle();
            subDest = new Rectangle();

            CurrentJobType = JobType.NONE;
            CurrentBuildingType = BuildingType.STRAW_HUT;

            buttons.Add(new ButtonUI(buttonBackground, ResourceBank.Sprites["harvest_icon"]));
            buttons.Add(new ButtonUI(buttonBackground, ResourceBank.Sprites["chop_icon"]));
            buttons.Add(new ButtonUI(buttonBackground, ResourceBank.Sprites["mine_icon"]));
            buttons.Add(new ButtonUI(buttonBackground, ResourceBank.Sprites["haul_icon"]));
            buttons.Add(new ButtonUI(buttonBackground, ResourceBank.Sprites["stoсkpile_icon"]));
            buttons.Add(new ButtonUI(buttonBackground, ResourceBank.Sprites["fish_icon"]));
            buttons.Add(new ButtonUI(buttonBackground, ResourceBank.Sprites["build_icon"]));
            

            int xStart = (Game1.ScreenRectangle.Width / 2) - (buttons.Count * GUIManager.BUTTON_SIZE + buttons.Count * GUIManager.OFFSET) / 2;
            int yStart = Game1.ScreenRectangle.Height - (GUIManager.BUTTON_SIZE + GUIManager.OFFSET);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].X = xStart + i * GUIManager.BUTTON_SIZE + i * GUIManager.OFFSET;
                buttons[i].Y = yStart;
                buttons[i].Width = GUIManager.BUTTON_SIZE;
                buttons[i].Height = GUIManager.BUTTON_SIZE;
            }

            dest = new Rectangle(xStart, yStart,
                buttons.Count * GUIManager.BUTTON_SIZE + buttons.Count * GUIManager.OFFSET,
                (GUIManager.BUTTON_SIZE + GUIManager.OFFSET));

            ConnectButtonWithJobAction(buttons[0], JobType.HARVEST);
            ConnectButtonWithJobAction(buttons[1], JobType.CHOP);
            ConnectButtonWithJobAction(buttons[2], JobType.MINE);
            ConnectButtonWithJobAction(buttons[3], JobType.HAUL);
            ConnectButtonWithJobAction(buttons[4], JobType.STOCKPILE);
            ConnectButtonWithJobAction(buttons[5], JobType.FISH);

            buttons[6].OnButtonDownCallback(delegate (ButtonUI button)
            {
                button.Selected = true;

                subButtons.Add(new ButtonUI(buttonBackground, new TextUI(ResourceBank.Fonts["mountFont"], "Straw Hut")));

                subButtons.Add(new ButtonUI(buttonBackground, new TextUI(ResourceBank.Fonts["mountFont"], "Tools Workbench")));

                int startX = (int)button.X;
                int startY = (int)button.Y - subButtons.Count * subButtons[0].Height - subButtons.Count * GUIManager.OFFSET;

                int maxWidth = 0;

                for (int i = 0; i < subButtons.Count; i++)
                {
                    subButtons[i].X = startX;
                    subButtons[i].Y = startY + subButtons[i].Height * i + GUIManager.OFFSET * i;

                    if (subButtons[i].Width > maxWidth)
                        maxWidth = subButtons[i].Width;
                }

                subDest = new Rectangle(startX, startY, maxWidth, subButtons.Count * subButtons[0].Height + (subButtons.Count - 1) * GUIManager.OFFSET);
            });
        }

        public override void Update(GameTime gameTime)
        {
            // По нажатию правой кнопки все выбранные действия отменяются
            if(InputManager.GetMouseButtonDown(MouseInput.RightButton))
            {
                ResetAllAction();
            }

            if (InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                // Всплывающие текстовые кнопки
                if (subButtons.Count > 0)
                {
                    if (InputManager.GetX() >= subDest.X && InputManager.GetX() <= subDest.Right &&
                    InputManager.GetY() >= subDest.Y && InputManager.GetY() <= subDest.Bottom)
                    {
                        GUIManager.MouseOnUI = true;

                        for (int i = 0; i < subButtons.Count; i++)
                        {
                            ButtonUI button = subButtons[i];
                            if (button.Intersects(InputManager.GetX(), InputManager.GetY()))
                            {
                                CurrentJobType = JobType.BUILDING;
                                CurrentBuildingType = (BuildingType)i;
                                switch(CurrentBuildingType)
                                {
                                    case BuildingType.STRAW_HUT:
                                        CurrentBuilding = new StrawHut();
                                        GamePlayState.Entities.Add(CurrentBuilding);
                                        break;
                                    case BuildingType.TOOLS_WORKBENCH:
                                        CurrentBuilding = new ToolsWorkbench();
                                        GamePlayState.Entities.Add(CurrentBuilding);
                                        break;
                                }
                                break;
                            }
                        }
                    }

                    subButtons.Clear();
                    buttons[5].Selected = false;
                }

                // Кнопки на главной панели
                if (InputManager.GetX() >= dest.X && InputManager.GetX() <= dest.Right &&
                InputManager.GetY() >= dest.Y && InputManager.GetY() <= dest.Bottom)
                {
                    GUIManager.MouseOnUI = true;

                    for (int i = 0; i < buttons.Count; i++)
                    {
                        ButtonUI button = buttons[i];
                        if (button.Intersects(InputManager.GetX(), InputManager.GetY()))
                        {
                            UnselectAllButtons();
                            button.ButtonDown();
                            break;
                        }
                    }
                }
            }
        }

            
        private void ResetAllAction()
        {
            UnselectAllButtons();
            CurrentJobType = JobType.NONE;
            if (CurrentBuilding != null)
            {
                GamePlayState.Entities.Remove(CurrentBuilding);
            }

            subButtons.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if(buttons[i].Visible)
                    buttons[i].Draw(spriteBatch);
            }


            for (int i = 0; i < subButtons.Count; i++)
            {
                if (subButtons[i].Visible)
                    subButtons[i].Draw(spriteBatch);
            }
        }

        // При нажатии на кнопку она выделяется, про повторном нажатии выделение отменяется
        private void ConnectButtonWithJobAction(ButtonUI button, JobType jobAction)
        {
            button.OnButtonDownCallback(delegate
            {
                if (CurrentJobType == jobAction)
                {
                    CurrentJobType = JobType.NONE;
                    button.Selected = false;
                }
                else
                {
                    CurrentJobType = jobAction;
                    button.Selected = true;
                }
            });
        }

        private void UnselectAllButtons()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Selected = false;
            }
        }

        public override bool Intersects(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
