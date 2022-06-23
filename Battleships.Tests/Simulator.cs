using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Tests;

public class Simulator
{
    public ICollection<string> SimulatedUserInputs { get; set; }
    public ICollection<string> DisplayedLines { get; } = new List<string>();
    public int ClearedScreenActionsCount { get; private set; } = 0;
    public bool HasExitedGameLoop { get; private set; }


    public void HandleDisplayLineAction(string value)
    {
        DisplayedLines.Add(value);
    }

    public void HandleClearDisplayScreenAction()
    {
        ClearedScreenActionsCount += 1;
    }

    public string HandleUserInputSelector()
    {
        var userInput = SimulatedUserInputs.First();
        SimulatedUserInputs.Remove(userInput);
        return userInput;
    }

    public void RunGame(Game game)
    {
        Task.Run(() =>
        {
            game.RunLoop();
            HasExitedGameLoop = true;
        });
    }

    public async Task Wait()
    {
        int retry = 0;
        while (retry < 5)
        {
            await Task.Delay(100);
            retry++;
        }
    }
}