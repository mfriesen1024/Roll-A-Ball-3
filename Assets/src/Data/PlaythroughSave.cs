using KeystoneUtils.FileSystem.Binary;
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
    internal class PlaythroughSave:IBinarySerializable<PlaythroughSave>
    {
        const byte reqBytes = 8;
        public int level;
        public int checkpoint;

        public PlaythroughSave FromBinary(byte[] data)
        {
            PlaythroughSave save = new()
            {
                level = BitConverter.ToInt32(data),
                checkpoint = BitConverter.ToInt32(data, 4)
            };

            return save;
        }

        public byte[] ToBinary(PlaythroughSave data)
        {
            byte[] bytes = [.. BitConverter.GetBytes(data.level), .. BitConverter.GetBytes(data.checkpoint)];
            return bytes;
        }

        int IBinarySerializable<PlaythroughSave>.GetReqBytes()
        {
            return reqBytes;
        }
    }
}
