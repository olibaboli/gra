using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UWAGA:
//Aby to rozwi�zanie zadzia�a�o, nale�y w menu Edytora Unity wej�� w opcj� "Edit"->"Project Settings...",...
//... w zak�adce po lewej stronie wybra� "Physics" i zaznaczy� opcj� "Auto Sync Transforms"

public class MyMovingPlatform : MonoBehaviour
{
    //Kolekcja refrerencji na komponent Transform (wype�niana w Edytorze Unity)
    public Transform[] waypoints;

    //Publiczne pole przechowuj�ce pr�dko�� poruszania si� platformy
    public float speed = 1;

    //Prywatne pole przechowuj�ce referencj� do komponentu Rigidbody
    private Rigidbody _rb;

    //Prywatne pole przechowuj�ce indeks (numer) aktualnego waypointa w kolekcji "waypoints"
    private int _target = 0;

    //Prywatne pole przechowuj�ce warto�� zmiany pola _target, aby przej�� do kolejnego indeksu waypointa
    private int _direction = 1;

    private void Start()
    {
        //Pobranie referencji do komponentu Rigidbody (w ramach tego samego obiektu)
        _rb = GetComponent<Rigidbody>();

        //Na wszelki, gdyby kto� zapomnia� skonfigurowa� komponent Rigidbody w Edytorze Unity
        _rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        //Ustawienie pozycji obiektu poprzez wpisanie do pola "position" komponentu Transform (do kt�rego referencj� mamy we wbudowanym polu "transform")...
        //... na pozycj� wyliczon� przez metod� "MoveTowards".
        transform.position = Vector3.MoveTowards(transform.position, waypoints[_target].position, speed * Time.deltaTime);

        //Je�eli odleg�o�� mi�dzy pozycj� obiektu a pozycj� aktualnego waypointa (jest on w kolekcji waypoints pod numerem przechowywanym w polu _target)..
        //... jest mniejsza ni� 0.05, to zmieniamy aktualny waypoint.
        if (Vector3.Distance(transform.position, waypoints[_target].position) < 0.05f)
        {
            //Je�eli indeks aktualnego waypoointa (liczba w polu _target) jest r�wny indeksowi ostatniego waypointa w kolekcji, to ...
            if (_target == waypoints.Length - 1)
            {
                _direction = -1; //... kierunek zmian indeks�w ustawiamy na ujemny
            }
            else if (_target == 0) //Je�eli nie wcze�niejszy warunek, to je�eli indeks jest r�wny zero (indeks pierwszego waypointa w kolekcji), to...
            {
                _direction = 1; //... kierunek zmian indeks�w ustawiamy na dodatni
            }

            //Przechodzimy do kolejnego waypointa zgodnie z kierunkiem zmian indeks�w
            _target += _direction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Je�eli do obszaru triggera wszed� collider, kt�rego GameObject jest otagowany jako "Player"...
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform); //...to uczy� go w hierarchii swoim dzieckiem: Rodzicem komponentu Transform "Playera" (other.transform) uczy� sw�j komponent Transform
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Je�eli obszar triggera opu�ci� collider , kt�rego GameObject jest otagowany jako "Player"...
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null); //...to uczy� jego rodzicem w hierarchii "scen�" (null jako parametr wywo�ania metody SetParent)
        }
    }
}