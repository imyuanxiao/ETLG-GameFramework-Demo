using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using TMPro;
using ETLG.Data;
using Unity.VisualScripting;
using GameFramework.Event;

namespace ETLG
{
    public class UIMapPlayerInfoForm : UGuiFormEx
    {

        public TextMeshProUGUI Description = null;
        public RectTransform PlanetsContainer;
        public Button playerMenuButton;
        public Text Domian;
        public Text Course;
        public Text ChapterHead;
        public Text Chapter;
        public Button GOButton;
        public RawImage defaultTitle;
        public Canvas defaultLearningPath;
        public RectTransform RewardIconContainer;
        public RawImage CollapseIcon;
        public RawImage TalkIcon;
        public RawImage QuizIcon;
        public Button DefaultHeadButton;
        public Button RecommendedChapterInfoButton;
        public ScrollRect RecommendedChapterInfoScroll;
        public VerticalLayoutGroup DefaultLearningVerticalLayoutGroup;
        public VerticalLayoutGroup Container2LayoutGroup;
        public HorizontalLayoutGroup DefaultButtonsHorizontalLayoutGroup;

        private bool refreshPlanetsContainer;
        private DataLearningPath dataLearningPath;
        private LearningPath recommendLearningPath;
        private DataLearningProgress dataLearningProgress;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            playerMenuButton.onClick.AddListener(OnPlayerMenuButtonClick);
            GOButton.onClick.AddListener(OnGoButtonClick);
            RecommendedChapterInfoButton.onClick.AddListener(OnGoButtonClick);
            DefaultHeadButton.onClick.AddListener(OnDefaultHeadButtonClick);
            dataLearningPath = GameEntry.Data.GetData<DataLearningPath>();
            dataLearningProgress = GameEntry.Data.GetData<DataLearningProgress>();
        }
        private void OnPlayerMenuButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.PlayerMenu")));
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Description.text = GameEntry.Localization.GetString("Map_Welcome_Desc");

            GameEntry.Event.Subscribe(PlanetExpandedEventArgs.EventId, OnPlanetExpanded);

            if (GameEntry.UI.HasUIForm(EnumUIForm.UITipForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UITipForm));
            }
            RecommendedChapterInfoScroll.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)DefaultLearningVerticalLayoutGroup.transform);
            // tutorial in spaceship menu
            GameEntry.Data.GetData<DataTutorial>().OpenGroupTutorials(2);
            dataLearningProgress.update = true;
        }

        private void setDefaultLearningPath()
        {
            if (recommendLearningPath == null)
            {
                defaultTitle.gameObject.SetActive(false);
                defaultLearningPath.gameObject.SetActive(false);
            }
            else
            {
                ChapterHead.text = dataLearningPath.getCurrentPath().getCurrentChapter();
                Domian.text = dataLearningPath.getCurrentPath().getCurrentDomian();
                Course.text = dataLearningPath.getCurrentPath().getCurrentCourse();
                Chapter.text = dataLearningPath.getCurrentPath().getCurrentChapter();

                ShowItem<ItemRewardIcon>(EnumItem.ItemRewardIcon, (item) =>
                {
                    item.transform.SetParent(RewardIconContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero;
                    item.GetComponent<ItemRewardIcon>().SetData(dataLearningPath.getCurrentPath().NPCId);
                });
                if (dataLearningPath.getCurrentPath().getCurrentType() == "Teacher")
                {
                    TalkIcon.gameObject.SetActive(true);
                    QuizIcon.gameObject.SetActive(false);
                }
                else
                {
                    TalkIcon.gameObject.SetActive(false);
                    QuizIcon.gameObject.SetActive(true);
                }
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            LayoutRebuilder.ForceRebuildLayoutImmediate(PlanetsContainer);

            if (refreshPlanetsContainer)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(PlanetsContainer);
                refreshPlanetsContainer = false;
            }
            recommendLearningPath = dataLearningPath.getCurrentPath();
            if (dataLearningProgress.update)
            {
                setDefaultLearningPath();
                ShowPlanets();
                dataLearningProgress.update = false;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)DefaultLearningVerticalLayoutGroup.transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)Container2LayoutGroup.transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)DefaultButtonsHorizontalLayoutGroup.transform);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(PlanetExpandedEventArgs.EventId, OnPlanetExpanded);

        }

        private void ShowPlanets()
        {
            HideAllItem();
            int[] PlanetIDs = GameEntry.Data.GetData<DataPlanet>().GetAllPlanetIDs();

            foreach (var PlanetID in PlanetIDs)
            {
                ShowItem<ItemPlanetSelect>(EnumItem.ItemPlanetSelect, (item) =>
                {
                    item.transform.SetParent(PlanetsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.GetComponent<ItemPlanetSelect>().SetData(PlanetID);
                });
            }
        }

        private void OnPlanetExpanded(object sender, GameEventArgs e)
        {
            PlanetExpandedEventArgs ne = (PlanetExpandedEventArgs)e;
            if (ne == null)
                return;

            refreshPlanetsContainer = true;
        }

        private void OnGoButtonClick()
        {
            GameEntry.Data.GetData<DataNPC>().currentNPCId = dataLearningPath.getCurrentPath().NPCId;
            if (dataLearningPath.getCurrentPath().getCurrentType() == "Teacher")
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCDialogForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCDialogForm));
                }
                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCDialogForm);
            }
            else
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizForm));
                }
                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCQuizForm);
            }
        }

        private void OnDefaultHeadButtonClick()
        {
            bool isActive = RecommendedChapterInfoScroll.gameObject.activeSelf;
            setExpandIcon(isActive);
            if (isActive)
            {
                RecommendedChapterInfoScroll.gameObject.SetActive(false);
            }
            else
            {
                RecommendedChapterInfoScroll.gameObject.SetActive(true);
            }
        }

        private void setExpandIcon(bool active)
        {
            int Type;
            if (active)
            {
                Type = Constant.Type.ARROW_RIGHT;
            }
            else
            {
                Type = Constant.Type.ARROW_DOWN;
            }
            string texturePath = AssetUtility.GetArrowImg(Type);
            Texture texture = Resources.Load<Texture>(texturePath);
            CollapseIcon.texture = texture;
        }
    }
}
