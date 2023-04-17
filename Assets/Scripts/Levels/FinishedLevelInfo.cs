using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FinishedLevelInfo
{
    public Dictionary<string, LevelInfo> finishedLevels = new();

    public FinishedLevelInfo()
    {

    }
}

public class LevelInfo
{
    public string levelName;
    public int stars;
    public bool completed;

    public LevelInfo()
    {

    }
}
