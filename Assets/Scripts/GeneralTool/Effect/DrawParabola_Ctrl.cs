using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawParabola_Ctrl : MonoBehaviour
{
    public LineRenderer line;
    public float initVelocity = 10;
    public float gravity = 9.8f;
    public float length = 10;
    List<Vector3> linePoss = new List<Vector3>();

    public bool isFixedDeltaTime = true;
    public float timeMultiple = 1;
    public float currentDeltaTime
    {
        get
        {
            return Mathf.Max(0.001f, (isFixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime) * timeMultiple);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = this.transform.position;
        Vector3 lastPos = currentPos;
        Vector3 direction = this.transform.up;

        Vector3 currentVelocity = direction * initVelocity;
        linePoss.Clear();
        float currentTime = 0;
        float currentLengh = 0;
        while (currentLengh <= length)
        {
            currentTime += currentDeltaTime;
            lastPos = currentPos;
            linePoss.Add(currentPos);
            currentPos = this.transform.position+ currentVelocity * currentTime;
            currentPos.y -= 0.5f * gravity * currentTime * currentTime;
            currentLengh += (currentPos - lastPos).magnitude;
        }
        line.positionCount = linePoss.Count;
        line.SetPositions(linePoss.ToArray());
    }
    public void Update(Vector3 targetPos, Vector3 direction,float velocity,float length)
    {
        this.transform.position = targetPos;
        this.transform.up = direction;
        this.initVelocity = velocity;
        this.length = length;
        Update();
    }
}
