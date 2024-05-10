using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Session;

namespace Server
{
    class GameRoom
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        object _lock = new object();

        public void Move(ClientSession session, C_MoveStone packet)
        {
            lock (_lock)
            {
                // 돌 받고
                session.StonePosition = packet.StonePosition;

                // 모두에게 알림
                S_BroadCastStone move = new S_BroadCastStone();
                move.StonePosition = session.StonePosition + 1;
                BroadCast(move.Write());

            }
        }

        public void CheckCard(ClientSession session, C_CheckCard packet)
        {
            lock (_lock)
            {
                session.SelectNum = packet.SelectIdx;
                session.CardNum = packet.Answer;


                foreach (ClientSession s in _sessions)
                {
                    Console.WriteLine($"서버게임룸 - SessionId : {s.SessionId}");
                    Console.WriteLine($"서버게임룸 - 목적지id : {packet.destinationId}");
                    if (s.SessionId == packet.destinationId)
                    {
                        Console.WriteLine($"서버게임룸 - 선택인덱스 : {packet.SelectIdx}");
                        Console.WriteLine($"서버게임룸 - Answer : {packet.Answer}");

                        S_CheckCard s_CheckCard = new S_CheckCard();
                        s_CheckCard.SelectIdx = session.SelectNum;
                        s_CheckCard.Answer = session.CardNum;
                        s.Send(s_CheckCard.Write());
                    }
                }
            }
        }

        public void RandomCard(ClientSession session, C_RandomCard packet)
        {
            lock (_lock)
            {
                session.RandomNum = packet.Num;
                session.CardType = packet.Color;
            }
            foreach (ClientSession s in _sessions)
            {
                Console.WriteLine($"게임룸 - SessionId : {s.SessionId}");
                if (s.SessionId == packet.destinationId)
                {
                    Console.WriteLine($"게임룸 - 숫자 : {packet.Num}");
                    Console.WriteLine($"게임룸 - 목적지id : {packet.destinationId}");
                    Console.WriteLine($"게임룸 - SessionId : {s.SessionId}");

                    S_RandomCard s_CheckCard = new S_RandomCard();
                    s_CheckCard.Num = session.RandomNum;
                    s_CheckCard.Color = session.CardType;
                    s.Send(s_CheckCard.Write());
                }
            }
        }


        public void Enter(ClientSession session)
        {
            lock (_lock)
            {   // 신규 유저 추가
                _sessions.Add(session);
                session.Room = this;

                // 신규 유저 접속시, 기존 유저 목록 전송
                S_PlayerList players = new S_PlayerList();
                foreach (ClientSession s in _sessions)
                {
                    players.players.Add(new S_PlayerList.Player()
                    {
                        isSelf = (s == session),
                        playerId = s.SessionId,
                    });
                }
                session.Send(players.Write());

                // 신규 유저 접속 전체 공지
                S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
                enter.playerId = session.SessionId;
                BroadCast(enter.Write());
            }

        }

        public void Leave(ClientSession session)
        {
            lock (_lock)
            {
                // 플레이어 제거하고
                _sessions.Remove(session);

                // 모두에게 알린다
                S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
                leave.playerId = session.SessionId;
                BroadCast(leave.Write());
            }
        }

        public void BroadCast(ArraySegment<byte> segment)
        {
            ArraySegment<byte> packet = segment;

            lock (_lock) // 
            {
                foreach (ClientSession s in _sessions)
                {
                    s.Send(segment);    // 리스트에 들어있는 모든 클라에 전송
                }
            }
        }

    }
}
