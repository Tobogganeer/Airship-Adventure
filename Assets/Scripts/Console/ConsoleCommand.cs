using System;

public class ConsoleCommandBase
{
    public readonly string commandID;
    public readonly string commandDescription;
    public readonly string commandFormat;

    public ConsoleCommandBase(string commandID, string commandDescription, string commandFormat)
    {
        this.commandID = commandID;
        this.commandDescription = commandDescription;
        this.commandFormat = commandFormat;
    }
}

public class ConsoleCommand : ConsoleCommandBase
{
    private readonly Action command;

    public ConsoleCommand(string commandID, string commandDescription, string commandFormat, Action command) : base(commandID, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class ConsoleCommand<T1> : ConsoleCommandBase
{
    private readonly Action<bool, T1> command;

    public ConsoleCommand(string commandID, string commandDescription, string commandFormat, Action<bool, T1> command) : base(commandID, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke(bool isValidInput, T1 value)
    {
        command.Invoke(isValidInput, value);
    }
}

public class ConsoleCommand<T1, T2> : ConsoleCommandBase
{
    private readonly Action<bool, T1, T2> command;

    public ConsoleCommand(string commandID, string commandDescription, string commandFormat, Action<bool, T1, T2> command) : base(commandID, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke(bool isValidInput, T1 value, T2 value2)
    {
        command.Invoke(isValidInput, value, value2);
    }
}