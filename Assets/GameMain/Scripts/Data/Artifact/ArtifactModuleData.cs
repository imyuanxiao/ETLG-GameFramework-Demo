﻿
namespace ETLG.Data
{
    public sealed class ArtifactModuleData : ArtifactDataBase
    {

        public int Classification { get; set; }
        public int[] Attributes { get; set; }

        public ArtifactModuleData(DRArtifact dRArtifact, DRModule dRModule)
        {
            // Artifact 的基本属性
            Id = dRArtifact.Id;
            Name = GameEntry.Localization.GetString(Constant.Key.PRE_ARTIFACT + dRArtifact.NameID);
            Description = GameEntry.Localization.GetString(Constant.Key.PRE_ARTIFACT + dRArtifact.NameID + Constant.Key.POST_DESC);
            NameID = dRArtifact.NameID;
            Type = dRArtifact.Type;
            Tradeable = dRArtifact.Tradeable;
            Value = dRArtifact.Value;
            MaxNumber = dRArtifact.MaxNumber;

            // Module 的属性
            Classification = dRModule.Classification;
            Attributes = dRModule.Attributes;
        }


    }

}
