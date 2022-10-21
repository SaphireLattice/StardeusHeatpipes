using KL.Console;
using UnityEngine;

public class ConsoleCommandTest : ConsoleCommand
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void Load()
	{
		ConsoleCommand.Register(new ConsoleCommandTest());
	}

	public override ConsoleCommandResult Execute(ConsoleCommandArguments args)
	{
		return OK("Yay!");
	}

	public override void Initialize()
	{
		Name = "lunar_test";
		HelpLine = "Just a test";
	}
}