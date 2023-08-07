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

        //public TextMeshProUGUI Description = null;
        public RectTransform PlanetsContainer;
        public Button playerMenuButton;
        public Text Domian;
        public Text Course;
        public Text ChapterHead;
        public Text Chapter;
        public Text CourseDescription;
        public Text ChapterDescription;
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
        public VerticalLayoutGroup CourseInfoVerticalLayoutGroup;

        private bool refreshPlanetsContainer;
        private DataLearningPath dataLearningPath;
        private LearningPath recommendLearningPath;
        private DataLearningProgress dataLearningProgress;

        public Button expandAllButton;
        public RawImage expandButtonIcon;

        public TMP_InputField tmpInputField;
        private bool refresh;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            playerMenuButton.onClick.AddListener(OnPlayerMenuButtonClick);
            GOButton.onClick.AddListener(OnGoButtonClick);
            RecommendedChapterInfoButton.onClick.AddListener(OnGoButtonClick);
            DefaultHeadButton.onClick.AddListener(OnDefaultHeadButtonClick);
            dataLearningPath = GameEntry.Data.GetData<DataLearningPath>();
            dataLearningProgress = GameEntry.Data.GetData<DataLearningProgress>();

            expandAllButton.onClick.AddListener(OnExpandAllButtonClick);
            tmpInputField.onEndEdit.AddListener(OnInputEndEdit);
        }
        private void OnPlayerMenuButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.PlayerMenu")));
        }



        private void OnExpandAllButtonClick()
        {
            GameEntry.Data.GetData<DataPlanet>().expandAll = !GameEntry.Data.GetData<DataPlanet>().expandAll;

            if (GameEntry.Data.GetData<DataPlanet>().expandAll)
            {
                SetArrowIcon(Constant.Type.ARROW_DOWN);
            }
            else
            {
                SetArrowIcon(Constant.Type.ARROW_RIGHT);
            }

            this.refresh = true;
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            this.refresh = true;

            //Description.text = GameEntry.Localization.GetString("Map_Welcome_Desc");

            GameEntry.Event.Subscribe(PlanetExpandedEventArgs.EventId, OnPlanetExpanded);

            if (GameEntry.UI.HasUIForm(EnumUIForm.UITipForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UITipForm));
            }


            RecommendedChapterInfoScroll.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)DefaultLearningVerticalLayoutGroup.transform);

            GameEntry.Data.GetData<DataTutorial>().OpenGroupTutorials(Constant.Type.TUTORIAL_MAP);
            dataLearningProgress.update = true;
            dataLearningProgress.open = true;
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
                ChapterHead.text = dataLearningPath.getCurrentPath().getPathNPCData().Chapter;
                Domian.text = dataLearningPath.getCurrentPath().getPathNPCData().Domain;
                Course.text = dataLearningPath.getCurrentPath().getPathNPCData().Course;
                Chapter.text = dataLearningPath.getCurrentPath().getPathNPCData().Chapter;
                if (!string.IsNullOrEmpty(dataLearningPath.getCurrentPath().getPathNPCData().CourseDescription))
                {
                    CourseDescription.gameObject.SetActive(true);
                    CourseDescription.text = dataLearningPath.getCurrentPath().getPathNPCData().CourseDescription;
                }
                else
                {
                    CourseDescription.gameObject.SetActive(false);
                }
                if (!string.IsNullOrEmpty(dataLearningPath.getCurrentPath().getPathNPCData().ChapterDescription))
                {
                    ChapterDescription.gameObject.SetActive(true);
                    ChapterDescription.text = dataLearningPath.getCurrentPath().getPathNPCData().ChapterDescription;
                }
                else
                {
                    ChapterDescription.gameObject.SetActive(false);
                }

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
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)CourseInfoVerticalLayoutGroup.transform);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);


            if (refresh)
            {
                HideAllItem();
                ShowPlanets();

               // LayoutRebuilder.ForceRebuildLayoutImmediate(UIContainer);

                refresh = false;
            }

            if (refreshPlanetsContainer)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(PlanetsContainer);
                refreshPlanetsContainer = false;
            }


           // LayoutRebuilder.ForceRebuildLayoutImmediate(PlanetsContainer);

      /*      if (refreshPlanetsContainer)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(PlanetsContainer);
                refreshPlanetsContainer = false;
            }*/
            
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
            dataLearningProgress.open = false;
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(PlanetExpandedEventArgs.EventId, OnPlanetExpanded);
        }

        private void ShowPlanets()
        {
            HideAllItem();
            int[] PlanetIDs = GameEntry.Data.GetData<DataPlanet>().GetAllPlanetIDs();

            foreach (var PlanetID in PlanetIDs)
            {

                if (!PlanetHasRelatedCourses(PlanetID))
                {
                    continue;
                }

                ShowItem<ItemPlanetSelect>(EnumItem.ItemPlanetSelect, (item) =>
                {
                    item.transform.SetParent(PlanetsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.GetComponent<ItemPlanetSelect>().SetData(PlanetID);
                });
            }
        }


        private bool PlanetHasRelatedCourses(int PlanetID)
        {
            DataPlanet planet = GameEntry.Data.GetData<DataPlanet>();
            string keyword = planet.keyword;
            if (keyword == null || keyword.Equals(""))
            {
                return true;
            }

            int[] courseId = planet.GetPlanetData(PlanetID).LandingPoints;
            foreach (var courseID in courseId)
            {
                string title = GameEntry.Data.GetData<DataLandingPoint>().GetLandingPointData(courseID).Title;
                if (title.Contains(keyword)) { return true; }
            }

            return false;
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

        public void OnInputEndEdit(string inputText)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                GameEntry.Data.GetData<DataPlanet>().keyword = inputText;
                GameEntry.Data.GetData<DataPlanet>().expandAll = true;
                refresh = true;
            }
        }

        private void SetArrowIcon(int Type)
        {
            string texturePath = AssetUtility.GetArrowImg(Type);
            Texture texture = Resources.Load<Texture>(texturePath);
            expandButtonIcon.texture = texture;
        }

        public void OnPointerEnter()
        {
            MapManager.Instance.isOverUI = true;
        }

        public void OnPointerExit()
        {
            MapManager.Instance.isOverUI = false;
        }
    }
}
