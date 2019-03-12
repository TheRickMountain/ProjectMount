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
   
    public class ActionPanelGUI : IGUI
    {
        private List<Button> buttons;

        public JobType CurrentJobType
        {
            get; private set;
        }

        public bool MouseOnUI;

        public ActionPanelGUI(bool active) : base(active)
        {
            buttons = new List<Button>();

            CurrentJobType = JobType.NONE;

            buttons.Add(new Button("GATHER", ResourceBank.Sprites["gather_icon"], true));
            buttons.Add(new Button("CUT", ResourceBank.Sprites["cut_icon"], true));
            buttons.Add(new Button("MINE", ResourceBank.Sprites["mine_icon"], true));
            buttons.Add(new Button("BUILD", ResourceBank.Sprites["build_icon"], true));
            buttons.Add(new Button("STORAGE", ResourceBank.Sprites["storage_icon"], true));

            int xStart = Game1.ScreenRectangle.Width - (buttons.Count * GUIManager.BUTTON_SIZE + buttons.Count * GUIManager.OFFSET);
            int yStart = Game1.ScreenRectangle.Height - (GUIManager.BUTTON_SIZE + GUIManager.OFFSET);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].X = xStart + i * GUIManager.BUTTON_SIZE + i * GUIManager.OFFSET;
                buttons[i].Y = yStart;
                buttons[i].Widtth = GUIManager.BUTTON_SIZE;
                buttons[i].Height = GUIManager.BUTTON_SIZE;
            }

            ConnectButtonWithJobAction(buttons[0], JobType.GATHER);
            ConnectButtonWithJobAction(buttons[1], JobType.CUT);
            ConnectButtonWithJobAction(buttons[2], JobType.MINE);
            ConnectButtonWithJobAction(buttons[3], JobType.BUILD);
            ConnectButtonWithJobAction(buttons[4], JobType.STORAGE);
        }

        public override void Update(GameTime gameTime)
        {
            if(InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    Button button = buttons[i];
                    if(button.Intersects(InputManager.GetX(), InputManager.GetY()))
                    {
                        UnselectAllButtons();
                        button.ButtonDown();

                        MouseOnUI = true;
                    }
                }
            }  
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < buttons.Count; i++)
                buttons[i].Draw(spriteBatch);
        }

        // При нажатии на кнопку она выделяется, про повторном нажатии выделение отменяется
        private void ConnectButtonWithJobAction(Button button, JobType jobAction)
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
    }
}
