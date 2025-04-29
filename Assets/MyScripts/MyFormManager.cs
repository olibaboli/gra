using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyFormManager : MonoBehaviour
{
    public Selectable firstInput;// public TMP_InputField firstInput;

    private void OnEnable()
    {
        firstInput.Select();
    }
}
