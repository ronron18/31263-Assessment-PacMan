using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directions
{
    // Player animation directions are also the same as this.
    public enum PlayerAnimDirections {
        up = 0,
        right = 1,
        down = 2,
        left = 3
    }

    // This is just for the ghosts, it contains the directions for ghost animation 
    public enum GhostAnimDirections {
        up = 0,
        down = 1,
        side = 2
    }
}
