//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-08-15 21:55:42.544
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
    /// 飞船属性配置表，配置编号需要和Entity的保持一直。
    /// </summary>
    public class DRSpaceship : DataRowBase
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
        /// 获取飞船名字ID。
        /// </summary>
        public string NameId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取飞船脚本。
        /// </summary>
        public string Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取飞船类型。
        /// </summary>
        public string SType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取飞船大小。
        /// </summary>
        public string SSize
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
        /// 获取敏捷。
        /// </summary>
        public float Agility
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取能量。
        /// </summary>
        public float Energy
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
        /// 获取初始技能。
        /// </summary>
        public int[] Skills
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
            SType = columnStrings[index++];
            SSize = columnStrings[index++];
            EntityId = int.Parse(columnStrings[index++]);
            Durability = float.Parse(columnStrings[index++]);
            Shields = float.Parse(columnStrings[index++]);
            Agility = float.Parse(columnStrings[index++]);
            Energy = float.Parse(columnStrings[index++]);
            Firepower = float.Parse(columnStrings[index++]);
            FireRate = float.Parse(columnStrings[index++]);
                Skills = DataTableExtension.ParseInt32Array(columnStrings[index++]);

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
                    SType = binaryReader.ReadString();
                    SSize = binaryReader.ReadString();
                    EntityId = binaryReader.Read7BitEncodedInt32();
                    Durability = binaryReader.ReadSingle();
                    Shields = binaryReader.ReadSingle();
                    Agility = binaryReader.ReadSingle();
                    Energy = binaryReader.ReadSingle();
                    Firepower = binaryReader.ReadSingle();
                    FireRate = binaryReader.ReadSingle();
                        Skills = binaryReader.ReadInt32Array();
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
