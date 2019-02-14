using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public enum AnimationKey { Down, Left, Right, Up }

    public class Animation : ICloneable
    {
        Rectangle[] frames;
        int framesPerSecond;
        TimeSpan frameLength;
        TimeSpan frameTimer;
        int currentFrame;
        int defaultFrame;
        int frameWidth;
        int frameHeight;

        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set
            {
                framesPerSecond = (int)MathHelper.Clamp(value, 1, 60);
                frameLength = TimeSpan.FromSeconds(1 / (double)framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRect
        {
            get { return frames[currentFrame]; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
        }

        public int DefaultFrame
        {
            get { return defaultFrame; }
        }

        public int FrameWidth
        {
            get { return frameWidth; }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
        }

        public Animation(int frameCount, int defaultFrame, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            frames = new Rectangle[frameCount];
            this.defaultFrame = defaultFrame;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                    xOffset + (frameWidth * i),
                    yOffset,
                    frameWidth, frameHeight);
            }
            FramesPerSecond = 5;
            Reset();
        }

        private Animation(Animation animation)
        {
            this.frames = animation.frames;
            FramesPerSecond = 5;
        }

        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime;

            if (frameTimer >= frameLength)
            {
                frameTimer = TimeSpan.Zero;
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }

        public void Reset()
        {
            currentFrame = defaultFrame;
            frameTimer = TimeSpan.Zero;
        }

        public object Clone()
        {
            Animation animationClone = new Animation(this);

            animationClone.defaultFrame = this.defaultFrame;
            animationClone.frameWidth = this.frameWidth;
            animationClone.frameHeight = this.frameHeight;
            animationClone.Reset();

            return animationClone;
        }

    }
}
