
namespace ETLG.Data
{
    public sealed class QuestData
    {
        private DRQuest dRQuest;
        
        public int Id
        {
            get
            {
                return dRQuest.Id;
            }
        }

        public string Description
        {
            get
            {
                return dRQuest.Description;
            }
        }

        public int PreQuestID
        {
            get
            {
                return dRQuest.PreQuestID;
            }
        }
        public int ProQuestID
        {
            get
            {
                return dRQuest.ProQuestID;
            }
        }

        public int Money
        {
            get
            {
                return dRQuest.Money;
            }
        }


        // 构造方法
        public QuestData(DRQuest dRQuest)
        {
            this.dRQuest = dRQuest;
        }

    }

}
