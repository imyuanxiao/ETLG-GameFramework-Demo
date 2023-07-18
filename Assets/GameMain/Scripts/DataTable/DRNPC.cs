﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-07-18 16:26:30.036
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
    /// NPC配置表。
    /// </summary>
    public class DRNPC : DataRowBase
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
        /// 获取NPC名字。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取头像。
        /// </summary>
        public string Avatar
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取领域。
        /// </summary>
        public string Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取教授类型。
        /// </summary>
        public string Type
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
        /// 获取初始金钱。
        /// </summary>
        public int Money
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取初始道具(道具ID,数量)。
        /// </summary>
        public int[] Artifacts
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取任务ID。
        /// </summary>
        public int[] Quests
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对话文本路径。
        /// </summary>
        public string XMLDialogSource
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取考试文本路径。
        /// </summary>
        public string XMLQuizSource
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
            Avatar = columnStrings[index++];
            Domain = columnStrings[index++];
            Type = columnStrings[index++];
            Description = columnStrings[index++];
            Money = int.Parse(columnStrings[index++]);
                Artifacts = DataTableExtension.ParseInt32Array(columnStrings[index++]);
                Quests = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            XMLDialogSource = columnStrings[index++];
            XMLQuizSource = columnStrings[index++];

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
                    Avatar = binaryReader.ReadString();
                    Domain = binaryReader.ReadString();
                    Type = binaryReader.ReadString();
                    Description = binaryReader.ReadString();
                    Money = binaryReader.Read7BitEncodedInt32();
                        Artifacts = binaryReader.ReadInt32Array();
                        Quests = binaryReader.ReadInt32Array();
                    XMLDialogSource = binaryReader.ReadString();
                    XMLQuizSource = binaryReader.ReadString();
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
