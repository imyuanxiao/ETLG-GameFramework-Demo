# ETLG-GameFramework-Demo
"Emerging Technology Learning Game" is a 3D sci-fi adventure game in which players explore six planets, each representing a distinct field of knowledge. Players aim to acquire knowledge and enhance their spaceship's capabilities. These six planets represent artificial intelligence, cybersecurity, cloud computing, blockchain, the Internet of Things, and data science, with all content sourced from IBM Skills Build courses. Players interact with non-player characters (NPCs) on each planet, answering questions to help them gain practical skills. This game combines education and entertainment, designed to spark players' interest in learning and enable them to acquire practical knowledge while having fun.
Demo Video: <https://github.com/imyuanxiao/ETLG-GameFramework-Demo>
## Packaging Project Precess
Here's how to package our frontend project in Unity:
Backend Project Repository: <https://github.com/xw22087/rbac-backend-etlg.git>
### Follow these steps to package the project:
1. Open the GameStart Scene and click on "GameFramework" in the Hierarchy. Then select "Builtin".

2. In the Inspector window, uncheck the "Editor Resource Mode" option under the "Base script" component.
<img width="1271" alt="EditorResourceMod" src="https://github.com/imyuanxiao/Emerging-technology-Learning-game/assets/119969754/d632580f-991b-4bac-9b07-b9946c8ba300">

3. Click on "Builtin" in the Hierarchy and choose "Resource". In the Inspector, select "package" for the "Resource script" component's "Resource Mode".
<img width="1274" alt="Resource_Package" src="https://github.com/imyuanxiao/Emerging-technology-Learning-game/assets/119969754/13a4a8e5-f337-4dc8-9823-6b334dba315b">

4. Go to the Unity menu and select "Game Framework", then choose "Resource Tools" and click on "Resource Editor". Add the required assets to the "Resource List" in the Asset List section and click "Save".
<img width="1072" alt="ResourceEditor" src="https://github.com/imyuanxiao/Emerging-technology-Learning-game/assets/119969754/87ba4b68-87a7-4da1-98ad-42a3f573a89f">

5. Next, select "Resource Builder" from "Resource Tools". Modify the "Internal Resource Version" (e.g., set it to 10), choose your desired "Output Dictionary" path, check the "Generate" option under "Output Package Path", and click "Start Build Resource".
<img width="643" alt="ResourceBuilder" src="https://github.com/imyuanxiao/Emerging-technology-Learning-game/assets/119969754/36b27a9b-9784-4def-92a0-4cdf49829694">

6. Locate the recently built Resource folder and open the corresponding Resource Version folder (e.g., 0_1_10) within the "Package" directory. Copy all the files from the Windows folder to the "Assets/StreamingAssets" directory in your project.
![image6](https://github.com/imyuanxiao/Emerging-technology-Learning-game/assets/119969754/33b681bb-4f8a-4b2c-992f-a3c3ed22f866)

7. Go to the Unity menu and select "File", then choose "Build Settings". Click "Build" and select the desired save location.
![BuildSettings](https://github.com/imyuanxiao/Emerging-technology-Learning-game/assets/119969754/fd9a0606-6229-4964-b848-4194598f2cba)

8. Open the newly built folder and run "ETLG-GameFramework.exe" to launch the game.
![image8](https://github.com/imyuanxiao/Emerging-technology-Learning-game/assets/119969754/06a70d34-9c85-4189-8822-28cdb64e2025)

