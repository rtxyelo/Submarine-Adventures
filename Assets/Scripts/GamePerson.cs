using UnityEngine;

/// <summary>
/// Represents the base class of player and enemy classes with movement and rotation parameters.
/// </summary>
public class GamePerson : MonoBehaviour
{
    /// <summary>
    /// The movement speed of the game person.
    /// </summary>
    public float MovementSpeed = 1.4f;

    /// <summary>
    /// The rotation speed of the game person.
    /// </summary>
    public float RotationSpeed = 0.023f;
}
