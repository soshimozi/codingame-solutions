using System;
using Utility;

namespace CodeRoyale
{
    public class Unit : Entity
    {
        public Unit(int _x, int _y, AllianceType _allianceType, UnitType _unitType, int _health = -1)
        {
            m_position = new Vector2(_x, _y);
            m_allianceType = _allianceType;
            m_unitType = _unitType;
            
            switch (_unitType)
            {
                case UnitType.QUEEN:
                    m_radius = 30;
                    m_movementSpeed = 60;
                    m_health = new Random().Next(25,100);
                    m_weight = 100;
                    break;
                case UnitType.KNIGHT:
                    m_radius = 20;
                    m_movementSpeed = 100;
                    m_health = 25;
                    m_weight = 4;
                    break;
                case UnitType.ARCHER:
                    m_radius = 25;
                    m_movementSpeed = 75;
                    m_health = 45;
                    m_weight = 9;
                    break;
                case UnitType.GIANT:
                    m_radius = 40;
                    m_movementSpeed = 50;
                    m_health = 200;
                    m_weight = 20;
                    break;
            }

            if (_health != -1) { m_health = _health; }
        }


        public AllianceType m_allianceType;
        public UnitType m_unitType;
        public int m_health;
        public int m_movementSpeed;
    }
}