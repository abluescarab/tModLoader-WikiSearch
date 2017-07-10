using Microsoft.Xna.Framework.Input;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace WikiSearch {
    public class HotKey {
        private string name;
        private Keys defaultKey;

        public string Name { get { return name; } }
        public Keys DefaultKey { get { return defaultKey; } }

        public HotKey(string name, Keys defaultKey) {
            this.name = name;
            this.defaultKey = defaultKey;
        }

        public static string GetTriggerName(Mod mod, string name) {
            return mod.Name + ": " + name;
        }

        public static bool JustPressed(Mod mod, string name) {
            return PlayerInput.Triggers.JustPressed.KeyStatus[GetTriggerName(mod, name)];
        }

        public static bool JustReleased(Mod mod, string name) {
            return PlayerInput.Triggers.JustReleased.KeyStatus[GetTriggerName(mod, name)];
        }

        public bool JustPressed(Mod mod) {
            return PlayerInput.Triggers.JustPressed.KeyStatus[GetTriggerName(mod, name)];
        }

        public bool JustReleased(Mod mod) {
            return PlayerInput.Triggers.JustReleased.KeyStatus[GetTriggerName(mod, name)];
        }
    }
}
