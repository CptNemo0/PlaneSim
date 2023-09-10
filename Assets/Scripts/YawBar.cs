using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YawBar : MonoBehaviour
{
    [SerializeField]
    List<Text> texts;

    List<Transform> transforms;
    [SerializeField]
    RectTransform Stick;
    [SerializeField]
    float Min, Max;
    void Start()
    {
        transforms = new List<Transform>();

        foreach (var text in texts)
        {
            transforms.Add(text.GetComponent<Transform>());
        }
    }

    public void SetText(int number, string direction)
    {
        if(direction == string.Empty) 
        {
            foreach (var text in texts)
            {
                if (number != 0)
                {
                    text.text = number.ToString();
                    Stick.sizeDelta = new Vector2(Stick.sizeDelta.x, Max);
                }
                else
                {
                    text.text = "";
                    Stick.sizeDelta = new Vector2(Stick.sizeDelta.x, Min);

                }
            }
        }
        else
        {
            foreach (var text in texts)
            {
                text.text = direction;
            }
        }
    }

    public void UpdateRoll(float angle)
    {
        foreach (var transform in transforms)
        {
            //transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}
