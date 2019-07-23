using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Miniscript;


public class DynamicScript
{
    private Interpreter interpreter = null;
    public delegate void scriptAction(ValMap v);

    private Dictionary<string, scriptAction> actionList = new Dictionary<string, scriptAction>();

    // TODO. script내용 string을 넘겨 받던, 경로를 넘겨 받던가 해야 한다.
    public void Create()
    {
        interpreter = new Interpreter();

        interpreter.standardOutput = (string s) => Say(s);
        interpreter.implicitOutput = (string s) => Say("<color=#66bb66>" + s + "</color>");
        interpreter.errorOutput = (string s) =>
        {
            Say("<color=red>" + s + "</color>");
            interpreter.Stop();
        };

        /*
                Intrinsic f = Intrinsic.Create("throw");
                f.AddParam("param", "");
                f.code = (context, partialResult) =>
                {
                    //var rs = context.interpreter.hostData as ReindeerScript;
                    //Reindeer reindeer = rs.reindeer;
                    //float eCost = (float)context.GetVar("energy").DoubleValue();
                    //if (reindeer.Throw(eCost))
                    //    return Intrinsic.Result.True;

                    string param = context.GetVar("param").ToString();

                    Say("throw : " + param);
                    return Intrinsic.Result.True;
                };
        */

        // 일단 최대 파라메터는 2개
        Intrinsic f = Intrinsic.Create("throwCode");
        f.AddParam("codename"); // 코드이름
        f.AddParam("param1");
        f.AddParam("param2");
        f.code = throwCode;


        //         interpreter.SetGlobalValue("_param", new ValString("preaction"));
        //         interpreter.RunUntilDone();
        // 
        // 
        //         interpreter.Restart();
        //         interpreter.SetGlobalValue("_param", new ValString("discard"));
        //         interpreter.RunUntilDone();
    }

    public void Destroy()
    {
        interpreter.Stop();
        actionList.Clear();
    }

    public void AddAction(string name, scriptAction f)
    {
        if (actionList.ContainsKey(name) == false)
        {
            actionList.Add(name, f);
        }
        else
        {
            Debug.LogError("exist action : " + name);
        }
    }

    public void Say(string s)
    {
        Debug.Log(s);
    }

    public void CompileScript()
    {
        string script = File.ReadAllText(Application.dataPath + "/Resources/Script/testscript.txt");

        interpreter.Reset(script);
        interpreter.Compile();

/*
        string encodescript = File.ReadAllText(Application.dataPath + "/Resources/Script/encoded.txt");
        string script = CompressionHelper.DecompressString(encodescript);
        interpreter.Reset(script);
        interpreter.Compile();
*/

        // Encode Script 원본 스크립트가 서명있는 유니코드여야함
/*
        string compressed = CompressionHelper.CompressString(script);
        File.WriteAllText(Application.dataPath + "/Resources/Script/encoded.txt", compressed);
        string ss = CompressionHelper.DecompressString(compressed);
*/
    }

    public void RunScript(string method)
    {
        interpreter.Restart();
        interpreter.SetGlobalValue("_param", new ValString(method));
        interpreter.RunUntilDone();
    }

    Intrinsic.Result throwCode(TAC.Context context, Intrinsic.Result partialResult)
    {
        string codename = context.GetVar("codename").ToString();
        if (actionList.ContainsKey(codename))
        {
            actionList[codename](context.variables);
            Say("throw : " + codename);
        }
        else
        {
            Say("Error throw : " + codename);
        }

        return Intrinsic.Result.True;
    }
}
