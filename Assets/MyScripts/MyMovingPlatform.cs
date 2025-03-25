using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UWAGA:
//Aby to rozwi¹zanie zadzia³a³o, nale¿y w menu Edytora Unity wejœæ w opcjê "Edit"->"Project Settings...",...
//... w zak³adce po lewej stronie wybraæ "Physics" i zaznaczyæ opcjê "Auto Sync Transforms"

public class MyMovingPlatform : MonoBehaviour
{
    //Kolekcja refrerencji na komponent Transform (wype³niana w Edytorze Unity)
    public Transform[] waypoints;

    //Publiczne pole przechowuj¹ce prêdkoœæ poruszania siê platformy
    public float speed = 1;

    //Prywatne pole przechowuj¹ce referencjê do komponentu Rigidbody
    private Rigidbody _rb;

    //Prywatne pole przechowuj¹ce indeks (numer) aktualnego waypointa w kolekcji "waypoints"
    private int _target = 0;

    //Prywatne pole przechowuj¹ce wartoœæ zmiany pola _target, aby przejœæ do kolejnego indeksu waypointa
    private int _direction = 1;

    private void Start()
    {
        //Pobranie referencji do komponentu Rigidbody (w ramach tego samego obiektu)
        _rb = GetComponent<Rigidbody>();

        //Na wszelki, gdyby ktoœ zapomnia³ skonfigurowaæ komponent Rigidbody w Edytorze Unity
        _rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        //Ustawienie pozycji obiektu poprzez wpisanie do pola "position" komponentu Transform (do którego referencjê mamy we wbudowanym polu "transform")...
        //... na pozycjê wyliczon¹ przez metodê "MoveTowards".
        transform.position = Vector3.MoveTowards(transform.position, waypoints[_target].position, speed * Time.deltaTime);

        //Je¿eli odleg³oœæ miêdzy pozycj¹ obiektu a pozycj¹ aktualnego waypointa (jest on w kolekcji waypoints pod numerem przechowywanym w polu _target)..
        //... jest mniejsza ni¿ 0.05, to zmieniamy aktualny waypoint.
        if (Vector3.Distance(transform.position, waypoints[_target].position) < 0.05f)
        {
            //Je¿eli indeks aktualnego waypoointa (liczba w polu _target) jest równy indeksowi ostatniego waypointa w kolekcji, to ...
            if (_target == waypoints.Length - 1)
            {
                _direction = -1; //... kierunek zmian indeksów ustawiamy na ujemny
            }
            else if (_target == 0) //Je¿eli nie wczeœniejszy warunek, to je¿eli indeks jest równy zero (indeks pierwszego waypointa w kolekcji), to...
            {
                _direction = 1; //... kierunek zmian indeksów ustawiamy na dodatni
            }

            //Przechodzimy do kolejnego waypointa zgodnie z kierunkiem zmian indeksów
            _target += _direction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Je¿eli do obszaru triggera wszed³ collider, którego GameObject jest otagowany jako "Player"...
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform); //...to uczyñ go w hierarchii swoim dzieckiem: Rodzicem komponentu Transform "Playera" (other.transform) uczyñ swój komponent Transform
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Je¿eli obszar triggera opuœci³ collider , którego GameObject jest otagowany jako "Player"...
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null); //...to uczyñ jego rodzicem w hierarchii "scenê" (null jako parametr wywo³ania metody SetParent)
        }
    }
}