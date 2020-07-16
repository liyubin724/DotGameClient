using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEditor.Lua.Gen
{
    public class GenAssembly
    {
        public string Name;
        public List<GenType> Types = new List<GenType>();
    }

    public class GenType
    {
        public Type Type;
        public List<FieldInfo> Fields = new List<FieldInfo>();
        public List<PropertyInfo> Properties = new List<PropertyInfo>();
        public List<MethodInfo> Methods = new List<MethodInfo>();
    }

    public class GenData
    {
        public List<GenAssembly> AssemblyDatas = new List<GenAssembly>();

        public GenData()
        {
            GenAssemblyConfig assemblyConfig = GenAssemblyConfig.GetConfig();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                string assemblyName = assembly.GetName().Name;
                if(!assemblyConfig.HasAssembly(assemblyName))
                {
                    continue;
                }

                GenAssembly assemblyData = new GenAssembly()
                {
                    Name = assemblyName,
                };

                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsNotPublic || type.IsInterface || (!type.IsSealed && type.IsAbstract))
                    {
                        continue;
                    }
                    if (type.IsNested && !type.IsNestedPublic)
                    {
                        continue;
                    }
                    string ns = type.Namespace;
                    if(string.IsNullOrEmpty(ns))
                    {
                        ns = string.Empty;
                    }
                    bool isValid = assemblyConfig.HasSpace(assemblyName, ns);
                    if(isValid)
                    {
                        GenType typeData = new GenType()
                        {
                            Type = type,
                            Fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList(),
                            Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList(),
                            Methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList(),
                        };

                        typeData.Fields.Sort((item1, item2) =>
                        {
                            return item1.Name.CompareTo(item2.Name);
                        });
                        typeData.Properties.Sort((item1, item2) =>
                        {
                            return item1.Name.CompareTo(item2.Name);
                        });
                        typeData.Methods.Sort((item1, item2) =>
                        {
                            return item1.Name.CompareTo(item2.Name);
                        });

                        assemblyData.Types.Add(typeData);
                    }
                }

                if(assemblyData.Types.Count>0)
                {
                    assemblyData.Types.Sort((item1, item2) =>
                    {
                        return item1.Type.FullName.CompareTo(item2.Type.FullName);
                    });
                    AssemblyDatas.Add(assemblyData);
                }  
            }
        }
    }

}
