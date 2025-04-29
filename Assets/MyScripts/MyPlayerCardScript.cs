using UnityEngine;
using TMPro;

public class MyPlayerCardScript : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text newDescriptionText;
    public TMP_Text AgeText;

    public void UpdateName(strig newName)
    {
        nameText.text = newName;
    }
    public void UpdateDescriptionText(string newDescription)
    {
        newDescriptionText.text = newDescription;
    }
    public void UpdateAge(float newValue)
    {
        AgeText.text = newValue.ToString() + "years old";
    }
}