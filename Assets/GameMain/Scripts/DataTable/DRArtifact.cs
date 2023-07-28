//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-07-28 04:01:57.733
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
    /// 道具配置表（贴图直接根据ID获取）。
    /// </summary>
    public class DRArtifact : DataRowBase
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
        /// 获取道具枚举ID。
        /// </summary>
        public string NameID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取道具类型(1-Trade,2-Special, 3-Module)。
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否可交易。
        /// </summary>
        public bool Tradeable
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取交易价值。
        /// </summary>
        public int Value
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取道具上限。
        /// </summary>
        public int MaxNumber
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
            NameID = columnStrings[index++];
            Type = int.Parse(columnStrings[index++]);
            Tradeable = bool.Parse(columnStrings[index++]);
            Value = int.Parse(columnStrings[index++]);
            MaxNumber = int.Parse(columnStrings[index++]);

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
                    NameID = binaryReader.ReadString();
                    Type = binaryReader.Read7BitEncodedInt32();
                    Tradeable = binaryReader.ReadBoolean();
                    Value = binaryReader.Read7BitEncodedInt32();
                    MaxNumber = binaryReader.Read7BitEncodedInt32();
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
