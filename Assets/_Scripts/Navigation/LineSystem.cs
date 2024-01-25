using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class LineSystem : MonoBehaviour
{
    [Header("External References")]
    public NavigationBackground background;
    public GameObject linePrefab;

    [Header("Line Stats")]
    public int numLinesBeforeNewCharacteristic = 5;
    public float minTimeBetweenlines = 0.5f;
    public float maxTimeBetweenlines = 5f;
    public float speed = 10f;

    [Header("Line Shape and Position")]
    public float lineHeightMultiplier = 5f;
    public float minZPosition = -190f;
    public float maxZPosition = 310f;

    private int _linesCreated = 0;
    private List<float> _lastZPositions;
    private List<float> _nextZPositions;
    
    private float _timeOfLastLine = 0f;
    private float _timeBetweenLines = 2f;
    
    private List<NavigationLine> _navigationLines = new List<NavigationLine>();

    public List<float> GetRandomLineZPositions(int segments)
    {
        List<float> positions = new List<float>();
        for (int k = 0; k < segments; k++)
        {
            positions.Add(0f);
        }

        int numberOfCharacteristics = Random.Range(1, 5); // up to 4 random maximas/minimas
        int startPosition = 0;

        // populate characteristics
        int[] characteristics = new int[numberOfCharacteristics];
        for (int i = 0; i < numberOfCharacteristics; i++)
        {
            characteristics[i] = Random.Range(0, 3);

            // ensure characteristics are not sequentially the same
            if ((i != 0 && characteristics[i] == characteristics[i - 1]) ||
                (i == 0 && characteristics[i] == startPosition))
            {
                if (characteristics[i] == 2)
                {
                    characteristics[i] -= 1;
                }
                else
                {
                    characteristics[i] += 1;
                }
            }
        }
        
        // ensure downwards curve
        if (numberOfCharacteristics > 2)
        {
            if (characteristics[^1] > 1)
            {
                characteristics[^1] -= 1;
            }
        }

        int cc = 0;
        // fill z values
        for (int j = 0; j < segments; j++)
        {
            if (j == 0)
            {
                positions[j] = startPosition;
            }
            else
            {
                if (Mathf.Abs(positions[j - 1] - characteristics[cc]) <= 0.05f)
                {
                    positions[j] = characteristics[cc];
                }
                else
                {
                    float lerpValue = Random.Range(0.075f, 0.4f);
                    positions[j] = Mathf.Lerp(positions[j - 1], characteristics[cc], lerpValue);
                }

                if (Mathf.Abs(positions[j - 1] - characteristics[cc]) <= 0.01f && cc < numberOfCharacteristics - 1)
                {
                    cc++;
                }
            }
        }

        // multiply z values
    for (int p = 0; p < segments; p++)
        {
            positions[p] *= lineHeightMultiplier;
        }
        
        return positions;
    }

    private NavigationLine CreateLine()
    {
        GameObject obj = Instantiate(linePrefab, this.transform);
        NavigationLine line = obj.GetComponent<NavigationLine>();

        if (_linesCreated % this.numLinesBeforeNewCharacteristic == 0)
        {
            line.zPositions = this._nextZPositions;
            this._lastZPositions = line.zPositions;
            this._nextZPositions = this.GetRandomLineZPositions(line.segments);
        }
        else
        {
            float lerpValue = (_linesCreated % this.numLinesBeforeNewCharacteristic) / (float)this.numLinesBeforeNewCharacteristic;
            for (int i = 0; i < line.segments; i++)
            {
                line.zPositions[i] = Mathf.Lerp(this._lastZPositions[i], this._nextZPositions[i], lerpValue);
            }
        }

        _linesCreated++;
        return line;
    }

    private void InitializeLines()
    {
        this.UpdateLineTime();
        float lineDistance = this.speed * this._timeBetweenLines;

        while (this._navigationLines.Count <= 1 ||
               this._navigationLines[0].transform.position.z >= this.minZPosition) 
        {
            var line = this.CreateLine();
            line.transform.Translate(new Vector3(0f, 0f, -lineDistance * this._linesCreated), Space.Self);
            _navigationLines.Insert(0, line);
        }

        this._timeOfLastLine = Time.time;
    }

    private bool CheckLineOutOfBounds(NavigationLine line)
    {
        if (line.transform.position.z <= this.minZPosition)
        {
            return true;
        }

        return false;
    }

    private void MoveLines(float amount)
    {
        foreach (var line in this._navigationLines)
        {
            line.transform.Translate(new Vector3(0f, 0f,-amount), Space.Self);
        }
    }

    private void UpdateLineTime()
    {
        this._timeBetweenLines = Mathf.Lerp(this.minTimeBetweenlines, this.maxTimeBetweenlines, 1 - this.background.fractionRotation);
    }
    
    void Start()
    {
        this._nextZPositions = this.GetRandomLineZPositions(linePrefab.GetComponent<NavigationLine>().segments);
        this._lastZPositions = this._nextZPositions;
        this.InitializeLines();
    }

    void Update()
    {
        this.UpdateLineTime();
        
        float dt = Time.deltaTime;
        this.MoveLines(dt * speed);
        
        if (this.CheckLineOutOfBounds(this._navigationLines[0]))
        {
            Destroy(this._navigationLines[0].gameObject);
            this._navigationLines.RemoveAt(0);
        }

        if (Time.time - this._timeOfLastLine >= this._timeBetweenLines)
        { 
            var line = this.CreateLine();
            this._navigationLines.Add(line);
            this._timeOfLastLine = Time.time;

            if (Random.Range(0, 10) == 1)
            {
                this._timeOfLastLine -= 0.75f;
            }
        }
    }
}
