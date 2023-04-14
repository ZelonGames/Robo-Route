using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameHelper
{
    public static bool IsUsingMapEditor()
    {
        return SceneManager.GetActiveScene().name == "Editor";
    }

    public static bool IsTesting()
    {
        return SceneManager.GetActiveScene().name == "Test";
    }

    public static void AddConstraint(this Rigidbody2D rigidbody, RigidbodyConstraints2D constraint)
    {
        rigidbody.constraints |= constraint;
    }

    public static void RemoveConstraint(this Rigidbody2D rigidbody, RigidbodyConstraints2D constraint)
    {
        rigidbody.constraints &= ~constraint;
    }
}
