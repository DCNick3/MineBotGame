using System.IO;

namespace MineBotGame.GameObjects
{
    public class BuildingOperation
    {
        private BuildingOperation()
        {
            Done = 0;
            ResourceConsumation = new ResourceTotality();
        }
        
        public int Done { get; set; }
        public int NeedDone { get; private set; }
        public BuildingOperationType Type { get; private set; }
        public int ParA { get; private set; }
        public int ParB { get; private set; }

        public ResourceTotality ResourceConsumation { get; private set; }
        public int EnergyConsumation { get; private set; }

        public static BuildingOperation NewUnit()
        {
            return new BuildingOperation() { Type = BuildingOperationType.NewUnit, NeedDone = 2 };
        }

        public static BuildingOperation NewUpgrade(int unitId, UnitUpgrade up)
        {
            return new BuildingOperation() { Type = BuildingOperationType.NewUpgrade, NeedDone = 2, ParA = unitId, ParB = (int)up };
        }

        public static BuildingOperation NewModule(int unitId, UnitModule mod)
        {
            return new BuildingOperation() { Type = BuildingOperationType.NewModule, NeedDone = 2, ParA = unitId, ParB = (int)mod };
        }

        public static BuildingOperation NewGlobalResearch(GlobalResearch res)
        {
            return new BuildingOperation() { Type = BuildingOperationType.DoGlobalResearch, NeedDone = 2, ParA = (int)res };
        }

        public static BuildingOperation NewLocalResearch(LocalResearch res)
        {
            return new BuildingOperation() { Type = BuildingOperationType.DoLocalResearch, NeedDone = 2, ParA = (int)res,
                EnergyConsumation = GetEnergyL(res), ResourceConsumation = GetResL(res) };
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
