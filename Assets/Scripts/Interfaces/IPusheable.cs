using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPusheable<T>
{
   void Push(T force);
}
