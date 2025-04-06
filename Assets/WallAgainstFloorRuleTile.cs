using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Wall Against Floor RuleTile")]
public class WallAgainstFloorRuleTile : RuleTile<WallAgainstFloorRuleTile.Neighbor>
{
    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        // 2 = проверять наличие пола
        public const int Floor = 2;
    }

    public Tilemap floorTilemap;

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        if (neighbor == Neighbor.Floor)
        {
            // Это специальная проверка: есть ли ТАЙЛ пола в этой клетке
            return tile != null && tile.name.Contains("Floor");
        }

        return base.RuleMatch(neighbor, tile);
    }
}