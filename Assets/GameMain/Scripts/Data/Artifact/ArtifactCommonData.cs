
namespace ETLG.Data
{
    public sealed class ArtifactCommonData : ArtifactDataBase
    {

        public string Classification { get; set; }
        public string Attribute { get; set; }
        public int Effect { get; set; }

        public ArtifactCommonData(DRArtifact dRArtifact)
        {
            // Artifact 的基本属性
            Id = dRArtifact.Id;
            Name = GameEntry.Localization.GetString(Constant.Key.PRE_ARTIFACT + dRArtifact.NameID);
            Description = GameEntry.Localization.GetString(Constant.Key.PRE_ARTIFACT + dRArtifact.NameID + Constant.Key.POST_DESC);
            NameID = dRArtifact.NameID;
            Type = dRArtifact.Type;
            Tradeable = dRArtifact.Tradeable;
            Value = dRArtifact.Value;
            isTrade = false;

        }


    }

}
