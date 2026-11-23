using Raylib_cs;
using System.Numerics;
using static Checklist.UI;

namespace Checklist;

public sealed class MenuBar
{
    internal Button btnNew  = new("New",    new(Program.theme.Padding / 2 + ((Program.theme.MenuBarButtonSize + Program.theme.MenuBarButtonSpacing) * 0), Program.theme.Padding / 2), "Assets\\Icons\\New.png",  menuBar: true);
    internal Button btnOpen = new("Open",   new(Program.theme.Padding / 2 + ((Program.theme.MenuBarButtonSize + Program.theme.MenuBarButtonSpacing) * 1), Program.theme.Padding / 2), "Assets\\Icons\\Open.png", menuBar: true);
    internal Button btnSave = new("Save",   new(Program.theme.Padding / 2 + ((Program.theme.MenuBarButtonSize + Program.theme.MenuBarButtonSpacing) * 2), Program.theme.Padding / 2), "Assets\\Icons\\Save.png", menuBar: true);
    internal Button btnAdd =  new("Add",    new(Program.theme.Padding / 2 + ((Program.theme.MenuBarButtonSize + Program.theme.MenuBarButtonSpacing) * 3), Program.theme.Padding / 2), "Assets\\Icons\\Add.png",  menuBar: true);

    public MenuBar() {
        btnAdd.clickEventData = [true]; // scroll to list
        btnAdd.clickEvent = Program.NewList;
    }

    /// <summary>
    /// Renders the menu bar and it's buttons and manages menu hotkey inputs
    /// </summary>
    public void Update() {
        // TODO: hotkeys

        // background rectangle
        Raylib.DrawRectangleV(Vector2.Zero, new(Raylib.GetScreenWidth(), Program.theme.MenuBarButtonSize + Program.theme.Padding), Program.theme.MenuBarColor);
        
        // update buttons
        btnNew.Update();
        btnOpen.Update();
        btnSave.Update();
        btnAdd.Update();
    }

    /// <summary>
    /// Unloads button icons
    /// </summary>
    public void Unload() {
        btnNew.Unload();
        btnOpen.Unload();
        btnSave.Unload();
        btnAdd.Unload();
    }

    public void GenTextureMipmaps() {
        btnNew.GenTextureMipmaps();
        btnOpen.GenTextureMipmaps();
        btnSave.GenTextureMipmaps();
        btnAdd.GenTextureMipmaps();
    }

    public void SetFilter(TextureFilter filter) {
        btnNew.SetFilter(filter);
        btnOpen.SetFilter(filter);
        btnSave.SetFilter(filter);
        btnAdd.SetFilter(filter);
    }
}