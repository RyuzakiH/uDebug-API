using System.Linq;

namespace uDebug.API
{
    public static class Utilities
    {

        public static Judge GetJudge(string judge)
        {
            return Constants.Judges.FirstOrDefault(j => j.Value.Equals(judge)).Key;
        }


    }
}
