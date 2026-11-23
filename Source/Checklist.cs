using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using static Checklist.UI;

namespace Checklist;

internal sealed class Checklist
{
    public string name;
    public Dictionary<string, Button> list = [];
    private List<bool> listValues = [];
    internal static Texture2D checkmarkOff;
    internal static Texture2D checkmarkOn;
    internal float width;
    internal Guid guid = Guid.NewGuid();
    internal Button addBtn = new("New Item");

    /// <summary>
    /// Asynchronously loads the checkbox textures
    /// </summary>
    public static async Task LoadTextures() {
        checkmarkOff = Raylib.LoadTexture("Assets\\Icons\\CheckOff.png");
        checkmarkOn = Raylib.LoadTexture("Assets\\Icons\\CheckOn.png");

        if(Program.theme.IconFilter > 0) {
            Raylib.GenTextureMipmaps(ref checkmarkOff);
            Raylib.GenTextureMipmaps(ref checkmarkOn);
        }

        Raylib.SetTextureFilter(checkmarkOff, (TextureFilter)Program.theme.IconFilter);
        Raylib.SetTextureFilter(checkmarkOn, (TextureFilter)Program.theme.IconFilter);
    }

    public Checklist(string name = "Checklist", Dictionary<string, Button> list = default) {
        this.name = name;
        this.list = list;

        UpdateWidth();
        UpdateValues();
    }

    /// <summary>
    /// Sets the checklist's width.
    /// </summary>
    internal void UpdateWidth() {
        width = Raylib.MeasureTextEx(Program.theme.font32, name, Program.theme.ChecklistTitleSize, 1f).X;

        for(int i = 0; i < list.Count; i++) {
            float measure = Raylib.MeasureTextEx(Program.theme.font28, list.ElementAt(i).Key, Program.theme.ChecklistItemSize, 1f).X;
            if (width > measure) continue;

            width = measure;
        }

        width += Program.theme.ChecklistWidth;
    }

    /// <summary>
    /// Updates the checkbox values, and adds more of them as false when it maxes out.
    /// </summary>
    internal void UpdateValues() {
        for(int i = 0; i < list.Count; i++) {
            if(i < listValues.Count) {
                list.ElementAt(i).Value.SetIcon(listValues[i] ? checkmarkOn : checkmarkOff);
            } else {
                listValues.Add(false);
                list.ElementAt(i).Value.SetIcon(checkmarkOff);
            }

            list.ElementAt(i).Value.clickOnDown = true;
            list.ElementAt(i).Value.menuBar = true;
            list.ElementAt(i).Value.clickEvent = Toggle;
            list.ElementAt(i).Value.clickEventData = [i];
        }
    }

    internal void Toggle(object[] data) {
        listValues[(int)data[0]] = !listValues[(int)data[0]];
        list.ElementAt((int)data[0]).Value.SetIcon(listValues[(int)data[0]] ? checkmarkOn : checkmarkOff);
    }

    /// <summary>
    /// Renders the checklist at the position provided by Program.cs
    /// </summary>
    /// <param name="position">Physical position</param>
    internal void Render(Vector2 position) {
        // background
        float height = Program.theme.ChecklistTitleSize + Program.theme.Padding * 2 + ((Program.theme.ChecklistItemSize + Program.theme.ChecklistItemSpacing) * list.Count);

        Raylib.DrawRectangleV(position,
            new(width + Program.theme.Padding * 2,
            Program.theme.PadChecklistEmptySpace ?
                MathF.Max((Raylib.GetScreenHeight() - Program.theme.MenuBarButtonSize - Program.theme.Padding * 3),
                height)
            : height),
            Program.theme.ChecklistBgColor);

        // list
        for(int i = 0; i < list.Count + 1; i++) {
            Vector2 textPos =
                new(position.X + Program.theme.Padding,
                    position.Y + Program.theme.ChecklistTitleSize + Program.theme.Padding * 2 + Program.theme.ChecklistItemSpacing / 2 + ((Program.theme.ChecklistItemSize + Program.theme.ChecklistItemSpacing) * i)); // im crine
            Rectangle itemSpaceRec = new(
                new(textPos.X - Program.theme.Padding,
                    textPos.Y - Program.theme.ChecklistItemSpacing / 2),
                new(width + Program.theme.Padding * 2,
                    Program.theme.ChecklistItemSize + Program.theme.ChecklistItemSpacing));

            if (i == list.Count) {
                itemSpaceRec.Shrink(5);
                addBtn.rec = itemSpaceRec;
                addBtn.Update();
                addBtn.clickEvent = Add;
                addBtn.clickEventData = [$"Item {list.Count + 1}"];
                break;
            }

            // dark half background
            if (i % 2 == 0) {
                Raylib.DrawRectangleRec(itemSpaceRec, Program.theme.ChecklistSecondaryBgColor);
            }

            // item label
            Raylib.DrawTextEx(
                Program.theme.font28,
                list.ElementAt(i).Key,
                textPos,
                Program.theme.ChecklistItemSize,
                1,
                Program.theme.ChecklistItemColor);
        }

        // checkbox button
        for (int i = 0; i < list.Count; i++) {
            list.ElementAt(i).Value.rec.Position = new(position.X + width - Program.theme.ChecklistBoxSize + Program.theme.Padding * 2, position.Y + Program.theme.ChecklistTitleSize + Program.theme.Padding * 2 + Program.theme.ChecklistItemSpacing / 2 + ((Program.theme.ChecklistItemSize + Program.theme.ChecklistItemSpacing) * i));
            list.ElementAt(i).Value.rec.Size = new(Program.theme.ChecklistBoxSize);
            list.ElementAt(i).Value.Update();
        }

        // title
        Raylib.DrawTextEx(
            Program.theme.font32,
            name,
            new(position.X + Program.theme.Padding,
                position.Y + Program.theme.Padding),
            Program.theme.ChecklistTitleSize,
            1,
            Program.theme.ChecklistTitleColor);
    }

    /// <summary>
    /// Adds a new item to the list and updates the width of the checklist body
    /// </summary>
    /// <param name="data">Index 0 is name of item. This will silently fail if it already exists</param>
    public void Add(object[] data) {
        if (list.ContainsKey((string)data[0])) return;

        list.Add((string)data[0], new((string)data[0]));
        UpdateWidth();
        UpdateValues();
    }

    /// <summary>
    /// Removes an item from the list and updates the width of the checklist body
    /// </summary>
    /// <param name="key">Index 0 is name of item. This will silently fail if it doesn't exist</param>
    public void Remove(object[] data) {
        if (!list.ContainsKey((string)data[0])) return;

        list.Remove((string)data[0]);
        UpdateWidth();
        UpdateValues();
    }

    /// <summary>
    /// Unloads checkmark textures
    /// </summary>
    public static void Unload() {
        Raylib.UnloadTexture(checkmarkOff);
        Raylib.UnloadTexture(checkmarkOn);
    }
}
