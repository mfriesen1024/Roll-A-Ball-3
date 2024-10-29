using KeystoneUtils.FileSystem.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Data
{
    /// <summary>
    /// Represents a score/time the player set for a level.
    /// </summary>
    public class ScoreSave : IBinarySerializable<ScoreSave>
    {
        const int reqBytes = 12;
        public int score = 0;
        public long time = 0;

        ScoreSave IBinarySerializable<ScoreSave>.FromBinary(byte[] data)
        {
            ScoreSave save = new()
            {
                score = BitConverter.ToInt32(data),
                time = BitConverter.ToInt64(data, 4)
            };

            return save;
        }

        int IBinarySerializable<ScoreSave>.GetReqBytes()
        {
            return reqBytes;
        }

        byte[] IBinarySerializable<ScoreSave>.ToBinary(ScoreSave data)
        {
            byte[] bytes = [.. BitConverter.GetBytes(score), .. BitConverter.GetBytes(time)];
            return bytes;
        }
    }
}
