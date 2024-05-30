//localhost: 127.0.0.1:13333
//carnap ip: 10.1.44.126:13333

//LLMUnity Server command on linux: 
//./ape-x86_64.elf llamafile-0.8.1  --port 13333 -m mistral-7b-instruct-v0.2.Qcarnap:~$ ./ape-x86_64.elf llamafile-0.8.1 --port 13333 -m mistral-7b-instruct-v0.2.Q4_K_M.gguf -c 4096 -b 512 --log-disable  -np 1 -ngl 80 --host 0.0.0.0 -cb -ts "3,2" --numa distribute --parallel 32


//LLMUnity Server command on mac:
//sh - c "/Users/nadaalalyani/Desktop/DIANA-2022/BlocksWorld/Assets/StreamingAssets/llamafile-0.6.2.exe
//--port 13333 -m /Users/nadaalalyani/Desktop/DIANA-2022/BlocksWorld/Assets/StreamingAssets/openhermes-2.5-mistral-7b.Q4_K_M.gguf
//-c 0 -b 512 --log-disable  -np 1 -ngl 0 --gpu no"

//llama.ccp server unix CL (cd build/bin/):
//./server -m MREG-13b-32-STEPS-q8_0.gguf -c 2048 --port 8000 --ctx_size 8192 -ngl 30 --host 0.0.0.0

//input example:
//"PinkBlock2 , (0.295275000: 1.224503000: 0.150793400) , None , (0.295275: 1.149781: 0.1507934) ) , under & support & touching(Table, PinkBlock2) and behind(RedBlock2, PinkBlock1) and behind(RedBlock2, cup) and in_front(BlueBlock2, PinkBlock2) and behind(GreenBlock2, GreenBlock1) and under & touching & support(Table, YellowBlock1) and behind(plate, BlueBlock1) and touching(YellowBlock2, Table) and hold(Diana2, PinkBlock2) and in_front(PinkBlock1, cup) and under(Table, RedBlock1) and under(Floor, BlueBlock2) and under(Floor, cup) and in_front(BlueBlock1, plate) and under(Floor, RedBlock2) and left & touching(BlueBlock2, BlueBlock1) and under & touching & support(Table, GreenBlock2) and under & touching & support(Table, plate) and under(Floor, BlueBlock1) and behind(YellowBlock2, BlueBlock2) and in_front(YellowBlock1, plate) and right(GreenBlock2, YellowBlock2) and under & touching & support(Table, BlueBlock1) and in_front(PinkBlock2, YellowBlock2) and under & support & touching(Table, RedBlock2) and left(BlueBlock1, RedBlock1) and support(BlueBlock2, RedBlock2) and behind(cup, PinkBlock1) and behind(plate, YellowBlock1) and under(Floor, YellowBlock1) and under(Floor, YellowBlock2) and in_front(GreenBlock1, GreenBlock2) and under(Floor, GreenBlock1) and in_front(plate, YellowBlock2) and under & touching & support(Table, YellowBlock2) and under & support & touching(Table, BlueBlock2) and under(Table, GreenBlock1)and in_front(BlueBlock2, plate) and left(plate, cup) and support(RedBlock2, PinkBlock2) and touching(GreenBlock2, Table)and left(YellowBlock2, GreenBlock2) and contain(plate, PinkBlock2) and behind(YellowBlock2, YellowBlock1) and touching(RedBlock2, Table) and under(Floor, PinkBlock2) and right(GreenBlock1, BlueBlock2) and hold(Diana2, RedBlock2) and touching(Table, Floor) andtouching(BlueBlock2, Table) and under(Floor, RedBlock1) and left(BlueBlock2, RedBlock1) and under(Floor, PinkBlock1) and behind(plate, BlueBlock2) and touching(BlueBlock1, Table) and under(Floor, plate) and under & touching & support(Table, cup)and right(cup, plate) and behind(YellowBlock2, plate) and in_front(PinkBlock1, RedBlock2) and under(Floor, GreenBlock2) and support(BlueBlock1, PinkBlock2) and left(YellowBlock2, RedBlock2) and right(RedBlock2, YellowBlock2) and right(GreenBlock1, BlueBlock1) and right(RedBlock1, BlueBlock2) and in_front(cup, RedBlock2) and touching(YellowBlock1, Table) and behind(PinkBlock2, BlueBlock2) and left(GreenBlock2, RedBlock2) and left(BlueBlock1, GreenBlock1) and touching(cup, Table) and right(RedBlock1, BlueBlock1) and touching(plate, Table) and support & right & touching(BlueBlock1, BlueBlock2) and in_front(BlueBlock2, YellowBlock2) and under(Table, PinkBlock1) and left(BlueBlock2, GreenBlock1) and support(GreenBlock2, BlueBlock2) and behind(YellowBlock2, PinkBlock2 , RedBlock1(-0.467860300,1.124870000,-0.376820000) and RedBlock2(0.296575000,1.125972000,0.154085400) and GreenBlock1(-0.114465600,1.124870000,-0.306613400) and GreenBlock2(-0.083701150,1.124844000,0.360018700) and BlueBlock1(0.198836000,1.124891000,-0.344644900) and BlueBlock2(0.299118700,1.125184000,-0.344177400) and PinkBlock1(-0.226616100,1.124870000,-0.162015900) and PinkBlock2(0.295275000,1.224503000,0.150793400) and YellowBlock1(0.432537600,1.124879000,-0.188665600) and YellowBlock2(0.352077900,1.124870000,0.323333000) ,  put put grasp put put put put put put grasp put put put grasp put put put put, PinkBlock2 BlueBlock1 BlueBlock1 RedBlock2 BlueBlock2 YellowBlock1 YellowBlock2 YellowBlock1 RedBlock2 RedBlock2 RedBlock2 BlueBlock2 GreenBlock2 GreenBlock2 PinkBlock2 BlueBlock2 PinkBlock2 BlueBlock1";


//Future work: 
//consider bidirictional interaction
//consider the relationship between events e.g., grasp this block, put it on the green block
//increase the inference efffecency

using UnityEngine;
using System.Net;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;
using VoxSimPlatform.SpatialReasoning;
using System.Timers;

public class MREG : ModuleBase
{
    //Generation setting parameters
    //public int n_predict = 20;
    //public int

    string instruction = "Generate a referring expression for an object.";
    //public string input = "PinkBlock2 , (0.295275000: 1.224503000: 0.150793400) , None , (0.295275: 1.149781: 0.1507934) ) , under & support & touching(Table, PinkBlock2) and behind(RedBlock2, PinkBlock1) and behind(RedBlock2, cup) and in_front(BlueBlock2, PinkBlock2) and behind(GreenBlock2, GreenBlock1) and under & touching & support(Table, YellowBlock1) and behind(plate, BlueBlock1) and touching(YellowBlock2, Table) and hold(Diana2, PinkBlock2) and in_front(PinkBlock1, cup) and under(Table, RedBlock1) and under(Floor, BlueBlock2) and under(Floor, cup) and in_front(BlueBlock1, plate) and under(Floor, RedBlock2) and left & touching(BlueBlock2, BlueBlock1) and under & touching & support(Table, GreenBlock2) and under & touching & support(Table, plate) and under(Floor, BlueBlock1) and behind(YellowBlock2, BlueBlock2) and in_front(YellowBlock1, plate) and right(GreenBlock2, YellowBlock2) and under & touching & support(Table, BlueBlock1) and in_front(PinkBlock2, YellowBlock2) and under & support & touching(Table, RedBlock2) and left(BlueBlock1, RedBlock1) and support(BlueBlock2, RedBlock2) and behind(cup, PinkBlock1) and behind(plate, YellowBlock1) and under(Floor, YellowBlock1) and under(Floor, YellowBlock2) and in_front(GreenBlock1, GreenBlock2) and under(Floor, GreenBlock1) and in_front(plate, YellowBlock2) and under & touching & support(Table, YellowBlock2) and under & support & touching(Table, BlueBlock2) and under(Table, GreenBlock1)and in_front(BlueBlock2, plate) and left(plate, cup) and support(RedBlock2, PinkBlock2) and touching(GreenBlock2, Table)and left(YellowBlock2, GreenBlock2) and contain(plate, PinkBlock2) and behind(YellowBlock2, YellowBlock1) and touching(RedBlock2, Table) and under(Floor, PinkBlock2) and right(GreenBlock1, BlueBlock2) and hold(Diana2, RedBlock2) and touching(Table, Floor) andtouching(BlueBlock2, Table) and under(Floor, RedBlock1) and left(BlueBlock2, RedBlock1) and under(Floor, PinkBlock1) and behind(plate, BlueBlock2) and touching(BlueBlock1, Table) and under(Floor, plate) and under & touching & support(Table, cup)and right(cup, plate) and behind(YellowBlock2, plate) and in_front(PinkBlock1, RedBlock2) and under(Floor, GreenBlock2) and support(BlueBlock1, PinkBlock2) and left(YellowBlock2, RedBlock2) and right(RedBlock2, YellowBlock2) and right(GreenBlock1, BlueBlock1) and right(RedBlock1, BlueBlock2) and in_front(cup, RedBlock2) and touching(YellowBlock1, Table) and behind(PinkBlock2, BlueBlock2) and left(GreenBlock2, RedBlock2) and left(BlueBlock1, GreenBlock1) and touching(cup, Table) and right(RedBlock1, BlueBlock1) and touching(plate, Table) and support & right & touching(BlueBlock1, BlueBlock2) and in_front(BlueBlock2, YellowBlock2) and under(Table, PinkBlock1) and left(BlueBlock2, GreenBlock1) and support(GreenBlock2, BlueBlock2) and behind(YellowBlock2, PinkBlock2 , RedBlock1(-0.467860300,1.124870000,-0.376820000) and RedBlock2(0.296575000,1.125972000,0.154085400) and GreenBlock1(-0.114465600,1.124870000,-0.306613400) and GreenBlock2(-0.083701150,1.124844000,0.360018700) and BlueBlock1(0.198836000,1.124891000,-0.344644900) and BlueBlock2(0.299118700,1.125184000,-0.344177400) and PinkBlock1(-0.226616100,1.124870000,-0.162015900) and PinkBlock2(0.295275000,1.224503000,0.150793400) and YellowBlock1(0.432537600,1.124879000,-0.188665600) and YellowBlock2(0.352077900,1.124870000,0.323333000) ,  put put grasp put put put put put put grasp put put put grasp put put put put, PinkBlock2 BlueBlock1 BlueBlock1 RedBlock2 BlueBlock2 YellowBlock1 YellowBlock2 YellowBlock1 RedBlock2 RedBlock2 RedBlock2 BlueBlock2 GreenBlock2 GreenBlock2 PinkBlock2 BlueBlock2 PinkBlock2 BlueBlock1";
    string input;
    public GameObject behaviorController;
    public RelationTracker relationTracker;
    public Transform grabbableObjects;

    public double FocusTimerTime = 2000;
    System.Timers.Timer FocusTimer;

    public double TargetTimerTime = 30;
    System.Timers.Timer TargetTimer;

    bool FocusElapse = false;
    bool TargetElapse = false;
    List<string> describedBlocks = new List<string>();
    List<string> actions = new List<string>();

    public static int focusTimeoutTime = 100;
    bool focusCirc = false;

    public float timePeriod = 2;
    public float height = 30f;
    public float startAngle;
    private float timeSinceStart;
    private Vector3 pivot;


    string tepBlock;

    string[] config;

    string[] focus ;

    string[] diana_output;

    string[] human_output;

    bool[] visit;
    string[] diana_names;

    string[] human_names;
    string name;
    public void Start()
    {
        base.Start();
        // Nada: added for relational REs Understanding 
        behaviorController = GameObject.Find("BehaviorController");
        // Nada: added for relational REs Understanding 
        relationTracker = behaviorController.GetComponent<RelationTracker>();

        if (TargetTimerTime > 0)
        {
            TargetTimer = new System.Timers.Timer(TargetTimerTime);
            TargetTimer.Enabled = false;
            TargetTimer.Elapsed += TargetTimeElapse;
    }

}

    public void Awake()
    {
        
    }

    [Obsolete]
    private void Update()
    {
        //for realtime generation of MREs
        if (FocusTimerTime > 0)
            {
                FocusTimer = new System.Timers.Timer(FocusTimerTime);
                FocusTimer.Enabled = false;
                FocusTimer.Elapsed += FocusTimeElapse;
            }

        if (TargetTimerTime > 0)
        {
            TargetTimer = new System.Timers.Timer(TargetTimerTime);
            //TargetTimer.Enabled = false;
            TargetTimer.Elapsed += TargetTimeElapse;
        }

        RealTimeGeneration();
        // If focus GREs time is elapsed 
        if (FocusElapse)
        {
            // input to LLM: has target object
            SetValue("me:speech:intent", "put it on another block please!", string.Empty);
            FocusElapse = false;
        }
        
    }

    public void RealTimeGeneration()
    {
        foreach (Transform t in grabbableObjects)
        {
            if (FocusTimer.Enabled == false && TargetTimer.Enabled == false)
            {
                //Debug.Log("TEST0: " + t.gameObject.name);

                if ((!t.gameObject.name.Equals("cup")) && (!t.gameObject.name.Equals("plate")) && (!t.gameObject.name.Equals("knife")) && (!t.gameObject.name.Equals("bottle")))
                {
                    if (t.gameObject.active && !describedBlocks.Contains(t.gameObject.name))
                    {
                        //Debug.Log("TEST1: " + t.gameObject.name);

                        string RE_tuple = ToLLAMA(t.gameObject.name, t.position.ToString(), "None", "None", RelationsLog(),
                            Configurations(), actions.ToString(), describedBlocks.ToString());

                        SetValue("me:intent:lookAt", t.gameObject.name, string.Empty);
                        SetValue("me:intent:targetObj", t.gameObject.name, string.Empty);
                        SetValue("me:intent:action", "point", string.Empty);
                        SetValue("me:actual:handPosR", t.position, string.Empty);
                        SetValue("me:speech:intent", "Grasp this " + t.gameObject.name, string.Empty);

                        //Debug.Log("TEST2: " + t.gameObject.name);

                        describedBlocks.Add(t.gameObject.name);
                        tepBlock = t.gameObject.name;
                        FocusTimer.Enabled = true;
                    }
                }
            }
        }
    }

    // When waiting time for response to generated REs for focus is elapsed 
    void FocusTimeElapse(object sender, ElapsedEventArgs e)
    {
        FocusElapse = true;
        TargetTimer.Enabled = true;
        FocusTimer.Interval = FocusTimerTime;
        FocusTimer.Enabled = false;
    }

    // When waiting time for response to generated REs to transfer focus to target is elapsed 
    void TargetTimeElapse(object sender, ElapsedEventArgs e)
    {
        TargetElapse = true;
        //FocusTimer.Enabled = false;
        TargetTimer.Interval = TargetTimerTime;
        TargetTimer.Enabled = false;
    }

    public string ToLLAMA(string focus, string focusPos, string target, string targetPos,
        string rel, string config, string prevAction, string prevObjs)
    {
        //input = "PinkBlock2 , (0.295275000: 1.224503000: 0.150793400) ,None , (0.295275: 1.149781: 0.1507934) ) , under & support & touching(Table, PinkBlock2) and behind(RedBlock2, PinkBlock1) and behind(RedBlock2, cup) and in_front(BlueBlock2, PinkBlock2) and behind(GreenBlock2, GreenBlock1) and under & touching & support(Table, YellowBlock1) and behind(plate, BlueBlock1) and touching(YellowBlock2, Table) and hold(Diana2, PinkBlock2) and in_front(PinkBlock1, cup) and under(Table, RedBlock1) and under(Floor, BlueBlock2) and under(Floor, cup) and in_front(BlueBlock1, plate) and under(Floor, RedBlock2) and left & touching(BlueBlock2, BlueBlock1) and under & touching & support(Table, GreenBlock2) and under & touching & support(Table, plate) and under(Floor, BlueBlock1) and behind(YellowBlock2, BlueBlock2) and in_front(YellowBlock1, plate) and right(GreenBlock2, YellowBlock2) and under & touching & support(Table, BlueBlock1) and in_front(PinkBlock2, YellowBlock2) and under & support & touching(Table, RedBlock2) and left(BlueBlock1, RedBlock1) and support(BlueBlock2, RedBlock2) and behind(cup, PinkBlock1) and behind(plate, YellowBlock1) and under(Floor, YellowBlock1) and under(Floor, YellowBlock2) and in_front(GreenBlock1, GreenBlock2) and under(Floor, GreenBlock1) and in_front(plate, YellowBlock2) and under & touching & support(Table, YellowBlock2) and under & support & touching(Table, BlueBlock2) and under(Table, GreenBlock1)and in_front(BlueBlock2, plate) and left(plate, cup) and support(RedBlock2, PinkBlock2) and touching(GreenBlock2, Table)and left(YellowBlock2, GreenBlock2) and contain(plate, PinkBlock2) and behind(YellowBlock2, YellowBlock1) and touching(RedBlock2, Table) and under(Floor, PinkBlock2) and right(GreenBlock1, BlueBlock2) and hold(Diana2, RedBlock2) and touching(Table, Floor) andtouching(BlueBlock2, Table) and under(Floor, RedBlock1) and left(BlueBlock2, RedBlock1) and under(Floor, PinkBlock1) and behind(plate, BlueBlock2) and touching(BlueBlock1, Table) and under(Floor, plate) and under & touching & support(Table, cup)and right(cup, plate) and behind(YellowBlock2, plate) and in_front(PinkBlock1, RedBlock2) and under(Floor, GreenBlock2) and support(BlueBlock1, PinkBlock2) and left(YellowBlock2, RedBlock2) and right(RedBlock2, YellowBlock2) and right(GreenBlock1, BlueBlock1) and right(RedBlock1, BlueBlock2) and in_front(cup, RedBlock2) and touching(YellowBlock1, Table) and behind(PinkBlock2, BlueBlock2) and left(GreenBlock2, RedBlock2) and left(BlueBlock1, GreenBlock1) and touching(cup, Table) and right(RedBlock1, BlueBlock1) and touching(plate, Table) and support & right & touching(BlueBlock1, BlueBlock2) and in_front(BlueBlock2, YellowBlock2) and under(Table, PinkBlock1) and left(BlueBlock2, GreenBlock1) and support(GreenBlock2, BlueBlock2) and behind(YellowBlock2, PinkBlock2 , RedBlock1(-0.467860300,1.124870000,-0.376820000) and RedBlock2(0.296575000,1.125972000,0.154085400) and GreenBlock1(-0.114465600,1.124870000,-0.306613400) and GreenBlock2(-0.083701150,1.124844000,0.360018700) and BlueBlock1(0.198836000,1.124891000,-0.344644900) and BlueBlock2(0.299118700,1.125184000,-0.344177400) and PinkBlock1(-0.226616100,1.124870000,-0.162015900) and PinkBlock2(0.295275000,1.224503000,0.150793400) and YellowBlock1(0.432537600,1.124879000,-0.188665600) and YellowBlock2(0.352077900,1.124870000,0.323333000) ,  put put grasp put put put put put put grasp put put put grasp put put put put, PinkBlock2 BlueBlock1 BlueBlock1 RedBlock2 BlueBlock2 YellowBlock1 YellowBlock2 YellowBlock1 RedBlock2 RedBlock2 RedBlock2 BlueBlock2 GreenBlock2 GreenBlock2 PinkBlock2 BlueBlock2 PinkBlock2 BlueBlock1";

        input = focus + " , " + focusPos.Replace(",", ":") + " , " + target + " , " + targetPos + " , " +
            rel + " , " + config + " , " + prevAction + " , " + prevObjs;

        string PROMPT_TEMPLATE = "Below is an instruction that describes a task, paired with an input that " +
       "provides further context. Write a response that appropriately completes the request.  # noqa: E501\\n### Instruction:\\n" + instruction +
       "\\n### Input:\\n" + input + "\\n### Response:";


        // convert to JSON
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://falcon.cs.colostate.edu:8000/completion");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        string result;
        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            Debug.Log("MREGLLM Start writing json ...");

            //string json = @"{""prompt"" : """ + prompt + """, "" + password + "" : """password"""}";
            //string json1 = @"{'prompt':'" + prompt + "','n_predict':'" + n_predict + "'}";

            string json = "{\"prompt\":\"" + PROMPT_TEMPLATE + "\"}";

            streamWriter.Write(json);
            Debug.Log("MREGLLM End writing json ...");
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            Debug.Log("MREGLLM Start streeming ...");
            result = streamReader.ReadToEnd();
            Debug.Log("MREGLLM End streeming ...");
        }
        //var responseJson = JObject.Parse(result);
        //string content = responseJson["content"].ToString();
        Debug.Log("MREGLLM result ..." + result);

        return result;
    }

    // ----------------------Function: Configurations()---------------------------------
    public string Configurations()
    {

        string config = "";
        Vector3 red1 = GameObject.Find("RedBlock1").transform.position;
        Vector3 red2 = GameObject.Find("RedBlock2").transform.position;
        Vector3 green1 = GameObject.Find("GreenBlock1").transform.position;
        Vector3 green2 = GameObject.Find("GreenBlock2").transform.position;
        Vector3 blue1 = GameObject.Find("BlueBlock1").transform.position;
        Vector3 blue2 = GameObject.Find("BlueBlock2").transform.position;
        Vector3 pink1 = GameObject.Find("PinkBlock1").transform.position;
        Vector3 pink2 = GameObject.Find("PinkBlock2").transform.position;
        Vector3 yellow1 = GameObject.Find("YellowBlock1").transform.position;
        Vector3 yellow2 = GameObject.Find("YellowBlock2").transform.position;
        config = "RedBlock1" + red1.ToString("f9") + " and " + "RedBlock2" + red2.ToString("f9") + " and " + "GreenBlock1" + green1.ToString("f9") + " and "
            + "GreenBlock2" + green2.ToString("f9") + " and " + "BlueBlock1" + blue1.ToString("f9") + " and " + "BlueBlock2" + blue2.ToString("f9") + " and "
            + "PinkBlock1" + pink1.ToString("f9") + " and " + "PinkBlock2" + pink2.ToString("f9") + " and " + "YellowBlock1" + yellow1.ToString("f9") + " and "
            + "YellowBlock2" + yellow2.ToString("f9");


        return config;
    }


    // ----------------------Function: RelationsLog()---------------------------------
    public string RelationsLog()
    {

        string srelation, finalrel;

        List<String> SceneRelations = new List<String>();

        foreach (DictionaryEntry entry in relationTracker.relations)
        {
            string v = (string)entry.Value;
            List<GameObject> keys = (List<GameObject>)entry.Key;
            List<String> relkey = new List<String>();

            foreach (GameObject key1 in (List<GameObject>)entry.Key)
            {
                relkey.Add(key1.name);
            }

            srelation = v + "(" + relkey[0] + ":" + relkey[1] + ")";
            if (srelation.Contains(","))
            {
                SceneRelations.Add(srelation.Replace(",", " & "));
            }
            if (srelation.Contains(":"))
            {
                SceneRelations.Add(srelation.Replace(":", ", "));
            }
            else
            {
                SceneRelations.Add(srelation);
            }
        }
        finalrel = string.Join(" and ", SceneRelations);

        return finalrel;
    }
}