using UnityEngine;

public class MainBuildingScript : BuildingAbstract
{
    public void Awake()
    {
        transform.gameObject.GetComponent<Healths>().SetUp(10);
        gold_produce = 0;
        actions_produce = 3;
    }

    public override void OnTurnEnd()
    {
        cam.GetComponent<CameraController>().AddValueToCounters(gold_produce, actions_produce);
    }

    private void OnDestroy()
    {
        if (GetComponent<BuildingStruct>().command_attachment == Commands.Red)
        {
            Debug.Log("Blue Win!!");
        }
        else if (GetComponent<BuildingStruct>().command_attachment == Commands.Blue)
        {
            Debug.Log("Red Win!!");
        }
    }
}
