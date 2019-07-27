using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace EmitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var student = new { Name = "12222" };//匿名类形式
            //var student = new Student { Name = "12222" };//
            PropertyInfo propertyInfo = student.GetType().GetProperty("Name");
            PropertyEmit propertyEmit = new PropertyEmit(propertyInfo);
            propertyEmit.SetValue(student, "张三");//设置值
            var name = propertyEmit.GetValue(student);//读取值
            Console.ReadLine();
        }


        public class Student
        {
            public string Name { get; set; }
        }

        public class PropertyEmit
        {

            private PropertySetterEmit setter;
            private PropertyGetterEmit getter;
            public String PropertyName { get; private set; }
            public PropertyInfo Info { get; private set; }

            public PropertyEmit(PropertyInfo propertyInfo)
            {
                if (propertyInfo == null)
                {
                    throw new ArgumentNullException("属性不能为空");
                }

                if (propertyInfo.CanWrite)
                {
                    setter = new PropertySetterEmit(propertyInfo);
                }

                if (propertyInfo.CanRead)
                {
                    getter = new PropertyGetterEmit(propertyInfo);
                }

                this.PropertyName = propertyInfo.Name;
                this.Info = propertyInfo;
            }


            /// <summary>
            /// 属性赋值操作（Emit技术）
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="value"></param>
            public void SetValue(Object instance, Object value)
            {
                this.setter?.Invoke(instance, value);
            }

            /// <summary>
            /// 属性取值操作(Emit技术)
            /// </summary>
            /// <param name="instance"></param>
            /// <returns></returns>
            public Object GetValue(Object instance)
            {
                return this.getter?.Invoke(instance);
            }

            private static readonly ConcurrentDictionary<Type, PropertyEmit[]> securityCache = new ConcurrentDictionary<Type, PropertyEmit[]>();

            /// <summary>
            /// 获取对象属性
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <returns></returns>
            public static PropertyEmit[] GetProperties(Type type)
            {
                return securityCache.GetOrAdd(type, t => t.GetProperties().Select(p => new PropertyEmit(p)).ToArray());
            }
        }

        /// <summary>
        /// Emit 动态构造 Get方法
        /// </summary>
        public class PropertyGetterEmit
        {

            private readonly Func<Object, Object> getter;
            public PropertyGetterEmit(PropertyInfo propertyInfo)
            {
                //Objcet value = Obj.GetValue(Object instance);
                if (propertyInfo == null)
                {
                    throw new ArgumentNullException("propertyInfo");
                }
                this.getter = CreateGetterEmit(propertyInfo);

            }

            public Object Invoke(Object instance)
            {
                return getter?.Invoke(instance);
            }

            private Func<Object, Object> CreateGetterEmit(PropertyInfo property)
            {
                if (property == null)
                    throw new ArgumentNullException("property");

                MethodInfo getMethod = property.GetGetMethod(true);

                DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(Object),
                    new Type[] { typeof(Object) },
                    property.DeclaringType, true);

                ILGenerator il = dm.GetILGenerator();

                if (!getMethod.IsStatic)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.EmitCall(OpCodes.Callvirt, getMethod, null);
                }
                else
                    il.EmitCall(OpCodes.Call, getMethod, null);

                if (property.PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, property.PropertyType);
                il.Emit(OpCodes.Ret);
                return (Func<Object, Object>)dm.CreateDelegate(typeof(Func<Object, Object>));
            }
        }

        /// <summary>
        /// Emit动态构造Set方法
        /// </summary>
        public class PropertySetterEmit
        {
            private readonly Action<Object, Object> setFunc;
            public PropertySetterEmit(PropertyInfo propertyInfo)
            {
                //Obj.Set(Object instance,Object value)
                if (propertyInfo == null)
                {
                    throw new ArgumentNullException("propertyInfo");
                }
                this.setFunc = CreatePropertySetter(propertyInfo);

            }

            private Action<Object, Object> CreatePropertySetter(PropertyInfo property)
            {
                if (property == null)
                    throw new ArgumentNullException("property");

                MethodInfo setMethod = property.GetSetMethod(true);

                DynamicMethod dm = new DynamicMethod("PropertySetter", null,
                    new Type[] { typeof(Object), typeof(Object) }, property.DeclaringType, true);

                ILGenerator il = dm.GetILGenerator();

                if (!setMethod.IsStatic)
                {
                    il.Emit(OpCodes.Ldarg_0);
                }
                il.Emit(OpCodes.Ldarg_1);

                EmitCastToReference(il, property.PropertyType);
                if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
                {
                    il.EmitCall(OpCodes.Callvirt, setMethod, null);
                }
                else
                    il.EmitCall(OpCodes.Call, setMethod, null);

                il.Emit(OpCodes.Ret);
                return (Action<Object, Object>)dm.CreateDelegate(typeof(Action<Object, Object>));
            }

            private static void EmitCastToReference(ILGenerator il, Type type)
            {
                if (type.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, type);
                else
                    il.Emit(OpCodes.Castclass, type);
            }

            public void Invoke(Object instance, Object value)
            {
                this.setFunc?.Invoke(instance, value);
            }
        }
    }
}
