
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

        public bool HasText
        {
            get
            {
                return dRTutorial.Text;
            }
        }

        public string Title
        {
            get
            {
                return GameEntry.Localization.GetString(Constant.Key.PRE_TUTORIAL + Id + Constant.Key.POST_TITLE);
            }
        }

        public string Description
        {
            get
            {
                return GameEntry.Localization.GetString(Constant.Key.PRE_TUTORIAL + Id + Constant.Key.POST_DESC);
            }
        }

        public TutorialData(DRTutorial dRTutorial)
        {
            this.dRTutorial = dRTutorial;
        }
    }

}
