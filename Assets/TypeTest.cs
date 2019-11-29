using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public enum TestEnumType
{
    A,
    B,
}
public struct TestStruct
{
    public int a;
}

public class TestData
{
    public Type type;
    public object value;
}

public class TypeTest : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        //TestData[] datas = new TestData[]
        //{
        //    new TestData(){type = typeof(int),value = 11},
        //    new TestData(){type = typeof(float),value = 11},
        //    new TestData(){type = typeof(string),value = 11},
        //    new TestData(){type = typeof(TestStruct),value = 11},
        //    new TestData(){type = typeof(TestEnumType),value = 11},
        //    new TestData(){type = typeof(List<int>),value = 11},
        //    new TestData(){type = typeof(List<TypeTest>),value = 11},
        //    new TestData(){type = typeof(float[]),value = 11},
        //    new TestData(){type = typeof(TypeTest[]),value = 11},
        //    new TestData(){type = typeof(TestStruct[]),value = 11},

        //};

        Type[] types = new Type[]
        {
            typeof(int),
            typeof(float),
            typeof(string),
            typeof(TestStruct),
            typeof(TestEnumType),
            typeof(List<int>),
            typeof(List<TypeTest>),
            typeof(float[]),
            typeof(TypeTest[]),
            typeof(TestStruct[]),
        };

        StringBuilder sb = new StringBuilder();
        foreach(var type in types)
        {
            sb.AppendLine(type.FullName);

            PropertyInfo[] ps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(var p in ps)
            {
                if(p.CanRead)
                {
                    sb.AppendLine("\t" + p.Name + ":" + p.GetValue(type));

                }
            }

            sb.AppendLine();
        }

        File.WriteAllText("D:/t.txt", sb.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
