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

    // ���� ���� ��Ȳ.
    private enum GameProgress
    {
        None = 0,       // ���� ���� ��.
        Ready,          // ���� ���� ��ȣ ǥ��.
        Turn,           // ���� ��.
        Result,         // ��� ǥ��.
        GameOver,       // ���� ����.
        Disconnect,     // ���� ����.
    };

    // �� ����.
    private enum Turn
    {
        Own = 0,        // �ڻ��� ��.
        Opponent,       // ����� ��.
    };

    // ������ ��.
    private Turn turn;

    // ���� ��ȣ.
    private Turn localTurn;

    // ����Ʈ ��ȣ.
    private Turn remoteTurn;

    // ���� ��Ȳ.
    private GameProgress progress;

    // ���� ���� �÷���.
    private bool isGameOver;

    // ��Ʈ��ũ.
    private NetworkManager network = null;


    // Use this for initialization
    void Start()
    {
        // Network Ŭ������ ������Ʈ ��������.
        GameObject obj = GameObject.Find("NetworkManager");
        network = obj.GetComponent<NetworkManager>();
        if (network != null)
        {
            network.RegisterEventHandler(EventCallback);
        }

        // ������ �ʱ�ȭ�մϴ�.
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

    // ���� ����, �ܺ� UI���� ȣ����.
    public void GameStart()
    {
        // ���� ���� ���·� �մϴ�.
        progress = GameProgress.Ready;

        // ������ ���� �ϰ� �����մϴ�.
        turn = Turn.Own;

        // �ڽŰ� ����� ��ȣ�� �����մϴ�.
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
        Debug.Log($"���� ����? : {localTurn.ToString()}");

        // ���� ������ Ŭ�����մϴ�.
        isGameOver = false;
    }


    void UpdateReady()
    {
        // ���� �����Դϴ�.
        // Ÿ�̸� �ڵ带 �����µ� �̻��� ����� ��ġ��������
        progress = GameProgress.Turn;
    }

    void UpdateTurn()
    {
        Debug.Log($"�� ���� :{turn}");
        bool setMark = false;
        if (turn == localTurn)
        {
            setMark = DoOwnTurn();

            //�� �� ���� ��Ҹ� ������ Ŭ���� ����ȿ���� ���ϴ�.
            if (setMark == false && Input.GetMouseButtonDown(0))
            {

            }
        }
        else
        {
            setMark = DoOppnentTurn();
            //�� �� ���� �� ������ Ŭ���� ���� ȿ���� ���ϴ�.
            if (Input.GetMouseButtonDown(0))
            {

            }
        }

        if (setMark == false)
        {
            // ���� ���� ���� ���Դϴ�.	
            return;
        }
        else
        {
            //��ȣ�� ���̴� ���� ȿ���� ���ϴ�. 
        }

        // ���� �����մϴ�.

        turn = (turn == Turn.Own) ? Turn.Opponent : Turn.Own;
        Debug.Log($"�� ���� :{turn}");
    }

    // ���� ���� ó��
    void UpdateGameOver()
    {
        // ������ �����մϴ�.
        // 1�� ī��尡 �־���.
        Reset();
        isGameOver = true;
    }

    // �ڽ��� ���� ���� ó��.
    bool DoOwnTurn()
    {
        //��� �����ߴ���
        int index = 0;



        // ������ ĭ�� ������ �۽��մϴ�.
        byte[] buffer = new byte[1];
        buffer[0] = (byte)index;
        Debug.Log($"�۽� : {buffer[0]}");
        C_CheckCard movePacket = new C_CheckCard();
        movePacket.SelectIdx = index;       //�̰Ͱ�
        movePacket.Answer = 0;              //�̰��� ������
        network.Send(movePacket.Write());

        return true;
    }

    // ����� ���� ���� ó��.
    bool DoOppnentTurn()
    {
        Debug.Log("DoOppnentTurn");

        // ����� ������ �����մϴ�.
        int index = PlayerManager.Instance.returnStone();
        if (index <= -1)
        {
            // ���� ���ŵ��� �ʾҽ��ϴ�.
            Debug.Log($"���ŵ� �� : {index}");
            return false;
        }

        // ������� �� Ŭ���̾�Ʈ��� ���� �����մϴ�.
        Turn mark = (network.IsServer() == true) ? Turn.Opponent : Turn.Own;
        Debug.Log("�޴ټ��ż���");

        // ������ ������ ���õ� ĭ���� ��ȯ�մϴ�. 
        Debug.Log("Recv:" + index + " [" + network.IsServer() + "]");


        return true;
    }

    // ���� ����.
    void Reset()
    {
        turn = Turn.Own;
        progress = GameProgress.None;

    }

    // ���� ����.
    void NotifyDisconnection()
    {
        string message = "ȸ���� ������ϴ�.\n\n��ư�� ��������.";

    }



    // ���� ���� üũ.
    public bool IsGameOver()
    {
        return isGameOver;
    }

    // �̺�Ʈ �߻� ���� �ݹ� �Լ�.
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
        print("��ư������");

    }


}
