﻿using System;
using TheRockPaperScissorsGame.Client.Contracts.Enums;

namespace TheRockPaperScissorsGame.Client.Contracts
{
    public class GameOptionsDTO
    {
        public GameOptionsDTO(RoomType roomType, string roomNumber)
        {
            RoomType = roomType;
            RoomNumber = roomNumber;
        }

        public RoomType RoomType { get; set; }

        public string RoomNumber { get; set; }
    }
}
