/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/25 16:11

* 描述： object对象池，用于unity和C#提供的 类 进行回收
* 例如List这种已经封装好，但没有对象池的类
* 这个池的功能和ObjectPool一样，但用的是Type，返回的是Objet
* 暂时无用

*/
using System;

namespace WorldTree
{
    /// <summary>
    /// object对象池
    /// </summary>
    public class TypePool : GenericPool<object>
    {
        public TypePool(Type type)
        {
            ObjectType = type;
            NewObject = ObjectNew;
            DestroyObject = ObjectDestroy;
        }
        private object ObjectNew(IPool pool)
        {
            return Activator.CreateInstance(ObjectType, true);
        }

        private static void ObjectDestroy(object obj)
        {
            if (obj is IDisposable)
            {
                (obj as IDisposable)?.Dispose();
            }
        }

        public override string ToString()
        {
            return "[ObjectPool<" + ObjectType + ">]";
        }
    }
}
