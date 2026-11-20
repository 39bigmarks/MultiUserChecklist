using Raylib_cs;
using System.Numerics;

namespace Checklist;

internal class ChecklistObject
{
    public string name = "Checklist A";
    public Dictionary<string, bool> list = new() {
        {"Objective A", true },
        {"Objective B", false },
        {"Objective C", false },
        {"Objective D", false },
        {"Objective Esadasasdas", false },
        {"Objective F", false },
        {"Objective G", false },
    };

    public float width = 300f;

    public ChecklistObject() {
        Init();
    }

    internal void Init() {
        width = Raylib.MeasureText(name, 30);

        for(int i = 0; i < list.Count; i++) {
            if ((i == 0 && list.ElementAt(0).Key.Length > width) || (i > 0 && list.ElementAt(i).Key.Length > list.ElementAt(i-1).Key.Length))
                width = Raylib.MeasureText(list.ElementAt(i).Key, 20);
        }
        
        if(list.Count > 0)
            width += 100;
    }

    internal void Render(Vector2 position) {
        Raylib.DrawRectangleV(position, new(width, 60 + (40 * list.Count)), new(32, 32, 40));

        for(int i = 0; i < list.Count; i++) {
            if(i % 2 == 0) {
                Raylib.DrawRectangleV(new(position.X, position.Y + 55 + 40 * i), new(width, 40), new(23, 23, 30));
            }
            Raylib.DrawTextEx(Raylib.GetFontDefault(), list.ElementAt(i).Key, new(position.X + 25, position.Y + 65 + 40 * i), 20, 1, Color.RayWhite);
        }

        Raylib.DrawTextEx(Raylib.GetFontDefault(), name, new(position.X + 10, position.Y + 10), 30, 1, Color.RayWhite);
    }

    public void Add(string key) {
        if (list.ContainsKey(key)) return;

        list.Add(key, false);
    }

    public void Remove(string key) {
        if (!list.ContainsKey(key)) return;

        list.Remove(key);
    }
}
