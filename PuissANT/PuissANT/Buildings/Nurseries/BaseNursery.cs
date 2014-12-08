using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PuissANT.Actors.Ants;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PuissANT.Buildings
{
    abstract class BaseNursery : BaseBuilding
    {
        protected Ant _unit;
        protected int _productionTime; //milliseconds to produce 1 ant
        protected int _currentProductionTime;

        /// <summary>
        /// The production rate of the nursery's units 
        /// </summary>
        protected int _productionRate;

        /// <summary>
        /// Keeps track of the last known number of Nurseries of the same type.
        /// Each Nursery will need a static variable to keep track of the current number.
        /// </summary>
        protected int _previousNurseryCount;

        public BaseNursery(
            Point position, Texture2D texture, long buildTime, double repairRate, 
            int baseProductionTime, int totalHealth, short maxBuilders, 
            int currentNurseryCount, double builderProductionFactor = 1.0)
        :base(position, texture, buildTime, repairRate, totalHealth, maxBuilders, builderProductionFactor)
        {
            _productionTime = _currentProductionTime = baseProductionTime;
            _previousNurseryCount = currentNurseryCount;

            _productionRate = 1;
        }

        /// <summary>
        /// Will be used to calculate the production rate based on the 
        /// number of other nurseries that are available.
        /// </summary>
        protected void CalculateProductionRate(int currentNurseryCount)
        {
            switch (currentNurseryCount)
            {
                case 2:
                    _productionRate += 3;
                    break;

                case 3:
                    _productionRate += 2;
                    break;

                default:
                    _productionRate += 1;
                    break;
            }

            _previousNurseryCount = currentNurseryCount;
        }

        public abstract void Update(GameTime gameTime);

        protected abstract void SpawnAnt();

        protected abstract void KillBuilders();

        public abstract bool AddBuilder();

        public abstract void Draw(SpriteBatch spriteBatch, Rectangle gameWindow); 
    }
}
