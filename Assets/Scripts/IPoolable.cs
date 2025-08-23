using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IPoolable
{
    public string PoolTag { get; set; }
    void ReturnToPool();
    void Initialize(Vector3 position);
}

