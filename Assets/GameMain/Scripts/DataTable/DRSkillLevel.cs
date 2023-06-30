//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-06-30 17:47:33.448
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
    /// 技能等级配置表。
    /// </summary>
    public class DRSkillLevel : DataRowBase
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
        /// 获取使用消耗能源。
        /// </summary>
        public float Energy
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取升级消耗金币。
        /// </summary>
        public int Coins
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取升级消耗资源1。
        /// </summary>
        public int Resource1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取升级消耗资源2。
        /// </summary>
        public int Resource2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取升级消耗资源3。
        /// </summary>
        public int Resource3
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
            Energy = float.Parse(columnStrings[index++]);
            Coins = int.Parse(columnStrings[index++]);
            Resource1 = int.Parse(columnStrings[index++]);
            Resource2 = int.Parse(columnStrings[index++]);
            Resource3 = int.Parse(columnStrings[index++]);

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
                    Energy = binaryReader.ReadSingle();
                    Coins = binaryReader.Read7BitEncodedInt32();
                    Resource1 = binaryReader.Read7BitEncodedInt32();
                    Resource2 = binaryReader.Read7BitEncodedInt32();
                    Resource3 = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, int>[] m_Resource = null;

        public int ResourceCount
        {
            get
            {
                return m_Resource.Length;
            }
        }

        public int GetResource(int id)
        {
            foreach (KeyValuePair<int, int> i in m_Resource)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetResource with invalid id '{0}'.", id.ToString()));
        }

        public int GetResourceAt(int index)
        {
            if (index < 0 || index >= m_Resource.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetResourceAt with invalid index '{0}'.", index.ToString()));
            }

            return m_Resource[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_Resource = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, Resource1),
                new KeyValuePair<int, int>(2, Resource2),
                new KeyValuePair<int, int>(3, Resource3),
            };
        }
    }
}
