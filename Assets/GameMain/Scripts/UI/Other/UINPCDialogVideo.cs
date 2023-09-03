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
    public class UINPCDialogVideo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Button CoverButton;
        public Canvas PauseCover;
        public VideoPlayer videoPlayer;
        public Image PlayPauseIcon;
        public Button PlayButton;
        public Button FullScreenButton;

        private DataVideo dataVideo;

        private void Start()
        {
            PauseCover.gameObject.SetActive(false);
            CoverButton.onClick.AddListener(OnPlayButtonClick);
            PlayButton.onClick.AddListener(OnPlayButtonClick);
            FullScreenButton.onClick.AddListener(OnFullScreen);
            dataVideo = GameEntry.Data.GetData<DataVideo>();
            dataVideo.isFullScreen = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PauseCover.gameObject.SetActive(true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            PauseCover.gameObject.SetActive(false);
        }

        private void OnPlayButtonClick()
        {
            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Pause();
            }
        }

        private void OnFullScreen()
        {
            videoPlayer.Pause();
            dataVideo.renderTexture = videoPlayer.targetTexture;
            dataVideo.videoPlayerBase = videoPlayer;
            dataVideo.isFullScreen = true;
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIVideoFullScreenForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIVideoFullScreenForm));
            }
            GameEntry.UI.OpenUIForm(EnumUIForm.UIVideoFullScreenForm);
        }

    }
}