using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public enum AnimationKey { Down, Left, Right, Up, Sleep }

    public class Animation : ICloneable
    {
        Rectangle[] frames;
        int framesPerSecond;
        TimeSpan frameLength;
        TimeSpan frameTimer;

        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set
            {
                framesPerSecond = MathHelper.Clamp(value, 1, 60);
                frameLength = TimeSpan.FromSeconds(1 / (double)framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRect
        {
            get { return frames[CurrentFrame]; }
        }

        public int CurrentFrame
        {
            get; private set;
        }

        public int DefaultFrame
        {
            get; private set;
        }

        public int FrameWidth
        {
            get; private set;
        }

        public int FrameHeight
        {
            get; private set;
        }

        public Animation(int frameCount, int defaultFrame, int frameWidth, int frameHeight, int xOffset, int yOffset, int speed = 5)
        {
            frames = new Rectangle[frameCount];
            DefaultFrame = defaultFrame;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                    xOffset + (frameWidth * i),
                    yOffset,
                    frameWidth, frameHeight);
            }
            FramesPerSecond = speed;
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
                CurrentFrame = (CurrentFrame + 1) % frames.Length;
            }
        }

        public void Reset()
        {
            CurrentFrame = DefaultFrame;
            frameTimer = TimeSpan.Zero;
        }

        public object Clone()
        {
            Animation animationClone = new Animation(this);

            animationClone.DefaultFrame = DefaultFrame;
            animationClone.FrameWidth = FrameWidth;
            animationClone.FrameHeight = FrameHeight;
            animationClone.Reset();

            return animationClone;
        }

    }
}
