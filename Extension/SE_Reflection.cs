using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SDHK_Tool.Static;
using System.Globalization;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.18
 * 
 * 功能：反射的扩展方法
 */


namespace SDHK_Tool.Extension
{

    public static class SE_Reflection
    {
        #region 反射DLL

        /// <summary>
        /// 反射扩展：读取DLL：获取数据元
        /// </summary>
        /// <param name="DllPath">DLL路径</param>
        /// <returns>数据元</returns>
        public static Assembly SE_GetDLL_Assembly(this string DllPath)
        {
            return Assembly.LoadFrom(DllPath);
        }

        /// <summary>
        /// 反射扩展：读取DLL：获取类型集合
        /// </summary>
        /// <param name="DllPath">DLL路径</param>
        /// <returns>类型集合</returns>
        public static Type[] SE_GetDLL_Types(this string DllPath)
        {
            return Assembly.LoadFrom(DllPath).GetTypes();
        }

        /// <summary>
        /// 反射扩展：读取DLL：获取类型集合(单独反射消耗大)
        /// </summary>
        /// <param name="DllPath">DLL路径</param>
        /// <param name="ObjectName">类型名字</param>
        /// <returns>类型</returns>
        public static Type GetDLL_Types(this string DllPath, string ObjectName)
        {
            return Assembly.LoadFrom(DllPath).GetType(ObjectName);
        }

        /// <summary>
        /// 反射扩展：读取DLL：新建实例化类(单独反射消耗大)
        /// </summary>
        /// <param name="DllPath">DLL路径</param>
        /// <param name="ObjectName">类型名字</param>
        /// <returns>实例化类</returns>
        public static object SE_GetDLL_Object(this string DllPath, string ObjectName)
        {
            return Activator.CreateInstance(Assembly.LoadFrom(DllPath).GetType(ObjectName));
        }

        #endregion



        #region 反射类

        /// <summary>
        /// 根据名字从数据元中实例化类
        /// </summary>
        /// <param name="assembly">数据元</param>
        /// <param name="ObjectName">类型名字(包括命名空间)</param>
        /// <returns>实例化类</returns> 
        public static object SE_GetObject(this Assembly assembly, string ObjectName)
        {
            return Activator.CreateInstance(assembly.GetType(ObjectName));
        }

        /// <summary>
        /// 根据名字从数据元中获取类型
        /// </summary>
        /// <param name="assembly">数据元</param>
        /// <param name="ObjectName">类型名字(包括命名空间)</param>
        /// <returns>类型</returns>
        public static Type SE_GetType(this Assembly assembly, string ObjectName)
        {
            return assembly.GetType(ObjectName);
        }

        /// <summary>
        /// 反射扩展：根据名字实例化类
        /// </summary>
        /// <param name="ObjectName">类型名字(包括命名空间)</param>
        /// <returns>实例化类</returns>
        public static object SE_GetObject(this string ObjectName)
        {
            return Activator.CreateInstance(Type.GetType(ObjectName));
        }

        /// <summary>
        /// 反射扩展：根据类型新建类
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <returns>实例化类</returns>
        public static object SE_GetObject(this Type ObjectType)
        {
            return Activator.CreateInstance(ObjectType);
        }

        /// <summary>
        /// 反射扩展：根据名字新建类
        /// </summary>
        /// <param name="ObjectName">类型名字(包括命名空间)</param>
        /// <param name="args">构造函数参数</param>
        /// <returns>实例化类</returns>
        public static object SE_GetObject(this string ObjectName, object[] args)
        {
            return Activator.CreateInstance(Type.GetType(ObjectName), args);
        }

        /// <summary>
        /// 反射扩展：根据类型新建类
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="args">构造函数参数</param>
        /// <returns>实例化类</returns>
        public static object SE_GetObject(this Type ObjectType, object[] args)
        {
            return Activator.CreateInstance(ObjectType, args);
        }




        #endregion


        #region  反射字段


        #region 获取字段信息	

        /// <summary>
        /// 反射扩展：获取字段信息集合：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段信息集合</returns>
        public static FieldInfo[] SE_GetInfos_Field(this Type ObjectType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return ObjectType.GetFields(bindingFlags);
        }

        /// <summary>
        /// 反射扩展：获取字段信息集合：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段信息集合</returns>
        public static FieldInfo[] SE_GetInfos_Field(this string ObjectName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return Type.GetType(ObjectName).GetFields(bindingFlags);
        }


        /// <summary>
        /// 反射扩展：获取字段信息集合：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段信息集合</returns>
        public static FieldInfo[] SE_GetInfos_Field<T>(this T _Object, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return _Object.GetType().GetFields(bindingFlags);
        }


        /// <summary>
        /// 反射扩展：获取字段信息：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段信息</returns>
        public static FieldInfo SE_GetInfo_Field(this Type ObjectType, string FieldName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return ObjectType.GetField(FieldName, bindingFlags);
        }

        /// <summary>
        /// 反射扩展：获取字段信息：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段信息</returns>
        public static FieldInfo SE_GetInfo_Field(this string ObjectName, string FieldName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return Type.GetType(ObjectName).SE_GetInfo_Field(FieldName, bindingFlags);
        }


        /// <summary>
        /// 反射扩展：获取字段信息：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段信息</returns>
        public static FieldInfo SE_GetInfo_Field<T>(this T _Object, string FieldName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return _Object.GetType().GetField(FieldName, bindingFlags);
        }

        #endregion

        #region 获取字段

        //===============================================

        /// <summary>
        /// 反射扩展：获取字段：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段</returns>
        public static object SE_Get_Field<T>(this T _Object, string FieldName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return _Object.GetType().GetField(FieldName, bindingFlags).GetValue(_Object);
        }

        /// <summary>
        /// 反射扩展：获取静态类字段：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段</returns>
        public static object SE_Get_Field(this Type ObjectType, string FieldName, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public)
        {
            return ObjectType.GetField(FieldName, bindingFlags).GetValue(null);
        }

        /// <summary>
        /// 反射扩展：获取静态类字段：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>字段</returns>
        public static object SE_Get_Field(this string ObjectName, string FieldName, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public)
        {
            return Type.GetType(ObjectName).GetField(FieldName, bindingFlags).GetValue(null);
        }

        #endregion

        #region 设置字段

        /// <summary>
        /// 反射扩展：设置字段：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="value">设置值</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="binder">自定义绑定器</param>
        /// <param name="culture">设置文化</param>
        public static object SE_Set_Field<T>(this T _Object, string FieldName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, Binder binder = null, CultureInfo culture = null)
        {
            _Object.GetType().GetField(FieldName, bindingFlags).SetValue(_Object, value, bindingFlags, binder, culture);
            return _Object;
        }

        /// <summary>
        /// 反射扩展：设置静态类字段：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="value">设置值</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="binder">自定义绑定器</param>
        /// <param name="culture">设置文化</param>
        public static void SE_Set_Field(this Type ObjectType, string FieldName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, Binder binder = null, CultureInfo culture = null)
        {
            ObjectType.GetField(FieldName, bindingFlags).SetValue(null, value, bindingFlags, binder, culture);
        }

        /// <summary>
        /// 反射扩展：设置静态类字段：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名字(包括命名空间)</param>
        /// <param name="PropertyName">字段名</param>
        /// <param name="value">设置值</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="binder">自定义绑定器</param>
        /// <param name="culture">设置文化</param>
        public static void SE_Set_Field(this string ObjectName, string FieldName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, Binder binder = null, CultureInfo culture = null)
        {
            Type.GetType(ObjectName).GetField(FieldName, bindingFlags).SetValue(null, value, bindingFlags, binder, culture);
        }

        #endregion



        #endregion




        #region  反射属性


        #region 获取属性信息

        /// <summary>
        /// 反射扩展：获取属性信息集合：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>属性信息集合</returns>
        public static PropertyInfo[] SE_GetInfos_Property(this Type ObjectType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return ObjectType.GetProperties(bindingFlags);
        }

        /// <summary>
        /// 反射扩展：获取属性信息集合：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>属性信息集合</returns>
        public static PropertyInfo[] SE_GetInfos_Property(this string ObjectName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return Type.GetType(ObjectName).GetProperties(bindingFlags);
        }

        /// <summary>
        /// 反射扩展：获取属性信息集合：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>属性信息集合</returns>
        public static PropertyInfo[] SE_GetInfos_Property<T>(this T _Object, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return _Object.GetType().GetProperties(bindingFlags);
        }




        /// <summary>
        /// 反射扩展：获取属性信息：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>属性信息</returns>
        public static PropertyInfo SE_GetInfo_Property(this Type ObjectType, string PropertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return ObjectType.GetProperty(PropertyName, bindingFlags);
        }

        /// <summary>
        /// 反射扩展：获取属性信息：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="returnType">返回参数类型</param>
        /// <param name="types">参数类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="parameters">传入参数</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>属性信息</returns>
        public static PropertyInfo SE_GetInfo_Property(this Type ObjectType, string PropertyName, Type returnType, Type[] types, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, ParameterModifier[] parameters = null, Binder binder = null)
        {
            return ObjectType.GetProperty(PropertyName, bindingFlags, binder, returnType, types, parameters);
        }


        /// <summary>
        /// 反射扩展：获取属性信息：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>属性信息</returns>
        public static PropertyInfo SE_GetInfo_Property(this string ObjectName, string PropertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return Type.GetType(ObjectName).GetProperty(PropertyName, bindingFlags);
        }


        /// <summary>
        /// 反射扩展：获取属性信息：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="returnType">返回参数类型</param>
        /// <param name="types">参数类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="parameters">传入参数</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>属性信息</returns>
        public static PropertyInfo SE_GetInfo_Property(this string ObjectName, string PropertyName, Type returnType, Type[] types, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, ParameterModifier[] parameters = null, Binder binder = null)
        {
            return Type.GetType(ObjectName).GetProperty(PropertyName, bindingFlags, binder, returnType, types, parameters);
        }


        /// <summary>
        /// 反射扩展：获取属性信息：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>属性信息</returns>
        public static PropertyInfo SE_GetInfo_Property<T>(this T _Object, string PropertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return _Object.GetType().GetProperty(PropertyName, bindingFlags);
        }


        /// <summary>
        /// 反射扩展：获取属性信息：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="returnType">返回参数类型</param>
        /// <param name="types">参数类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="parameters">传入参数</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>属性信息</returns>
        public static PropertyInfo SE_GetInfo_Property<T>(this T _Object, string PropertyName, Type returnType, Type[] types, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, ParameterModifier[] parameters = null, Binder binder = null)
        {
            return _Object.GetType().GetProperty(PropertyName, bindingFlags, binder, returnType, types, parameters);
        }

        #endregion



        #region 获取属性

        /// <summary>
        /// 反射扩展：获取属性：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="index">索引器</param>
        /// <returns>属性</returns>
        public static object SE_Get_Property<T>(this T _Object, string PropertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, object[] index = null)
        {
            return _Object.GetType().GetProperty(PropertyName, bindingFlags).GetValue(_Object, index);
        }

        /// <summary>
        /// 反射扩展：获取静态类属性：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="index">索引器</param>
        /// <returns>属性</returns>
        public static object SE_Get_Property(this Type ObjectType, string PropertyName, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public, object[] index = null)
        {
            return ObjectType.GetProperty(PropertyName, bindingFlags).GetValue(null, index);
        }

        /// <summary>
        /// 反射扩展：获取静态类属性：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="index">索引器</param>
        /// <returns>属性</returns>
        public static object SE_Get_Property(this string ObjectName, string PropertyName, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public, object[] index = null)
        {
            return Type.GetType(ObjectName).GetProperty(PropertyName, bindingFlags).GetValue(null, index);
        }

        #endregion



        #region  设置属性

        /// <summary>
        /// 反射扩展：设置属性：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="value">设置值</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="binder">自定义绑定器</param>
        /// <param name="index">索引器</param>
        /// <param name="culture">设置文化</param>
        public static object SE_Set_Property<T>(this T _Object, string PropertyName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, Binder binder = null, object[] index = null, CultureInfo culture = null)
        {
            _Object.GetType().GetProperty(PropertyName, bindingFlags).SetValue(_Object, value, bindingFlags, binder, index, culture);
            return _Object;
        }

        /// <summary>
        /// 反射扩展：设置静态类属性：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="value">设置值</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="binder">自定义绑定器</param>
        /// <param name="index">索引器</param>
        /// <param name="culture">设置文化</param>
        public static void SE_Set_Property(this Type ObjectType, string PropertyName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, Binder binder = null, object[] index = null, CultureInfo culture = null)
        {
            ObjectType.GetProperty(PropertyName, bindingFlags).SetValue(null, value, bindingFlags, binder, index, culture);
        }

        /// <summary>
        /// 反射扩展：设置静态类属性：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名字(包括命名空间)</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="value">设置值</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="binder">自定义绑定器</param>
        /// <param name="index">索引器</param>
        /// <param name="culture">设置文化</param>
        public static void SE_Set_Property(this string ObjectName, string PropertyName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, Binder binder = null, object[] index = null, CultureInfo culture = null)
        {
            Type.GetType(ObjectName).GetProperty(PropertyName, bindingFlags).SetValue(null, value, bindingFlags, binder, index, culture);
        }

        #endregion


        #endregion



        #region  反射方法


        #region  获取方法信息

        /// <summary>
        /// 反射扩展：获取方法信息集合：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法信息集合</returns>
        public static MethodInfo[] SE_GetInfos_Method(this Type ObjectType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return ObjectType.GetMethods(bindingFlags);
        }

        /// <summary>
        /// 反射扩展：获取方法信息集合：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法信息集合</returns>
        public static MethodInfo[] SE_GetInfos_Method(this string ObjectName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return Type.GetType(ObjectName).GetMethods(bindingFlags);
        }


        /// <summary>
        /// 反射扩展：获取方法信息集合：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法信息集合</returns>
        public static MethodInfo[] SE_GetInfos_Method<T>(this T _Object, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return _Object.GetType().GetMethods(bindingFlags);
        }




        /// <summary>
        /// 反射扩展：获取方法信息：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法信息</returns>
        public static MethodInfo SE_GetInfo_Method(this Type ObjectType, string MethodName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return ObjectType.GetMethod(MethodName, bindingFlags);
        }



        /// <summary>
        /// 反射扩展：获取方法信息：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="types">参数类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="modifiers">参数修改器</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>方法信息</returns>
        public static MethodInfo SE_GetInfo_Method(this Type ObjectType, string MethodName, Type[] types, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, ParameterModifier[] modifiers = null, Binder binder = null)
        {
            return ObjectType.GetMethod(MethodName, bindingFlags, binder, types, modifiers);
        }


        /// <summary>
        /// 反射扩展：获取方法信息：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法信息</returns>
        public static MethodInfo SE_GetInfo_Method(this string ObjectName, string MethodName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return Type.GetType(ObjectName).GetMethod(MethodName, bindingFlags);
        }


        /// <summary>
        /// 反射扩展：获取方法信息：根据类型名字
        /// </summary>
        /// <param name="ObjectType">类型名(包括命名空间)</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="types">参数类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="modifiers">参数修改器</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>方法信息</returns>
        public static MethodInfo SE_GetInfo_Method(this string ObjectName, string MethodName, Type[] types, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, ParameterModifier[] modifiers = null, Binder binder = null)
        {
            return Type.GetType(ObjectName).GetMethod(MethodName, bindingFlags, binder, types, modifiers);
        }


        /// <summary>
        /// 反射扩展：获取方法信息：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法信息</returns>
        public static MethodInfo SE_GetInfo_Method<T>(this T _Object, string MethodName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return _Object.GetType().GetMethod(MethodName, bindingFlags);
        }


        /// <summary>
        /// 反射扩展：获取方法信息：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="types">参数类型</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="modifiers">参数修改器</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>方法信息</returns>
        public static MethodInfo SE_GetInfo_Method<T>(this T _Object, string MethodName, Type[] types, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, ParameterModifier[] modifiers = null, Binder binder = null)
        {
            return _Object.GetType().GetMethod(MethodName, bindingFlags, binder, types, modifiers);
        }

        #endregion



        #region 调用方法

        /// <summary>
        /// 反射扩展：调用方法：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="parameters">参数</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法返回值</returns>
        public static object SE_Call_Method<T>(this T _Object, string MethodName, object[] parameters = null, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return _Object.GetType().GetMethod(MethodName, bindingFlags).Invoke(_Object, parameters);
        }


        /// <summary>
        /// 反射扩展：调用方法：根据实例化类
        /// </summary>
        /// <param name="_Object">实例化类</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="types">参数类型</param>
        /// <param name="parameters">参数</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="modifiers">参数修改器</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>方法返回值</returns>
        public static object SE_Call_Method<T>(this T _Object, string MethodName, Type[] types, object[] parameters, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public, ParameterModifier[] modifiers = null, Binder binder = null)
        {
            return _Object.GetType().GetMethod(MethodName, bindingFlags, binder, types, modifiers).Invoke(_Object, parameters);
        }


        /// <summary>
        /// 反射扩展：调用静态方法：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="parameters">参数</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法返回值</returns>
        public static object SE_Call_Method(this string ObjectName, string MethodName, object[] parameters = null, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public)
        {
            return Type.GetType(ObjectName).GetMethod(MethodName, bindingFlags).Invoke(null, parameters);
        }


        /// <summary>
        /// 反射扩展：调用静态方法：根据类型名字
        /// </summary>
        /// <param name="ObjectName">类型名(包括命名空间)</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="types">参数类型</param>
        /// <param name="parameters">参数</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="modifiers">参数修改器</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>方法返回值</returns>
        public static object SE_Call_Method(this string ObjectName, string MethodName, Type[] types, object[] parameters, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public, ParameterModifier[] modifiers = null, Binder binder = null)
        {
            return Type.GetType(ObjectName).GetMethod(MethodName, bindingFlags, binder, types, modifiers).Invoke(null, parameters);
        }


        /// <summary>
        /// 反射扩展：调用静态方法：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="parameters">参数</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <returns>方法返回值</returns>
        public static object SE_Call_Method(this Type ObjectType, string MethodName, object[] parameters = null, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public)
        {
            return ObjectType.GetMethod(MethodName, bindingFlags).Invoke(null, parameters);
        }


        /// <summary>
        /// 反射扩展：调用静态方法：根据类型
        /// </summary>
        /// <param name="ObjectType">类型</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="types">参数类型</param>
        /// <param name="parameters">参数</param>
        /// <param name="bindingFlags">属性过滤</param>
        /// <param name="modifiers">参数修改器</param>
        /// <param name="binder">自定义绑定器</param>
        /// <returns>方法返回值</returns>
        public static object SE_Call_Method(this Type ObjectType, string MethodName, Type[] types, object[] parameters, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public, ParameterModifier[] modifiers = null, Binder binder = null)
        {
            return ObjectType.GetMethod(MethodName, bindingFlags, binder, types, modifiers).Invoke(null, parameters);
        }

        #endregion


        #endregion




    }
}