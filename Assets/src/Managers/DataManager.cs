using KeystoneUtils.FileSystem.Binary;
using KeystoneUtils.Logging;
using RollABall.Assets.src.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RollABall.Assets.src.Managers
{
    internal class DataManager
    {
        string dataPath = "data\\";
        string savesPath = "saves\\";
        string scoresPath = "scores\\";

        public void Save(ScoreSave save)
        {
            string fName = GetUniqueFileName(savesPath+scoresPath, "","score");

            IBinarySerializable<ScoreSave> data = save;

            File.WriteAllBytesAsync(fName, data.ToBinary(save));
        }

        string GetUniqueFileName(string path, string name, object extension)
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
            Directory.CreateDirectory(path);
                int num = 1;
                string finalName = $"{path}{name}-{dateOnly.ToShortDateString()}-{num}.{extension}";
                while (File.Exists(finalName))
                {
                    num++;
                    finalName = $"{path}{name}-{dateOnly.ToShortDateString()}-{num}.{extension}";
                }

                File.Create(finalName).Close();

            return finalName;
        }
    }
}
