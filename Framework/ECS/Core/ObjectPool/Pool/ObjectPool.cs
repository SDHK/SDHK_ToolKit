/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/25 15:16

* 描述： 泛型对象池，用于unity和C#提供的 类 进行回收
* 例如List这种已经封装好，但没有对象池的类
* 

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 泛型对象池
    /// </summary>
    public class ObjectPool<T> : GenericPool<T>
        where T : class
    {
        public ObjectPool()
        {
            ObjectType = typeof(T);
            NewObject = ObjectNew;
            DestroyObject = ObjectDestroy;
        }

        private T ObjectNew(IPool pool)
        {
            T obj = Activator.CreateInstance(ObjectType, true) as T;
            return obj;
        }

        private static void ObjectDestroy(T obj)
        {
            if (obj is IDisposable)
            {
                (obj as IDisposable)?.Dispose();
            }
        }

        public override string ToString()
        {
            return "[ObjectPool<" + ObjectType+ ">]";
        }

    }


}
