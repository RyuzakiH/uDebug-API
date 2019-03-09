using System.Collections.Generic;

namespace uDebug.API
{
    public class Constants
    {
        public const string BASE_URL = "https://www.udebug.com";


        public static readonly Dictionary<Judge, string> Judges = new Dictionary<Judge, string>()
        {
            { Judge.Toph, "Toph" },
            { Judge.Dev_Skill, "DS" },
            { Judge.CATS, "CATS" },
            { Judge.URI, "URI" },
            { Judge.Facebook_Hacker_Cup, "FBHC" },
            { Judge.Light, "LOJ" },
            { Judge.Google_Code_Jam, "GCJ" },
            { Judge.UVa, "UVa" },
            { Judge.ACM_ICPC_Live_Archive, "LA" }
        };
    }
}
