using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class UI : IGUI
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

        public UI() : base(true)
        {
            texture = ResourceBank.UITexture;

            int tmp = 4;

            topLeftSrc = new Rectangle(0, 0, tmp, tmp);
            leftSrc = new Rectangle(0, 4, tmp, tmp);
            bottomLeftSrc = new Rectangle(0, 8, tmp, tmp);

            topSrc = new Rectangle(4, 0, tmp, tmp);

            topRightSrc = new Rectangle(8, 0, tmp, tmp);
            rightSrc = new Rectangle(8, 4, tmp, tmp);
            bottomRightSrc = new Rectangle(8, 8, tmp, tmp);

            bottomSrc = new Rectangle(4, 8, tmp, tmp);

            centerSrc = new Rectangle(4, 4, tmp, tmp);
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
    }
}
