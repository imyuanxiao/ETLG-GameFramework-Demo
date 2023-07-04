
namespace ETLG.Data
{
    // NPCId, Money, Artifacts
    public sealed class PlayerNPCData
    {
        
        public int NPCId { get; set; }

        public int Money { get; set; }

        public int[] Artifacts { get; set; }

        public PlayerNPCData(NPCData npcData)
        {
            this.NPCId = npcData.Id;
            this.Money = npcData.Money;
            Artifacts = (int[])npcData.Artifacts.Clone();
        }

    }

}
