using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedList<T>
{
    public int count = 0;
    private Node head = null;
    private Node tail = null;

    public void AddNode(T t)
    {

        Node newNode = new Node();
        newNode.value = t;
        if (count > 0)
        {
            newNode.previous = tail;
            tail.next = newNode;
        }
        else
        {
            head = newNode;
            newNode.previous = null;
        }
        newNode.next = null;
        tail = newNode;
        count++;
    }

    public T GetHead()
    {
        if (head != null)
            return head.value;
        else
            return default(T);
    }

    public T GetTail()
    {
        if (head != null)
            return tail.value;
        else
            return default(T);
    }

    public T Get(int index)
    {
        if (index < count)
        {
            int cpt = 0;
            Node n = head;
            while (cpt < index)
            {
                n = n.next;
                cpt++;
            }
            return n.value;
        }
        else
            return default(T);
    }

    public void Set(int index, T value)
    {
        if (index < count)
        {
            int cpt = 0;
            Node n = head;
            while (cpt < index)
            {
                n = n.next;
                cpt++;
            }
            n.value = value;
        }
    }

    public T this[int index]
    {
        get
        {
            return Get(index);
        }
        set
        {
            Set(index, value);
        }
    }

    private class Node
    {
        public T value;
        public Node next;
        public Node previous;

    }
}
