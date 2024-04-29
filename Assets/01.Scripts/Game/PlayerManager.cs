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
    int StonePosition = 0;
    bool CheckingTurn = false; // 
    int TestNum = 0;
    public int SelectIdx, Answer;

    // 접속되있는 플레이어들의 목록
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();


    // 돌 정보 수신
    public void BroadCastStone(S_BroadCastStone packet)
    {
        StonePosition = packet.StonePosition;
        Debug.Log($"스톤포지션 수신 : {StonePosition}");
    }

    //public void BroadCastCards(S_BroadCastCard packet)
    //{
    //    StonePosition = packet.Answer;
    //    text.text = $"선택: {packet.SelectIdx} 값: {packet.Answer}";
    //    Debug.Log("텍스트 띄우기");
    //}

    // 돌 정보 수신  
    //public void CastStone(S_MoveStone packet)
    //{
    //    StonePosition = packet.select;
    //    Debug.Log($"스톤포지션 수신 : {StonePosition}");
    //}

    public int returnStone() // 계속 -1로 있다가 캐스트 스톤이 실행 될 때만 바뀌고 다시 -1
    {
        int returnStone = StonePosition;
        StonePosition = -1;
        return returnStone;
    }

    // 플레이어 리스트 생성&갱신
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

    // 나 혹은 누군가가 새로 접속했을 때
    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
            return;

        Object obj = Resources.Load("Player");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player player = go.AddComponent<Player>();
        _players.Add(packet.playerId, player);
    }

    // 나 혹은 누군가가 게임을 떠났을 때
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

    public void CastCheckCard(S_CheckCard packet)
    {
        Debug.Log("체크카드받기");
        CheckingTurn = true;
        Debug.Log($"Idx: {packet.SelectIdx} Answer: {packet.Answer}");
        SelectIdx = packet.SelectIdx;
        Answer = packet.Answer;
    }

    public bool returnCard()
    {
        bool returnbool = CheckingTurn;
        CheckingTurn = false;
        return returnbool;
    }
}
