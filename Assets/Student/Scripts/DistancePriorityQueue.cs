using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DistancePriorityQueue<Tkey>
{
    List<float> distanceList = new List<float>();
    List<Tkey> keyList = new List<Tkey>();

    public int SetCapacity
    {
        get { return keyList.Capacity; }
        set
        {
            keyList.Capacity = value;
            distanceList.Capacity = value;
        }
    }
    public DistancePriorityQueue()
    {
        Tkey item = default(Tkey);
        keyList.Add(item);

        distanceList.Add(float.NaN);
    }
    public DistancePriorityQueue(int verticies)
    {
        keyList.Capacity = verticies;
        distanceList.Capacity = verticies;
    }
    public int Count { get { return keyList.Count - 1; } }
    public void Queue(Tkey key, float distance)
    {
        keyList.Add(key);
        distanceList.Add(distance);

        Swim(keyList.Count - 1);

        //InsertionSort();
    }
    public void Update(Tkey key, float distance)
    {
        if (Count != 0)
        {
            int index = keyList.IndexOf(key);
            distanceList[index] = distance;
            Sink(index);
        }
    }

    public Tkey PopMinimumItem()
    {
        //Need to implement a null in both list for binary heap
        //float pop = valueList.First();


        int itemIndex = keyList.Count - 1;
        //Debug.Log(itemIndex);
        distanceList.RemoveAt(itemIndex);

        Tkey pop = keyList[itemIndex];
        keyList.RemoveAt(itemIndex);

        return pop;
    }


    public bool ContainsKey(Tkey key)
    {
        if (keyList.Count > 1)
        {
            int index = keyList.IndexOf(key, 1);
            if (index != -1)
                return true;
        }

        return false;
    }

    private void InsertionSort()
    {
        for (int i = distanceList.Count - 1; i > 0; i--)
        {
            int j = i - 1;

            Debug.Log("J:" + j + " I: " + i + " Capacity: " + distanceList.Capacity);
            if (j >= 0)
            {

                if (distanceList[i] < distanceList[j])
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
    private void Swim(int itemIndex)
    {
        while (itemIndex > 1 && Less(itemIndex / 2, itemIndex))
        {
            Exchange(itemIndex / 2, itemIndex);
            itemIndex /= 2;
        }
    }

    private void Sink(int index)
    {
        while (2 * index <= Count)
        {
            int childIndex = 2 * index;
            if (childIndex < Count && Less(childIndex, childIndex + 1))
                childIndex++;

            if (!Less(index, childIndex))
                break;

            Exchange(index, childIndex);

            index = childIndex;
        }
    }

    private void Exchange(int targetIndex, int exchangeIndex)
    {
        Tkey exchangeKeyItem = keyList[exchangeIndex];
        keyList[exchangeIndex] = keyList[targetIndex];
        keyList[targetIndex] = exchangeKeyItem;

        float exchangeFloatItem = distanceList[exchangeIndex];
        distanceList[exchangeIndex] = distanceList[targetIndex];
        distanceList[targetIndex] = exchangeFloatItem;
    }

    private bool Less(int i, int j)
    {
        Debug.Log($" {distanceList.Count} + {distanceList.Capacity} + {i} + {j}");
        float test = distanceList[i];
        float test2 = distanceList[j];
        if (distanceList[i] < distanceList[j])
            return true;

        return false;
    }

}

