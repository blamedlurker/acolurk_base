using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using acolurk_base.helpers;

namespace acolurk_base.classes;

public class Command
{
    public List<string> names = new List<string>();
    public string description = String.Empty;
    public List<string> tags = new List<string> { "default" };
    public List<string> argNames = new List<string>();
    public List<Type> argTypes = new List<Type>();
    public Action<List<string>, UIChat, ulong> OnCommandCall;
    public static List<Command> commands = new List<Command>();

    public Command(List<string> names, string description, List<string> tags, List<string> argNames, List<Type> argTypes, Action<List<string>, UIChat, ulong> OnCommandCall)
    {
        this.names = names;
        this.description = description;
        foreach (string tag in tags) this.tags.Add(tag);
        this.argNames = argNames;
        this.argTypes = argTypes;
        this.OnCommandCall = OnCommandCall;
    }

    public void Register()
    {
        commands.Add(this);
    }
    public void CallCommand(List<string> args, UIChat chat, ulong clientId)
    {
        OnCommandCall?.Invoke(args, chat, clientId);
    }
}