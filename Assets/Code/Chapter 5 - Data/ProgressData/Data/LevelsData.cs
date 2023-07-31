using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpinalPlay
{
    public class LevelsData : DataBase
    {
        public int CurrentLevelId;
        public int maxReachedLevel;
        public int lastScore;
        public Dictionary<int, int> levelIDToScore;

        public LevelsData()
        {
            Reset();
        }

        public int IDToScore(int levelId)
        {
            if (levelIDToScore.ContainsKey(levelId))
            {
                return levelIDToScore[levelId];
            }
            return 0;
        }

        public override void Reset()
        {
            CurrentLevelId = 0;
            maxReachedLevel = 0;
            lastScore = 0;
            levelIDToScore = new Dictionary<int, int>();
        }
    }
}