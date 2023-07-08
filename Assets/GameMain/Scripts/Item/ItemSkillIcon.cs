using ETLG.Data;
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

        //private PlayerSkillData playerSkillData;
        private int currentSkillID;

        public RawImage skillIcon;
        public RawImage bgColorImage;

        public Button iconButton;

        public int Type { get; private set; }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            iconButton.onClick.AddListener(OnIconButtonClick);

            //EventTrigger trigger = iconButton.gameObject.AddComponent<EventTrigger>();

        }

        private void OnIconButtonClick()
        {
            Log.Debug("click {0} ", Type);


            if (Type == Constant.Type.SKILL_ICON_SELECT_SPACESHIP)
            {
                return;
            }

            GameEntry.Event.Fire(this, SkillUpgradeInfoUIChangeEventArgs.Create(Constant.Type.UI_OPEN));


        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 获得挂载对象的位置
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 offset = new Vector3(100f, 0f, 0f);
            Vector3 newPosition = itemPosition + offset;

            if (Type == Constant.Type.SKILL_ICON_SELECT_SPACESHIP)
            {
                newPosition = new Vector3(660f, 190f, 0f);
            }

            if (Type == Constant.Type.SKILL_SKILL_TREE_MAP)
            {
                newPosition = new Vector3(50f, 120f, 0f);
            }


            //dataSkill.currentPlayerSkillData = this.playerSkillData;

            dataSkill.currentSkillID = this.currentSkillID;

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
            //this.playerSkillData = playerSkillData;

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

            // change icon style according to playerSkillData.ActivateState

            if(Type == Constant.Type.SKILL_ICON_SELECT_SPACESHIP)
            {
                ColorUtility.TryParseHtmlString("#4fa6b0", out bgColor);
            }
            else
            {
                PlayerSkillData playerSkillData = dataPlayer.GetPlayerData().getSkillById(currentSkillID);
                if (Constant.Type.SKILL_LOCKED == playerSkillData.ActiveState)
                {
                    iconColor = Color.black;
                }
                else if (Constant.Type.SKILL_UPGRADED == playerSkillData.ActiveState)
                {
                    ColorUtility.TryParseHtmlString("#4fa6b0", out bgColor);
                }
            }
            skillIcon.color = iconColor;
            bgColorImage.color = bgColor;

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            //inSelectScene = false;

            base.OnHide(isShutdown, userData);

        }

    }
}


