using System.Collections;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> {
    T[] items;
    int currentItemCount = 0;

    public Heap(int maxSize)
    {
        items = new T[maxSize];
    }

    public void Add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount++] = item;
        sortUp(item);
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        sortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        sortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    void sortDown(T item)
    {
         while (true)
        {
            // Formula to get the node left child: 2n + 1
            int childIndexLeft = item.heapIndex * 2 + 1;
            // Formula to get the node right child: 2n + 2
            int childIndexRight = item.heapIndex * 2 + 2;
            int swapIndex = 0;
            // Compare the index and if it is higher
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                    return;
            }
            else
                return;
        }
    }

    void sortUp(T item)
    {
        // formula: (n - 1) / 2
        int parentIndex = Mathf.RoundToInt((item.heapIndex - 1) * 0.5f);
        while (true)
        {
            T parentItem = items[parentIndex];

            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
                parentIndex = (item.heapIndex - 1) / 2;
            }
            else
            {
                break;
            }
        }
    }
    
    void Swap(T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;
        int itemAIndex = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int heapIndex
    {
        get;set;
    }
}