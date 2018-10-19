namespace CodeRoyale
{
    public enum UnitType
    {
        UNSET,
        QUEEN,
        KNIGHT,
        ARCHER,
        GIANT
    }

    public enum BuildingType
    {
        NONE,
        MINE,
        TOWER,
        BARRACKS_KNIGHT,
        BARRACKS_ARCHER,
        BARRACKS_GIANT
    }

    public enum AllianceType
    {
        NEUTRAL,
        FRIENDLY,
        HOSTILE
    }

    public enum EntityType
    {
        LOCATION,
        UNIT
    }
}