﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-09-11 14:27:43.834
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
    /// 飞船模组配置表。
    /// </summary>
    public class DRModule : DataRowBase
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
        /// 获取道具ID。
        /// </summary>
        public int ArtifactID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取模块功能(1-weapon,2-ATTACK,3-DEFENSE,4-POWERDRIVE,5-SUPPORT)。
        /// </summary>
        public int Classification
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取属性(0特殊,1-Durability;2-SHIELDS;3-AGILITY;4-ENERGY;5-FIREPOWER;;6-FIRERATE)。
        /// </summary>
        public int[] Attributes
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
            ArtifactID = int.Parse(columnStrings[index++]);
            Classification = int.Parse(columnStrings[index++]);
                Attributes = DataTableExtension.ParseInt32Array(columnStrings[index++]);

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
                    ArtifactID = binaryReader.Read7BitEncodedInt32();
                    Classification = binaryReader.Read7BitEncodedInt32();
                        Attributes = binaryReader.ReadInt32Array();
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
