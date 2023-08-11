//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-08-11 06:05:17.765
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
    /// Enemy DataTable。
    /// </summary>
    public class DRBossEnemy : DataRowBase
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
        /// 获取敌人名字Id。
        /// </summary>
        public string NameId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取敌人类型。
        /// </summary>
        public string Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取耐久。
        /// </summary>
        public float Durability
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取护盾。
        /// </summary>
        public float Shields
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取火力。
        /// </summary>
        public float Firepower
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击频率。
        /// </summary>
        public float FireRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取敏捷。
        /// </summary>
        public float Agility
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取速度。
        /// </summary>
        public float Speed
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
            Type = columnStrings[index++];
            EntityId = int.Parse(columnStrings[index++]);
            Durability = float.Parse(columnStrings[index++]);
            Shields = float.Parse(columnStrings[index++]);
            Firepower = float.Parse(columnStrings[index++]);
            FireRate = float.Parse(columnStrings[index++]);
            Agility = float.Parse(columnStrings[index++]);
            Speed = float.Parse(columnStrings[index++]);

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
                    Type = binaryReader.ReadString();
                    EntityId = binaryReader.Read7BitEncodedInt32();
                    Durability = binaryReader.ReadSingle();
                    Shields = binaryReader.ReadSingle();
                    Firepower = binaryReader.ReadSingle();
                    FireRate = binaryReader.ReadSingle();
                    Agility = binaryReader.ReadSingle();
                    Speed = binaryReader.ReadSingle();
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
