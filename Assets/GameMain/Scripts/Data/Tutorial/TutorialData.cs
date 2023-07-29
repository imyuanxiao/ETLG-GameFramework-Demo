
namespace ETLG.Data
{
    public sealed class TutorialData
    {
        private DRTutorial dRTutorial;
        
        public int Id
        {
            get
            {
                return dRTutorial.Id;
            }
        }


        public string Title
        {
            get
            {
                //return GameEntry.Localization.GetString(Constant.Key.PRE_TUTORIAL + Id + Constant.Key.POST_TITLE);
                return dRTutorial.Title;
            }
        }

        public int Group
        {
            get
            {
                return dRTutorial.Group;
            }
        }


        public TutorialData(DRTutorial dRTutorial)
        {
            this.dRTutorial = dRTutorial;
        }
    }

}
