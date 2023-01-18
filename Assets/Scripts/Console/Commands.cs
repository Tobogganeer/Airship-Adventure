using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Commands
{
    static ConsoleCommand help = new ConsoleCommand
        ("help", "Prints commands", "help", () =>
        {
            Debug.Log("Commands:");
            foreach (var cmd in DebugConsole.instance.commands.Values)
            {
                Debug.Log($"- {cmd.commandFormat} : {cmd.commandDescription}");
            }
            Debug.Log("---------\n");
        });

    static ConsoleCommand clearConsole = new ConsoleCommand
        ("clear_console", "Clears the console", "clear_console", () =>
        {
            DebugConsole.instance.messages.Clear();
        });

    static ConsoleCommand<string> biome = new ConsoleCommand<string>
        ("biome", "Changes the worlds biome", "biome [grass, snow, desert]", (valid, biome) =>
        {
            if (biome == "snow")
            {
                ProcGen.instance.currentBiome = Biome.Snow;
                ProcGen.mainMenuBiome = Biome.Snow;
                ProcGen.instance.Gen();
                Debug.Log("Set biome to Biome.Snow");
            }
            if (biome == "grass")
            {
                ProcGen.instance.currentBiome = Biome.Grasslands;
                ProcGen.mainMenuBiome = Biome.Grasslands;
                ProcGen.instance.Gen();
                Debug.Log("Set biome to Biome.Grasslands");
            }
            if (biome == "desert")
            {
                ProcGen.instance.currentBiome = Biome.Desert;
                ProcGen.mainMenuBiome = Biome.Desert;
                ProcGen.instance.Gen();
                Debug.Log("Set biome to Biome.Desert");
            }
        });

    static ConsoleCommand<int> time = new ConsoleCommand<int>
        ("time", "Changes the time of day (0 is sunrise)", "time [0-24]", (valid, time) =>
        {
            float timeFloat = time / 24f;
            DayNightController.instance.timeOfDay = timeFloat;
            Debug.Log("Set time to " + timeFloat);
        });

    static ConsoleCommand<int> fuel = new ConsoleCommand<int>
        ("fuel", "Changes the ship's fuel", "fuel [0-100]", (valid, fuel) =>
        {
            float fuelFloat = fuel / 100f;
            Airship.Fuel01 = fuel;
            Debug.Log("Set fuel to " + fuel);
        });

    static ConsoleCommand<int> fullscreen = new ConsoleCommand<int>
        ("fullscreen", "Changes fullscreen mode", "fullscreen [0, 1]", (valid, val) =>
        {
            if (!valid) Debug.Log("Fullscreen is currently " + Screen.fullScreen);
            else
            {
                bool fullScreen = val == 1 ? true : val == 0 ? false : Screen.fullScreen;
                Resolution maxRes = Screen.resolutions[Screen.resolutions.Length - 1];

                if (fullScreen)
                    Screen.SetResolution(maxRes.width, maxRes.height, FullScreenMode.FullScreenWindow, maxRes.refreshRate);
                else
                    Screen.SetResolution(1280, 720, FullScreenMode.Windowed, maxRes.refreshRate);
            }
        });

    static ConsoleCommand<int> nox = new ConsoleCommand<int>
        ("nox", "Changes the ships's nitrous tank", "nox [0-30]", (valid, val) =>
    {
        Airship.Nox = val;
        Debug.Log("Set nox to " + val);
    });

    public static void Register()
    {
        DebugConsole.Register(help);
        DebugConsole.Register(clearConsole);
        DebugConsole.Register(biome);
        DebugConsole.Register(time);
        DebugConsole.Register(fuel);
        DebugConsole.Register(fullscreen);
        DebugConsole.Register(nox);
    }
}
