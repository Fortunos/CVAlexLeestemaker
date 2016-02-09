using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    static class Methods
    {
        //["Spawner", "Road", "Fork","Crossroad"];

        public static bool CheckValidConnections(SimControl s)
         {
             s.simulationMap.CreateMap();
            foreach (Tile t in s.simulationMap.GetMap())
             {
                foreach (int direction in t.Directions)
                {
                    if (s.simulationMap.GetSurroundingTiles(t.position)[direction - 1] == null)
                     {
                         Tile OtherTile = s.simulationMap.GetSurroundingTiles(t.position)[direction - 1];
                        if (OtherTile == null || !OtherTile.doesConnect(direction))
                            return false;
                        return false;
                     }
                }
             }
             return true;
         }

        public static bool TileConnectionisValid(SimControl simcontrol, Tile currentBuildTile,Point tilePosition)
        {
            if (currentBuildTile.name == "Crossroad" || currentBuildTile.name == "Fork")
            {
                foreach (Tile t in simcontrol.simulationMap.GetSurroundingTiles(new Point((tilePosition.X/100)*100,(tilePosition.Y/100)*100)))
                {
                    if (t != null && (t.name.Equals("Fork") || t.name.Equals("Crossroad")))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /*controleert of de tile een rechte weg is en checkt of de weg naar de goede kant doorloopt zodat je een hele weg kunt 
         * maken door rechtdoor te slepen. Hierdoor kun je alleen rechte wegen door slepen op de kaart aanbrengen. Dit verhoogt 
         * het gebruiksgemak omdat het wegen leggen zo een stuk sneller gaat.
        */
        public static bool TileIsStraight(SimControl s, Point mouseDown, Point mousePoint)
        {
            if (s.currentBuildTile.name == "Road" && s.state == "building")
            {
                Road tile = (Road)s.currentBuildTile;
                if ((tile.StartDirection + tile.EndDirection) % 2 == 0)
                {
                    if (tile.StartDirection == 2 && mouseDown.Y < mousePoint.Y && mouseDown.Y + 100 > mousePoint.Y)
                        return true;
                    if (tile.StartDirection == 1 && mouseDown.X < mousePoint.X && mouseDown.X + 100 > mousePoint.X)
                        return true;
                }
            }
            return false;
        }
        //methode maakt een kopie van de huidige tile die net getekend is, zodat dezelfde tile nog een keer getekend kan worden.
        public static Tile CopyCurrentTile(SimControl s,Tile startTile)
        {
            Tile tile;
            string tileName = startTile.name;
            switch (tileName)
            {
                case "Spawner": Spawner currentSpawnerTile = (Spawner)startTile;
                    tile = new Spawner(s, currentSpawnerTile.Direction);
                    break;
                case "Crossroad": tile = new Crossroad(s);
                    break;
                case "Road": Road currentRoadTile = (Road)startTile;
                    tile = new Road(currentRoadTile.StartDirection, currentRoadTile.EndDirection);
                    break;
                case "Fork": Fork currentForkTile = (Fork)startTile;
                    tile = new Fork(s, currentForkTile.NotDirection);
                    break;
                default: tile = new Crossroad(s);
                    break;
            }
            return tile;
        }
    }
}
