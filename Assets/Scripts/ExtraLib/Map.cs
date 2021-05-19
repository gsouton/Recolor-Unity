using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map<T1, T2>
{
    private Dictionary<T1, T2> dict;
    private Dictionary<T2, T1> reverseDict;

    public Map(){
        dict = new Dictionary<T1, T2>();
        reverseDict = new Dictionary<T2, T1>();
    }

    public void Add(T1 t1, T2 t2){
        dict.Add(t1, t2);
        reverseDict.Add(t2, t1);
    }

    public T2 Forward(T1 key){
        T2 t2;
        dict.TryGetValue(key, out t2);
        return t2;
    }

    public T1 Reverse(T2 key){
        T1 t1;
        reverseDict.TryGetValue(key, out t1);
        return t1;
    }
}
