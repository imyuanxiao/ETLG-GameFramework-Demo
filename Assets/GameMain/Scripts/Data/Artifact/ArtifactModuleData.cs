
namespace ETLG.Data
{
    public sealed class ArtifactModuleData : ArtifactDataBase
    {

        public string Classification { get; set; }
        public string Attribute { get; set; }
        public int Effect { get; set; }

        public ArtifactModuleData(DRArtifact dRArtifact, DRModule dRModule)
        {
            // Artifact 的基本属性
            Id = dRArtifact.Id;
            Name = dRArtifact.Name;
            Type = dRArtifact.Type;
            Tradeable = dRArtifact.Tradeable;
            Value = dRArtifact.Value;
            MaxNumber = dRArtifact.MaxNumber;

            // Module 的属性
            Classification = dRModule.Classification;
            Attribute = dRModule.Attribute;
            Effect = dRModule.Effect;

        }


    }

}
