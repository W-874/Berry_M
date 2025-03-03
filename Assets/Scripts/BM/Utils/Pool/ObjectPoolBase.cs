using UnityEngine;
using Object = UnityEngine.Object;

namespace BM.Utils.Pool
{
    public abstract class ObjectPoolBase<T> where T : MonoBehaviour
    {
        protected T poolObject;

        protected ObjectPoolBase(T @object, int poolLength, Transform parent = null) {}

        protected T CreateObject()
        {
            var obj = Object.Instantiate(poolObject, Vector3.zero, Quaternion.identity);
            obj.gameObject.SetActive(false);
            return obj;
        }

        protected virtual T GetObject() => null;

        public virtual void ReturnObject(T obj)
        {
        }
    }
}
