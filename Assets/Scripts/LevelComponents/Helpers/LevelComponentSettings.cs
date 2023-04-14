using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LevelComponentSettings : MonoBehaviour
{
    public Dictionary<string, object> settings = new Dictionary<string, object>();
    CustomPosition startingPosition = new CustomPosition();

    private void Start()
    {
        
    }

    public void Update()
    {
        startingPosition.x = transform.position.x;
        startingPosition.y = transform.position.y;

        UpdateSetting(nameof(startingPosition), startingPosition);
    }

    public void UpdateSetting(string name, object value)
    {
        if (!settings.ContainsKey(name))
            settings.Add(name, value);
        else
            settings[name] = value;
    }
}
