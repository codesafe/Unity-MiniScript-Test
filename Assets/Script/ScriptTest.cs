using System.IO;
using UnityEngine;
using Miniscript;

public class ScriptTest : MonoBehaviour
{
    DynamicScript script = new DynamicScript();

    void Start()
    {
        script.Create();
        script.AddAction("test1", test1);
        script.AddAction("test2", test2);

        script.CompileScript();
        script.RunScript("preaction");
    }

    private void OnDestroy()
    {
        script.Destroy();
    }

    public void test1(ValMap v)
    {
        int i1 = v["param1"].IntValue();
        int i2 = v["param2"].IntValue();

        Say(i1.ToString());
    }

    public void test2(ValMap v)
    {
        int i1 = v["param1"].IntValue();
        int i2 = v["param2"].IntValue();

        Say(i1.ToString());
    }

    public void Say(string s)
    {
        Debug.Log(s);
    }
}
