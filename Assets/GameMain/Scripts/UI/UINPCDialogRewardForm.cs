using ETLG.Data;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UINPCDialogRewardForm : UGuiFormEx
    {
        public Button ExitButton;
        public Button GetButton;
        public Button CloseButton;
        public TextMeshProUGUI AwardHint;
        public Canvas AwardList;
        public VerticalLayoutGroup AwardListVerticalLayoutGroup;

        private DataNPC dataNPC;
        private DataDialog dataDialog;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            ExitButton.onClick.AddListener(OnExitButtonClick);
            GetButton.onClick.AddListener(OnGetButtonClick);
            CloseButton.onClick.AddListener(OnCloseButtonClick);
            dataNPC = GameEntry.Data.GetData<DataNPC>();
            dataDialog = GameEntry.Data.GetData<DataDialog>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            setButtons();
            ShowRewards();
            AwardHint.text = "Congratulation!\n" + "Here are your awards!";
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)AwardListVerticalLayoutGroup.transform);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void setButtons()
        {
            Text getButtonText = GetButton.GetComponentInChildren<Text>();
            getButtonText.text = "GET AWARDS";
            getButtonText.fontSize = 24;
            Text exitButtonText = ExitButton.GetComponentInChildren<Text>();
            exitButtonText.text = "EXIT";
        }

        private void ShowRewards()
        {
            Transform AwardListTransfrom = AwardList.GetComponentInChildren<Transform>();
            NPCData npcData = dataNPC.GetCurrentNPCData();

            if (npcData.RewardArtifacts.Length > 1)
            {
                int[] rewardArtifacts = npcData.RewardArtifacts;
                for (int i = 0; i < rewardArtifacts.Length; i += 2)
                {
                    int id = rewardArtifacts[i];
                    int num = rewardArtifacts[i + 1];
                    ShowItem<ItemRewardPreview>(EnumItem.ItemRewardPreview, (item) =>
                    {
                        item.transform.SetParent(AwardListTransfrom, false);
                        item.transform.localScale = Vector3.one;
                        item.transform.eulerAngles = Vector3.zero;
                        item.transform.localPosition = Vector3.zero;
                        item.GetComponent<ItemRewardPreview>().SetRewardData(id, num, Constant.Type.REWARD_TYPE_ARTIFACT);
                    });
                }
            }

            if (npcData.RewardSkill != 0)
            {
                int id = npcData.RewardSkill;
                ShowItem<ItemRewardPreview>(EnumItem.ItemRewardPreview, (item) =>
                {
                    item.transform.SetParent(AwardListTransfrom, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero;
                    item.GetComponent<ItemRewardPreview>().SetRewardData(id, 1, Constant.Type.REWARD_TYPE_SKILL);
                });
            }
        }

        private void OnCloseButtonClick()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCDialogRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCDialogRewardForm));
            }
        }

        private void OnExitButtonClick()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCDialogRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCDialogRewardForm));
            }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIErrorMessageForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIErrorMessageForm));
            }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCDialogForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCDialogForm));
            }
        }

        private void OnGetButtonClick()
        {
            if (GetButton.GetComponentInChildren<Text>().text == "GET AWARDS")
            {
                if (!dataDialog.award)
                {
                    dataDialog.clickGetButton = true;
                    AwardHint.text = "Collect awards successfully!";
                }
                else
                {
                    AwardHint.text = "You have collected my awards before!\n" + "You can NOT collect one more.";
                }
                GetButton.GetComponentInChildren<Text>().text = "GO BACK TO DIALOG";
                GetButton.GetComponentInChildren<Text>().fontSize = 14;
                dataDialog.report = true;
            }
            else
            {
                //learn again todo 
                dataDialog.reset();
                OnCloseButtonClick();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)AwardListVerticalLayoutGroup.transform);
        }
    }
}