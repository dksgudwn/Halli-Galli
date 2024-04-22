using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DummyClient;
using System.Reflection;
using TMPro;

public class DaVinciCode : MonoBehaviour
{
    [SerializeField] TMP_InputField input;

    // 게임 진행 상황.
    private enum GameProgress
    {
        None = 0,       // 시합 시작 전.
        Ready,          // 시합 시작 신호 표시.
        Turn,           // 시함 중.
        Result,         // 결과 표시.
        GameOver,       // 게임 종료.
        Disconnect,     // 연결 끊기.
    };

    // 턴 종류.
    private enum Turn
    {
        Own = 0,        // 자산의 턴.
        Opponent,       // 상대의 턴.
    };

    // 현재의 턴.
    private Turn turn;

    // 로컬 기호.
    private Turn localTurn;

    // 리모트 기호.
    private Turn remoteTurn;

    // 진행 상황.
    private GameProgress progress;

    // 게임 종료 플래그.
    private bool isGameOver;

    // 네트워크.
    private NetworkManager network = null;


    // Use this for initialization
    void Start()
    {
        // Network 클래스의 컴포넌트 가져오기.
        GameObject obj = GameObject.Find("NetworkManager");
        network = obj.GetComponent<NetworkManager>();
        if (network != null)
        {
            network.RegisterEventHandler(EventCallback);
        }

        // 게임을 초기화합니다.
        Reset();
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        //switch (progress)
        //{
        //    case GameProgress.Ready:
        //        UpdateReady();
        //        break;

        //    case GameProgress.Turn:
        //        UpdateTurn();
        //        break;

        //    case GameProgress.GameOver:
        //        UpdateGameOver();
        //        break;
        //}
    }

    // 게임 시작, 외부 UI에서 호출함.
    public void GameStart()
    {
        // 게임 시작 상태로 합니다.
        progress = GameProgress.Ready;

        // 서버가 먼저 하게 설정합니다.
        turn = Turn.Own;

        // 자신과 상대의 기호를 설정합니다.
        if (network.IsServer() == true)
        {
            localTurn = Turn.Own;
            remoteTurn = Turn.Opponent;
        }
        else
        {
            localTurn = Turn.Opponent;
            remoteTurn = Turn.Own;
        }
        Debug.Log($"나는 누구? : {localTurn.ToString()}");

        // 이전 설정을 클리어합니다.
        isGameOver = false;
    }


    void UpdateReady()
    {
        // 게임 시작입니다.
        // 타이머 코드를 지웠는데 이상이 생기면 고치겠지렁이
        progress = GameProgress.Turn;
    }

    void UpdateTurn()
    {
        Debug.Log($"턴 시작 :{turn}");
        bool setMark = false;
        if (turn == localTurn)
        {
            setMark = DoOwnTurn();

            //둘 수 없는 장소를 누르면 클릭용 사운드효과를 냅니다.
            if (setMark == false && Input.GetMouseButtonDown(0))
            {

            }
        }
        else
        {
            setMark = DoOppnentTurn();
            //둘 수 없을 때 누르면 클릭용 사운드 효과를 냅니다.
            if (Input.GetMouseButtonDown(0))
            {

            }
        }

        if (setMark == false)
        {
            // 놓을 곳을 검토 중입니다.	
            return;
        }
        else
        {
            //기호가 놓이는 사운드 효과를 냅니다. 
        }

        // 턴을 갱신합니다.

        turn = (turn == Turn.Own) ? Turn.Opponent : Turn.Own;
        Debug.Log($"턴 갱신 :{turn}");
    }

    // 게임 종료 처리
    void UpdateGameOver()
    {
        // 게임을 종료합니다.
        // 1초 카운드가 있었다.
        Reset();
        isGameOver = true;
    }

    // 자신의 턴일 때의 처리.
    bool DoOwnTurn()
    {
        //어딜 선택했는지
        int index = 0;



        // 선택한 칸의 정보를 송신합니다.
        byte[] buffer = new byte[1];
        buffer[0] = (byte)index;
        Debug.Log($"송신 : {buffer[0]}");
        C_CheckCard movePacket = new C_CheckCard();
        movePacket.SelectIdx = index;       //이것과
        movePacket.Answer = 0;              //이것을 보낸다
        network.Send(movePacket.Write());

        return true;
    }

    // 상대의 턴일 때의 처리.
    bool DoOppnentTurn()
    {
        Debug.Log("DoOppnentTurn");

        // 상대의 정보를 수신합니다.
        int index = PlayerManager.Instance.returnStone();
        if (index <= -1)
        {
            // 아직 수신되지 않았습니다.
            Debug.Log($"수신된 값 : {index}");
            return false;
        }

        // 서버라면 ○ 클라이언트라면 ×를 지정합니다.
        Turn mark = (network.IsServer() == true) ? Turn.Opponent : Turn.Own;
        Debug.Log("받다수신수신");

        // 수신한 정보를 선택된 칸으로 변환합니다. 
        Debug.Log("Recv:" + index + " [" + network.IsServer() + "]");


        return true;
    }

    // 게임 리셋.
    void Reset()
    {
        turn = Turn.Own;
        progress = GameProgress.None;

    }

    // 끊김 통지.
    void NotifyDisconnection()
    {
        string message = "회선이 끊겼습니다.\n\n버튼을 누르세요.";

    }



    // 게임 종료 체크.
    public bool IsGameOver()
    {
        return isGameOver;
    }

    // 이벤트 발생 시의 콜백 함수.
    public void EventCallback(NetEventState state)
    {
        switch (state.type)
        {
            case NetEventType.Disconnect:
                if (progress < GameProgress.Result && isGameOver == false)
                {
                    progress = GameProgress.Disconnect;
                }
                break;
        }
    }

    public void TestBtn1()
    {
        int selectIndex = 1;
        int num = int.Parse(input.text);

        C_CheckCard cardPacket = new C_CheckCard();
        cardPacket.SelectIdx = selectIndex;
        cardPacket.Answer = num;
        network.Send(cardPacket.Write());
        print("버튼누르기");

    }


}
