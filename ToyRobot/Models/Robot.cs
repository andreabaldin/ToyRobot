using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyRobot.Models
{
    public class Robot
    {
        private int NumCols { get; set; }
        private int NumRows { get; set; }

        public int? X { get; set; }                                                                 // ... current X position
        public int? Y { get; set; }                                                                 // ... current Y position
        public Direction? Face { get; set; }                                                        // ... current orientation (0-N / 1-E / 2-S / 3-W)
        public int MoveCounter { get; set; }
        public string LastAction { get; set; }
        public bool LastActionAccepted { get; set; }

        public Direction _F;                                                                        // ... private attribute

        public Robot(int nC = 5, int nR = 5)
        {
            NumCols = nC;
            NumRows = nR;
        }

        #region Movements

        public bool Place(int x = -1, int y = -1, string face = null)
        {
            this.LastActionAccepted = CheckValidity(x, y, face);                                    // ... Test valid placing
            if (this.LastActionAccepted)
            {
                this.X = x;
                this.Y = y;
                this.Face = _F;
                this.LastAction = string.Format("PLACE {0}, {1}, {2}", X, Y, Face);
                this.MoveCounter++;
            }
            else
                this.LastAction = string.Format("PLACE command to {0}, {1}, {2} ignored !", x, y, face);

            return LastActionAccepted;
        }


        public bool Move()
        {
            if (this.IsValid())
            {
                // ... get current coordinates
                int x = (int)this.X;
                int y = (int)this.Y;

                // ... compute requested move
                switch (this.Face)
                {
                    case Direction.NORTH: y++; break;
                    case Direction.SOUTH: y--; break;
                    case Direction.EAST: x++; break;
                    case Direction.WEST: x--; break;
                    default: break;                                                                 // ... this is impossible !!!                       
                }

                this.LastActionAccepted = CheckValidity(x, y, this.Face.ToString());                // ... Check move correctness.  Notice current provisioned Face
                if (this.LastActionAccepted)
                {
                    this.LastAction = string.Format("MOVE from {0},{1} to {2},{3}  still pointing: {4}", this.X, this.Y, x, y, this.Face);
                    this.X = x;
                    this.Y = y;
                    this.MoveCounter++;
                }
                else
                    this.LastAction = string.Format("IMPOSSIBLE MOVE pointing: {2} to an invalid position: {0},{1}", x, y, this.Face);

                return LastActionAccepted;
            }

            this.LastAction = "MOVE command impossible. Position undefined...";
            return false;

        }


        public bool Left()
        {
            if (this.IsValid())
            {
                // ... compute requested move
                int ToyRobot = ((int)this.Face + 3) % 4;
                this._F = (Direction)ToyRobot;

                this.LastAction = string.Format("LEFT from {0},{1},{2} to: {3})", this.X, this.Y, this.Face, _F);
                this.Face = _F;
                this.MoveCounter++;
                this.LastActionAccepted = true;
                return true;
            }

            this.LastAction = "LEFT command impossible. Position undefined...";
            return false;
        }


        public bool Right()
        {
            if (this.IsValid())
            {
                // ... compute requested move
                int ToyRobot = ((int)this.Face + 1) % 4;
                this._F = (Direction)ToyRobot;

                this.LastAction = string.Format("RIGHT from {0},{1},{2} to: {3})", this.X, this.Y, this.Face, _F);
                this.Face = _F;
                this.MoveCounter++;
                this.LastActionAccepted = true;
                return true;
            }

            this.LastAction = "RIGHT command impossible. Position undefined...";
            return false;
        }


        public string Report()
        {
            this.LastAction = null;

            if (this.IsValid())
                return string.Format("CURRENT POSITION is {0},{1},{2}", this.X, this.Y, this.Face, _F);
            else
                return "Position undefined. PLACE X,Y,F required...";
        }


        #endregion Movements


        #region Public Methods

        public bool CheckValidity(int? x, int? y, string face)
        {
            // ... if a number then refactor to a valid value
            int _i;
            if (int.TryParse(face, out _i))
                face = (Math.Abs(_i) % 4).ToString();
                
            bool validFace = Enum.TryParse(face, true, out _F);
            return (x >= 0 && x < NumCols) &&
                        (y >= 0 && y < NumRows) &&
                             validFace;
        }

        public bool IsValid()
        {
            return CheckValidity(this.X, this.Y, this.Face.ToString());
        }

        #endregion Public Methods

    }
}
