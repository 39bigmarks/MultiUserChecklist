using Raylib_cs;
using LiteNetLib;
using System.Numerics;
using static Checklist.UI;
using System.Globalization;

namespace Checklist;

internal class Program
{
    public const int width = 1280, height = 720;

    public static Theme theme = Theme.DarkTheme;
    public static Viewport viewport = new();
    public static MenuBar menuBar = new();

    private static List<Checklist> checklists = new() {
    };

    /// <summary>
    /// Main window init and loop.
    /// Renders every checklist.
    /// </summary>
    static async Task Main(string[] args) {
        //Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(width, height, "Multi-User Checklist");
        Raylib.SetTargetFPS(60);
        Raylib.MaximizeWindow();
        //Raylib.SetExitKey(KeyboardKey.Null);
        
        for(int i = 0; i < checklists.Count; i++)
            checklists[i].UpdateValues();

        await theme.LoadTheme();

        viewport = new();
        menuBar = new();

        for(int i = 0; i < checklists.Count; i++)
            checklists[i].UpdateWidth();

        float cursor = theme.Padding;

        Button newListBtn = new("New Checklist") {
            clickEvent = NewList
        };

        while (!Raylib.WindowShouldClose()) {

            viewport.Update();
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(theme.BackgroundColor);

            // render checklists
            for(int i = 0; i < checklists.Count + 1; i++) {

                // render end button
                if(i == checklists.Count) {
                    newListBtn.rec = new(cursor + ((theme.Padding * 2 + theme.ChecklistSpacing) * i) + viewport.scroll, theme.MenuBarButtonSize + theme.Padding * 2, 100, 200);
                    newListBtn.Update();
                    break;
                }

                checklists[i].Render(new(cursor + ((theme.Padding * 2 + theme.ChecklistSpacing) * i) + viewport.scroll, theme.MenuBarButtonSize + theme.Padding * 2));
                cursor += checklists[i].width + theme.Padding;
            }

            // reset cursor at end
            cursor = theme.Padding;

            // update menu bar
            menuBar.Update();

            Raylib.EndDrawing();
        }
        theme.Unload();
        menuBar.Unload();
        Checklist.Unload();

        Raylib.CloseWindow();
    }
    
    /// <summary>
    /// Adds a new checklist
    /// </summary>
    /// <param name="data">Whether it should auto scroll to the new checklist</param>
    internal static void NewList(object[] data) {
        checklists.Add(new($"Checklist {checklists.Count + 1}", new() {
            {"Item 1", new()}
        }));

        if (data.Length == 0) return;

        bool setScrollToList = (bool)data[0];
        if (setScrollToList && Settings.ScrollToNewList) {
            float scrollAmnt = 0;

            for(int i = 0; i < checklists.Count - 1; i++) {
                scrollAmnt += checklists[i].width + theme.ChecklistSpacing + theme.Padding * 3;
            }

            viewport.scroll = -scrollAmnt;
        }
    }
    
    /// <summary>
    /// Removes a checklist
    /// </summary>
    /// <param name="data">The index of the checklist to remove</param>
    internal static void RemoveList(object[] data) {
        int index = (int)data[0];

        if(index == checklists.Count - 1) {
            checklists.RemoveAt(index);
            return;
        }

        for(int i = index + 1; i < checklists.Count; i++) {
            checklists[i - 1] = checklists[i];
        }
        checklists.RemoveAt(checklists.Count - 1);
    }
}
