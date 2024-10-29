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

        public void SaveScore(ScoreSave save)
        {
            string fName = GetUniqueFileName(savesPath+scoresPath, "","score");

            IBinarySerializable<ScoreSave> data = save;

            File.WriteAllBytesAsync(fName, data.ToBinary(save));
        }

        public void SavePlaythrough(int level, int checkpoint, int lives)
        {
            // Define vars
            string ptFName = "run.save";

            IBinarySerializable<PlaythroughSave> blerg1 = new PlaythroughSave();

            // Get previous saves.
            PlaythroughSave playthrough = new();

            playthrough.checkpoint=checkpoint;
            playthrough.level=level;
            playthrough.lives=lives;

            File.WriteAllBytesAsync(savesPath+ptFName, blerg1.ToBinary(playthrough));
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
