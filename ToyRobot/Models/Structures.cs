using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobot.Models
{
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH, 
        WEST
    };

    public enum ActionEnum
    {
        PLACE,
        MOVE,
        LEFT,
        RIGHT,
        REPORT
    };

    public enum Detail
    {
        X,
        Y,
        FACE,
        MOVECOUNTER,
        LASTACTION,
        LASTACTIONACCEPTED
    };

}
