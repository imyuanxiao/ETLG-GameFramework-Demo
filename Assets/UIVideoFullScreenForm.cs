using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using UnityEngine.Video;
using UnityEngine.EventSystems;

namespace ETLG
{
    public class UIVideoFullScreenForm : UGuiFormEx
    {
        public Button CoverButton;
        public Canvas TopCover;
        public Canvas ButtomCover;
        public VideoPlayer videoPlayer;
        public Image PlayPauseIcon;
        public Button PlayButton;
        public Button FullScreenButton;
        public Button CloseButton;
        public RawImage rawImage;

        private bool isPause;
        private Sprite playIcon;
        private Sprite pauseIcon;
        private DataVideo dataVideo;
        private bool isCoverHover;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            TopCover.gameObject.SetActive(true);
            ButtomCover.gameObject.SetActive(true);
            CoverButton.onClick.AddListener(OnPlayButtonClick);
            PlayButton.onClick.AddListener(OnPlayButtonClick);
            FullScreenButton.onClick.AddListener(OnWindow);
            CloseButton.onClick.AddListener(OnWindow);
            dataVideo = GameEntry.Data.GetData<DataVideo>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //videoPlayer.clip = dataVideo.clip;
            //videoPlayer.targetTexture = dataVideo.renderTexture;
            videoPlayer = dataVideo.videoPlayerBase;
            rawImage.texture = dataVideo.renderTexture;
            //videoPlayer.time = dataVideo.playbackTime;
            videoPlayer.Play();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        //private IEnumerator HideCoversAfterDelay(float delay)
        //{
        //    yield return new WaitForSeconds(delay);
        //    if (!isCoverHover)
        //    {
        //        TopCover.gameObject.SetActive(false);
        //        ButtomCover.gameObject.SetActive(false);
        //    }
        //}

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    isCoverHover = true;
        //    TopCover.gameObject.SetActive(true);
        //    ButtomCover.gameObject.SetActive(true);

        //    StartCoroutine(HideCoversAfterDelay(3f));
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    TopCover.gameObject.SetActive(false);
        //    ButtomCover.gameObject.SetActive(false);
        //}

        private void OnPlayButtonClick()
        {
            if (isPause)
            {
                videoPlayer.Play();
                isPause = false;
            }
            else
            {
                videoPlayer.Pause();
                isPause = true;
            }
        }

        private void OnWindow()
        {
            dataVideo.playbackTime = videoPlayer.time;
            dataVideo.isFullScreen = false;
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIVideoFullScreenForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIVideoFullScreenForm));
            }
        }
        //private void HideTopCover()
        //{
        //    if (!isCoverHover)
        //    {
        //        TopCover.gameObject.SetActive(false);
        //        ButtomCover.gameObject.SetActive(false);
        //    }
        //}
    }
}