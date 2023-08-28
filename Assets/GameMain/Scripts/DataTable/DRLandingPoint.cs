//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-08-28 02:30:10.796
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
    /// 登录点配置表。
    /// </summary>
    public class DRLandingPoint : DataRowBase
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
        /// 获取课程名称。
        /// </summary>
        public string Course
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取课程描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取偏移量。
        /// </summary>
        public int Offset
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取NPCsID。
        /// </summary>
        public int[] NPCsID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取NPCsString。
        /// </summary>
        public string NPCsString
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取VlookupLandingPoint编号。
        /// </summary>
        public int VlookupLandingPointId
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
            PlanetId = int.Parse(columnStrings[index++]);
            Domain = columnStrings[index++];
            Course = columnStrings[index++];
            Description = columnStrings[index++];
            Offset = int.Parse(columnStrings[index++]);
                NPCsID = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            NPCsString = columnStrings[index++];
            VlookupLandingPointId = int.Parse(columnStrings[index++]);

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
                    PlanetId = binaryReader.Read7BitEncodedInt32();
                    Domain = binaryReader.ReadString();
                    Course = binaryReader.ReadString();
                    Description = binaryReader.ReadString();
                    Offset = binaryReader.Read7BitEncodedInt32();
                        NPCsID = binaryReader.ReadInt32Array();
                    NPCsString = binaryReader.ReadString();
                    VlookupLandingPointId = binaryReader.Read7BitEncodedInt32();
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
