using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Buildings
{
    public enum BuildingState
    {
        BUILDING,
        COMPLETE,
        DESTROYED
    }

    abstract class BaseBuilding
    {
        /// <summary>
        /// Max number of builders that can work on this building
        /// </summary>
        protected short _maxBuilders;
        
        /// <summary>
        /// Current number of builders working on this building
        /// </summary>
        protected short _currentBuilders;

        /// <summary>
        /// Represents how fast production increases with the
        /// addition of another builder. Reduces the current
        /// build time by this factor for each ant for each 
        /// millisecond
        /// </summary>
        protected double _builderProductionFactor;

        /// <summary>
        /// Time it takes to build this structure with 1 builder
        /// </summary>
        protected long _buildTime;

        /// <summary>
        /// How much progress has been made on the building so far
        /// </summary>
        protected long _currentBuildTime;

        /// <summary>
        /// The rate that workers can repair a building. This
        /// rate represents the amount of hp repaired per millisecond.
        /// Will also scale with the builderProductionFactor.
        /// </summary>
        protected int _repairRate;

        /// <summary>
        /// the total health of the building
        /// </summary>
        protected int _totalHealth;

        /// <summary>
        /// The current health of the building
        /// </summary>
        protected int _currentHealth;

        protected bool _tookDamage;

        protected BuildingState _buildingState;

        /// <summary>
        /// The Position of the upper left corner
        /// </summary>
        protected Point _buildingPosition;

        protected Texture2D _texture;

        public short MaxBuilders { get { return _maxBuilders; } }
        public short CurrentBuilders { get { return _currentBuilders; } }

        public long TotalBuildTime { get { return _buildTime; } }
        public long RemainingBuildTime { get { return _currentBuildTime; } }
        public int RepairRate { get { return _repairRate; } }
        public double PercentOfBuildCompleted { get { return ((TotalBuildTime - RemainingBuildTime) / (double)TotalBuildTime); } }
        
        public int TotalHealth { get { return _totalHealth; } }
        public int RemainingHealth { get { return _currentHealth; } }
        public double PercentOfHealth { get { return (RemainingHealth / TotalHealth); } }

        public Point Position { get { return _buildingPosition; } }
        public Point Center { get { return new Point(_buildingPosition.X - _texture.Width / 2, _buildingPosition.Y - _texture.Height / 2); } }

        public BuildingState State { get { return _buildingState; } }

        public BaseBuilding(
            Point position, Texture2D texture, long buildTime, int repairRate, 
            int totalHealth, short maxBuilders, double builderProductionFactor = 1.0)
        {
            _buildingPosition = position;
            _texture = texture;
            _buildTime = _currentBuildTime = buildTime;
            _repairRate = repairRate;
            _totalHealth = _currentHealth = totalHealth;
            _maxBuilders = maxBuilders;
            _currentBuilders = 0;
            _builderProductionFactor = builderProductionFactor;
            _tookDamage = false;
            _buildingState = BuildingState.BUILDING;
        }

        public void Attack(int damage)
        {
            _currentHealth -= damage;
            _tookDamage = true;
        }
    }
}
