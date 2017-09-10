using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects
{
    public class NewUnit : BuildingOperation
    {
        static readonly int needDone = 2;
        static readonly int type = 1;
        static readonly int startEnergyCostumation = 3;
        private Vector2 spawnPosition;
        public NewUnit(Building building, Vector2 spawnPosition) : base(needDone,type,new ResourceTotality(new int[] { 5, 0, 0, 0 }), startEnergyCostumation, building)
        {
            this.spawnPosition = spawnPosition;
            StartOperation();
        }
        public override bool CanBeDone()
        {
            if (base.CanBeDone())
            {
                if (building.GetNearFreeSpace().Contains(spawnPosition))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public override void FinalizeOperation()
        {
            building.OwnerPlayer.AddObject(Unit.NewUnit(spawnPosition,building.OwnerPlayer,GameObject.createId()));
        }
    }
}
