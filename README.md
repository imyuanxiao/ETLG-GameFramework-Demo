# ETLG-GameFramework-Demo

## 1. 如何创建Procedure

如果需要创建一个名为ProcedureNew的流程：

1. 在```GameMain/Procedure```文件夹下创建新脚本，命名为```ProcedureNew.cs```
2. 打开该脚本，设定其命名空间namespace为ETLG，删掉```Start()```和```Update()```方法，删去默认继承的```MonoBehavior```，使其继承于```ProcedureBase```
3. 复写其父类中的生命周期方法，```OnInit()```，```OnEnter()```，```OnUpdate()```，```OnLeave()```，```OnDestroy()```
   ```
   protected override void OnInit(ProcedureOwner procedureOwner)
   {
        base.OnInit(procedureOwner);
   }

   protected override void OnEnter(ProcedureOwner procedureOwner)
   {
        base.OnEnter(procedureOwner);
   }

   protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
   {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
   }

   protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
   {
        base.OnLeave(procedureOwner, isShutdown);
   }

   protected override void OnDestroy(ProcedureOwner procedureOwner)
   {
        base.OnDestroy(procedureOwner);
   }
   ```
4. 回到Unity编辑器中，选中Hierarchy窗口中GameFramework的Procedure物体，在对应的Inspector窗口中，勾选刚刚创建的ProcedureNew。


## 2. 如何切换Procedure

假设我们要从ProcedureNew切换到ProcedureSecond：

- 打开```ProcedureNew.cs```，在其```OnUpdate()```中写入：
   ```
   ChangeState<ProcedureSecond>(procedureOwner);
   ```
切换流程时，会先后执行当前流程的```OnLeave()```方法和新流程的```OnEnter()```方法。

**注意：在我们的游戏里，切换场景时会自动切换流程，切换到的新场景以及其对应进入的流程位于配置文件```GameMain/DataTables/Scene.txt```中。因此若想在切换场景的同时切换流程，只需要在上述txt文件中配置好就行，不用手动切换流程。**

## 3. 如何新建场景

假设我们新建一个场景NewScene.unity

1. 在Project窗口中，在```GameMain/Scenes```下新建文件夹```NewScene```，在```NewScene```文件夹下，右键 -> Create -> Scene，为其命名为```NewScene```
2. 在Unity编辑器中，选择File -> Build Settings，将刚刚创建的NewScene.unity拖入Scenes In Build栏
3. 打开```GameMain/DataTables/AssetsPath.txt```，接着已经配置好的Scene条目，加上一条
   ```
   107	新场景	Assets/GameMain/Scenes/NewScene/NewScene.unity	5
   ```
   注意，中间用Tab分割，资源编号不能与已存在的值冲突
4. 打开```GameMain/DataTables/Scene.txt```，接着已经配置好的Scene条目，加上一条
   ```
   7	新场景	107	ProcedureNew
   ```
   注意，中间用Tab分割，资源Id必须与上一步中AssetsPath.txt中的资源编号一致，场景编号不能与已存在的值冲突。ProcedureNew是切换到NewScene后会进入的流程
5. 打开```GameMain/Configs/DefaultConfig.txt```，接着已经配置好的Scene条目，加上一条
   ```
   Scene.NewScene       7
   ```
   注意，中间用2个Tab分割，配置值必须与上一步中Scene.txt中的场景编号一致。
6. 回到Unity编辑器，选择Tools，分别点击Generate DataTables，Generate DataTableCode和Generate DataTableEnum。

## 4. 如何切换场景

切换场景需要在相关Procedure中进行。假设我们当前场景为NewScene，当前Procedure为ProcedureNew，我们想切换到SecondScene。

1. 打开```GameMain/Procedure/ProcedureNew.cs```脚本，为其添加以下成员变量
   ```
   private bool changeScene;
   private ProcedureOwner procedureOwner;
   ```
2. 在```OnEnter()```方法中，设置
   ```
   changeScene = false;  // 很重要
   this.procedureOwner = procedureOwner;
   ```
3. 在```OnEnter()```方法中，订阅切换场景事件
   ```
   GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
   ```
4. 在```OnLeave()```方法中，取消订阅切换场景事件
   ```
   GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
   ```
5. 在```ProcedureNew.cs```中实现事件处理方法```OnChangeScene```
   ```
   private void OnChangeScene(object sender, GameEventArgs e)
   {
       ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
       if (ne == null)
           return;

       changeScene = true;
       procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
   }
   ```
6. 在```OnUpdate()```方法中，添加以下代码
   ```
   if (changeScene)
   {
       ChangeState<ProcedureLoadingScene>(procedureOwner);
   }
   ```

以上完成了切换场景事件的订阅以及事件处理函数的实现，那么如何触发该事件呢？假设当前场景中存在UI，名为```UINewScene.prefab```，其中有一个按钮名为```SecondSceneButton```，我们通过点击这个按钮触发场景切换事件：

1. 打开该UI所挂载的脚本（一般在```GameMain/Scripts/UI```下，且继承于```UGuiFormEx```），为SecondSceneButton定义成员变量
   ```
   public Button secondSceneButton;
   ```
   在Unity编辑器中，将该按钮组件拖入脚本刚刚定义的变量中。
2. 在该脚本的```OnInit()```方法中，为该按钮订阅点按事件处理函数
   ```
   secondSceneButton.onClick.AddListener(OnBtnClick);
   ```
3. 实现该事件处理函数
   ```
   private void OnBtnClick()
   {
       GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.SecondScene")));
   }
   ```
   ```Create()```方法接受的参数就是希望切换到的场景Id

这样，就能通过点击按钮触发场景转换事件，从而实现场景转换（同时流程也会被切换到SecondScene对应的Procedure）

## 5. 如何新建UI

假设我们想新建UI，名为```UINew```

1. 将做好的UI prefab - ```UINew.prefab```保存到```GameMain/UI/UIForms```下，确保其挂载了一个继承于```UGuiFormEx```的脚本。
2. 打开```GameMain/DataTables/AssetsPath.txt```，在```#UI```下，接着已经配置好的UI条目，加上一条
   ```
   1030	新界面	Assets/GameMain/UI/UIForms/UINew.prefab	0
   ```
   注意，中间用Tab分割，资源编号不能与已存在的值冲突
3. 打开```GameMain/DataTables/UIForm.txt```，接着已经配置好的UI条目，加上一条
   ```
   1030	新界面	UINew	xxxx	1030	TRUE/FALSE	TRUE/FALSE
   ```
   注意，中间用Tab分割，资源Id必须与上一步中AssetsPath.txt中的资源编号一致，界面编号不能与已存在的值冲突。
4. 回到Unity编辑器，选择Tools，分别点击Generate DataTables，Generate DataTableCode和Generate DataTableEnum。
5. 之后，可以使用如下代码开启UI
   ```
   GameEntry.UI.OpenUIForm(EnumUIForm.UINew);
   ```

## 6. 如何新建DataTable

使用Excel制作表格，之后到处为txt文本保存于```GameMain/DataTables```下。假设导出的的txt为```New.txt```
```
#	New			
#	Id		field1	field2
#	int		int	string
#	编号	备注	列1	列2
	1	备注1	101	placeholder
	2	备注2	102	placeholder
	3	备注3	103	placeholder
	4	备注4	104	placeholder
	5	备注5	105	placeholder
	6	备注6	106	placeholder
```

1. 回到Unity编辑器，选择Tools，分别点击Generate DataTables，Generate DataTableCode和Generate DataTableEnum。
2. 在```GameMain/Data```下新建文件夹```New```，在```New```下新建2个脚本```NewData.cs```和```DataNew.cs```。
3. 打开```DataNew.cs```，设其命名空间为```ETLG.Data```，使```DataNew```继承于```DataBase```
4. 声明2个成员变量
   ```
   private IDataTable<DRNew> dtNew;
   private Dictionary<int, NewData> dicNewData;
   ```
5. 重写父类方法```OnPreload()```
   ```
   protected override void OnPreload()
   {
       base.OnPreload();
       LoadDataTable("New");
   }
   ```
6. 重写父类方法```OnLoad()```
   ```
   protected override void OnLoad()
   {
       base.OnLoad();

       dicNewData = new Dictionary<int, NewData>();

       dtNew = GameEntry.DataTable.GetDataTable<DRNew>();

       if (dtNew == null)
           throw new System.Exception("Can not get data table New");

       DRNew[] dRNews = dtNew.GetAllDataRows();

       foreach (DRNew dRNew in dRNews)
       {
           NewData newData = new NewData(dRNew);
           dicNewData.Add(dRNew.Id, newData);
       }
   }
   ```
7. 重写父类方法```OnUnload()```
   ```
   protected override void OnUnload()
   {
       base.OnUnload();

       GameEntry.DataTable.DestroyDataTable<DRBossEnemy>();

       dtNew = null;
       dicNewData = null;
   }

   ```
8. 实现数据获取方法
   ```
   public NewData GetNewData(int id) 
   {
       if (dicNewData.ContainsKey(id))
       {
           return dicNewData[id];
       }
       return null;
   }

   public NewData[] GetAllNewData() 
   {
       int index = 0;
       NewData[] results = new NewData[dicNewData.Count];
       foreach (NewData newData in dicNewData.Values)
       {
           results[index++] = newData;
       }

       return results;
   }
   ```
9. 打开```NewData.cs```，设其命名空间为```ETLG.Data```，其不继承于任何父类
10. 声明以下成员变量
    ```
    private DRNew dRNew;

    public int Id { get { return dRNew.Id; } }

    public int Field1 { get { return dRNew.field1; } }

    public string Field2 { get { return dRNew.field2 } }
    ```
11. 实现构造函数
    ```
    public NewData(DRNew dRNew)
    {
        this.dRNew = dRNew;
    }
    ```
12. 回到Unity编辑器，选中Hierarchy中GameFramework的Customs中的Data，在对应Inspector窗口中，勾选ETLG.Data.DataNew

之后，可以通过如下代码访问NewData
```
NewData newData = GameEntry.Data.GetData<DataNew>().GetNewData(Id);
int field1 = newData.Field1;
string field2 = newData.Field2;
```
其中，```Id```为New.txt文件中Id列的值，可以通过Id取出不同行的数据。

## 7. 如何将数据传入UI

假设我们希望将New.txt中的某一行数据展示到UINew中，如何将相关数据传递给UI？

这里假设我们要传入的数据为NewData类型的newData。

（虽然我们可以直接在UINew所挂载的脚本中用```GameEntry.Data.GetData<DataNew>().GetNewData(Id)```获取数据，但这里为了演示，采取另一种方式，这种方式适合将无法被全局获取的数据传入UI）。

1. 打开UINew界面，并同时传入newData
   ```
   GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleWin, newData);
   ```
2. 打开UINew所挂载的脚本，在```OnOpen()```方法中添加
   ```
   NewData newData = (NewData) userData;
   int field1 = newData.Field1;
   string field2 = newData.Field2;
   ```
   注意需要先将```object```类型的```userData```强制转换为```NewData```类型。

由此我们在将数据newData传递到了UINew所挂载的脚本中，可以在该脚本中使用相关数据，并把它们展示到UI上。

## 8. 如何新建Event

假设我们要新建事件```NewEvent```，且希望该事件接收一个```int```类型的参数```eventParam```

1. 在```GameMain/Scripts/Event```文件夹下新建脚本```NewEventArgs.cs```
2. 打开该脚本，首先修改其为如下
   ```
   namespace ETLG
   {
      public class NewEventArgs : GameEventArgs
      {
         public static readonly int EventId = typeof(NewEventArgs).GetHashCode();

         public override int Id { get { return EventId; } }

         public int EventParam { get; private set; }

         public static NewEventArgs Create(int eventParam)
         {
            NewEventArgs e = ReferencePool.Acquire<NewEventArgs>();
            e.EventParam = eventParam;
            return e;
         }

         public override void Clear()
         {
            EventParam = 0;
         }
      }
   }
   ```
3. 现在可以在任何需要的地方订阅该事件，订阅事件方法如下
   ```
   GameEntry.Event.Subscribe(NewEventArgs.EventId, OnNewEvent);
   ```
   注意，所有的事件订阅方法都一定要对应一个取消订阅的方法
   ```
   GameEntry.Event.Unsubscribe(NewEventArgs.EventId, OnNewEvent);
   ```
   比如，在某个Procedure的OnEnter()方法内订阅的事件，一定要在该Procedure的OnLeave()方法中取消订阅
4. 实现事件处理函数```OnNewEvent```
   ```
   private void OnChangeScene(object sender, GameEventArgs e)
   {
      NewEventArgs ne = (NewEventArgs) e;
      if (ne == null)
         return;
      
      // 获取事件触发时被传入的参数
      int eventParam = ne.EventParam;
      // 事件触发时需要执行的代码   
      // ...   
   }
   ```
   一个事件可以有多个事件处理函数，当事件被触发时，所有订阅了该事件的函数都会被执行。
5. 在新建完事件以及订阅了相关的事件处理函数后，可以在需要的地方触发该事件
   ```
   int eventParam = ??? // 触发事件时需要传入的参数
   GameEntry.Event.Fire(this, NewEventArgs.Create(eventParam));
   ```