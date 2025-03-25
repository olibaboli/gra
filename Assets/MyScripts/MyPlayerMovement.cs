using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//definicja klasy ("formularza") o nazwie "MyPlayerMovement", kt�ra mo�e by� komponentem GameObject'u
public class MyPlayerMovement : MonoBehaviour
{
    //pola publiczne (widoczne w Inspektorze Unity) przechowuj�ce warto�ci liczbowe.
    public float MovementSpeed = 5.0f; //Warto�ci pocz�tkowe - s� u�ywane gdy dodajemy komponent do GameObject'u. P�niej mo�emy te warto�ci edytowa�...
    public float RotationSpeed = 0.5f; //... w Inspektorze Unity
    public float JumpSpeed = 5.0f;
    public float Gravity = -9.81f;

    public bool IsGrounded = false;
    public Vector3 MoveDirection = Vector3.zero;

    //pole prywatne (niewidoczne w Inspektorze Unity) o nazwie "cc"...
    //przechowuj�ce referencj� do obiektu (komponentu) CharacterController
    private CharacterController cc;

    private float yVeclocity = 0;

    private float jumpDelay = 0;

    void Start()
    {
        //pobranie referencji do komponentu CharacterController w tym samym GameObject ...
        //... i zapisanie jej w polu cc
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        jumpDelay += Time.deltaTime;

        IsGrounded = cc.isGrounded;

        transform.Rotate(0, Input.GetAxis("Mouse X") * RotationSpeed, 0);

        var move = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;

        MoveDirection = MovementSpeed * Vector3.ClampMagnitude(move, 1f);

        if (cc.isGrounded && jumpDelay > 0.02f)
        {
            yVeclocity = Gravity;

            if (Input.GetButtonDown("Jump"))
            {
                yVeclocity = JumpSpeed;
                jumpDelay = 0;
            }
        }

        MoveDirection.y = yVeclocity;
    }

    void FixedUpdate()
    {
        yVeclocity += Gravity * Time.deltaTime;

        MoveDirection.y = yVeclocity;

        cc.Move(Time.deltaTime * MoveDirection);
    }
}