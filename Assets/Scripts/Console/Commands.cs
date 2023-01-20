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

    static ConsoleCommand<int> AirshipSpeed = new ConsoleCommand<int>
        ("airship_speed", "Changes the ship's speed", "airship_speed [speed: default 7]", (valid, speed) =>
        {
            if (!valid)
            {
                Debug.Log("Current speed: " + Airship.instance.baseSpeed);
                return;
            }

            Airship.instance.baseSpeed = speed;
            Debug.Log("Set speed to " + speed);
        });

    static ConsoleCommand<int> NoxMult = new ConsoleCommand<int>
       ("nox_mult", "Changes the Nox multiplier", "nox_mult [mult: default 3]", (valid, mult) =>
       {
           if (!valid)
           {
               Debug.Log("Current mult: " + Airship.instance.nitrousSpeedMult);
               return;
           }

           Airship.instance.nitrousSpeedMult = mult;
           Debug.Log("Set speed to " + mult);
       });

    static ConsoleCommand<string> Spawn = new ConsoleCommand<string>
       ("spawn", "Spawn's a item", "spawn [Fuel, Coins, Nox]", (valid, spawn) =>
       {
           if(spawn == "Fuel")
           {
              // Airship.Spawn();
           }
        

           Debug.Log("Spawned " + spawn);
       });

    static ConsoleCommand<float> altitude = new ConsoleCommand<float>
       ("altitude", "Changes the Ship's altitude ", "altitude []", (valid, height) =>
       {
           if (!valid)
           {
               Debug.Log("Current altitude: " + Altitude.Instance.height);
               return;
           }

          //height = Mathf.Clamp01(height);
          //
          //float desired = Remap.Float(Mathf.Max(height, 0.1f), 0, 1, -Altitude.Instance.actualHeightRange, Altitude.Instance.actualHeightRange);
          //float delta = desired - Airship.Transform.position.y;
          //
          //Airship.MoveAllObjects(Airship.Transform.position + Vector3.up * delta);

           Altitude.Instance.height = height;
           Debug.Log("Set height to " + height);
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
        DebugConsole.Register(AirshipSpeed);
        DebugConsole.Register(NoxMult);
        DebugConsole.Register(altitude);
    }
}
