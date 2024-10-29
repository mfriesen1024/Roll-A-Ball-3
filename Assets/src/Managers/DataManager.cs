using KeystoneUtils.FileSystem.Binary;
using RollABall.Assets.src.Data;
using System;
using System.IO;

namespace RollABall.Assets.src.Managers
{
    internal class DataManager
    {
        string dataPath = "data\\";
        string savesPath = "saves\\";
        string scoresPath = "scores\\";
        string runFName = "run.save";
        string progressionFName = "progression.save";

        // Data objects.
        private PlaythroughSave runData = new();
        private ProgressionSave progression = new();

        public void SaveScore(ScoreSave save)
        {
            string fName = GetUniqueFileName(savesPath + scoresPath, "", "score");

            IBinarySerializable<ScoreSave> data = save;

            File.WriteAllBytesAsync(fName, data.ToBinary(save));
        }

        public void RecordPlaythrough(int level, int checkpoint, int lives)
        {
            runData.checkpoint = checkpoint;
            runData.level = level;
            runData.lives = lives;
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

        public void OnShutdown()
        {
            // Save everything before we close the game.

            // Write stuff to files.
            File.WriteAllBytesAsync(savesPath + runFName, runData.ToBinary(runData));
            File.WriteAllBytes(savesPath + progressionFName, progression.ToBinary(progression));
        }

        public void OnStartup()
        {
            // Ensure saves exists, so we dont get DirectoryNotFounds.
            Directory.CreateDirectory(savesPath);

            if (File.Exists(savesPath+runFName)) { runData = runData.FromBinary(File.ReadAllBytes(savesPath + runFName)); }
            if (File.Exists(savesPath+progressionFName)) { progression = progression.FromBinary(File.ReadAllBytes(savesPath + progressionFName)); }
        }
    }
}
