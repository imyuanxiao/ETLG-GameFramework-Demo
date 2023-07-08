
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
            Name = dRArtifact.NameID;
            Type = dRArtifact.Type;
            Tradeable = dRArtifact.Tradeable;
            Value = dRArtifact.Value;
            MaxNumber = dRArtifact.MaxNumber;

        }


    }

}
