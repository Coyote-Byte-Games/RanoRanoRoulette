using System;
using System.Collections;
using System.Collections.Generic;

//TODO: GET RID OF THE FUCKING CASTING YOURE GONNA KILL PERFORMANCE KID
//haha dotnet pipeline
public class PlayerInfoV2<T> : IEnumerable where T : IModifierTrait
{
    //the Items the player has collected, swapable at any time.
    private List<T> Items = new List<T>();

    private int currentItemIndex = 0;
    ////we had states independent of Items, but it would be better to combine the two, to modify the states and Items of mod A, rather than having two seperate dialogues.
    //SEPERATE BUT EQUAL jkkjkjkjkjk dont kill me plz ANYHOW seperate trackers for both Items and states. We'll make it nice after we get the underside done and nice!
    ////I WOULD try to make a generic version and make a state and Item version but we're too deep in now.
    //lmao im doin it

    public void AddItem(T Item)
    {
        this.Items.Add(Item);
    }
    public bool IsEmpty()
    {
        try
        {
            var trustTheProcessDude = Items[0];
            return false;

        }
        catch (System.ArgumentOutOfRangeException)
        {
            return true;
        }

    }

    internal T GetItem()
    {
        try
        {
            return Items[currentItemIndex];
        }
        catch (System.Exception e)
        {

            throw new Exception(e.StackTrace);
        }

    }
    internal T GetItem(int index)
    {
        currentItemIndex = index;
        return Items[index];
    }
    public int ChangeItem()//basic behaviour
    {
        if (++currentItemIndex >= Items.Count)
        {
            currentItemIndex = 0;
        }
        return currentItemIndex;
    }

    internal void UpdateWithNewModifier(IModifier mod)
    {
        //! add in code to add Item, change jump, whatevers

    }

    //why is this a thing

    public IEnumerator<T> GetEnumerator()
    {
        return (Items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}