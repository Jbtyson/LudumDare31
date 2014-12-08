using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Actors.Ants;
using PuissANT.Actors;

namespace PuissANT.Buildings.Nurseries
{
    class WorkerNursery : BaseNursery
    {

        private static int _currentWorkerNurseryCount = 0;

        private double percentPerLine;
        private double previousDrawnPercent;
        private int nextLineNumberToDraw;
        private Color[] reusableColorBuffer;
        private Color[] textureColorBuffer;

        //Defaults
        private const long d_buildTime = 5000;
        private const int d_repairRate = 2;
        private const int d_baseProductionTime = 1000;
        private const int d_totalHealth = 3000;
        private const short d_maxBuilders = 3;
        private const double d_builderProductionFactor = 1.0;

        public WorkerNursery( 
            Point position, Texture2D texture)
        :base(position, texture, d_buildTime, d_repairRate, d_baseProductionTime, 
            d_totalHealth, d_maxBuilders, ++_currentWorkerNurseryCount, d_builderProductionFactor)
        {
            percentPerLine = 100 / _texture.Height;
            previousDrawnPercent = 0.0d;
            nextLineNumberToDraw = _texture.Height - 1;
            reusableColorBuffer = new Color[_texture.Width];
            textureColorBuffer = new Color[_texture.Width * _texture.Height];
            _texture.GetData<Color>(textureColorBuffer);

            Random r = new Random();
            Point randomVariance = new Point(
                r.Next(_texture.Width) - _texture.Width / 2,
                r.Next(_texture.Height) - _texture.Height / 2);

            _unit = new WorkerAnt(_buildingPosition + randomVariance, 1,1);
        }

        #region Debug Methods

        private static WorkerNursery debugNursery;
        public static WorkerNursery Debug_Spawn(Point position, Texture2D testTex)
        {
            Color[] colorBuf = new Color[testTex.Width * testTex.Height];

            for (int i = 0; i < colorBuf.Length; i++)
            {
                if (i < colorBuf.Length * .35 || i > colorBuf.Length * .65)
                    colorBuf[i] = new Color(0, 0, 0, 0);
                else
                    colorBuf[i] = Color.Black;
            }

            testTex.SetData<Color>(colorBuf);

            debugNursery = new WorkerNursery(position, testTex);

            return debugNursery;
        }

        public void Debug_InitializeTest()
        {
            _currentBuilders = 1;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {

            if (_currentWorkerNurseryCount != _previousNurseryCount)
                CalculateProductionRate(_currentWorkerNurseryCount);

            double millisecondsPassed = gameTime.ElapsedGameTime.TotalMilliseconds;

            switch (_buildingState)
            {
                case BuildingState.BUILDING:
                    Update_BuildingPhase((int)millisecondsPassed);
                    break;

                case BuildingState.COMPLETE:
                    Update_CompletePhase((int)millisecondsPassed);
                    break;

                case BuildingState.DESTROYED:
                    Update_DestroyedPhase((int)millisecondsPassed);
                    break;
            }

        }

        #region Update Methods

        private void Update_BuildingPhase(int millisecondsPassed)
        {
            _currentBuildTime -= (long)(_builderProductionFactor * _currentBuilders) * millisecondsPassed;

            if (_currentBuildTime <= 0)
            {
                _buildingState = BuildingState.COMPLETE;
                _currentBuildTime = _buildTime;
                return;
            }

            if (_currentBuildTime <= 0)
            {
                _currentBuildTime = 0;
                KillBuilders();
            }
        }

        private void Update_CompletePhase(int millisecondsPassed)
        {
            _currentProductionTime -= millisecondsPassed * _productionRate;

            if (_currentProductionTime <= 0)
            {
                _currentProductionTime = _productionTime;
                SpawnAnt();
            }



            if (_currentHealth <= 0)
            {
                _buildingState = BuildingState.DESTROYED;
                _currentHealth = 0;
            }
        }

        private void Update_DestroyedPhase(int millisecondsPassed)
        {
            _currentHealth += millisecondsPassed * _repairRate;

            if (_currentHealth >= _totalHealth)
            {
                _currentHealth = _totalHealth;
                _buildingState = BuildingState.COMPLETE;
            }
        }

        #endregion 

        public override void Draw(SpriteBatch spriteBatch, Rectangle gameWindow)
        {
            Point gameWindowOffset = new Point(gameWindow.X, gameWindow.Y);

            switch (_buildingState)
            {
                case BuildingState.BUILDING:
                    Draw_BuildingPhase();
                    break;
                case BuildingState.COMPLETE:
                    Draw_CompletedPhase(spriteBatch, gameWindowOffset);
                    break;
                case BuildingState.DESTROYED:
                    Draw_DestroyedPhase(spriteBatch, gameWindowOffset);
                    break;
            }
        }

        #region Draw Methods
        
        private void Draw_BuildingPhase()
        {
            double percentComplete = this.PercentOfBuildCompleted;

            if ((percentComplete - previousDrawnPercent)*100 >= percentPerLine)
            {
                //Draw new Line
                
                //Fill Color Buffer
                for (int i = 0; i < _texture.Width; i++)
                {
                    reusableColorBuffer[i] = textureColorBuffer[(nextLineNumberToDraw * _texture.Width) + i];
                }

                //Get position
                Rectangle bounds = new Rectangle(_buildingPosition.X, _buildingPosition.Y + nextLineNumberToDraw, _texture.Width, 1);

                Color[] blendedColorBuf = TerrainManager.ClearColorMask(bounds, reusableColorBuffer);

                for (int i = 0; i < _texture.Width; i++)
                {
                    textureColorBuffer[(nextLineNumberToDraw * _texture.Width) + i] = blendedColorBuf[i];
                }

                _texture.SetData<Color>(textureColorBuffer);

                nextLineNumberToDraw--;
                previousDrawnPercent = percentComplete;
            }
        }

        private void Draw_CompletedPhase(SpriteBatch spriteBatch, Point gameWindow)
        {
            //draw texture over position that was cleared out and shade white
            Color tint = Color.White;
            if (_tookDamage)
                tint = Color.Red;

            spriteBatch.Draw(_texture, new Vector2(_buildingPosition.X + gameWindow.X, _buildingPosition.Y + gameWindow.Y), tint);
        }

        private void Draw_DestroyedPhase(SpriteBatch spriteBatch, Point gameWindow)
        {
            //draw texture over position that was cleared out and shade gray
            spriteBatch.Draw(_texture, new Vector2(_buildingPosition.X + gameWindow.X, _buildingPosition.Y + gameWindow.Y), new Color(70, 70, 70, 255));
        }

        #endregion

        protected override void SpawnAnt()
        {
            //ActorManager.Instance.Add(new WorkerAnt(new Point(xpoint,ypoint));
            throw new NotImplementedException();
        }

        protected override void KillBuilders()
        {
            _currentBuilders = 0;
            throw new NotImplementedException();
        }

        protected override bool AddBuilder()
        {
            if (_currentBuilders + 1 <= _maxBuilders)
            {
                _currentBuilders++;
                return true;
            }

            return false;
        }
    }
}
