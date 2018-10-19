using System;

namespace CodeRoyale
{
    public static class HeuristicData
    {
        public const float MAP_MAX_DISTANCE = 1200.0f;
        public static float MAXIMUM_LEVEL = 5;

        public static int MAX_MINE_UPGRADE_LEVEL = 5;
        public static int TURNS_FOR_MIDGAME = 40;
        public static float QUEEN_IS_RUSHING_DISTANCE = 100f;

        internal static void Set(string[] _args)
        {
            if (_args.Length < 9) { return; }
            THREAT_RANGE = int.Parse(_args[0]);
            MINE_RANGE = int.Parse(_args[1]);
            LOCATION_DISTANCE_TO_QUEEN_RATING = int.Parse(_args[2]);
            UPGRADE_RATING = int.Parse(_args[3]);
            IDEAL_IS_DESIRED_RATING = int.Parse(_args[4]);
            ENEMIES_IN_RANGE_RATING = int.Parse(_args[5]);
            ENEMY_LOCATION_WITHIN_HUB_RATING = int.Parse(_args[6]);
            LOCATION_UNOCCUPIED_RATING = int.Parse(_args[7]);
            WITHIN_HUB_RANGE_RATING = int.Parse(_args[8]);
        }

        public static float IDEAL_TOWER_TO_MINE_RATIO = 2f;
        public static float MAX_ENEMIES_IN_RANGE = 10;
        public const float HUB_RADIUS = 800.0f;

      
        public static float THREAT_RANGE = 400;
        public static float MINE_RANGE = 400;
        public static float LOCATION_DISTANCE_TO_QUEEN_RATING = 35000;
        public static float UPGRADE_RATING = 100;
        public static float IDEAL_IS_DESIRED_RATING = 10000;
        public static float ENEMIES_IN_RANGE_RATING = 100;
        public static float ENEMY_LOCATION_WITHIN_HUB_RATING = 30;
        public static float LOCATION_UNOCCUPIED_RATING = 15000;
        public static float WITHIN_HUB_RANGE_RATING = 25000;
    }
} 