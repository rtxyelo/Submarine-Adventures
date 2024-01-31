using System;
using UnityEngine;

/// <summary>
/// Calculates the center position of a given Vector3 position on the floor.
/// </summary>
public class TailCenterCalculate : MonoBehaviour
{
    /// <summary>
    /// Returns the center position on the floor for the given Vector3 position.
    /// </summary>
    /// <param name="pos">The position to be centered.</param>
    /// <returns>The centered position on the floor.</returns>
    public Vector2 CenteringPosition(Vector3 pos)
    {
        Vector2 floor = FloorPosition(pos);
        floor.x += 0.5f;
        floor.y += 0.5f;

        return floor;
    }

    /// <summary>
    /// Returns the floor position (rounded down) for the given Vector3 position.
    /// </summary>
    /// <param name="pos">The position to be floored.</param>
    /// <returns>The floored position.</returns>
    private Vector2 FloorPosition(Vector3 pos)
    {
        Vector2 res = new Vector2();
        res.x = (float)Math.Floor(pos.x);
        res.y = (float)Math.Floor(pos.z);
        return res;
    }
}
