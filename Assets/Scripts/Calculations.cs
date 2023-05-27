using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Calculations
{
    /// <summary>
    /// Moves bottom to top
    /// </summary>
    /// <param name="bottomBoxCollider">The gameobject to move</param>
    /// <param name="topBoxCollider"></param>
    public static void AlignBottomWithTop(this Collider2D bottomBoxCollider, Collider2D topBoxCollider, float offset = 0)
    {
        Bounds bounds = bottomBoxCollider.bounds;
        Vector2 bottomEdge = new Vector2(bounds.center.x, bounds.GetBottomEdge());

        Bounds otherBounds = topBoxCollider.bounds;
        Vector2 topEdge = new Vector2(otherBounds.center.x, otherBounds.GetTopEdge());

        float yOffset = topEdge.y - bottomEdge.y;
        bottomBoxCollider.transform.position = new Vector2(
            bottomBoxCollider.transform.position.x, 
            bottomBoxCollider.transform.position.y + yOffset + offset);
    }

    public static void AlignLeftWithRight(this Collider2D leftBoxCollider, Collider2D rightBoxCollider)
    {
        Bounds leftBounds = leftBoxCollider.bounds;
        Vector2 leftEdge = new Vector2(leftBounds.min.x, leftBounds.center.y);

        Bounds rightBounds = rightBoxCollider.bounds;
        Vector2 rightEdge = new Vector2(rightBounds.max.x, rightBounds.center.y);

        float xOffset = rightEdge.x - leftEdge.x;
        leftBoxCollider.transform.position = new Vector2(
            leftBoxCollider.transform.position.x + xOffset,
            leftBoxCollider.transform.position.y);
    }

    public static void AlignRightWithLeft(this Collider2D rightBoxCollider, Collider2D leftBoxCollider)
    {
        Bounds rightBounds = rightBoxCollider.bounds;
        Vector2 rightEdge = new Vector2(rightBounds.GetRightEdge(), rightBounds.center.y);

        Bounds leftBounds = leftBoxCollider.bounds;
        Vector2 leftEdge = new Vector2(leftBounds.GetLeftEdge(), leftBounds.center.y);

        float xOffset = leftEdge.x - rightEdge.x;
        rightBoxCollider.transform.position = new Vector2(
            rightBoxCollider.transform.position.x + xOffset,
            rightBoxCollider.transform.position.y);
    }

    public static float GetRightEdge(this Bounds bounds)
    {
        return bounds.max.x;
    }

    public static float GetLeftEdge(this Bounds bounds)
    {
        return bounds.min.x;
    }

    public static float GetBottomEdge(this Bounds bounds)
    {
        return bounds.min.y;
    }

    public static float GetTopEdge(this Bounds bounds)
    {
        return bounds.max.y;
    }

    public static bool ListEquals<T>(this List<T> list1, List<T> list2)
    {
        if (list1 == null && list2 == null)
            return true;
        else if (list1 == null || list2 == null || list1.Count != list2.Count)
            return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if (!Equals(list1[i], list2[i]))
                return false;
        }

        return true;
    }
}
