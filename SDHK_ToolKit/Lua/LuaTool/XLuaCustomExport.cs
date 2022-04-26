
/******************************

 * Author: 闪电黑客

 * 日期: 2022/02/06 13:57:09

 * 最后日期: 2022/02/06 13:57:39

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XLua;

public class ObjectList : List<object> { }

/// <summary>
/// xlua自定义导出
/// </summary>
public static class XLuaCustomExport
{

    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> Tools_lua_call_cs_list = new List<Type>()
    {
        typeof(SDHK_Extension.DictionaryExtension),
        typeof(SDHK_Extension.GameObjectExtension),
        typeof(SDHK_Extension.RectTransformExtension),
        typeof(SDHK_Extension.StructExtension),
        typeof(SDHK_Extension.TransformExtension),
        typeof(SDHK_Extension.ObjectExtension),
        typeof(PathAssets.PathAsset),
        typeof(WindowUI.WindowBaseExtension),
        typeof(ObjectFactory.ObjectPoolExtension),
        typeof(InputKeys.StringExtension),
        typeof(AsyncAwaitEvent.LuaAsyncAwait),
        typeof(EventMachine.EventExecutorExtension),

        typeof(System.Linq.Enumerable),

    };

    [CSharpCallLua]
    public static List<Type> delegate_types = new List<Type>()
    {
        typeof(System.Predicate<int>),
        typeof(System.Predicate<string>),

        typeof(UnityEngine.Events.UnityAction),
        typeof(UnityEngine.Events.UnityAction<int>),
        typeof(UnityEngine.Events.UnityAction<string>),

        typeof(Action<EventMachine.EventExecutor>),
        typeof(Action<string, string, InputKeys.InputKeyCodes>),
        typeof(Action<string, string, InputKeys.InputKeyCodes, InputKeys.InputKeyCodes, List<string>>),

    };

    /// <summary>
    /// dotween的扩展方法在lua中调用
    /// </summary>
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> dotween_lua_call_cs_list = new List<Type>()
    {
        typeof(DG.Tweening.AutoPlay),
        typeof(DG.Tweening.AxisConstraint),
        typeof(DG.Tweening.Ease),
        typeof(DG.Tweening.LogBehaviour),
        typeof(DG.Tweening.LoopType),
        typeof(DG.Tweening.PathMode),
        typeof(DG.Tweening.PathType),
        typeof(DG.Tweening.RotateMode),
        typeof(DG.Tweening.ScrambleMode),
        typeof(DG.Tweening.TweenType),
        typeof(DG.Tweening.UpdateType),

        typeof(DG.Tweening.DOTween),
        typeof(DG.Tweening.DOTweenModuleUI),
        typeof(DG.Tweening.DOVirtual),
        typeof(DG.Tweening.EaseFactory),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.Sequence),
        typeof(DG.Tweening.TweenParams),
        typeof(DG.Tweening.Core.ABSSequentiable),

        typeof(DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>),

        typeof(DG.Tweening.TweenCallback),
        typeof(DG.Tweening.TweenExtensions),
        typeof(DG.Tweening.TweenSettingsExtensions),
        typeof(DG.Tweening.ShortcutExtensions),
        // typeof(DG.Tweening.ShortcutExtensions43),
        // typeof(DG.Tweening.ShortcutExtensions46),
        // typeof(DG.Tweening.ShortcutExtensions50),
       
        //dotween pro 的功能
        // typeof(DG.Tweening.DOTweenPath),
        // typeof(DG.Tweening.DOTweenVisualManager),
        
    };



}