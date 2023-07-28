//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-07-29 00:08:30.236
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
    /// 成就配置表。
    /// </summary>
    public class DRAchievement : DataRowBase
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
        /// 获取成就名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取成就类型Id。
        /// </summary>
        public int TypeId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取解锁条件Id。
        /// </summary>
        public int ConditionId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取解锁条件。
        /// </summary>
        public int[] Count
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取成就点数。
        /// </summary>
        public int[] Points
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
            Name = columnStrings[index++];
            TypeId = int.Parse(columnStrings[index++]);
            ConditionId = int.Parse(columnStrings[index++]);
                Count = DataTableExtension.ParseInt32Array(columnStrings[index++]);
                Points = DataTableExtension.ParseInt32Array(columnStrings[index++]);

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
                    TypeId = binaryReader.Read7BitEncodedInt32();
                    ConditionId = binaryReader.Read7BitEncodedInt32();
                        Count = binaryReader.ReadInt32Array();
                        Points = binaryReader.ReadInt32Array();
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
