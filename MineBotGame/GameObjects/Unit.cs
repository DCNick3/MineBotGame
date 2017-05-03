using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MineBotGame.GameObjects
{
    public class Unit : GameObject
    {
        private Unit(Vector2 pos)
        {
            _pos = pos;
            unitModules = new bool[Enum.GetValues(typeof(UnitModules)).Length];
            unitUpgrades = new int[Enum.GetValues(typeof(UnitUpgrade)).Length];
        }

        bool[] unitModules;
        int[] unitUpgrades;

        private int _hp, _def, _id;
        private Vector2 _pos;

        public override int HP
        {
            get
            {
                return _hp;
            }
        }

        public override int Defence
        {
            get
            {
                return _def;
            }
        }

        public override int Id
        {
            get
            {
                return _id;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return _pos;
            }
        }

        public void SetModule(UnitModules module, bool value)
        {
            unitModules[(int)module] = value;
        }

        public bool GetModule(UnitModules module)
        {
            return unitModules[(int)module];
        }

        public void SetUpgrade(UnitUpgrade upgrade, int value)
        {
            unitUpgrades[(int)upgrade] = value;
        }

        public int GetUpgrade(UnitUpgrade upgrade)
        {
            return unitUpgrades[(int)upgrade];
        }
    }
}
