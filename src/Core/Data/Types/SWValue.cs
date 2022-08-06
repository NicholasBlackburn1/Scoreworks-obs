﻿namespace NEP.Scoreworks.Core.Data
{
    [System.Serializable]
    public class SWValue
    {
        public SWValue(SWScoreType scoreType)
        {
            var dictionary = DataManager.scoreValues;
            SWValueTemplate valueTemplate = dictionary[scoreType];

            this.scoreType = scoreType;

            name = valueTemplate.name;
            score = valueTemplate.score;
            type = SWValueType.Score;
            stack = valueTemplate.stack;

            duration = maxDuration;

            CreateScore(this);
        }

        public SWValue(SWMultiplierType multiplierType)
        {
            var dictionary = DataManager.multiplierValues;
            SWValueTemplate valueTemplate = dictionary[multiplierType];

            this.multiplierType = multiplierType;

            name = valueTemplate.name;
            score = valueTemplate.score;
            multiplier = valueTemplate.multiplier;
            maxDuration = valueTemplate.maxDuration;
            duration = maxDuration;
            type = SWValueType.Multiplier;
            stack = valueTemplate.stack;

            CreateMultiplier(this);
        }

        private static int scoreuwu = 0;
        private static int lastscorebeforeautoureset = 0;
        // this shoudl return data from speififed type
        public static void getKills(SWValue value)
        {
            
            if(value.scoreType == Data.SWScoreType.SW_SCORE_KILL) {
                scoreuwu += 1;

                MelonLoader.MelonLogger.Msg("kills "+ scoreuwu);
                MelonLoader.MelonLogger.Msg("total kills " + lastscorebeforeautoureset);

            }
        }

        public string name;
        public int score;
        public float multiplier;
        public bool stack;

        public SWScoreType scoreType;
        public SWMultiplierType multiplierType;

        public bool cleaned = false;

        public SWValueType type;

        public float maxDuration = 5f;
        public float duration;

        public void CreateScore(SWValue value)
        {
            if (value.type == SWValueType.Score)
            {
                API.OnScorePreAdded?.Invoke(value);
                API.OnScoreAdded?.Invoke(value);
                lastscorebeforeautoureset = scoreuwu;
            }
         
        }

        public void DestroyScore(SWValue value)
        {
            if (value.type == SWValueType.Score)
            {
                API.OnScorePreRemoved?.Invoke(value);
                API.OnScoreRemoved?.Invoke(value);
                lastscorebeforeautoureset += scoreuwu;
                MelonLoader.MelonLogger.Msg("total kills before reset from last score " + lastscorebeforeautoureset + " " + " score from reset" + scoreuwu);
                scoreuwu = 0;
            }
        }

        public void CreateMultiplier(SWValue value)
        {
            if (value.type == SWValueType.Multiplier)
            {
                API.OnMultiplierPreAdded?.Invoke(value);
                API.OnMultiplierAdded?.Invoke(value);
                API.OnMultiplierChanged?.Invoke(value);
            }
        }

        public void DestroyMultiplier(SWValue value)
        {
            if (value.type == SWValueType.Multiplier)
            {
                API.OnMultiplierPreRemoved?.Invoke(value);
                API.OnMultiplierRemoved?.Invoke(value);
                API.OnMultiplierChanged?.Invoke(value);
            }
        }

        public void Update()
        {
            duration -= UnityEngine.Time.deltaTime;

            if (duration <= 0f)
            {
                if (type == SWValueType.Score)
                {
                    DestroyScore(this);
                }
                else if (type == SWValueType.Multiplier)
                {
                    DestroyMultiplier(this);
                }
            }
        }

        public void ResetDuration()
        {
            duration = maxDuration;
        }
    }
}