using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Input;

namespace MountPRG
{
    public enum MouseInput
    {
        None,
        LeftButton,
        MiddleButton,
        RightButton,
        Button1,
        Button2
    }

    public class InputManager : GameComponent
    {
        private static KeyboardState keyboardState;
        private static KeyboardState lastKeyboardState;

        private static MouseState mouseState;
        private static MouseState lastMouseState;

        private static GamePadState[] gamePadStates;
        private static GamePadState[] lastGamePadStates;

        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        public static KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }
        
        public static MouseState MouseState
        {
            get { return mouseState; }
        }

        public static MouseState LastMouseState
        {
            get { return LastMouseState; }
        }

        public static GamePadState[] GamePadStates
        {
            get { return gamePadStates; }
        }

        public static GamePadState[] LastGamePadStates
        {
            get { return lastGamePadStates; }
        }

        public InputManager(Game game) 
            : base(game)
        {
            keyboardState = Keyboard.GetState();

            mouseState = Mouse.GetState();

            gamePadStates = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                gamePadStates[(int)index] = GamePad.GetState(index);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            lastGamePadStates = (GamePadState[])gamePadStates.Clone();
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                gamePadStates[(int)index] = GamePad.GetState(index);

            base.Update(gameTime);
        }

        public static void Flush()
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
        }

        public static bool GetKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        public static bool GetKeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }

        public static bool GetKey(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static int Scroll
        {
            get
            {
                if (mouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
                    return -1;
                else if (mouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)
                    return 1;

                return 0;
            }
        }

        public static int GetX()
        {
            int tmp = mouseState.X;
            if (tmp >= Game1.ScreenRectangle.Width)
                return Game1.ScreenRectangle.Width;
            else if (tmp <= 0)
                return 0;

            return tmp;
        }

        public static int GetY()
        {
            int tmp = mouseState.Y;
            if (tmp >= Game1.ScreenRectangle.Height)
                return Game1.ScreenRectangle.Height;
            else if (tmp <= 0)
                return 0;

            return tmp;
        }

        public static bool GetMouseButtonDown(MouseInput input)
        {
            switch(input)
            {
                case MouseInput.LeftButton:
                    return mouseState.LeftButton == ButtonState.Pressed && 
                        lastMouseState.LeftButton == ButtonState.Released;
                case MouseInput.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Pressed && 
                        lastMouseState.MiddleButton == ButtonState.Released;
                case MouseInput.RightButton:
                    return mouseState.RightButton == ButtonState.Pressed &&
                        lastMouseState.RightButton == ButtonState.Released;
                case MouseInput.Button1:
                    return mouseState.XButton1 == ButtonState.Pressed &&
                        lastMouseState.XButton1 == ButtonState.Released;
                case MouseInput.Button2:
                    return mouseState.XButton2 == ButtonState.Pressed &&
                        lastMouseState.XButton2 == ButtonState.Released;
            }

            return false;
        }

        public static bool GetMouseButtonReleased(MouseInput input)
        {
            switch (input)
            {
                case MouseInput.LeftButton:
                    return mouseState.LeftButton == ButtonState.Released &&
                        lastMouseState.LeftButton == ButtonState.Pressed;
                case MouseInput.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Released &&
                        lastMouseState.MiddleButton == ButtonState.Pressed;
                case MouseInput.RightButton:
                    return mouseState.RightButton == ButtonState.Released &&
                        lastMouseState.RightButton == ButtonState.Pressed;
                case MouseInput.Button1:
                    return mouseState.XButton1 == ButtonState.Released &&
                        lastMouseState.XButton1 == ButtonState.Pressed;
                case MouseInput.Button2:
                    return mouseState.XButton2 == ButtonState.Released &&
                        lastMouseState.XButton2 == ButtonState.Pressed;
            }

            return false;
        }

        public static bool GetMouseButton(MouseInput input)
        {
            switch (input)
            {
                case MouseInput.LeftButton:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseInput.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                case MouseInput.RightButton:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseInput.Button1:
                    return mouseState.XButton1 == ButtonState.Pressed;
                case MouseInput.Button2:
                    return mouseState.XButton2 == ButtonState.Pressed;
            }

            return false;
        }

        public static bool ButtonReleased(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int)index].IsButtonUp(button) &&
                lastGamePadStates[(int)index].IsButtonDown(button);
        }

        public static bool ButtonPressed(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int)index].IsButtonDown(button) &&
                lastGamePadStates[(int)index].IsButtonUp(button);
        }

        public static bool ButtonDown(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int)index].IsButtonDown(button);
        }

    }
}
