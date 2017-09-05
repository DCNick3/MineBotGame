using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace SimpleBotDrawer
{
    public class MouseGameState
    {
        Vector2 clickPos = new Vector2(0, 0);
        public bool wasPressedMouse = false;
        public float increase;
        public Vector2 screenCenterPosition;
        public Vector2 firstPosition = new Vector2(50,50);
        const float moveK = 0.05f;
        const float increaseK = 1.1f;
        float wheelRotate = 0;
        float lastWheelRotate;
        public MouseGameState(MouseState mouse)
        {
            lastWheelRotate = mouse.ScrollWheelValue;
        }
        public void Update(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (wasPressedMouse)
                {
                    screenCenterPosition = new Vector2((firstPosition.X + moveK * (clickPos.X - mouse.X)), (firstPosition.Y + moveK * (clickPos.Y - mouse.Y)));
                    if (screenCenterPosition.X > Game1.poleWidth)
                    {
                        screenCenterPosition.X = Game1.poleWidth;
                    }
                    if (screenCenterPosition.X < 0)
                    {
                        screenCenterPosition.X = 0;
                    }
                    if (screenCenterPosition.Y > Game1.poleHeight)
                    {
                        screenCenterPosition.Y = Game1.poleHeight;
                    }
                    if (screenCenterPosition.Y < 0)
                    {
                        screenCenterPosition.Y = 0;
                    }
                }
                else
                {
                    wasPressedMouse = true;
                    clickPos = new Vector2(mouse.X, mouse.Y);
                }
            }
            else
            {
                if (wasPressedMouse)
                {
                    firstPosition -= (moveK * (new Vector2(mouse.X, mouse.Y) - clickPos));
                    wasPressedMouse = false;
                }
            }
            float wheelRotate2 = wheelRotate + mouse.ScrollWheelValue - lastWheelRotate;
            lastWheelRotate = mouse.ScrollWheelValue;
            int size = (int)(Tile.startTileSize * Math.Pow(increaseK, wheelRotate2/120));
            if (size < Tile.maxTileSize && size > Tile.minTileSize)
            {
                Tile.tileSize = size;
                wheelRotate = wheelRotate2;
            }
        }
    }
}
