//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-08-05 21:24:58.420
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
    /// 星球配置表。
    /// </summary>
    public class DRPlanet : DataRowBase
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
        /// 获取名字。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取类型。
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取坐标(x,y轴）。
        /// </summary>
        public int[] Coordinates
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源ID（模型路径）。
        /// </summary>
        public int AssetID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取登录点ID。
        /// </summary>
        public int[] LandingPoints
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取登录点IDString。
        /// </summary>
        public string LandingPointsString
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取VlookUP编号。
        /// </summary>
        public int VlookupId
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
            Description = columnStrings[index++];
            Type = int.Parse(columnStrings[index++]);
                Coordinates = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            AssetID = int.Parse(columnStrings[index++]);
                LandingPoints = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            LandingPointsString = columnStrings[index++];
            VlookupId = int.Parse(columnStrings[index++]);

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
                    Description = binaryReader.ReadString();
                    Type = binaryReader.Read7BitEncodedInt32();
                        Coordinates = binaryReader.ReadInt32Array();
                    AssetID = binaryReader.Read7BitEncodedInt32();
                        LandingPoints = binaryReader.ReadInt32Array();
                    LandingPointsString = binaryReader.ReadString();
                    VlookupId = binaryReader.Read7BitEncodedInt32();
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
