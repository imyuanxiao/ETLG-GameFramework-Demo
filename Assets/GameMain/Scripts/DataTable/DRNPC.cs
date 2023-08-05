//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-08-05 03:06:21.667
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETLG
{
    /// <summary>
    /// NPC配置表。
    /// </summary>
    public class DRNPC : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取配置编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取NPC名字。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取PlanetId。
        /// </summary>
        public int PlanetId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取领域名称。
        /// </summary>
        public string Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取LandingPointId。
        /// </summary>
        public int LandingPointId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取课程名称。
        /// </summary>
        public string Course
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取章节名称。
        /// </summary>
        public string Chapter
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取NPC类型(1-teacher,2-examiner)。
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取初始金钱。
        /// </summary>
        public int Money
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取初始道具(道具ID,数量)。
        /// </summary>
        public int[] Artifacts
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取道具奖励（ID+数量）。
        /// </summary>
        public int[] RewardArtifacts
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能奖励（ID）。
        /// </summary>
        public int RewardSkill
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对话文本id。
        /// </summary>
        public int DialogXML
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取题库文本id。
        /// </summary>
        public int QuizXML
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取VlookupNPCId。
        /// </summary>
        public int VlookupNPCId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取VlookupLandingPointId。
        /// </summary>
        public int VlookupLandingPointId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取VlookupPlanetId。
        /// </summary>
        public int VlookupPlanetId
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            Name = columnStrings[index++];
            PlanetId = int.Parse(columnStrings[index++]);
            Domain = columnStrings[index++];
            LandingPointId = int.Parse(columnStrings[index++]);
            Course = columnStrings[index++];
            Chapter = columnStrings[index++];
            Type = int.Parse(columnStrings[index++]);
            Money = int.Parse(columnStrings[index++]);
                Artifacts = DataTableExtension.ParseInt32Array(columnStrings[index++]);
                RewardArtifacts = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            RewardSkill = int.Parse(columnStrings[index++]);
            DialogXML = int.Parse(columnStrings[index++]);
            QuizXML = int.Parse(columnStrings[index++]);
            VlookupNPCId = int.Parse(columnStrings[index++]);
            VlookupLandingPointId = int.Parse(columnStrings[index++]);
            VlookupPlanetId = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Name = binaryReader.ReadString();
                    PlanetId = binaryReader.Read7BitEncodedInt32();
                    Domain = binaryReader.ReadString();
                    LandingPointId = binaryReader.Read7BitEncodedInt32();
                    Course = binaryReader.ReadString();
                    Chapter = binaryReader.ReadString();
                    Type = binaryReader.Read7BitEncodedInt32();
                    Money = binaryReader.Read7BitEncodedInt32();
                        Artifacts = binaryReader.ReadInt32Array();
                        RewardArtifacts = binaryReader.ReadInt32Array();
                    RewardSkill = binaryReader.Read7BitEncodedInt32();
                    DialogXML = binaryReader.Read7BitEncodedInt32();
                    QuizXML = binaryReader.Read7BitEncodedInt32();
                    VlookupNPCId = binaryReader.Read7BitEncodedInt32();
                    VlookupLandingPointId = binaryReader.Read7BitEncodedInt32();
                    VlookupPlanetId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
