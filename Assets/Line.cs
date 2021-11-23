using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private Transform fromPath;
    [SerializeField] private Transform toPath;
    [SerializeField] private float t;
    [SerializeField] private Transform startWall;
    [SerializeField] private Transform endWall;
    private const float sphereSize = 0.1f;
    private Vector3 straightCrossing;

    private void OnDrawGizmos()
    {
        DrawPath();
        DrawWall();
        DrawStraightCrossing();
        DrawPlayer();
    }

    private void DrawPath()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(fromPath.position, sphereSize);
        Gizmos.DrawSphere(toPath.position, sphereSize);
        Gizmos.DrawLine(fromPath.position, toPath.position);
    }

    private void DrawWall()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startWall.position, sphereSize);
        Gizmos.DrawSphere(endWall.position, sphereSize);
        Gizmos.DrawLine(startWall.position, endWall.position);
    }

    private void DrawStraightCrossing()
    {
        Gizmos.color = Color.green;
        straightCrossing = StraightCrossing(startWall.position, endWall.position, fromPath.position, toPath.position);
        Gizmos.DrawSphere(straightCrossing, sphereSize);
    }

    private void DrawPlayer()
    {
        Vector3 playerPosition = fromPath.position + Learp(fromPath.position, toPath.position, t);
        Gizmos.color = Color.white;

        float floorT = InverseLearp(fromPath.position, toPath.position, straightCrossing);
        if (floorT <= t)
        {
            Gizmos.color = Color.red;
            playerPosition.x = straightCrossing.x;
            playerPosition.y = straightCrossing.y;
            t = floorT;
        }
        Gizmos.DrawSphere(playerPosition, sphereSize);
    }

    private Vector3 StraightCrossing(Vector3 startWall, Vector3 endWall, Vector3 startPath, Vector3 endPath)
    {
        StraightEquation(startWall, endWall, out float wallM, out float  wallC);
        StraightEquation(startPath, endPath, out float pathM, out float pathC);
        float x = (pathC - wallC) / (wallM - pathM);
        float y = wallM * x + wallC;

        return new Vector3(x, y, 0);
    }

    private void StraightEquation (Vector3 start, Vector3 end, out float m, out float c)
    {
        float catetoOposto = end.y - start.y;
        float catetoAdjacente = end.x - start.x;
        m = catetoOposto / catetoAdjacente;
        c = -m * start.x + start.y;
    }

    private Vector3 Learp(Vector3 start, Vector3 end, float t)
    {
        float distanceLearpX = end.x - start.x;
        float positionLearpX = distanceLearpX * t;
        
        float distanceLearpY = end.y - start.y;
        float positionLearpY = distanceLearpY * t;
        
        return new Vector3(positionLearpX, positionLearpY, 0);
    }

    private float InverseLearp(Vector3 from, Vector3 to, Vector3 point)
    {
        return (point.x - from.x) / (to.x - from.x);
    }
}
