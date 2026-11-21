using Raylib_cs;
using LiteNetLib;
using System.Numerics;

namespace Checklist;

internal class Program
{
    public const int width = 1280, height = 720;

    public static Theme theme = Theme.DarkTheme;
    public static float uiScale = 1f;

    private static Viewport viewport = new();
    private static MenuBar menuBar = new();
    private static List<Checklist> checklists = new() {
        new("Shopping list") {
            list = {
                {"Milk", false},
                {"Eggs", false},
                {"Bread", false},
                {"Butter", false},
                {"Ground beef", false},
                {"Salad", false},
                {"Crouton", false},
            }
        },
        new("abcdefg"),
    };

    private static float cursor = 0;

    /// <summary>
    /// Main window loop.
    /// </summary>
    static void Main(string[] args) {

        Raylib.InitWindow(width, height, "MultiUser Checklist");
        Raylib.SetTargetFPS(60);

        theme.LoadTheme();

        while (!Raylib.WindowShouldClose()) {
            viewport.Update();
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(theme.backgroundColor);

            for(int i = 0; i < checklists.Count; i++) {
                checklists[i].Render(new(cursor + viewport.scroll + theme.padding, theme.menuBarButtonSize * uiScale + theme.padding * 2));
                cursor += checklists[i].width * uiScale + theme.padding;
            }
            cursor = 0;

            menuBar.Update();

            Raylib.EndDrawing();
        }
        theme.Unload();
        menuBar.Unload();

        Raylib.CloseWindow();
    }
}
