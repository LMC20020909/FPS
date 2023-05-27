# 游戏介绍

tzlFps是一款具有丰富战略元素的多人在线第一人称射击游戏，拥有实时联机对战的特性，为玩家带来了前所未有的刺激体验。

在tzlFps中，玩家将在精美的地图和环境中尽情体验射击的乐趣。游戏提供了前、后、左、右和跳跃等基本移动方式，玩家可以灵活利用这些移动来逃避敌人的攻击或寻找有利位置进行射击。

tzlFps强调个人技术和游戏策略的平衡。游戏提供了多人对战模式，玩家可以与来自各地的玩家进行激烈的对战。在游戏中，玩家死亡后可以选择重生，再次加入战斗。

玩家在游戏中可以自由更换武器和子弹，选择最适合自己的装备来应对战斗。游戏还提供了"开房间"的功能，允许玩家创建私人游戏空间，邀请朋友一起对战。

除此之外，tzlFps还提供了与NPC的交互功能，这不仅丰富了游戏的内容，也为玩家提供了更多的游戏体验。

此开发文档将详细介绍上述功能的实现细节，帮助开发者和玩家理解和改进游戏的各个方面。希望您在阅读此文档的过程中，能对tzlFps有更深入的了解，也期待您为游戏的优化和提升贡献自己的力量。

# 开发环境

## 开发工具

- Unity 2021.3.11f1c2
- Visual Stdio 2022
- Blender 2.93.0

## 开发语言/框架

游戏开发：C# (Unity)

后端开发：Python (Django)

# 玩法说明

## 游戏初始界面说明

在tzlFps游戏的主界面上，玩家将看到两个按钮：`Refresh`和`Build`。

### 创建房间

如果玩家希望创建一个新的游戏房间，可以点击`Build`按钮。在点击此按钮后，系统将自动为玩家创建一个新的房间，并且玩家会自动进入新创建的房间中开始游戏。

### 加入房间

如果玩家希望加入已存在的游戏房间，可以点击`Refresh`按钮。在点击此按钮后，系统将为玩家显示所有已存在的房间列表。玩家可以在列表中选择一个房间加入，与房间内的玩家进行对战。如果玩家不希望加入已存在的任何一个房间，也可以选择重新创建一个新的房间，便于玩家进行好友组队。

### 房间规则

每个游戏房间的游戏状态是相互独立的。当房间的创建者（即房主）退出游戏时，对应的房间将自动被系统销毁。

（目前由于服务器性能的限制，系统最多只能同时存在3个房间。这些房间的编号范围为Room-7777到Room-7779）

初始界面如下图所示：

![初始界面](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E8%BF%9B%E5%85%A5%E6%B8%B8%E6%88%8F.png)

## 角色移动

在tzlFps游戏中，人物的移动操作是通过键盘和鼠标来实现的。玩家需要使用键盘上的 W, S, A, D 键来分别控制人物的前进、后退、左移和右移。此外，玩家可以通过按下空格键来使人物进行跳跃操作。

在移动过程中，玩家还可以通过移动鼠标来改变视角，即使人物的视线朝向进行旋转，从而获取更广阔的视野，掌握更多战场信息。

## 射击、枪支设定、切枪和换弹

在tzlFps游戏中，射击操作是通过鼠标左键实现的，而换枪和换弹则通过键盘上的特定按键实现。以下是详细的操作方法：

### 射击

玩家可以通过按下鼠标左键进行射击操作。射击的目标是鼠标指针（屏幕准星）所指向的方向。

![射击](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%B0%84%E5%87%BB.png)

### 枪支设定和换枪

游戏中，玩家共有两种枪支可供选择：突击步枪M4和霰弹枪Benelli M4。可以通过按Q键来切换当前使用的枪支。

以下是两种枪支的详细属性：

1. **突击步枪M4**：能进行连续射击，初始伤害为3点，单次装填可容纳30发子弹，换弹时间为1秒。
2. **霰弹枪Benelli M4**：仅能进行单发射击，初始伤害为10点，单次装填可容纳8发子弹，换弹时间为2秒。

两种枪支的有效射程均为100米。

为了保证游戏公平性和更好的游戏体验，当玩家使用M4进行连发时将感受到逐渐增强的后坐力，造成准星偏移。对于新手玩家，我推荐采用点射的方式进行射击，以达到最佳的游戏效果。

### 换弹

在子弹用尽的情况下，游戏将自动进行换弹操作。玩家也可以通过按R键来主动进行换弹操作，换弹过程中玩家无法射击，但可以移动。

本游戏采取无限弹夹模式，这意味着在一局游戏中，玩家的弹夹数量没有上限。当子弹用光后，系统会自动进行换弹操作，无需玩家手动购买弹夹。

在游戏过程中，掌握和熟练运用这些操作，将极大地提升玩家的战斗效率和胜率。

## 游戏玩法

在tzlFps游戏中，每位玩家都从空中降落到地图上的随机位置开始游戏。以下是游戏的详细玩法机制：

### 血量和血条

每位玩家的初始血量为100点。玩家可以在屏幕下方看到一条白色的血条，该血条将实时显示玩家当前的血量。此外，每位玩家的头顶都有一个血条，这个血条会实时显示该玩家的血量，供其他玩家查看。

![对战](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%AF%B9%E6%88%98.png)

### 游戏模式

tzlFps是一款单人对战游戏，不存在团队的概念。玩家的目标是击杀尽可能多的其他玩家。在一局游戏结束时，击杀数最多的玩家将被判定为胜利者。

### 声音提示

在游戏中，当玩家开枪射击时，会发出射击的声响。玩家可以通过听这些声响，来判断其他玩家的大致位置，从而提前做好准备，抢占战斗的先机。

### 复活和无敌时间

当玩家的血量被消耗殆尽后，玩家将被击败。击败的玩家在3秒后将会在地图上的随机位置复活。复活的玩家在复活后的前5秒内会拥有无敌状态。在这5秒内，玩家可以利用这个无敌时间，观察游戏环境，寻找敌人的位置，为接下来的对战做好准备。

## NPC说明

在tzlFps游戏中，我们特别添加了一些以***《原神》***角色为模型的NPC，让游戏体验更加丰富和有趣。

### NPC介绍

***《原神》***是由米哈游自主研发的一款全新开放世界冒险游戏，其角色设计和世界观深受玩家们的喜爱。在tzlFps游戏中，我们将这些人气角色作为NPC的模型，给玩家带来更加丰富的视觉体验。

### NPC交互

在tzlFps游戏中，NPC不仅仅是一个简单的模型，它们还具有各自的待机动画和与玩家的交互效果。

以《原神》角色“可莉”为例，当玩家对可莉进行射击后，可莉会做出受击的表情动作，同时玩家的移动速度将会获得大幅提升，持续20秒。这种设计为玩家提供了新的战术选择：是否开枪射击NPC获得增益效果，还是选择隐藏自己，避免暴露自己的位置。这增加了游戏的策略性和挑战性。

由于时间原因，目前我仅实现了部分NPC与玩家的交互。我期待玩家们能提供宝贵的建议，帮助我在未来的更新中进一步完善NPC的设计和交互效果，使tzlFps游戏变得更加丰富和有趣。

![原神角色——丘丘人](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E4%B8%98%E4%B8%98%E4%BA%BA.png)

![原神角色——可莉](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%8F%AF%E8%8E%89.png)

## 退出游戏

玩家可以按下`windows+s`键或者`windows`键来重新获取光标，点击叉号即可退出；或者直接按下`alt+shift+F4`直接关掉游戏。

# 技术说明

> 由于篇幅原因，仅展示了游戏核心功能的实现，其他辅助功能如玩家血条和子弹数量的实时显示、枪支后坐力等则隐去不表。如有兴趣可查看源代码：[https://github.com/LMC20020909/FPS](https://github.com/LMC20020909/FPS)

## 玩家人物模型、地图、枪支模型和粒子特效

玩家人物模型来自于Unity资源商店中[`Low Poly Soldiers`](https://assetstore.unity.com/packages/3d/characters/humanoids/low-poly-soldiers-53612)资源包。

地图来自于Unity资源商店中[`Low Poly FPS Map Vol.1`](https://assetstore.unity.com/packages/3d/environments/urban/low-poly-fps-map-vol-1-195910)资源包。

枪支模型来自于Unity资源商店中[`3D GUNS | Guns Pack`](https://assetstore.unity.com/packages/3d/props/weapons/3d-guns-guns-pack-228975)资源包。

粒子特效来自于Unity资源商店中[`Particle Pack`](https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325)资源包。

## 初始界面UI

![初始界面UI](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%88%9D%E5%A7%8B%E7%95%8C%E9%9D%A2UI.png)

如上图所示，初始界面UI是一张canvas，由四个组件组成：Refresh按钮、Build按钮、玩家血条和剩余子弹数量。由于玩家进入游戏时仅能看到上面两个按钮，故初始状态下玩家血条和剩余子弹数量被禁用，只有当玩家进入游戏后才会实时显示。为了适应不同分辨率的屏幕，将玩家血条和剩余子弹数量的组件锚点分别设置为底部和右下角，保证显示在正确位置上。

### 房间功能的实现

在tzlFps中，将服务器的端口抽象为一个房间，服务器为腾讯云轻量级服务器，并在三个端口中分别独立运行着tzlFps的游戏服务器，以此来实现对局的独立和全局玩家状态的同步。开房间、删除房间等后端功能采用python (Django)实现，由于与本课程所授内容关系不大，故不在此详细展开。此处仅展示Unity界面UI的房间获取。

#### Refresh按钮功能实现

为Refresh按钮新增点击事件，当点击按钮时，新开一个线程，调用`UnityWebRequest.Get`函数访问后端提供好的url，返回的数据为当前存在的房间名称。然后对每一个返回的房间，为其创建一个按钮，将其由上到下实例化在界面UI上，并为这些房间按钮增加点击事件，当点击房间时，新建一个客户端，连接到后端服务器上对应端口的游戏服务器，即进入对局。

```c#
private void initButtons()
{
    refreshButton.onClick.AddListener(() =>
    {
        RefreshRoomList();
    });

    buildButton.onClick.AddListener(() =>
    {
        BuildRoomList();
    });
}

private void RefreshRoomList()
{
    StartCoroutine(RefreshRoomListRequest("http://101.34.2.178:8080/fpsapp/get_room_list/"));
}

IEnumerator RefreshRoomListRequest(string uri)
{
    UnityWebRequest uwr = UnityWebRequest.Get(uri);
    yield return uwr.SendWebRequest();

    if (uwr.result != UnityWebRequest.Result.ConnectionError)
    {
        // print(uwr.downloadHandler.text);    // 输出http请求的返回结果
        var resp = JsonUtility.FromJson<GetRoomListResponse>(uwr.downloadHandler.text);
        foreach (var room in rooms)
        {
            room.onClick.RemoveAllListeners();
            Destroy(room.gameObject);
        }
        rooms.Clear();

        int k = 0;
        foreach (var room in resp.rooms)
        {
            GameObject buttonObj = Instantiate(roomButtonPrefab, menuUI.transform);
            buttonObj.transform.localPosition = new Vector3(-20, 49 - k * 65, 0);
            Button button = buttonObj.GetComponent<Button>();
            button.GetComponentInChildren<TextMeshProUGUI>().text = room.name;
            button.onClick.AddListener(() =>
            {
                var transport = GetComponent<UNetTransport>();
                transport.ConnectPort = transport.ServerListenPort = room.port;
                NetworkManager.Singleton.StartClient();
                DestroyAllButtons();
            });
            rooms.Add(button);
            k++;
        }
    }
}
```

#### Build按钮功能的实现

```c#
private void initButtons()
{
    refreshButton.onClick.AddListener(() =>
    {
        RefreshRoomList();
    });

    buildButton.onClick.AddListener(() =>
    {
        BuildRoomList();
    });
}

private void BuildRoomList()
{
    StartCoroutine(BuildRoomListRequest("http://101.34.2.178:8080/fpsapp/build_room/"));
}

IEnumerator BuildRoomListRequest(string uri)
{
    UnityWebRequest uwr = UnityWebRequest.Get(uri);
    yield return uwr.SendWebRequest();

    if (uwr.result != UnityWebRequest.Result.ConnectionError)
    {
        var resp = JsonUtility.FromJson<BuildRoomResponse>(uwr.downloadHandler.text);
        if (resp.error_message == "success")
        {
            var transport = GetComponent<UNetTransport>();
            transport.ConnectPort = transport.ServerListenPort = resp.port;
            buildRoomPort = resp.port;
            NetworkManager.Singleton.StartClient();
            DestroyAllButtons();
        }
    }
}
```

同理，在点击Build按钮时，访问后端提供的创建房间的url，并直接创建游戏客户端，进入对局。

### 按钮的隐藏和血条、子弹数量的显示

在玩家进入房间开始对局后，Refresh按钮和Build按钮被隐藏，而玩家血条和剩余子弹数量则可以被玩家所见。

```c#
private void DestroyAllButtons()
{
    refreshButton.onClick.RemoveAllListeners();
    buildButton.onClick.RemoveAllListeners();

    Destroy(refreshButton.gameObject);
    Destroy(buildButton.gameObject);

    foreach (var room in rooms)
    {
        room.onClick.RemoveAllListeners();
        Destroy(room.gameObject);
    }
}

public void setPlayer(Player localPlayer)
{
    player = localPlayer;
    weaponManager = player.GetComponent<WeaponManager>();
    bulletsObject.SetActive(true);
    healthBarObject.SetActive(true);
}
```

在玩家点击房间或者创建房间进入对局后，将Refresh和Build以及所有房间按钮全部删除，并将玩家血条和剩余子弹数量组件激活。

### camera的设置

在游戏中，一共设置了两个camera，一个作为全局视角在进入对局前展示给玩家，另一个则在进入对局后作为第一人称视角放置在每个玩家的头部绑定。为了不引起冲突，在进入对局后将第一个camera禁用，在退出对局后再将其激活。

```c#
// 当玩家第一次成功加入网络时，执行一次
public override void OnNetworkSpawn()
{
    base.OnNetworkSpawn();
    
    sceneCamera = Camera.main;
    if (sceneCamera != null)
    {
        sceneCamera.gameObject.SetActive(false);
    }
}

// 当玩家断开网络时执行
public override void OnNetworkDespawn()
{
    base.OnNetworkDespawn();
    if (sceneCamera != null)
    {
        sceneCamera.gameObject.SetActive(true);
    }
}
```

## 人物移动、跳跃与视角旋转

`PlayerInput`

```c#
void Update()
{
    float xMov = Input.GetAxisRaw("Horizontal");
    float yMov = Input.GetAxisRaw("Vertical");

    Vector3 velocity = (transform.right * xMov + transform.forward * yMov).normalized * speed;
    controller.Move(velocity);

    float xMouse = Input.GetAxisRaw("Mouse X");
    float yMouse = Input.GetAxisRaw("Mouse Y");

    //左右旋转，旋转的是整个角色
    Vector3 yRotation = new Vector3(0f, xMouse, 0f) * lookSensitivity;
    //上下视角旋转， 旋转的是camera，所以两个操作不能向量合并
    Vector3 xRotation = new Vector3(-yMouse, 0f, 0f) * lookSensitivity;
    controller.Rotate(yRotation, xRotation);

    if (Input.GetButton("Jump"))
    {
        if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f))
        {
            Vector3 force = Vector3.up * thrusterForce;
            controller.Thrust(force);
        }
    }
}
```
`PlayerController`

```c#
private void PerformMovement()
{
    if (velocity != Vector3.zero)
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    if (thrusterForce != Vector3.zero)
    {
        rb.AddForce(thrusterForce); // 作用Time.FixedDeltaTime秒：0.02秒
        thrusterForce = Vector3.zero;
    }
}

private void PerformRotation()
{
    if (recoilForce < 0.1)
    {
        recoilForce = 0f;
    }

    if (yRotation != Vector3.zero || recoilForce > 0)
    {
        //rb.MoveRotation() 需要传一个四元数
        rb.transform.Rotate(yRotation + rb.transform.up * Random.Range(-2f * recoilForce, 2f * recoilForce));
    }

    if (xRotation != Vector3.zero || recoilForce > 0)
    {
        cameraRotationTotal += xRotation.x - recoilForce;
        cameraRotationTotal = Mathf.Clamp(cameraRotationTotal, -cameraRotationLimit, cameraRotationLimit);  //小于最小值就更新为最小值， 大于最大值就更新为最大值
        cam.transform.localEulerAngles = new Vector3(cameraRotationTotal, 0f, 0f);
    }

    recoilForce *= 0.5f;
}
```

该功能分为两个脚本实现，`PlayerInput`负责接受玩家的输入并传递给`PlayerController`实际操控玩家的移动。移动功能直接改变玩家位置即可，跳跃功能则首先检验玩家是否具备跳跃条件，即向玩家正下方发一条射线，如果在一定范围内检测到其他物体的碰撞体则可以跳跃，如果没有（例如在跳跃过程中或者在空中）则空格键无效。

此处难点在于通过鼠标控制玩家视角的旋转。由于第一人称视角的camera与玩家的头部绑定在了一起，而在真实游戏中，玩家左右移动鼠标旋转的是整个人物，而上下移动鼠标则旋转的是玩家的视角，即camera，所以需要分开进行实现。另外，为玩家摄像机视角的上下变化设置了旋转范围，防止出现看到人物模型内部的状况。

## 联机功能的实现

整个游戏编写过程中，最麻烦的地方就在于实现这一功能。由于本游戏是一款多人联机对战的fps游戏，所以玩家状态的同步是影响游戏体验的关键因素。

在Unity中，有两种实现联机的方式：

- Server判断：Client处理用户输入，将用户输入信息传给Server，Server控制角色移动，将结果返回给所有Client【延迟大；不易作弊】

- Client模式：Client处理用户用户输入以及角色移动，然后将结果发给Server，Server仅同步结果给其他Client【延迟小；易作弊】

在tzlFps中，网络同步联机主要通过Unity提供的`Netcode for GameObjects`工具包来实现。在scene中创建一个空物体，取名为`NetWorkManager`，接着设置需要同步的信息：

1. Player自身的状态

   ![同步player](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%90%8C%E6%AD%A5player.png)

2. Player中的camera

   ![同步camera](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%90%8C%E6%AD%A5camera.png)

3. 开局创建角色



![创建角色](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%88%9B%E5%BB%BA%E8%A7%92%E8%89%B2.png)

### 禁用非本地玩家的组件

此时运行游戏，在多人联机时，会发现两个角色会同时移动，并且Unity中显示有多个Audio被使用的警告。原因是在一个Player中，没有禁用其他Player的输入控制和Camera。

所以通过脚本在本地玩家加入网络联机时禁用本地玩家对远程玩家的操纵。

```c#
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    private Camera sceneCamera;
    
    // 当玩家第一次成功加入网络时，执行一次
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsLocalPlayer)
        {
            SetLayerMaskForAllChildren(transform, LayerMask.NameToLayer("Remote Player"));
            DisableComponents();
        }
        else
        {
            PlayerUI.Singleton.setPlayer(GetComponent<Player>());
            SetLayerMaskForAllChildren(transform, LayerMask.NameToLayer("Player"));
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
            transform.position = new Vector3(Random.Range(-40f, 20f), 35f, Random.Range(-8f, 60f));
        }

        string name = "Player " + GetComponent<NetworkObject>().NetworkObjectId.ToString();
        Player player = GetComponent<Player>();
        player.Setup();

        GameManager.Singleton.RegisterPlayer(name, player);
    }

    private void SetLayerMaskForAllChildren(Transform transform, LayerMask layerMask)
    {
        transform.gameObject.layer = layerMask;
        for (int i = 0; i < transform.childCount; i ++ )
        {
            SetLayerMaskForAllChildren(transform.GetChild(i), layerMask);
        }
    }

    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    // 当玩家断开网络时执行
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.Singleton.UnRegisterPlayer(transform.name);
    }
}
```

## 射击功能与换枪

```c#
if (currentWeapon.shootRate <= 0)   // 单发
{
    if (Input.GetButtonDown("Fire1") && ShootCoolDownTime >= currentWeapon.shootCoolDownTime)
    {
        autoShootCount = 0;
        Shoot();
        ShootCoolDownTime = 0f; // 重置冷却时间
    }
} else    // 连发
{
    if (Input.GetButtonDown("Fire1"))
    {
        autoShootCount = 0;
        // 重复执行一个函数，第一个参数为函数名，第二个参数为什么时候执行第一次， 第三个参数为两次执行的时间间隔
        InvokeRepeating("Shoot", 0f, 1f / currentWeapon.shootRate);
    } else if (Input.GetButtonUp("Fire1") || Input.GetKeyDown(KeyCode.Q))   // 松开鼠标左键或者切换武器，停止连发
    {
        CancelInvoke("Shoot");
    }
}
```

首先通过判断自己定义的武器射击频率变量来判断当前武器是单发还是连发。接着，通过`Input.GetButtonDown("Fire1")`获取玩家鼠标左键的点击，并调用`Shoot`函数进行射击判定；获取玩家键盘按下Q的操作，进行换枪。

### 射击判定

射击开枪的功能是通过发一条射线来实现的。通过`Physics.Raycast()`函数向玩家camera的方向发一条射线，射线长度为设置好的当前武器的有效射击距离。

将所有玩家的tag都设置为`PLAYER_TAG`，以此来判定是否射击到了玩家。

```c#
private void Shoot()
{
    if (currentWeapon.bullets <= 0 || currentWeapon.isReloading) return;

    currentWeapon.bullets--;

    if (currentWeapon.bullets <= 0)
    {
        weaponManager.reload(currentWeapon);
    }

    autoShootCount++;
    float recoilForce = currentWeapon.recoilForce;

    if (autoShootCount <= 3)
    {
        recoilForce *= 0.2f;
    }

    OnshootServerRpc(recoilForce);

    RaycastHit hit; // 射线击中的第一个物体
    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
    {
        if (hit.collider.tag == PLAYER_TAG)
        {
            ShootServerRpc(hit.collider.name, currentWeapon.damage);
            OnHitServerRpc(hit.point, hit.normal, HitEffectMaterial.Metal);
            SetIsHitted(hit.collider.name);
        }
        else
        {
            OnHitServerRpc(hit.point, hit.normal, HitEffectMaterial.Stone);
        }
        if (hit.collider.tag == "NPC")
        {
            NpcOnHitServerRpc(hit.collider.name);
        }
    }
}

[ServerRpc]
private void ShootServerRpc(string name, int damage)
{
    Player player = GameManager.Singleton.GetPlayer(name);
    player.TakeDamage(damage);
}
```

### 受击判定

为了尽量在游戏体验和游戏公平性之间做出权衡，本游戏采取了如下受击判定方法：

首先由本地玩家获取射击到的物体，如果本地判定射击到了其他玩家，则将这一信息上传到游戏服务器进行判定，游戏服务器首先在服务器端重新计算玩家的血量，之后将新的状态同步广播给所有客户端。

## 伤害计算、死亡与重生

### 伤害计算

```c#
// 受到了伤害, 只会在服务器端被调用
public void TakeDamage(int damage)
{
    if (isDead.Value || isInvincible.Value) return;   // 死了就不再受到伤害

    currentHealth.Value -= damage;

    if (currentHealth.Value <= 0)
    {
        currentHealth.Value = 0;
        isDead.Value = true;

        if (!IsHost)
        {
            DieOnServer();
        }
        DieClientRpc();
    }
}
```

### 死亡判定

```c#
private void DieOnServer()
{
    Die();
}

[ClientRpc]
private void DieClientRpc()
{
    Die();
}

private void Die()
{
    GetComponent<PlayerShooting>().StopShooting();  // 死亡时停止射击

    GetComponentInChildren<Animator>().SetInteger("direction", -1);
    GetComponentInChildren<Animator>().SetBool("isHitted", false);
    GetComponent<Rigidbody>().useGravity = false;

    for (int i = 0; i < componentsToDisable.Length; i ++ )
    {
        componentsToDisable[i].enabled = false;
    }
    Collider col = GetComponent<Collider>();
    col.enabled = false;

    StartCoroutine(Respawn());  // 开启一个新的线程执行重生函数
}
```

同理，死亡判定也是在服务器端进行，之后同步广播给所有客户端。

### 重生

```c#
private IEnumerator Respawn() // 重生
{
    yield return new WaitForSeconds(GameManager.Singleton.matchingSettings.respawnTime);    // 延迟3秒后重生

    SetDefaults();  // 恢复初始状态

    GetComponentInChildren<Animator>().SetInteger("direction", 0);
    GetComponent<Rigidbody>().useGravity = true;
    
    Vector3[] resPos = { new Vector3(0f, 10f, 0f), new Vector3(0f, 10f, 100f), new Vector3(-60f, 10f, 25f), new Vector3(45f, 10f, 27f) };
    transform.position = resPos[Random.Range(0, 4)];
}
```

首先设置等待3秒作为重生时间，接着设置四个重生点，保证玩家每次死亡后都会随机重生至不同的位置，提升游戏体验。

## NPC模型的实现

NPC模型采用游戏***《原神》***所提供的角色模型，参考老师上课所放[视频](https://www.bilibili.com/video/BV1G34y127e6/?spm_id_from=333.999.0.0&vd_source=7554f1c4b7a28b1f0e9ee9560c5d1e37)中的方法将模型导入Unity。大致步骤如下：

1. 将原神角色模型的.pmx文件导入Blender；
2. 使用插件对角色模型进行修复；
3. 导出为.fbx文件；
4. 将.fbx文件导入Unity并进行二次元渲染。（单独提取出角色材质并通过插件进行渲染）

接着，从开源免费动画网站[Mixamo](https://www.mixamo.com/#/)上下载NPC的角色动画（站立、跳舞等）应用到模型上，并增加受击特效和效果（如玩家移速提升）。但由于开源动画与原神人物模型并不适配，还需要手动调整角色模型的骨骼，使其变成标准的T-pose。

参考视频：[从Mixamo导入的动画用起来会变形怎么办？三分钟告诉你怎么解决。](https://www.bilibili.com/video/BV1fg411w7HT/?spm_id_from=333.999.0.0&vd_source=7554f1c4b7a28b1f0e9ee9560c5d1e37)

![角色骨骼调整](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/qin.png)

NPC动画转换功能实现如下：

```c#
private void AlterIdle()
{
    idleTime += Time.deltaTime;
    int idle = 0;
    if (idleTime > 8f)
    {
        idleTime = 0f;
        idle = 1;
    }
    animator.SetInteger("idle", idle);
}

// Update is called once per frame
void Update()
{
    if (skinned.GetBlendShapeWeight(8) > 0)
    {
        horribleTime += Time.deltaTime;
        if (horribleTime > 3f)
        {
            skinned.SetBlendShapeWeight(8, 0f);
            skinned.SetBlendShapeWeight(39, 0f);
            horribleTime = 0f;
        }
    }

    AlterIdle();
}
```

## 玩家人物动画系统的设计

![玩家人物动画](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%8A%A8%E7%94%BB.png)

本游戏实现了玩家人物待机、前进、后退、左移、右移、左后、右后、跳跃、受击、死亡的动画，力求增加玩家的游戏体验与真实效果，这些动画包含在人物模型的资源包中，所以与人物模型非常适配。

动画效果的切换通过脚本来实现：

首先为动画系统设置变量来控制动画的切换：

![动画变量](https://cdn.jsdelivr.net/gh/LMC20020909/BlogMaps2//img/%E5%8A%A8%E7%94%BB%E5%8F%98%E9%87%8F.png)

接着通过脚本实现不同情况下的动画展示：

```c#
private void PerformAnimation()
{
    Vector3 deltaPosition = transform.position - lastFramePosition;
    lastFramePosition = transform.position;

    float forward = Vector3.Dot(deltaPosition, transform.forward);
    float right = Vector3.Dot(deltaPosition, transform.right);

    int direction = 0;  // 静止

    if (forward > eps)
    {
        direction = 1;  // 向前
    }
    else if (forward < -eps)
    {
        if (right > eps)
        {
            direction = 4;  // 右后
        }
        else if (right < -eps)
        {
            direction = 6;  // 左后
        }
        else
        {
            direction = 5;  // 后
        }
    }
    else if (right > eps)
    {
        direction = 3;  // 右
    }
    else if (right < -eps)
    {
        direction = 7; // 左
    }

    if (!Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f))
    {
        direction = 8;
    }

    if (GetComponent<Player>().IsDead())
    {
        direction = -1;
    }

    animator.SetInteger("direction", direction);
}
```

# 总结

在这次的课程设计中，我有幸参与并完整地完成了一款多人联机第一人称射击游戏tzlFps的开发。这个过程不仅让我对Unity编程有了更深的理解，更锻炼了我的游戏开发能力，尤其是在设计游戏玩法、开发游戏功能、以及编写开发文档等方面。

整个项目从立项到结项历时一个月。每天我都会在紧凑的学习和生活中抽出时间来专心致志地开发游戏，完成每一个功能模块的实现。每当我完成一个功能，看到它在游戏中如我所愿地运行，我都会有一种强烈的成就感，这让我更加热爱这个项目，也更加坚定了我对游戏开发的热情。

由于我是单人组队，所有的开发任务都由我一个人来完成，包括游戏的界面设计、玩法规则的制定、以及开发文档的编写等。这的确让我付出了巨大的心力，但同时，我也在这个过程中得到了丰富的学习和成长。我更深入地理解了游戏开发的全过程，也更深入地领略了游戏开发的乐趣和挑战。

在游戏开发结束后，我还和我的室友一起体验了这款游戏。虽然我们的tzlFps在可玩性上还无法与市面上的知名FPS游戏如《守望先锋》、《吃鸡》等相媲美，但作为一个游戏开发的初学者，我对这款游戏的整体设计和实现还是感到很满意的。我认为这是一个很好的开始，是我在游戏开发之路上的一个重要里程碑。

当然，我也清楚地意识到，我们的tzlFps还存在一些bug和不足之处。如果有时间和机会的话，我希望能在之后的时间里进行修复和优化，以及增加更多的游戏功能，比如团队战、手雷等等。

最后，我要感谢这门课的指导老师。在我进行游戏开发的过程中，他们的指导和帮助让我在许多功能的实现上少走了弯路，也让我在遇到困难和挫折时能够及时找到解决问题的方向和方法。

总的来说，这次的课程设计是一次非常宝贵的学习和成长的经历。我深感幸运和感激，能够有这样的机会去实践我对游戏开发的热爱，去挑战我对游戏开发的理解和技能。我相信，这次的经历将对我未来的学习和发展产生深远的影响。

# 备注

首先，我非常感谢您能抽出宝贵的时间来体验我开发的这款游戏，tzlFps。作为游戏开发的初学者，我深知自己的知识和技能还有许多不足之处，也明白这款游戏在设计和实现上可能存在一些问题和缺陷。因此，我非常欢迎并期待您能提供宝贵的意见和建议，帮助我改进游戏，提升玩家的游戏体验。

同时，我也欢迎您在游戏中发现并提交bug。如果您在游戏中遇到任何问题或错误，您可以通过访问 https://github.com/LMC20020909/FPS/issues 来向我反映。我将会及时查看并处理您提交的问题，努力修复游戏中的bug，同时更新游戏，以确保每一位玩家都能在tzlFps中享受到流畅而愉快的游戏体验。

再次表示感谢，希望您在tzlFps的世界中找到乐趣，同时期待您的反馈和建议，帮助我不断提升和优化这款游戏。



游戏的源代码可以通过以下网址查看：https://github.com/LMC20020909/FPS

> 游戏的实机演示请见演示视频。
