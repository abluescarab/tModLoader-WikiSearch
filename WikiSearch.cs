using Terraria.ModLoader;

namespace WikiSearch {
    public class WikiSearch : Mod {

        WikiSearch() {
            ContentAutoloadingEnabled = true;
        }

        public override object Call(params object[] args) {
            string call = args[0] as string;
            Mod mod = args[1] as Mod;

            if(call.Equals("RegisterMod") && mod != null) {
                WikiSearchSystem.RegisterMod(args[1] as Mod, args[2] as string);
            }

            return null;
        }

        public static void Log(string message, params object[] args) {
            Logging.PublicLogger.InfoFormat(message, args);
        }
    }
}
