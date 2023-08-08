
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

        public string Domain
        {
            get
            {
                return dRNPC.Domain;
            }
        }

        public int PlanetId
        {
            get
            {
                return dRNPC.PlanetId;
            }
        }

        public string Course
        {
            get
            {
                return dRNPC.Course;
            }
        }

        public int LandingPointId
        {
            get
            {
                return dRNPC.LandingPointId;
            }
        }

        public string Chapter
        {
            get
            {
                return dRNPC.Chapter;
            }
        }

        public string CourseDescription
        {
            get
            {
                return dRNPC.CourseDescription;
            }
        }

        public string ChapterDescription
        {
            get
            {
                return dRNPC.ChapterDescription;
            }
        }

        public int Type
        {
            get
            {
                return dRNPC.Type;
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

        //public bool NoDialogXML
        //{
        //    get
        //    {
        //        return Constant.Type.NULL.Equals(dRNPC.DialogXML);
        //    }
        //}

        public int DialogXML
        {
            get
            {
                return dRNPC.DialogXML;
                //return AssetUtility.GetDialogXML(dRNPC.DialogXML);
            }
        }

        //public bool NoQuizXML
        //{
        //    get
        //    {
        //        return Constant.Type.NULL.Equals(dRNPC.QuizXML);
        //    }
        //}

        public int QuizXML
        {
            get
            {
                return dRNPC.QuizXML;
                //return AssetUtility.GetQuizXML(dRNPC.QuizXML);
            }
        }

        // 构造方法
        public NPCData(DRNPC dRNPC)
        {
            this.dRNPC = dRNPC;
        }

    }

}
