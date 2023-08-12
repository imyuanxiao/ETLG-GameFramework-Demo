using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameFramework.Localization;
using System;
using TMPro;
using GameFramework.Event;
using ETLG.Data;

namespace ETLG
{
    public class UILoadGameForm : UGuiFormEx
    {
        public GameObject createNewSaveButtonObj;
        public Button createNewSaveButton;
        public Button downloadSaveButton;
        public Button cancelButton;
        public SaveSlot[] saveSlots;

        private Dictionary<string, string> jsonStrDic;
        private int tempCloudSaveId = -1;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            cancelButton.onClick.AddListener(OnCancelButtonClick);
            createNewSaveButton.onClick.AddListener(OnCreateNewSave);
            downloadSaveButton.onClick.AddListener(OnDownloadButtonClicked);

            foreach (SaveSlot saveSlot in this.saveSlots)
            {
                saveSlot.loadBtn.onClick.AddListener(delegate{OnLoadButtonClicked(saveSlot.SaveId);});
                saveSlot.overwriteBtn.onClick.AddListener(delegate{OnOverwriteClicked(saveSlot.SaveId);});
                saveSlot.deleteBtn.onClick.AddListener(delegate{OnDeleteButtonClicked(saveSlot.SaveId);});
                saveSlot.uploadBtn.onClick.AddListener(delegate{OnUploadButtonClicked(saveSlot.SaveId);});
            }
        }

        private void OnUploadButtonClicked(int saveId)
        {
            Debug.Log("Upload Button Clicked : " + saveId);
            // UpdateUploadButton(saveId);
            tempCloudSaveId = saveId;
            Dictionary<string, string> jsonStrDic = SaveManager.Instance.UploadSave(saveId);
          
            BackendDataManager.Instance.HandleSaveUpLoad(jsonStrDic);


        }

        private void OnDownloadButtonClicked()
        {
            Debug.Log("Download Button CLicked");

            BackendDataManager.Instance.HandleLoad();
        }

        private void OnBackendFetched(object sender, GameEventArgs e)
        {
            BackendFetchedEventArgs ne = (BackendFetchedEventArgs) e;
            if (ne == null) { return; }
            Debug.Log("OnBackendFetched");

            if (ne.Type == Constant.Type.BACK_SAVE_DOWNLOAD_SUCCESS)
            {
                this.jsonStrDic = GameEntry.Data.GetData<DataBackend>().downLoadjsonStrDic;
                DownloadSaveUpdate();
            }
            else if(ne.Type == Constant.Type.BACK_SAVE_DOWNLOAD_FAILED)
            {
                Log.Error("Download save from cloud failed");
                return;
            }
            else if (ne.Type == Constant.Type.BACK_SAVE_UPLOAD_SUCCESS)
            {
                SaveManager.Instance.savedGamesInfo.cloudSaveId = tempCloudSaveId;
                UpdateUploadButton(SaveManager.Instance.savedGamesInfo.cloudSaveId);
            }

            
        }

        private void DownloadSaveUpdate()
        {
            if (jsonStrDic == null || jsonStrDic.Count ==0) { return; }
            
            int saveId = SaveManager.Instance.DownloadSave(jsonStrDic);

            SavedGamesInfo savedData = SaveManager.Instance.LoadObject<SavedGamesInfo>("SavedGamesInfo");
            
            this.saveSlots[saveId].isFilled = true;
            this.saveSlots[saveId].saveName.text = "Save-" + saveId.ToString();
            this.saveSlots[saveId].saveTime.text = savedData.savedGamesDic[saveId];
            this.saveSlots[saveId].saveSlotObj.SetActive(true);

            if (GameEntry.Procedure.CurrentProcedure is ProcedureMenu)
            {
                this.saveSlots[saveId].loadBtnObj.SetActive(true);
                this.saveSlots[saveId].overwriteBtnObj.SetActive(false);
            }
            else
            {
                this.saveSlots[saveId].loadBtnObj.SetActive(false);
                this.saveSlots[saveId].overwriteBtnObj.SetActive(true);
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

        // Create a new save slot (other than save slot 0)
        private void OnCreateNewSave()
        {
            int saveId = GetEmptySaveSlotId();
            Debug.Log("New Save Id = " + saveId);
            if (saveId == -1 || saveId == 0)
            {
                return;
            }
            SaveManager.Instance.SaveGame(saveId);
            
            SavedGamesInfo savedData = SaveManager.Instance.LoadObject<SavedGamesInfo>("SavedGamesInfo");
            
            this.saveSlots[saveId].isFilled = true;
            this.saveSlots[saveId].saveName.text = "Save-" + saveId.ToString();
            this.saveSlots[saveId].saveTime.text = savedData.savedGamesDic[saveId];
            this.saveSlots[saveId].saveSlotObj.SetActive(true);

            this.saveSlots[saveId].loadBtnObj.SetActive(false);
            this.saveSlots[saveId].overwriteBtnObj.SetActive(true);
            this.saveSlots[saveId].uploadBtn.transform.parent.GetChild(0).GetComponent<RawImage>().color =  new Color(249f, 230f, 196f, 255f);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(SaveGameEventArgs.EventId, OnGameSave);
            GameEntry.Event.Subscribe(BackendFetchedEventArgs.EventId, OnBackendFetched);

            this.jsonStrDic = null;
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
                // set the upload button icon color
                if (savedData.cloudSaveId != -1 && savedData.cloudSaveId < this.saveSlots.Length)
                {
                    this.saveSlots[savedData.cloudSaveId].uploadBtn.transform.parent.GetChild(0).GetComponent<RawImage>().color = Color.gray;
                }
                // can not upload the auto save
                if (savedData.savedGamesDic.Count > 0)
                {
                    this.saveSlots[0].uploadBtn.transform.parent.gameObject.SetActive(false);
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
            GameEntry.Event.Unsubscribe(BackendFetchedEventArgs.EventId, OnBackendFetched);
            this.jsonStrDic = null;
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
  /*          if (GameEntry.Procedure.CurrentProcedure is ProcedureMenu)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIMainMenuForm);
            }*/
 /*           else if (GameEntry.Procedure.CurrentProcedure is ProcedureMap)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIMapInfoForm);
                if (!GameEntry.UI.HasUIForm(EnumUIForm.UIMapPlayerInfoForm))
                {
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIMapPlayerInfoForm);
                }
            }*/
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
                if (!item.isFilled && item.SaveId != 0)
                {
                    return item.SaveId;
                }
            }
            return -1;
        }

        private void UpdateUploadButton(int saveId)
        {
            for (int i=0; i < this.saveSlots.Length; i++)
            {
                if (i == saveId)
                {
                    this.saveSlots[i].uploadBtn.transform.parent.GetChild(0).GetComponent<RawImage>().color = Color.gray;
                }
                else 
                {
                    this.saveSlots[i].uploadBtn.transform.parent.GetChild(0).GetComponent<RawImage>().color =  new Color(249f, 230f, 196f, 255f);
                }
            }
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
        public Button uploadBtn;
    }
}


