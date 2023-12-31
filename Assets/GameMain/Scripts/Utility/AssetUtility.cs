﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace ETLG
{
    public static class AssetUtility
    {
        public static string GetConfigAsset(string assetName, bool fromBytes = false)
        {
            return Utility.Text.Format("Assets/GameMain/Configs/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
        }

        public static string GetDataTableAsset(string assetName, bool fromBytes = true)
        {
            return Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
        }

        public static string GetDictionaryAsset(string assetName, bool fromBytes = false)
        {
            return Utility.Text.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.{2}", GameEntry.Localization.Language.ToString(), assetName, fromBytes ? "bytes" : "json");
        }

        public static string GetFontAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Fonts/{0}.ttf", assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Scenes/{0}/{1}.unity", assetName, assetName);
        }

        public static string GetMusicAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Music/{0}.mp3", assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Sounds/{0}.wav", assetName);
        }

        public static string GetEntityAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Entities/{0}.prefab", assetName);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UIDforms/{0}.prefab", assetName);
        }

        public static string GetUISoundAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UISounds/{0}.wav", assetName);
        }

        public static string GetIconMissing()
        {
            return "IconMissing";
        }

        public static string GetSkillIcon(string skillId)
        {
            return Utility.Text.Format("Skills/{0}", skillId);
        }
        public static string GetSkillIcon(string skillId, string state)
        {
            string iconName = skillId + state;
            return Utility.Text.Format("Skills/{0}", iconName);
        }

        public static string GetArtifactIcon(string iconName)
        {

            return Utility.Text.Format("Artifacts/{0}", iconName);
        }
        public static string GetUnLockAchievementIcon(string iconName)
        {

            return Utility.Text.Format("Achievement/{0}", iconName);
        }
        public static string GetErrorIcon(string iconName)
        {

            return Utility.Text.Format("Others/{0}", iconName);
        }
        public static string GetAvatarMissing()
        {
            return "Avatar/1000";
        }

        public static string GetNPCAvatar(string NPCID)
        {
            return Utility.Text.Format("Avatar/NPC/{0}", NPCID);
        }

        public static string GetXMLImage(string NPCID, string imageID)
        {
            return Utility.Text.Format("XMLImage/{0}/{1}", NPCID, imageID);
        }

        public static string GetXMLVideo(string NPCID, string videoID)
        {
            return Utility.Text.Format("XMLVideo/{0}/{1}", NPCID, videoID);
        }

        public static string GetXMLVideoRender(string NPCID, string videoID)
        {
            return Utility.Text.Format("XMLVideo/{0}/{1}", NPCID, videoID);
        }

        public static string GetVideoPlayerIcon(bool pause)
        {
            if (!pause)
            {
                return Utility.Text.Format("Others/videoplay");
            }
            else
            {
                return Utility.Text.Format("Others/videopause");
            }

        }
        public static string GetPlayerAvatar()
        {
            return Utility.Text.Format("Avatar/Player/1000");
        }

        public static string GetDialogXML(string path)
        {
            return Utility.Text.Format("DialogXML/{0}", path);
        }

        public static string GetQuizXML(string path)
        {
            return Utility.Text.Format("QuizXML/{0}", path);
        }

        public static string GetTutorialImg(string id)
        {
            return Utility.Text.Format("Tutorial/{0}", id);
        }

        public static string GetLostTutorialImg()
        {
            return Utility.Text.Format("Tutorial/1000");
        }

        public static string GetArrowImg(int Type)
        {
            if (Type.Equals(Constant.Type.ARROW_DOWN))
            {
                return Utility.Text.Format("PlanetMenu/TreeExpand");
            }
            return Utility.Text.Format("PlanetMenu/TreeCollapse");

        }
        public static string GetPlayerAvatar(string avatarId)
        {
            if(int.Parse(avatarId)<1000)
            {
                return Utility.Text.Format("Avatar/Player/{0}", "1000");
            }
            return Utility.Text.Format("Avatar/Player/{0}", avatarId);
        }

    }
}
