using System;
using System.Linq;
using System.Reflection;

//ref link:https://www.youtube.com/watch?v=rtwkJD_aGzg&list=PLRwVmtr-pp05brRDYXh-OTAIi-9kYcw3r&index=5// Attribute special class in .NET CSharp compiler...all attribute must inherit from attribute
// Attribute special class in .NET CSharp compiler...all attribute must inherit from attribute
// Assembly is all executable and dll
// reflection gives type and attribute
// Robust testing framework

class TestAttribute : Attribute { }
class TestMethodAttribute : Attribute { }

[Test]
class MyTestSuite
{
    public void HelperMethod()
    {
        // That helps our tests get their job done....
        Console.WriteLine("This method will never be invoked because it does not have a testMethodAttribute on it.");
    }
    [TestMethod]
    public void MyTest1()
    {
        HelperMethod();
        // Doing some testing...
        Console.WriteLine("Doing some testing...");
    }
    [TestMethod]
    public void MyTestMethod2()
    {
        HelperMethod();
        Console.WriteLine("Doing some other testing...");
    }
}

class MainClass
{
    static void Main()
    {
        var testSuites =
            from t in Assembly.GetExecutingAssembly().GetTypes()    // reflection sample
            where t.GetCustomAttributes(false).Any(a => a is TestAttribute)
            select t;

        foreach (Type t in testSuites)
        {
            Console.WriteLine("Running test in suite: " + t.Name);
            var testMethods =
                from m in t.GetMethods()    // reflection
                where m.GetCustomAttributes(false).Any(a => a is TestMethodAttribute)
                select m;
            object testSuiteInstance = Activator.CreateInstance(t); // instance of MyTestSuite
            foreach (MethodInfo mInfo in testMethods)
                mInfo.Invoke(testSuiteInstance, new object[0]);
        }
            //Console.WriteLine(t.Name);

    }
}