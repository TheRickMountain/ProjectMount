using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class PanelUI : UI
    {
        private Texture2D texture;

        private Rectangle topLeftSrc;
        private Rectangle leftSrc;
        private Rectangle bottomLeftSrc;
        private Rectangle topSrc;
        private Rectangle bottomSrc;
        private Rectangle topRightSrc;
        private Rectangle rightSrc;
        private Rectangle bottomRightSrc;
        private Rectangle centerSrc;

        private int borderSize = 8;

        public int X
        {
            get; set;
        }

        public int InnerX
        {
            get { return X + borderSize; }
        }

        public int Y
        {
            get; set;
        }

        public int InnerY
        {
            get { return Y + borderSize; }
        }

        public int Width
        {
            get { return InnerWidth + borderSize * 2; }
        }

        public int InnerWidth
        {
            get; set;
        }

        public int Height
        {
            get { return InnerHeight + borderSize * 2; }
        }

        public int InnerHeight
        {
            get; set;
        }

        public PanelUI()
        {
            Active = false;

            texture = ResourceBank.Sprites["panel"];

            int elementSize = texture.Width / 3;

            topLeftSrc = new Rectangle(0, 0, elementSize, elementSize);
            leftSrc = new Rectangle(0, elementSize, elementSize, elementSize);
            bottomLeftSrc = new Rectangle(0, elementSize * 2, elementSize, elementSize);

            topSrc = new Rectangle(elementSize, 0, elementSize, elementSize);

            topRightSrc = new Rectangle(elementSize * 2, 0, elementSize, elementSize);
            rightSrc = new Rectangle(elementSize * 2, elementSize, elementSize, elementSize);
            bottomRightSrc = new Rectangle(elementSize * 2, elementSize * 2, elementSize, elementSize);

            bottomSrc = new Rectangle(elementSize, elementSize * 2, elementSize, elementSize);

            centerSrc = new Rectangle(elementSize, elementSize, elementSize, elementSize);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(X, Y, borderSize, borderSize), topLeftSrc, Color.White);
            spriteBatch.Draw(texture, new Rectangle(X, Y + borderSize, borderSize, InnerHeight), leftSrc, Color.White);
            spriteBatch.Draw(texture, new Rectangle(X, Y + borderSize + InnerHeight, borderSize, borderSize), 
                bottomLeftSrc, Color.White);

            spriteBatch.Draw(texture, new Rectangle(X + borderSize, Y, InnerWidth, borderSize), topSrc, Color.White);

            spriteBatch.Draw(texture, new Rectangle(X + borderSize + InnerWidth, Y, borderSize, borderSize), topRightSrc, Color.White);
            spriteBatch.Draw(texture, new Rectangle(X + borderSize + InnerWidth, Y + borderSize, borderSize, InnerHeight), rightSrc, Color.White);
            spriteBatch.Draw(texture, new Rectangle(X + borderSize + InnerWidth, Y + borderSize + InnerHeight, borderSize, borderSize),
                bottomRightSrc, Color.White);

            spriteBatch.Draw(texture, new Rectangle(X + borderSize, Y + borderSize + InnerHeight, InnerWidth, borderSize), bottomSrc, Color.White);

            spriteBatch.Draw(texture, new Rectangle(X + borderSize, Y + borderSize, InnerWidth, InnerHeight), centerSrc, Color.White);
        }
        
        public override bool Intersects(int x, int y)
        {
            return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
        }
    }
}
