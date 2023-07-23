
namespace ETLG.Data
{
    public sealed class NPCData
    {
        private DRNPC dRNPC;
        
        public int Id
        {
            get
            {
                return dRNPC.Id;
            }
        }

        public string Name
        {
            get
            {
                return dRNPC.Name;
            }
        }

        public string Title
        {
            get
            {
                return GameEntry.Localization.GetString(Constant.Key.PRE_NPC + Id + Constant.Key.POST_TITLE);
            }
        }

        public string Description
        {
            get
            {
                // return dRPlanet.Description;
                return GameEntry.Localization.GetString(Constant.Key.PRE_NPC + Id + Constant.Key.POST_DESC);
            }
        }

        public string Type
        {
            get
            {
                switch (dRNPC.Type)
                {
                    case Constant.Type.NPC_TYPE_TEACHER:
                        return "Teacher";
                    case Constant.Type.NPC_TYPE_EXAMINER:
                        return "Teacher";
                    default:
                        return "Others";

                }

            }
        }

        public int Money
        {
            get
            {
                return dRNPC.Money;
            }
        }

        public int[] Artifacts
        {
            get
            {
                return dRNPC.Artifacts;
            }
        }

        public int[] RewardArtifacts
        {
            get
            {
                return dRNPC.RewardArtifacts;
            }
        }

        public int RewardSkill
        {
            get
            {
                return dRNPC.RewardSkill;
            }
        }

        public bool NoDialogXML
        {
            get
            {
                return Constant.Type.NULL.Equals(dRNPC.DialogXML);
            }
        }

        public string DialogXML
        {
            get
            {
                return AssetUtility.GetDialogXML(dRNPC.DialogXML);
            }
        }

        public bool NoQuizXML
        {
            get
            {
                return Constant.Type.NULL.Equals(dRNPC.QuizXML);
            }
        }

        public string QuizXML
        {
            get
            {
                return AssetUtility.GetQuizXML(dRNPC.QuizXML);
            }
        }

        // 构造方法
        public NPCData(DRNPC dRNPC)
        {
            this.dRNPC = dRNPC;
        }

    }

}
