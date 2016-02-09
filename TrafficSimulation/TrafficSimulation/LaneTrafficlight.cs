using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    class LaneTrafficlight
    {
        SimControl simcontrol;
        List<Trafficlight> trafficlights;
        Tile road;
        int Lanes, direction;

        public LaneTrafficlight(SimControl sim, Tile road, int Direction, int Lanes)
        {
            this.simcontrol = sim;
            trafficlights = new List<Trafficlight>();
            this.road = road;
            this.Lanes = Lanes;
            this.direction = Direction;

            switch (Lanes)
            {
                //all options for different amounts of lanes, done like this to also give them their proper lanetype
                case 1:
                    CreateSingleLane();
                    break;
                case 2:
                    CreateDoubleLane();
                    break;
                case 3:
                    CreateTripleLane();
                    break;
            }
        }

        //one lane
        public void CreateSingleLane()
        {
            for (int i = 0; i < Lanes; i++)
            {
                Point Position = GetPosition(direction, i);
                trafficlights.Add(new Trafficlight(simcontrol, road, Position, 1));
            }
        }

        //two lanes
        public void CreateDoubleLane()
        {
            for (int i = 0; i < Lanes; i++)
            {
                Point Position = GetPosition(direction, i);
                switch (i)
                {
                    case 0:
                        trafficlights.Add(new Trafficlight(simcontrol, road, Position, 5));
                        break;
                    case 1:
                        trafficlights.Add(new Trafficlight(simcontrol, road, Position, 2));
                        break;
                }
            }
        }

        //three lanes
        public void CreateTripleLane()
        {
            for (int i = 0; i < Lanes; i++)
            {
                Point Position = GetPosition(direction, i);
                switch (i)
                {
                    case 0:
                        trafficlights.Add(new Trafficlight(simcontrol, road, Position, 5));
                        break;
                    case 1:
                        trafficlights.Add(new Trafficlight(simcontrol, road, Position, 4));
                        break;
                    case 2:
                        trafficlights.Add(new Trafficlight(simcontrol, road, Position, 3));
                        break;
                }
            }
        }

        //method called to change the color of a trafficlight with certain given parameters
        public void ChangeColor(Color kleur, int LaneType)
        {
            for (int i = 0; i < trafficlights.Count; i++)
            {
                Trafficlight Light = (Trafficlight)trafficlights[i];
                if (LaneType == Light.LaneType)
                {
                    Light.UpdateColor(kleur);
                    UpdateTileAccess(i, kleur);
                }
            }
        }

        //calculate the actual position of the trafficlight on the map
        private Point GetPosition(int Direction, int NumberTrafficlight)
        {
            Point Position = new Point(0, 0);

            switch (Direction)
            {
                case 0:
                    Position.X = 37 - (NumberTrafficlight * 16);
                    Position.Y = 1;
                    break;
                case 1:
                    Position.X = 89;
                    Position.Y = 37 - (NumberTrafficlight * 16);
                    break;
                case 2:
                    Position.X = 53 + (NumberTrafficlight * 16);
                    Position.Y = 89;
                    break;
                case 3:
                    Position.X = 1;
                    Position.Y = 53 + (NumberTrafficlight * 16);
                    break;
            }

            return Position;
        }

        public void ChangeValues(Point position)
        {
            foreach (Trafficlight light in trafficlights)
            {
                light.DrawTrafficlight(Color.Red);
            }
        }

        private void UpdateTileAccess(int lane, Color kleur)
        {
            Tile Othertile = simcontrol.simulationMap.GetSurroundingTiles(road.position)[direction];
            if (Othertile != null)
            {
                int tileDirection = (direction + 2) % 4 + 1;
                if (kleur == Color.Green)
                    Othertile.Access[tileDirection - 1, lane] = true;
               else
                    Othertile.Access[tileDirection - 1, lane] = false;
            }
        }
    }
}