<p align="center">
  <img src="https://1000logos.net/wp-content/uploads/2021/09/Among-Us-Logo.png" width="400" height="200"> 
</p>  

Sugoma is a multiplayer first person shooter game made in Unity with [Photon engine](https://www.photonengine.com) as a networking tool and [PlayFab](https://playfab.com) as data management system.  
---  
***
# **Features!**
* A fully working login and signup system
* Available Europe server for multiplayer games
* Animated weapons using blender and with unity particles
* Player health, respawn, spawnpoints
* Intro dialogue scene done using unity timeline system
* In-game leaderboard (tab button toggle)
* Volume settings
* Score permanency connected to each player account (k/d ratio) 
***  
# **Screenshots**
![login](https://user-images.githubusercontent.com/88716637/220421310-b2cfe2bf-094b-44b6-9050-dd9e8c2c2500.png)
![intro](https://user-images.githubusercontent.com/88716637/220421341-2c9889ba-18ba-46df-8c58-268985e56f30.png)
![main](https://user-images.githubusercontent.com/88716637/220421346-82b43198-a62f-434a-952e-842cd8fda572.png)
![game](https://user-images.githubusercontent.com/88716637/220422880-ae4db5d9-ae66-4343-9390-43ba733df6c5.png)
![game2](https://user-images.githubusercontent.com/88716637/220422888-94cf17b9-6f8b-4734-bffd-a443b9086cfd.png)


# **Project tree**

 * [.vscode](/FPS/.vscode/)
 * [Assets](/FPS/Assets/)      
   * [Items](/FPS/Assets/Items/) 
   * [Materials](/FPS/Assets/Materials)
   * [Music](/FPS/Assets/Music)
   * [PhotonUnityNetworking](/FPS/Assets/PhotonUnityNetworking)
   * [PlayFabEditorExtensions](/FPS/Assets/PlayFabEditorExtensions)
   * [PlayFabSDK](/FPS/Assets/PlayFabSDK)
   * [PostProcessing](/FPS/Assets/PostProcessing)

   * [Prefabs](/FPS/Assets/Prefabs)
   * [Resources](/FPS/Assets/Resources)
   * [Scenes](/FPS/Assets/Scenes)
   * [Scripts](/FPS/Assets/Scripts)
     * [Effects](/FPS/Assets/Scripts/Effects)
     * [Managers](/FPS/Assets/Scripts/Managers)
     * [Models](/FPS/Assets/Scripts/Models)
   * [TextMesh_Pro](/FPS/Assets/TextMesh_Pro)
   * [images](/FPS/Assets/images)
 * [Packages](/FPS/Packages/)
 * [ProjectSettings](/FPS/ProjectSettings/)
 * [UserSettings](/FPS/UserSettings/)

*Assets folder structure breakdown*
> Items hold assets that define two types of guns: Pistol and Rifle.  
 Materials define all materials used for any objects used inside game scene (player, guns, map...).  
 Music contains all audio files used for effects or menu.  
 PostProcessing defines all profiles used to improve the game appearance.  
 Prefabs allows fully configured GameObjects to be saved in the Project for reuse (spawnpoints, maps...).  
 Resources contain photon prefabs used for syncing defined prefabs across all players.  
 Scenes hold our two playable planes menus and game screen.  
 Images hold all backgrounds and icons.  
 Scripts contain all the code required for our game to function divided in three subfolders:  
- Effects, that run audio files on actions and make sound global as opposed to local
- Managers, that are responsible for all settings, closing and opening menus, scoreboards and much more 
- Models, that define starting states, scoreboard kills and death info and communication with playfab  
# **How to use**
Open up project in Unity and then ```CTRL+B``` will prompt you to select folder in which the game would be built. From there running exe file will start the game.


