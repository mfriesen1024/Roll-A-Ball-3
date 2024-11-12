using KeystoneUtils.FileSystem.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Data
{
    /// <summary>
    /// Saves anything that gets unlocked (levels for example)
    /// 
    /// Currently this isn't used.
    /// </summary>
    public class ProgressionSave : IBinarySerializable<ProgressionSave>
    {
        const int reqBytes = 4;

        public int unlockedLevel = 0;

        public ProgressionSave FromBinary(byte[] data)
        {
            ProgressionSave save = new()
            {
                unlockedLevel = BitConverter.ToInt32(data)
            };

            return save;
        }

        int IBinarySerializable<ProgressionSave>.GetReqBytes()
        {
            return reqBytes;
        }

        public byte[] ToBinary(ProgressionSave data)
        {
            byte[] bytes = [.. BitConverter.GetBytes(data.unlockedLevel)];
            return bytes;
        }
    }
}
