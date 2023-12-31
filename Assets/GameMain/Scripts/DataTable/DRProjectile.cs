﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-09-11 14:27:43.839
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
    /// 发射物属性配置表。
    /// </summary>
    public class DRProjectile : DataRowBase
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
        /// 获取发射物实体编号。
        /// </summary>
        public int EntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取发射物类型。
        /// </summary>
        public string ProjectileType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取移动速度。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取伤害。
        /// </summary>
        public float Damage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取溅射伤害。
        /// </summary>
        public float SplashDamage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取溅射范围。
        /// </summary>
        public float SplashRange
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
            EntityId = int.Parse(columnStrings[index++]);
            ProjectileType = columnStrings[index++];
            Speed = float.Parse(columnStrings[index++]);
            Damage = float.Parse(columnStrings[index++]);
            SplashDamage = float.Parse(columnStrings[index++]);
            SplashRange = float.Parse(columnStrings[index++]);

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
                    EntityId = binaryReader.Read7BitEncodedInt32();
                    ProjectileType = binaryReader.ReadString();
                    Speed = binaryReader.ReadSingle();
                    Damage = binaryReader.ReadSingle();
                    SplashDamage = binaryReader.ReadSingle();
                    SplashRange = binaryReader.ReadSingle();
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
