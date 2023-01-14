using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Application.logMessageReceived += LogErrors;

        RegisterCommands();
        RunCommand("help");
    }

    [Space]
    //public int maxMessagesOnScreen = 10;
    public int consoleHeight = 500;
    public int maxMessages = 64;
    //public int messageHeight = 40;
    //public int messageSpacing = 5;
    public int fontSize = 18;
    const float FontSizeMult = 1.35f;
    int fontHeight;

    [Space]
    public int consoleXOffset = 0;

    [Space]
    public Color errorMessageColour = Color.red;
    public Color warningMessageColour = Color.yellow;
    public Color logMessageColour = Color.white;

    [Space]
    public Color mainBGColour = Color.black;
    public Color messageBGColour = Color.gray;
    public Color inputBGColour = Color.Lerp(Color.gray, Color.black, 0.5f);

    [Space]
    public Texture2D texture;

    public Dictionary<string, ConsoleCommandBase> commands = new Dictionary<string, ConsoleCommandBase>();



    private bool showConsole = false;
    public static bool Active { get; private set; }

    private bool logErrors = true;
    private bool logWarnings = true;
    private bool logMessages = true;
    private bool logMessageStacktraces = false;

    private GUIStyle boxStyle;
    private GUIStyle textStyle;

    public List<ConsoleMessage> messages = new List<ConsoleMessage>(64);

    Vector2 scroll;
    int curHeight;
    string input = string.Empty;
    bool removeFocus;
    int suggestionSelected;
    bool accept;

    private void Update()
    {
        if (Keyboard.current.backquoteKey.wasPressedThisFrame)
        {
            showConsole = !showConsole;
            Active = showConsole;
            //GUI.FocusControl(null);
            removeFocus = true;
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame && showConsole)
        {
            if (input.Contains('`'))
                input = input.Replace("`", string.Empty);

            RunCommand(input);
            input = "";
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            suggestionSelected--;
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            suggestionSelected++;
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            accept = true;
        }
    }

    private void RunCommand(string input)
    {
        string[] split = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (split.Length < 2)
        {
            split = new string[2] { split[0], "" };
        }

        if (commands.TryGetValue(split[0], out ConsoleCommandBase cmdBase))
        {
            try
            {
                if (cmdBase is ConsoleCommand command) command.Invoke();
                else if (cmdBase is ConsoleCommand<int> commandInt) commandInt.Invoke(int.TryParse(split[1], out int intValue), intValue);
                else if (cmdBase is ConsoleCommand<float> commandFloat) commandFloat.Invoke(float.TryParse(split[1], out float floatValue), floatValue);
                else if (cmdBase is ConsoleCommand<string> commandString) commandString.Invoke(split[1].Length > 0, split[1]);

                //command.execute(split);
            }
            catch
            {
                Debug.Log("Type it correct knob");
            }
        }
        else
        {
            Debug.Log("No registered command with ID of \"" + split[0] + "\"");
        }
    }

    const string InputControlName = "InputControl";

    private void OnGUI()
    {
        if (removeFocus)
        {
            removeFocus = false;
            //GUIUtility.keyboardControl = 0;
            // Actually add focus lol
            GUI.FocusControl(InputControlName);
        }

        if (input.Contains('`'))
            input = input.Replace("`", string.Empty);

        if (!showConsole) return;

        fontHeight = Mathf.RoundToInt(fontSize * FontSizeMult);

        if (boxStyle == null)
        {
            boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = texture == null ? Texture2D.whiteTexture : texture;
        }

        if (textStyle == null)
        {
            textStyle = new GUIStyle(GUI.skin.box);
            textStyle.alignment = TextAnchor.UpperLeft;
            textStyle.normal.background = texture == null ? Texture2D.whiteTexture : texture;
            textStyle.richText = true;
            textStyle.wordWrap = true;
        }

        int width = Screen.width / 2;

        GUI.backgroundColor = mainBGColour;
        //GUI.Box(new Rect(width - width / 2 + consoleXOffset, 0, width, messageHeight * maxMessages + 40), "", boxStyle);
        float height = consoleHeight + 40;
        GUI.Box(new Rect(consoleXOffset, 0, width, height), "", boxStyle); // Main background

        Rect viewPort = new Rect(0, 0, width - 30, curHeight);
        scroll = GUI.BeginScrollView(new Rect(consoleXOffset, 0, width, consoleHeight), scroll, viewPort);

        textStyle.fontSize = fontSize;

        GUI.backgroundColor = messageBGColour;

        curHeight = 0;

        for (int i = 0; i < messages.Count; i++)
        {
            ConsoleMessage message = messages[i];
            
            string strippedMessage = message.Message; // This combats seemingly random blank spaces // Actually doesn't
            strippedMessage = strippedMessage.Substring(17); // <color=#ff0000ff>
            strippedMessage.Remove(strippedMessage.Length - 8, 8); // </color>
            int messsageHeight = Mathf.RoundToInt(textStyle.CalcHeight(new GUIContent(strippedMessage), width - 10));// * FontSizeMult);
            GUI.Label(new Rect(5 + consoleXOffset, curHeight + 10, width - 10, messsageHeight), message.Message, textStyle);
            curHeight += messsageHeight;
        }

        GUI.EndScrollView();

        GUI.backgroundColor = inputBGColour;
        GUI.Box(new Rect(consoleXOffset, height, width, fontSize + 10), "", boxStyle); // Input
        GUI.backgroundColor = Color.clear;

        GUIStyle style = new GUIStyle(GUI.skin.textField);
        style.alignment = TextAnchor.MiddleLeft;

        GUI.Label(new Rect(consoleXOffset + 5, height, fontSize + 10, fontSize + 10), ">", style); // >
        GUI.SetNextControlName(InputControlName);
        input = GUI.TextField(new Rect(consoleXOffset + 20, height, width - 30, fontSize + 10), input, style); // Input field

        GUI.backgroundColor = messageBGColour;
        //int suggestions = 3; // test num
        List<ConsoleCommandBase> suggestions = GetAutoCompletedStrings(input);

        if (suggestionSelected >= suggestions.Count)
            suggestionSelected = 0;

        if (suggestions.Count > 0)
        {
            if (accept)
            {
                input = suggestions[suggestionSelected].commandID;
            }

            if (suggestionSelected < 0)
                suggestionSelected = suggestions.Count - 1;

            GUI.Box(new Rect(consoleXOffset + 20, height + fontSize + 10, width / 2, suggestions.Count * fontHeight * FontSizeMult), "", boxStyle);

            for (int i = 0; i < suggestions.Count; i++)
            {
                GUI.Label(new Rect(consoleXOffset + 20, height + fontSize * FontSizeMult * i + 30, width / 2, fontHeight * FontSizeMult), suggestions[i].commandFormat);
            }

            //GUIStyle thisBox = new GUIStyle(GUI.skin.box);
            //thisBox.normal.background = Texture2D.whiteTexture;
            GUI.backgroundColor = Color.white;
            GUI.Label(new Rect(consoleXOffset, height + fontSize * FontSizeMult * suggestionSelected + 30, 20, fontHeight * FontSizeMult), ">");
        }

        accept = false;
    }

    private void LogErrors(string condition, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                if (logErrors)
                    messages.Add(new ConsoleMessage(condition, stackTrace, type, this));
                break;
            case LogType.Warning:
                if (logWarnings)
                    messages.Add(new ConsoleMessage(condition, stackTrace, type, this));
                break;
            case LogType.Log:
                if (logMessages)
                    messages.Add(new ConsoleMessage(condition, stackTrace, type, this));
                break;
            default:
                break;
        }

        while (messages.Count > maxMessages)
        {
            messages.RemoveAt(0);
        }

        fontHeight = Mathf.RoundToInt(fontSize * FontSizeMult);

        //int pixelLimit = consoleHeight;// + 40;
        //int pixels = messages.Count * fontHeight;
        //int scrollPixels = Mathf.Max(pixels - pixelLimit, 0);
        //scroll = new Vector2(0, scrollPixels);
        scroll = new Vector2(0, 10000);
    }

    #region Message Log Tests

    [ContextMenu("Log Test Message")]
    public void LogTestMessage()
    {
        Debug.Log("Test Message");
    }

    [ContextMenu("Log Test Warning")]
    public void LogTestWarning()
    {
        Debug.LogWarning("Test Warning");
    }

    [ContextMenu("Log Test Error")]
    public void LogTestError()
    {
        Debug.LogError("Test Error");
    }

    [ContextMenu("Log Test Exception")]
    public void LogTestException()
    {
        Debug.LogException(new System.Exception("Test exception"));
    }
    #endregion

    //[ConsoleCommand(Description = "Spawns a player with <health> health")]
    //public void SpawnPlayer(int health) { }


    public struct ConsoleMessage
    {
        // <color=#ff0000ff>colorfully</color>

        /*
        
        case LogType.Error:
                    textStyle.normal.textColor = errorMessageColour;
                    break;
                case LogType.Exception:
                    textStyle.normal.textColor = errorMessageColour;
                    break;
                case LogType.Warning:
                    textStyle.normal.textColor = warningMessageColour;
                    break;
                case LogType.Log:
                    textStyle.normal.textColor = logMessageColour;
                    break;

        */

        public LogType Type { get; private set; }
        //private string condition;
        //private string stackTrace;

        public string Message { get; private set; }

        public ConsoleMessage(string condition, string stackTrace, LogType type, DebugConsole console)
        {
            this.Type = type;
            //this.condition = condition;
            //this.stackTrace = stackTrace;
            Message = $"<color=#{GetColour(type, console)}>> {FormatType(type)}{condition} {(type != LogType.Log || console.logMessageStacktraces ? stackTrace : string.Empty)}</color>";
        }

        static string FormatType(LogType type) => type switch
        {
            LogType.Error => "[ERROR]: ",
            LogType.Assert => "[ASSERT]: ",
            LogType.Warning => "[WARNING]: ",
            LogType.Log => string.Empty,
            LogType.Exception => "[EXCEPTION]: ",
            _ => throw new System.NotImplementedException(),
        };

        static string GetColour(LogType type, DebugConsole console) => type switch
        {
            LogType.Error => ColorUtility.ToHtmlStringRGB(console.errorMessageColour),
            LogType.Assert => throw new System.NotImplementedException(),
            LogType.Warning => ColorUtility.ToHtmlStringRGB(console.warningMessageColour),
            LogType.Log => ColorUtility.ToHtmlStringRGB(console.logMessageColour),
            LogType.Exception => ColorUtility.ToHtmlStringRGB(console.errorMessageColour),
            _ => throw new System.NotImplementedException(),
        };
        //ColorUtility.ToHtmlStringRGB(col);
    }

    void RegisterCommands()
    {
        // I am so tired I wish for sleep please
        Commands.Register();

        /*
        Debug.Log("==- CONSOLE -==\n\n");
        Debug.Log("Commands:");
        Debug.Log("- biome [snow, grass, desert]");
        Debug.Log("- time [0-24, 0 is sunrise]");
        Debug.Log("- fuel [0-100]");
        Debug.Log("---------\n");
        */
    }

    public static void Register(ConsoleCommandBase c)
    {
        instance.commands.Add(c.commandID, c);
    }


    List<ConsoleCommandBase> GetAutoCompletedStrings(string partialString, int maxResults = 25)
    {
        List<ConsoleCommandBase> autoCompletedStrings = new List<ConsoleCommandBase>();
        if (partialString.Length == 0)
            return autoCompletedStrings;

        partialString = partialString.ToLower();

        foreach (ConsoleCommandBase command in commands.Values)
        {
            if (autoCompletedStrings.Count >= maxResults) break;

            if (command.commandID.StartsWith(partialString) ||
                command.commandID.Contains(partialString))
                autoCompletedStrings.Add(command);
        }

        return autoCompletedStrings;
    }
}

public class Command
{
    public string commandID;
    public Action<string[]> execute;

    public Command(string commandID, Action<string[]> execute)
    {
        this.commandID = commandID;
        this.execute = execute;
    }
}

//[AttributeUsage(AttributeTargets.Method)]
//public class ConsoleCommand : Attribute
//{
//    public string Description { get; set; }
//}
