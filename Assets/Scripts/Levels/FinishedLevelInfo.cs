using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FinishedLevelInfo
{
    public Dictionary<int, LevelInfo> finishedLevels = new Dictionary<int, LevelInfo>();

    public FinishedLevelInfo()
    {

    }
}

public class LevelInfo
{
    public string levelName;
    public int levelNumber;
    public int stars;
    public bool cleared;

    public LevelInfo()
    {

    }
}
