using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ����ģʽ
    public static GameManager Singleton;

    [SerializeField]
    public MatchingSettings matchingSettings;

    // ��¼�����������Ҷ�Ӧ��ϵ���ֵ䣬ÿ��client��sever��ά��һ��
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    private static Dictionary<string, Npc> npcs = new Dictionary<string, Npc>();

    private void Awake()
    {
        Singleton = this;
    }

    public void RegisterPlayer(string name, Player player)
    {
        player.transform.name = name;
        players.Add(name, player);
    }

    public void RegisterNpc(string name, Npc npc)
    {
        npcs.Add(name, npc);
    }

    public Player GetPlayer(string name)
    {
        return players[name];
    }

    public Npc GetNpc(string name)
    {
        return npcs[name];
    }

    public void UnRegisterPlayer(string name)
    {
        players.Remove(name);
    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200f, 200f, 200f, 400f));
    //    GUILayout.BeginVertical();

    //    //GUI.contentColor = Color.black;
    //    //GUILayout.Label(info);

    //    GUI.color = Color.red;

    //    foreach (string name in players.Keys)
    //    {
    //        Player player = GetPlayer(name);
    //        GUILayout.Label(name + " - " + player.GetHealth());
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

}
