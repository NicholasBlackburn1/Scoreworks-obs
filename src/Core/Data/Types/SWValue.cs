

namespace NEP.Scoreworks.Core.Data
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

        // normal kill score 
        private static int scoreuwu = 0;
        private static int lastscorebeforeautoureset = 0;

        // headshot kill score 
        private static int headhotuwu = 0;
        private static int lastheadshotbeforereset = 0;

        // midair kill
        private static int midairkill = 0;
        private static int midairlastkill=0;


        // this shoudl return data from speififed type
        public static void getKills(SWValue value, Web_server server)
        {
            
            if(value.scoreType == Data.SWScoreType.SW_SCORE_KILL) {
                scoreuwu += 1;
                lastscorebeforeautoureset += 1;

                MelonLoader.MelonLogger.Msg("kills "+ scoreuwu);
                MelonLoader.MelonLogger.Msg("total kills " + lastscorebeforeautoureset);
                // sends the death tp flaks
                server.sendkillsAsync(server.deaths(lastscorebeforeautoureset), "setkills");

            }

             // Headshot 
             if(value.scoreType == Data.SWScoreType.SW_SCORE_HEADSHOT) {
                headhotuwu += 1;
                lastheadshotbeforereset += 1;

                MelonLoader.MelonLogger.Msg("Hedshot "+ headhotuwu);
                MelonLoader.MelonLogger.Msg("total Hedshots " + lastheadshotbeforereset);
                // sends the death tp flaks
                server.sendkillsAsync(server.deaths(lastheadshotbeforereset), "setHeadshot");

            }

               // MidAir kills 
             if(value.scoreType == Data.SWScoreType.SW_SCORE_MIDAIR_KILL) {
                midairkill += 1;
                midairlastkill += 1;

                MelonLoader.MelonLogger.Msg("Midair kills "+ midairkill);
                MelonLoader.MelonLogger.Msg("total Midair  " + midairlastkill);
                // sends the death tp flaks
                server.sendkillsAsync(server.deaths(midairlastkill), "setHeadshot");

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
              
            }
         
        }

        public void DestroyScore(SWValue value)
        {
            if (value.type == SWValueType.Score)
            {
                API.OnScorePreRemoved?.Invoke(value);
                API.OnScoreRemoved?.Invoke(value);
                MelonLoader.MelonLogger.Msg("total kills before reset from last score " + lastscorebeforeautoureset + " " + " score from reset" + scoreuwu);
                scoreuwu = 0;
                lastscorebeforeautoureset = 0;
                server.sendkillsAsync(server.deaths(lastscorebeforeautoureset), "setkills");
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