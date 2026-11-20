using LiteNetLib;
using Raylib_cs;
using System.Numerics;

namespace Checklist;

internal class Program
{
    public const int width = 1280, height = 720;
    internal static Color bgColor = new(12, 12, 16);

    private static Viewport viewport = new();
    private static List<ChecklistObject> checklists = new() {
        new()
    };

    private static float cursor = 0f;

    static void Main(string[] args) {
        Raylib.InitWindow(width, height, "Multiplayer Checklist");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose()) {
            viewport.Update();
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(bgColor);

            for(int i = 0; i < checklists.Count; i++) {
                checklists[i].Render(new(cursor + viewport.scroll, 0));
                cursor += checklists[i].width;
            }
            cursor = 0;

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
