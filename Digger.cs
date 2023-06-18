using System.Windows.Forms;

namespace Digger
{
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand { };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Player : ICreature
    {
        public static int xPos = 0;
        public static int yPos = 0;

        private bool CanMove(int x, int y)
        {
            return Game.Map[x, y] == null || Game.Map[x, y].GetImageFileName() != "Sack.png";
        }

        public CreatureCommand Act(int x, int y)
        {
            xPos = x;
            yPos = y;

            Keys key = Game.KeyPressed;

            switch (key)
            {
                case Keys.Down:
                    if (y < Game.MapHeight - 1 && CanMove(x, y + 1)) return new CreatureCommand { DeltaX = 0, DeltaY = 1 };
                    break;

                case Keys.Up:
                    if (y >= 1 && CanMove(x, y - 1)) return new CreatureCommand { DeltaX = 0, DeltaY = -1 };
                    break;

                case Keys.Right:
                    if (x < Game.MapWidth - 1 && CanMove(x + 1, y)) return new CreatureCommand { DeltaX = 1, DeltaY = 0 };
                    break;

                case Keys.Left:
                    if (x >= 1 && CanMove(x - 1, y)) return new CreatureCommand { DeltaX = -1, DeltaY = 0 };
                    break;
            }

            return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetImageFileName() == "Gold.png")
                Game.Scores += 10;

            return conflictedObject.GetImageFileName() == "Sack.png" || conflictedObject.GetImageFileName() == "Monster.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    public class Sack : ICreature
    {
        private int _falling = 0;

        public CreatureCommand Act(int x, int y)
        {
            int below = Game.MapHeight - 1;

            while (y != below)
            {
                if (Game.Map[x, y + 1] == null || ((Game.Map[x, y + 1].GetImageFileName() == "Digger.png" || Game.Map[x, y + 1].GetImageFileName() == "Monster.png") && _falling > 0))
                {
                    _falling++;
                    return new CreatureCommand { DeltaX = 0, DeltaY = 1 };
                }
                else if (_falling > 1)
                    return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
                else
                {
                    _falling = 0;
                    return new CreatureCommand { };
                }
            }

            if (_falling > 1)
                return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
            else
            {
                _falling = 0;
                return new CreatureCommand { };
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand { };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            int xTo = 0;
            int yTo = 0;

            if (FindPlayer())
            {
                if (Player.xPos == x)
                {
                    if (Player.yPos < y) yTo = -1;
                    else if (Player.yPos > y) yTo = 1;
                }
                else if (Player.yPos == y)
                {
                    if (Player.xPos < x) xTo = -1;
                    else if (Player.xPos > x) xTo = 1;
                }
                else
                {
                    if (Player.xPos < x) xTo = -1;
                    else if (Player.xPos > x) xTo = 1;
                }
            }
            else
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };

            if (!(x + xTo >= 0 && x + xTo < Game.MapWidth && y + yTo >= 0 && y + yTo < Game.MapHeight))
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };

            var map = Game.Map[x + xTo, y + yTo];
            if (map != null && (map.ToString() == "Digger.Terrain" || map.ToString() == "Digger.Sack" || map.ToString() == "Digger.Monster"))
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };

            return new CreatureCommand() { DeltaX = xTo, DeltaY = yTo };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            string fuck = conflictedObject.GetImageFileName();
            return fuck == "Sack.png" || fuck == "Monster.png";
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }

        static private bool FindPlayer()
        {
            for (int i = 0; i < Game.MapWidth; i++)
            {
                for (int j = 0; j < Game.MapHeight; j++)
                {
                    if (Game.Map[i, j] != null && Game.Map[i, j].GetImageFileName() == "Digger.png")
                    {
                        Player.xPos = i;
                        Player.yPos = j;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}