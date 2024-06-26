﻿using Server.Session;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Server
{
    class PacketHandler
    {
        // 돌 놓는거
        public static void C_MoveStoneHandler(PacketSession session, IPacket packet)
        {
            C_MoveStone movePacket = packet as C_MoveStone;
            ClientSession clientSession = session as ClientSession;
            if (clientSession.Room == null)
                return;
            Console.WriteLine($"{movePacket.StonePosition}");

            GameRoom room = clientSession.Room;
            room.Move(clientSession, movePacket);
        }

        // 클라가 떠났을 때, room에서 내쫓는 동작
        public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;

            if (clientSession.Room == null)
                return;

            GameRoom room = clientSession.Room;
            room.Leave(clientSession);
        }

        public static void C_CheckCardHandler(PacketSession session, IPacket packet)
        {
            C_CheckCard cardPacket = packet as C_CheckCard;
            ClientSession clientSession = session as ClientSession;
            if (clientSession.Room == null)
                return;
            Console.WriteLine($"선택:{cardPacket.SelectIdx} 답:{cardPacket.Answer}");

            GameRoom room = clientSession.Room;
            room.CheckCard(clientSession, cardPacket);
        }

        public static void C_RandomCardHandler(PacketSession session, IPacket packet)
        {
            C_RandomCard cardPacket = packet as C_RandomCard;
            ClientSession clientSession = session as ClientSession;
            if (clientSession.Room == null)
                return;
            Console.WriteLine($"서버 - 숫자:{cardPacket.Num} 색:{cardPacket.Color}");

            GameRoom room = clientSession.Room;
            room.RandomCard(clientSession, cardPacket);
        }
    }
}
