﻿using DesktopTracker.Models;
using DesktopTracker.Utils;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

var config = SettingsUtil.ReadConfiguration<Config>();
var settings = config.Settings;
int defaultDelay = 1;

Console.WriteLine($"Send to url: {settings?.SendTo}");
Console.WriteLine($"Timestamp:{settings?.Delay}");
Console.WriteLine($"AuthKey:{settings?.AuthKey}");
Console.WriteLine("1. Start  2.Change settings");

var input = Console.ReadLine();

switch (input)
{
    case "1":
        Console.WriteLine("Start work"); break;

    case "2":
        var previous = settings.SendTo;
        Console.WriteLine("Write value of url:");
        var url = Console.ReadLine();
        settings.SendTo = url == string.Empty ? previous : url ?? string.Empty;

        Console.WriteLine("Write value of timestamp:");
        var delay = Console.ReadLine();
        settings.Delay = delay == string.Empty ? defaultDelay : float.Parse(delay);

        Console.WriteLine("Write value of auth key:");
        var authKey = Console.ReadLine();
        previous = settings.AuthKey;
        settings.AuthKey = authKey == string.Empty ? previous : authKey ?? string.Empty;

        ChangeSettings(config);

        settings = SettingsUtil.ReadConfiguration<Config>().Settings;

        Console.WriteLine("Start work");
        break;

    default: throw new ArgumentException("InvalidInput");
}

int delayMs = Convert.ToInt32(Convert.ToDouble(settings?.Delay) * 6 * Math.Pow(10, 4)); // minutes to miliseconds

CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token = cancelTokenSource.Token;
Task start = Task.Run(() => Start(settings, delayMs), token);

Console.WriteLine("Press any key to stop this app");
input = Console.ReadLine();

if (input is not null)
{
    cancelTokenSource.Cancel();
}

public static partial class Program
{
    #region WinApi

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);
    #endregion


    private static IntPtr GetActiveWindow() => GetForegroundWindow();

    private static string? GetActiveWindowName(IntPtr handle)
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);

        if (GetWindowText(handle, Buff, nChars) > 0)
            return Buff.ToString();

        return null;
    }

    private static string? GetAActiveProcessName(IntPtr handle)
    {
        IntPtr activeAppProcessId;

        GetWindowThreadProcessId(handle, out activeAppProcessId);

        Process currentAppProcess = Process.GetProcessById((int)activeAppProcessId);

        return currentAppProcess?.MainModule?.ModuleName;
    }

    private static void ChangeSettings(Config config)
    {
        SettingsUtil.SaveChanges<Config>(config, true);
    }

    private static async Task Start(Settings settings, int delay)
    {
        while (true)
        {
            await Task.Delay(delay);
            var window = GetActiveWindow();
            var nameOfWindow = GetActiveWindowName(window);
            var nameOfProcess = GetAActiveProcessName(window);

            RequestDto rq = new RequestDto
            {
                WindowName = nameOfWindow ?? string.Empty,
                ProcessName = nameOfProcess ?? string.Empty,
            };

            string json = JsonConvert.SerializeObject(rq);

            Console.WriteLine(json);

            await Sender.SendToServer(json, settings, true);
        }
    }
}
