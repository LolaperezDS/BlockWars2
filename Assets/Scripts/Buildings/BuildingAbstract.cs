using System.Collections.Generic;
using UnityEngine;

public class BuildingAbstract : MonoBehaviour
{

    private Canvas main_canv;
    protected Camera cam;

    public int gold_produce;
    public int actions_produce;
    public Healths healths;


    // должно вызываться всегда при создании объекта
    public void SetUP(Commands command_attachment)
    {
        GetComponent<BuildingStruct>().SetCommandAttachment(command_attachment);
    }


    private void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        main_canv = GetComponentInChildren<Canvas>();
        main_canv.enabled = false;
        main_canv.worldCamera = cam;

        // index
        Vector3 positiOnOfThisBuild = transform.position;
        GetComponent<BuildingStruct>().index = new Vector2Int((int)positiOnOfThisBuild.x, (int)positiOnOfThisBuild.z);
    }

    public void GetUI()
    {
        if (GetComponent<BuildingStruct>().command_attachment == cam.GetComponent<CameraController>().GetTurn())
        {
            main_canv.enabled = true;
        }
    }

    public void HideUI()
    {
        main_canv.enabled = false;
    }

    public List<int> GetProduce()
    {
        return new List<int>() { gold_produce, actions_produce };
    }

    public void Repare()
    {
        if (cam.GetComponent<CameraController>().PossibilityToSpend(10, 1) && !healths.IsMaxHealed())
        {
            healths.Heal(2);
            cam.GetComponent<CameraController>().Spend(10, 1);
        }
        else if (healths.IsMaxHealed())
        {
            Debug.Log("У здания фулл хп");
        }
        else
        {
            Debug.Log("Недостаточно валюты для починки");
        }
    }


    public virtual void OnTurnEnd()
    {
        return;
    }


    public virtual void Fire(Vector2Int index)
    {
        return;
    }


    public void DestroyThis()
    {
        for (int i = cam.GetComponent<CameraController>().buildings.Count - 1; 0 <= i; i--)
        {
            if (cam.GetComponent<CameraController>().buildings[i].GetComponent<BuildingStruct>().index == GetComponent<BuildingStruct>().index)
            {
                cam.GetComponent<CameraController>().buildings.RemoveAt(i);
            }
        }
        Destroy(gameObject);
    }


    private void SetMaterial(Commands command_attachment)
    {
        Material m_Material = GetComponentInChildren<Renderer>().material;
        if (command_attachment == Commands.Red)
        {
            m_Material.color = Color.red;
        }
        else
        {
            m_Material.color = Color.blue;
        }
    }
}
