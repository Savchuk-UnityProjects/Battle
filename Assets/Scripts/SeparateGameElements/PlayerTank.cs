using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : Tank
{
    private Rigidbody2D ThisRigidbody2D;
    private Vector2 Velocity;

    public Vector3 StartPosition { get; private set; }

    private void Awake()
    {
        Velocity = Vector2.zero;
        ThisRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        void AddStartingVelocity(string nameOfTheButton, Vector2 direction)
        {
            if(Input.GetButton(nameOfTheButton))
            {
                Velocity += direction;
                transform.rotation = Quaternion.FromToRotation(Vector2.up, direction.normalized);
            }
        }
        AddStartingVelocity(ButtonsOfInputManager.Forward, Vector2.up * Speed);
        AddStartingVelocity(ButtonsOfInputManager.Back, Vector2.down * Speed);
        AddStartingVelocity(ButtonsOfInputManager.Left, Vector2.left * Speed);
        AddStartingVelocity(ButtonsOfInputManager.Right, Vector2.right * Speed);
        ThisRigidbody2D.velocity = Velocity;

        StartPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetButtonDown(ButtonsOfInputManager.ShootButton))
        {
            Shoot();
        }

        void MoveTankOnButton(string nameOfTheButton, Vector2 direction, float ZAngleToBeSet)
        {
            if (Input.GetButtonDown(nameOfTheButton))
            {
                Velocity += direction;
                transform.rotation = Quaternion.FromToRotation(Vector2.up, direction.normalized);
            }
            if (Input.GetButtonUp(nameOfTheButton))
            {
                Velocity -= direction;
            }
        }

        MoveTankOnButton(ButtonsOfInputManager.Forward, Vector2.up * Speed, 0);
        MoveTankOnButton(ButtonsOfInputManager.Back, Vector2.down * Speed, 180);
        MoveTankOnButton(ButtonsOfInputManager.Left, Vector2.left * Speed, 90);
        MoveTankOnButton(ButtonsOfInputManager.Right, Vector2.right * Speed, 270);

        ThisRigidbody2D.velocity = Velocity;
    }

    private new void Shoot()
    {
        base.Shoot().GetComponent<Bullet>().BulletType = BulletType.PlayerBullet;
    }
}