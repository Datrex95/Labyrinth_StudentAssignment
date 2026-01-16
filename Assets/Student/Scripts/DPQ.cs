using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DPQ<Tkey>
{
    List<float> distanceList = new List<float>();
    List<Tkey> keyList = new List<Tkey>();

    public int Count { get { return keyList.Count; } }
    public void Queue(Tkey key, float distance)
    {
        keyList.Add(key);
        distanceList.Add(distance);

        InsertionSort();
    }
    public void Update(Tkey key, float distance)
    {
        if (keyList.Count != 0)
        {
            int index = keyList.IndexOf(key);
            distanceList[index] = distance;
        }
    }

    public Tkey DeQueue()
    {

        //float pop = valueList.First();
        distanceList.RemoveAt(0);

        Tkey pop = keyList[0];
        keyList.RemoveAt(0);

        return pop;
    }


    public bool ContainsKey(Tkey key)
    {
        return keyList.Contains(key);
    }

    private void InsertionSort()
    {
        for (int i = distanceList.Count - 1; i > 0; i--)
        {
            int j = i - 1;

            Debug.Log("J:" + j + " I: " + i + " Capacity: " + distanceList.Capacity);
            if (j >= 0 )
            {

                if (distanceList[i]  < distanceList[j])
                {
                    float valueCopy = distanceList[i];
                    distanceList[i] = distanceList[j];
                    distanceList[j] = valueCopy;

                    Tkey keyCopy = keyList[i];
                    keyList[i] = keyList[j];
                    keyList[j] = keyCopy;
                }
                else
                {
                    break;
                }
            }
        }
    }
}

