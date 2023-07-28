//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-07-28 03:38:45.701
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
    /// 技能配置表。
    /// </summary>
    public class DRSkill : DataRowBase
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
        /// 获取枚举名字。
        /// </summary>
        public string NameId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能面板坐标（层数，位置）。
        /// </summary>
        public int[] Location
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取分类(0-Common, 1-Cloud,2-AI,3-Cyber,4-Data,5-Blockchain,6-IOT。
        /// </summary>
        public int Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否主动技能。
        /// </summary>
        public bool IsActiveSkill
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否战斗技能。
        /// </summary>
        public bool IsCombatSkill
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取激活状态（0-未解锁，1-已解锁，2-已升级）。
        /// </summary>
        public int ActiveState
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取等级列表。
        /// </summary>
        public int[] Levels
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取解锁点数。
        /// </summary>
        public int UnlockPoints
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取额外解锁条件。
        /// </summary>
        public bool NeedExtraCondition
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
            index++;
            NameId = columnStrings[index++];
                Location = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            Domain = int.Parse(columnStrings[index++]);
            IsActiveSkill = bool.Parse(columnStrings[index++]);
            IsCombatSkill = bool.Parse(columnStrings[index++]);
            ActiveState = int.Parse(columnStrings[index++]);
                Levels = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            UnlockPoints = int.Parse(columnStrings[index++]);
            NeedExtraCondition = bool.Parse(columnStrings[index++]);

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
                    NameId = binaryReader.ReadString();
                        Location = binaryReader.ReadInt32Array();
                    Domain = binaryReader.Read7BitEncodedInt32();
                    IsActiveSkill = binaryReader.ReadBoolean();
                    IsCombatSkill = binaryReader.ReadBoolean();
                    ActiveState = binaryReader.Read7BitEncodedInt32();
                        Levels = binaryReader.ReadInt32Array();
                    UnlockPoints = binaryReader.Read7BitEncodedInt32();
                    NeedExtraCondition = binaryReader.ReadBoolean();
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
