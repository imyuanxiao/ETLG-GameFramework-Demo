using ETLG.Data;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemSkillIcon : ItemLogicEx, IPointerEnterHandler, IPointerExitHandler
    {

        private DataSkill dataSkill;
        private DataPlayer dataPlayer;

        private int currentSkillID;

        public RawImage skillIcon;
        public RawImage bgColorImage;


        public Button iconButton;

        public TextMeshProUGUI Level;

        public bool refresh;
        public int Type { get; private set; }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            iconButton.onClick.AddListener(OnIconButtonClick);

        }

        private void setLevel()
        {
            Level.text = "1";

            if (!dataPlayer.inNewGame)
            {
                int curLevel = dataPlayer.GetPlayerData().GetSkillLevelById(currentSkillID);
                if (curLevel == Constant.Type.SKILL_LOCKED)
                {
                    curLevel = 0;
                }
                Level.text = curLevel.ToString();
            }
        }


        private void OnIconButtonClick()
        {

            if (Type == Constant.Type.SKILL_ICON_SELECT_SPACESHIP)
            {
                return;
            }
            dataSkill.lockCurrentSkillID = true;
            GameEntry.Event.Fire(this, SkillUpgradeInfoUIChangeEventArgs.Create(Constant.Type.UI_OPEN));


        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 获得挂载对象的位置
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 offset = new Vector3(-180f, -120f, 0f);
            Vector3 newPosition = itemPosition + offset;

            dataSkill.SetUpgradeInfoPosition(newPosition);

            if (Type == Constant.Type.SKILL_ICON_SELECT_SPACESHIP)
            {
                newPosition = new Vector3(660f, 190f, 0f);
            }

            if (Type == Constant.Type.SKILL_SKILL_TREE_MAP)
            {
                newPosition = new Vector3(50f, 120f, 0f);
            }

            dataSkill.SetCurrentSkillID(this.currentSkillID);

            dataSkill.skillInfoPosition = newPosition;

            dataSkill.hideSkillInfoBottomPart = Type == Constant.Type.SKILL_ICON_SELECT_SPACESHIP ? true : false;


            GameEntry.Event.Fire(this, SkillInfoUIChangeEventArgs.Create(Constant.Type.UI_OPEN));

        }

        public void OnPointerExit(PointerEventData eventData)
        {
           
            GameEntry.Event.Fire(this, SkillInfoUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetSkillData(int SkillId, int Type)
        {
            this.Type = Type;

            this.currentSkillID = SkillId;

            string texturePath = AssetUtility.GetSkillIcon(SkillId.ToString());
            Texture texture = Resources.Load<Texture>(texturePath);
            if (texture == null)
            {
                texturePath = AssetUtility.GetIconMissing();
                texture = Resources.Load<Texture>(texturePath);
            }
            skillIcon.texture = texture;

            Color iconColor = Color.white;
            Color bgColor = Color.white;
            ColorUtility.TryParseHtmlString("#57595b", out bgColor);

            if(Type == Constant.Type.SKILL_ICON_SELECT_SPACESHIP)
            {
                ColorUtility.TryParseHtmlString("#4fa6b0", out bgColor);
            }
            else
            {
               int skillLevel = dataPlayer.GetPlayerData().GetSkillLevelById(currentSkillID);

                if (skillLevel.Equals(Constant.Type.SKILL_LOCKED))
                {
                    iconColor = Color.black;
                }
                else if (skillLevel >= 1)
                {
                    ColorUtility.TryParseHtmlString("#4fa6b0", out bgColor);
                }
            }
            skillIcon.color = iconColor;
            bgColorImage.color = bgColor;

            setLevel();

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

        }

    }
}


