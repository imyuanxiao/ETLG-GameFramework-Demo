using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;
using UnityEngine.Video;
namespace ETLG.Data
{
    public class DataVideo : DataBase
    {
        public VideoPlayer videoPlayerBase;
        public VideoClip clip;
        public RenderTexture renderTexture;
        public double playbackTime;
        public bool isFullScreen;

    }
}