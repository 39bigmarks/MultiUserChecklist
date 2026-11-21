using Raylib_cs;
using System.Numerics;

namespace Checklist;

internal sealed class Checklist
{
    public string name = "Checklist A";
    public Dictionary<string, bool> list = new() {
        {"Objective A", true },
        {"Objective B", false },
        {"Objective C", false },
        {"Objective D", false },
        {"Objective E", false },
        {"Objective F", false },
        {"Objective G", false },
    };

    public float width;

    public Checklist(string name = "Checklist") {
        this.name = name;
        Init();
    }

    /// <summary>
    /// Sets the checklist's width.
    /// </summary>
    internal void Init() {
        width = Raylib.MeasureTextEx(Program.theme.font32, name, Program.theme.checklistTitleSize * Program.uiScale, 1f).X;
    }

    /// <summary>
    /// Renders the checklist at the position provided by Program.cs
    /// </summary>
    /// <param name="position">Physical position</param>
    internal void Render(Vector2 position) {
        // background
        Raylib.DrawRectangleV(position,
            new(width,
            MathF.Max(Raylib.GetScreenHeight() - (Program.theme.menuBarButtonSize * Program.uiScale) - Program.theme.padding * 3,
            Program.theme.checklistElementSize * Program.uiScale * list.Count)),
            Program.theme.checklistBgColor);

        // list
    }

    /// <summary>
    /// Adds a new item to the list
    /// </summary>
    /// <param name="key">Name of item. This will silently fail if it already exists</param>
    public void Add(string key) {
        if (list.ContainsKey(key)) return;

        list.Add(key, false);
    }

    /// <summary>
    /// Removes an item from the list
    /// </summary>
    /// <param name="key">Name of item. This will silently fail if it doesn't exist</param>
    public void Remove(string key) {
        if (!list.ContainsKey(key)) return;

        list.Remove(key);
    }
}
