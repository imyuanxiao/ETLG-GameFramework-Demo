
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

        public string ClassificationName()
        {
            switch (this.Classification)
            {
                case Constant.Type.MODULE_TYPE_WEAPON:
                    return "Weapon";
                case Constant.Type.MODULE_TYPE_ATTACK:
                    return "Attack";
                case Constant.Type.MODULE_TYPE_DEFENSE:
                    return "Defense";
                case Constant.Type.MODULE_TYPE_POWERDRIVE:
                    return "Powerdrive";
                case Constant.Type.MODULE_TYPE_SUPPORT:
                    return "Support";
                default:
                    return "Others";
            }
        }

        public string GetEffect()
        {


            int[] attrs = Attributes;

            // no arrtibutes, sepcial desc in Localization
            if (attrs.Length <= 1)
            {
                return GameEntry.Localization.GetString(Constant.Key.PRE_MODULE_EFFECT + this.NameID);
            }

            string result = "Add: ";

            for (int i = 0; i < attrs.Length; i += 2)
            {
                result += GetIndexAttrEffect(i, attrs);
                result += i == attrs.Length - 2 ? "." : ", ";
            }

            return result;

        }

        public string GetIndexAttrEffect(int i, int[] attrs)
        {
            string result = "";
            switch (attrs[i])
            {
                case Constant.Type.ATTR_Durability:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Durability";
                case Constant.Type.ATTR_Shields:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Shileds";
                case Constant.Type.ATTR_Firepower:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Firepower";
                case Constant.Type.ATTR_Energy:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Energy";
                case Constant.Type.ATTR_Agility:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Agility";
                case Constant.Type.ATTR_Speed:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Speed";
                case Constant.Type.ATTR_Detection:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Detection";
                case Constant.Type.ATTR_Capacity:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Capacity";
                case Constant.Type.ATTR_Firerate:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Firerate";
                case Constant.Type.ATTR_Dogde:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Dogde";
                default:
                    return "Unknown effect";
            }
        }

    }

}
