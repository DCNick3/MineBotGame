using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SimpleBotDrawer
{
    public class GameObject
    {
        protected int hp;
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                if (value > maxHP)
                {
                    hp = maxHP;
                }
                else if (value < 0)
                {
                    hp = 0;
                }
                else
                {
                    hp = value;
                }
            }
        }
        public Vector2 CenterPos
        {
            get
            {
                if (this is Building)
                {
                    switch ((this as Building).type)
                    {
                        case 0:
                            return position + new Vector2(2, 2);
                        case 1:
                            return position + new Vector2(1.5f, 2);
                        case 2:
                            return position + new Vector2(2, 2);
                        case 3:
                            return position + new Vector2(1, 1);
                        case 4:
                            return position + new Vector2(1, 1);
                        case 5:
                            return position + new Vector2(1, 1);
                        default:
                            return position;
                    }
                }
                else
                {
                    return position;
                }
            }
        }
        public Color color;
        public int maxHP;
        public Vector2 position;
    }
}
