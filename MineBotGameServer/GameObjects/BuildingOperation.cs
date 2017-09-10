using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MineBotGame.GameObjects
{
    /// <summary>
    /// Class, that represents Building operation (research, unit creation etc.)
    /// </summary>
    public class BuildingOperation
    {
        protected BuildingOperation(int needDone,int type, ResourceTotality resourceConsumation, int energyConsumation,Building building)
        {
            Done = 0;
            NeedDone = needDone;
            Type = (BuildingOperationType)type;
            ResourceConsumation = resourceConsumation;
            EnergyConsumation = energyConsumation;
            this.building = building;
        }
        public virtual bool CanBeDone()
        {
            if (building.OwnerPlayer.CheckResources(ResourceConsumation) && building.OwnerPlayer.CheckEnergy(EnergyConsumation))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual void FinalizeOperation()
        {

        }
        public virtual void StartOperation()
        {
            building.OwnerPlayer.UtilizeResources(ResourceConsumation);
            building.OwnerPlayer.UtilizeEnergy(EnergyConsumation);
        }
        protected Building building;
        public int Done { get; set; } 
        public int NeedDone { get; private set; }
        public BuildingOperationType Type { get; protected set; }
        public int ParA { get; private set; }
        public int ParB { get; private set; }

        public ResourceTotality ResourceConsumation { get; private set; }
        public int EnergyConsumation { get; private set; }

        public static BuildingOperation NewUnit(Building building,Vector2 spawnPos)
        {
            return new NewUnit(building,spawnPos);
        }

        public static BuildingOperation NewUpgrade(Building building,Unit unit, UnitUpgrade up)
        {
            return new NewUpgrade(building, unit, (int)up);
        }

        public static BuildingOperation NewModule(Building building, Unit unit, UnitModule mod)
        {
            return new NewModule(building, unit, mod);
        }

        public static BuildingOperation NewGlobalResearch(Building building, GlobalResearch res)
        {
            return new NewGlobalResearch(building,res);
        }

        public static BuildingOperation NewLocalResearch(Building building, LocalResearch res)
        {
            return new NewLocalResearch(building, res);
        }

        private static int GetEnergyL(LocalResearch res)
        {
            return 10;
        }

        private static ResourceTotality GetResL(LocalResearch res)
        {
            return new ResourceTotality().Add(new ResourceStack(ResourceType.Iron, 1));
        }


        public void Serialize(Stream str)
        {
            BinaryWriter bw = new BinaryWriter(str);

            bw.Write((int)Type);
            bw.Write(Done);
            bw.Write(NeedDone);
            bw.Write(ParA);
            bw.Write(ParB);
        }
    }
}
