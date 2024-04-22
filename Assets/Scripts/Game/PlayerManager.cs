using DummyClient;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DummyClient.S_PlayerList;

public class PlayerManager
{
    [SerializeField] TMP_Text text;

    Player _myPlayer;
    int StonePosition = -1;
    int TestNum = 0;

    // ���ӵ��ִ� �÷��̾���� ���
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();


    // �� ���� ����
    public void BroadCastStone(S_BroadCastStone packet)
    {
        StonePosition = packet.StonePosition;
        Debug.Log($"���������� ���� : {StonePosition}");
    }

    public void BroadCastCards(S_BroadCastCard packet)
    {
        StonePosition = packet.Answer;
        text.text = $"����: {packet.SelectIdx} ��: {packet.Answer}";
        Debug.Log("�ؽ�Ʈ ����");
    }

    // �� ���� ����  
    public void CastStone(S_MoveStone packet)
    {
        StonePosition = packet.select;
        Debug.Log($"���������� ���� : {StonePosition}");
    }

    public void CastCard(S_BroadCastCard packet)
    {

    }

    public int returnStone()
    {
        return StonePosition;
    }


    // �÷��̾� ����Ʈ ����&����
    public void Add(S_PlayerList packet)
    {
        Object obj = Resources.Load("Player");

        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go = Object.Instantiate(obj) as GameObject;

            if (p.isSelf)
            {
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = new Vector3(-200, 5, 0);
                _myPlayer = myPlayer;
            }
            else
            {
                Player player = go.AddComponent<Player>();
                player.PlayerId = p.playerId;
                player.transform.position = new Vector3(200, 5, 0);
                _players.Add(p.playerId, player);
            }
        }
    }

    // �� Ȥ�� �������� ���� �������� ��
    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
            return;

        Object obj = Resources.Load("Player");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player player = go.AddComponent<Player>();
        _players.Add(packet.playerId, player);
    }

    // �� Ȥ�� �������� ������ ������ ��
    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }

}
