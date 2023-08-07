using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using UnityEngine.Video;

namespace ETLG
{
    public class UINPCDialogVideo : UGuiFormEx
    {
        public Button BgButton;
        public Canvas PauseCover;
        public VideoPlayer videoPlayer;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            bool isPause = false;
            PauseCover.gameObject.SetActive(false);
            BgButton.onClick.AddListener(() =>
            {
                if (isPause)
                {
                    PauseCover.gameObject.SetActive(true);
                    videoPlayer.Play();
                    isPause = false;
                }
                else
                {
                    PauseCover.gameObject.SetActive(false);
                    videoPlayer.Pause();
                    isPause = true;
                }
            });
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}