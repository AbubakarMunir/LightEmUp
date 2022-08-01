using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
   
    public delegate void EnemyTowerPieceDown(GameObject towerPiece);
    public static EnemyTowerPieceDown OnEnemyTowerPieceDown;
    public static void DoFireOnEnemyTowerPieceDown(GameObject towerPiece) { OnEnemyTowerPieceDown?.Invoke(towerPiece); }

   
}