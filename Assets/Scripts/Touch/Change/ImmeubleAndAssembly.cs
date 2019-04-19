using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmeubleAndAssembly : Change
{
    private class Floor
    {
        public SpriteRenderer[] window;
        public Color startColor;
    }
    public List<Transform> floorsTransform = new List<Transform>();
    private List<Floor> floors = new List<Floor>();
    
    private int offset;
    private float lastTime;
    public float delay = 0.2f;
    public float calmDown = 0.4f;

    // Start is called before the first frame update
    public override void ChangeOnStart()
    {
        foreach(Transform floorTransfom in floorsTransform)
        {
            Floor floor = new Floor();
            floor.window = floorTransfom.GetComponentsInChildren<SpriteRenderer>(true);
            floor.startColor = floor.window[0].color;
            floors.Add(floor);
        }
    }

    public override void ChangeOnClick(int currentState, bool on)
    {
        //Do nothing
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {
        float valueZerOne = (rtpcValue + 48) / 48;

        float height = 1.0f;

        if(Time.timeSinceLevelLoad - lastTime > delay)
        {
            offset = Random.Range(0, 6);
            lastTime = Time.timeSinceLevelLoad;
        }

        foreach (Floor floor in floors)
        {
            int number = 1;
            foreach(SpriteRenderer sR in floor.window)
            {
                float heightPlusNumber = number == offset ? 1 : Mathf.Abs(number - offset);
                heightPlusNumber *= height;
                sR.color = floor.startColor + Color.yellow * valueZerOne * heightPlusNumber * calmDown;
                number++;
            }
            height /= 2.0f;
        }

    }
}
