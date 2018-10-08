using Utility;

namespace CodeRoyale
{
    struct Unit
    {
        public Unit(int _x, int _y, AllianceType _allianceType, UnitType _unitType, int _health)
        {
            m_position = new Vector2(_x, _y);
            m_allianceType = _allianceType;
            m_unitType = _unitType;
            m_health = _health;
        }
        public Vector2 m_position;
        public AllianceType m_allianceType;
        public UnitType m_unitType;
        public int m_health;
    }
}