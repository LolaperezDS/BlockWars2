using UnityEngine;
using UnityEngine.UI;

public class SaveTowerIdBtn : MonoBehaviour
{
    [SerializeField] private GameObject Red1;
    [SerializeField] private GameObject Red2;
    [SerializeField] private GameObject Blue1;
    [SerializeField] private GameObject Blue2;
    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(SetIds);
    }

    private void SetIds()
    {
        if (Red1.GetComponent<Text>().text != "" && Red2.GetComponent<Text>().text != "" && Blue1.GetComponent<Text>().text != "" && Blue2.GetComponent<Text>().text != "")
        {
            int[] blue = new int[2] { System.Convert.ToInt32(Blue1.GetComponent<Text>().text), System.Convert.ToInt32(Blue2.GetComponent<Text>().text) };
            int[] red = new int[2] { System.Convert.ToInt32(Red1.GetComponent<Text>().text), System.Convert.ToInt32(Red2.GetComponent<Text>().text) };
            GenerateNoise.SetCoordsOfTowers(blue, red);
        }
    }
}
