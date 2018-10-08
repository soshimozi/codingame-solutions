

namespace CodeRoyale
{
    class StrategyManager
    {
        public static void Execute(out string _queenInstructions, out string _trainingInstructions)
        {
            _queenInstructions = "WAIT";
            _trainingInstructions = "TRAIN";

            _queenInstructions = Strategies.GetBasicQueenStrategy();
            _trainingInstructions = Strategies.BuildAllPossibleKnights();
        }
    }
}