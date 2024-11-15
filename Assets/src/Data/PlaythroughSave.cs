﻿using KeystoneUtils.FileSystem.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Data
{
    /// <summary>
    /// Saves the player's progression for a given "run", but nothing that should persist between runs.
    /// </summary>
    public class PlaythroughSave:IBinarySerializable<PlaythroughSave>
    {
        const byte reqBytes = 12;
        public int level=0;
        public int checkpoint=0;
        public int lives=3;

        public PlaythroughSave FromBinary(byte[] data)
        {
            PlaythroughSave save = new()
            {
                level = BitConverter.ToInt32(data),
                checkpoint = BitConverter.ToInt32(data, 4),
                lives = BitConverter.ToInt32(data,8)
            };

            return save;
        }

        public byte[] ToBinary(PlaythroughSave data)
        {
            byte[] bytes = [.. BitConverter.GetBytes(data.level), .. BitConverter.GetBytes(data.checkpoint),.. BitConverter.GetBytes(data.lives)];
            return bytes;
        }

        int IBinarySerializable<PlaythroughSave>.GetReqBytes()
        {
            return reqBytes;
        }
    }
}
