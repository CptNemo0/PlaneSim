using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YawBar : MonoBehaviour
{
    [SerializeField]
    List<Text> texts;

    List<Transform> transforms;

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
                }
                else
                {
                    text.text = "";
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
            transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}
