using System;

public class PlayerState
{
    //the actions the player has collected, swapable at any time.
    private PlayerAction[] actions;
    private int currentActionIndex;
    internal PlayerAction GetAction()
    {
       return actions[currentActionIndex];
    }
    internal PlayerAction GetAction(int index)
    {
        currentActionIndex  = index;
       return actions[index];
    }
    public int ChangeAction()//basic behaviour
    {
        return ++currentActionIndex;
    }

    internal void UpdateWithNewModifier(IModifier mod)
    {
        //! add in code to add action, change jump, whatevers
      
    }
}