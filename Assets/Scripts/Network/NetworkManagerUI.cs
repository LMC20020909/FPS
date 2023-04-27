using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{

    [SerializeField]
    private Button refreshButton;
    [SerializeField]
    private Button buildButton;

    [SerializeField]
    private Canvas menuUI;
    [SerializeField]
    private GameObject roomButtonPrefab;

    private List<Button> rooms = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        setConfig();
        initButtons();
        RefreshRoomList();
    }

    private int buildRoomPort = -1; // -1表示不是房主，否则表示当前client是创建房间的房主，存储该房间的端口号

    // 在整个游戏退出前执行（点击叉号）
    private void OnApplicationQuit()
    {
        if (buildRoomPort != -1)    // 是房主，退出时自动移除房间
        {
            RemoveRoomList();
        }
    }

    private void setConfig()
    {
        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-port")
            {
                int port = int.Parse(args[i + 1]);
                var transport = GetComponent<UNetTransport>();
                transport.ConnectPort = transport.ServerListenPort = port;
            }
        }

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-lauch-as-server")
            {
                NetworkManager.Singleton.StartServer();
                DestroyAllButtons();
            }
        }
    }

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

    private void RemoveRoomList()
    {
        StartCoroutine(RemoveRoomListRequest("http://101.34.2.178:8080/fpsapp/remove_room/?port=" + buildRoomPort.ToString()));
    }

    IEnumerator RemoveRoomListRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.ConnectionError)
        {
            var resp = JsonUtility.FromJson<RemoveRoomResponse>(uwr.downloadHandler.text);

            if (resp.error_message == "success")
            {

            }
        }
    }
}
