using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameFramework.Localization;
using System;
using TMPro;
using GameFramework.Event;


namespace ETLG
{
    public class UILoadGameForm : UGuiFormEx
    {
        public GameObject createNewSaveButtonObj;
        public Button createNewSaveButton;
        public Button cancelButton;
        public SaveSlot[] saveSlots;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            cancelButton.onClick.AddListener(OnCancelButtonClick);
            createNewSaveButton.onClick.AddListener(OnCreateNewSave);

            foreach (SaveSlot saveSlot in this.saveSlots)
            {
                saveSlot.loadBtn.onClick.AddListener(delegate{OnLoadButtonClicked(saveSlot.SaveId);});
                saveSlot.overwriteBtn.onClick.AddListener(delegate{OnOverwriteClicked(saveSlot.SaveId);});
                saveSlot.deleteBtn.onClick.AddListener(delegate{OnDeleteButtonClicked(saveSlot.SaveId);});
            }
        }

        private void OnDeleteButtonClicked(int saveId)
        {
            SaveManager.Instance.DeleteSave(saveId);

            SavedGamesInfo savedData = SaveManager.Instance.LoadObject<SavedGamesInfo>("SavedGamesInfo");
            
            this.saveSlots[saveId].isFilled = false;
            this.saveSlots[saveId].saveName.text = "";
            this.saveSlots[saveId].saveTime.text = "";
            this.saveSlots[saveId].saveSlotObj.SetActive(false);
        }

        private void OnCreateNewSave()
        {
            int saveId = GetEmptySaveSlotId();
            Debug.Log("New Save Id = " + saveId);
            if (saveId == -1)
            {
                return;
            }
            SaveManager.Instance.SaveGame(saveId);
            
            SavedGamesInfo savedData = SaveManager.Instance.LoadObject<SavedGamesInfo>("SavedGamesInfo");
            
            this.saveSlots[saveId].isFilled = true;
            this.saveSlots[saveId].saveName.text = "Save-" + saveId.ToString();
            this.saveSlots[saveId].saveTime.text = savedData.savedGamesDic[saveId];
            this.saveSlots[saveId].saveSlotObj.SetActive(true);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(SaveGameEventArgs.EventId, OnGameSave);

            InitSaveSlot();
            
            // Player should not be able to create new save slot when they are in ProcedureMenu, since
            // the playerData may not have been instanciated yet
            if (GameEntry.Procedure.CurrentProcedure is ProcedureMenu)
            {
                this.createNewSaveButtonObj.SetActive(false);
            }
            else 
            {
                this.createNewSaveButtonObj.SetActive(true);
            }
        }

        private void InitSaveSlot()
        {
            SavedGamesInfo savedData = SaveManager.Instance.LoadObject<SavedGamesInfo>("SavedGamesInfo");

            // if player open this UIForm in ProcedureMenu, then saveData may be null
            if (savedData != null && savedData.savedGamesDic != null)
            {
                foreach (var item in savedData.savedGamesDic)
                {
                    this.saveSlots[item.Key].saveSlotObj.SetActive(true);
                    this.saveSlots[item.Key].isFilled = true;
                    this.saveSlots[item.Key].saveTime.text = item.Value;
                    this.saveSlots[item.Key].saveName.text = "Save-" + item.Key.ToString();
                    // Player can only load game in ProcedureMenu, can only overwrite save slot in ProcedureMap
                    if (GameEntry.Procedure.CurrentProcedure is ProcedureMenu)
                    {
                        this.saveSlots[item.Key].loadBtnObj.SetActive(true);
                        this.saveSlots[item.Key].overwriteBtnObj.SetActive(false);
                    }
                    else 
                    {
                        this.saveSlots[item.Key].loadBtnObj.SetActive(false);
                        this.saveSlots[item.Key].overwriteBtnObj.SetActive(true);
                    }
                }
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(SaveGameEventArgs.EventId, OnGameSave);
        }

        private void OnGameSave(object sender, GameEventArgs e)
        {
            SaveGameEventArgs ne = (SaveGameEventArgs) e;
            if (ne == null)
                Log.Error("Invalid Event SaveGameEventArgs");

            int saveId = ne.SaveId;
            if (saveId > 4 || saveId < 0)
                Log.Error("Invalid Save Id, SaveId must be between 0, 1, 2, 3, 4");
            
            saveSlots[saveId].saveSlotObj.SetActive(true);
            saveSlots[saveId].isFilled = true;
            saveSlots[saveId].saveTime.text = DateTime.Now.ToLongTimeString();
            saveSlots[saveId].saveName.text = "Save-" + saveId.ToString();
        }

        private void OnCancelButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            Close();
            if (GameEntry.Procedure.CurrentProcedure is ProcedureMenu)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIMainMenuForm);
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureMap)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIMapInfoForm);
                if (!GameEntry.UI.HasUIForm(EnumUIForm.UIMapPlayerInfoForm))
                {
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIMapPlayerInfoForm);
                }
            }
        }

        private void OnLoadButtonClicked(int id)
        {
            Debug.Log("SaveId = " + id);
            SaveManager.Instance.LoadGame(id);
        }

        private void OnOverwriteClicked(int id) 
        {
            Debug.Log("Overwirte Id = " + id);
            SaveManager.Instance.SaveGame(id);

            SavedGamesInfo savedData = SaveManager.Instance.LoadObject<SavedGamesInfo>("SavedGamesInfo");
            
            this.saveSlots[id].isFilled = true;
            this.saveSlots[id].saveName.text = "Save-" + id.ToString();
            this.saveSlots[id].saveTime.text = savedData.savedGamesDic[id];
        }

        private int GetEmptySaveSlotId()
        {
            foreach (var item in this.saveSlots)
            {
                if (!item.isFilled)
                {
                    return item.SaveId;
                }
            }
            return -1;
        }

    }

    [Serializable]
    public class SaveSlot
    {
        public GameObject saveSlotObj;
        public GameObject loadBtnObj;
        public GameObject overwriteBtnObj;
        public int SaveId;
        [HideInInspector] public bool isFilled;  // determine if the current save slot has been occupied
        public TextMeshProUGUI saveName;
        public TextMeshProUGUI saveTime;
        public Button loadBtn;
        public Button overwriteBtn;
        public Button deleteBtn;
    }
}


