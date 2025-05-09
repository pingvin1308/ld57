using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Artifacts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Game.Scripts.Levels
{
    public class LevelGenerator : MonoBehaviour
    {
        [field: SerializeField]
        public Player Player { get; private set; }

        [Header("Wall tiles")] [SerializeField]
        private TileBase[] wallTopTiles;

        [SerializeField] private TileBase[] wallBottomTiles;
        [SerializeField] private TileBase[] wallLeftTiles;
        [SerializeField] private TileBase[] wallRightTiles;
        [SerializeField] private TileBase[] wallCornerTLTiles;
        [SerializeField] private TileBase[] wallCornerTRTiles;
        [SerializeField] private TileBase[] wallCornerBLTiles;
        [SerializeField] private TileBase[] wallCornerBRTiles;

        [SerializeField] private TileBase[] wallTTopTiles;
        [SerializeField] private TileBase[] wallTBottomTiles;
        [SerializeField] private TileBase[] wallTLeftTiles;
        [SerializeField] private TileBase[] wallTRightTiles;

        [SerializeField] private TileBase[] wallOuterCornerTLTiles;
        [SerializeField] private TileBase[] wallOuterCornerTRTiles;
        [SerializeField] private TileBase[] wallOuterCornerBLTiles;
        [SerializeField] private TileBase[] wallOuterCornerBRTiles;

        [field: SerializeField]
        public ArtifactSpawner ArtifactSpawner { get; private set; }

        [field: SerializeField]
        public RuleTile FloorRuleTile { get; private set; }

        [Header("Room settings")]
        [field: SerializeField]
        public int MaxRooms { get; private set; }

        [field: SerializeField]
        public int MapWidth { get; private set; }

        [field: SerializeField]
        public int MapHeight { get; private set; }

        [field: SerializeField]
        public int ArtifactsPerRoom { get; private set; }

        [field: SerializeField]
        public LevelBase LevelPrefab { get; private set; }

        [field: SerializeField]
        public LevelSettingsDatabase LevelSettingsDatabase { get; private set; }

        public LevelBase Generate(int nextLevelNumber, LevelData data = null)
        {
            var seed = data?.Seed ?? Random.Range(0, int.MinValue);
            Random.InitState(seed);

            var levelSettings = LevelSettingsDatabase.LevelSettings
                .FirstOrDefault(x => x.LevelNumber == Math.Abs(nextLevelNumber));

            var levelGameObject = Instantiate(LevelPrefab, transform);
            
            levelGameObject.Init(
                data ?? new LevelData(
                    seed: seed,
                    oxygenConsumptionRate: levelSettings?.OxygenConsumptionRate ?? 10,
                    levelNumber: nextLevelNumber,
                    levelType: LevelType.Common
                )   
            );

            var rooms = GenerateFloor(levelGameObject.FloorTilemap);
            GenerateWalls(levelGameObject.FloorTilemap, levelGameObject.WallsTilemap);

            if (data != null)
            {
                for (var i = 0; i < data.Artifacts.Count; i++)
                {
                    var artifactData = data.Artifacts[i];
                    // data.Artifacts.Remove(artifactData);
                    ArtifactSpawner.DropArtifact(artifactData.Position, artifactData, levelGameObject);
                }
            }
            else
            {
                var artifacts = GenerateArtifacts(levelGameObject, rooms);
                levelGameObject.AddArtifacts(artifacts.Select(x => x.Data).ToArray());
            }

            levelGameObject.LevelEnabled.AddListener(Player.OnLevelEnabled);
            return levelGameObject;
        }

        private List<RectInt> GenerateFloor(Tilemap floorTilemap)
        {
            var rooms = new List<RectInt>();

            var maxRooms = MaxRooms;
            var mapWidth = MapWidth;
            var mapHeight = MapHeight;
            {
                var w = Random.Range(10, 15);
                var h = Random.Range(10, 15);
                var x = -(w / 2);
                var y = -(h / 2);
                var newRoom = new RectInt(x, y, w, h);
                rooms.Add(newRoom);
            }

            for (var i = 0; i < maxRooms; i++)
            {
                var w = Random.Range(10, 15);
                var h = Random.Range(10, 15);
                var x = Random.Range(-mapWidth / 2, mapWidth / 2 - w);
                var y = Random.Range(-mapHeight / 2, mapHeight / 2 - h);

                var newRoom = new RectInt(x, y, w, h);
                rooms.Add(newRoom);
            }

            foreach (var room in rooms)
            {
                for (var x = room.x; x < (room.x + room.width); x++)
                {
                    for (var y = room.y; y < (room.y + room.height); y++)
                    {
                        floorTilemap.SetTile(new Vector3Int(x, y, 0), FloorRuleTile);
                    }
                }
            }

            return rooms;
        }

        private List<Artifact> GenerateArtifacts(LevelBase levelGameObject, List<RectInt> rooms)
        {
            var filteredRooms = rooms.Where(x => x.Overlaps(new RectInt(0, 0, 1, 1)) == false);
            var artifacts = new List<Artifact>();

            var usedPositions = new HashSet<Vector2Int>();
            foreach (var room in filteredRooms)
            {
                for (var j = 0; j < ArtifactsPerRoom; j++)
                {
                    Vector2Int pos;

                    var attempts = 0;
                    do
                    {
                        var ax = Random.Range(room.x + 1, room.x + room.width - 1);
                        var ay = Random.Range(room.y + 1, room.y + room.height - 1);
                        pos = new Vector2Int(ax, ay);
                        attempts++;
                    } while (usedPositions.Contains(pos) && attempts < 10);

                    usedPositions.Add(pos);

                    var worldPos = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0);
                    var createdArtifact = ArtifactSpawner.SpawnArtifact(worldPos, levelGameObject);
                    artifacts.Add(createdArtifact);
                }
            }

            return artifacts;
        }

        private void GenerateWalls(Tilemap floorTilemap, Tilemap wallsTilemap)
        {
            var bounds = floorTilemap.cellBounds;

            for (int x = bounds.xMin - 1; x <= bounds.xMax + 1; x++)
            {
                for (int y = bounds.yMin - 1; y <= bounds.yMax + 1; y++)
                {
                    var pos = new Vector3Int(x, y, 0);

                    // Если здесь НЕТ пола, но есть пол РЯДОМ — ставим стену
                    if (!floorTilemap.HasTile(pos))
                    {
                        bool up = floorTilemap.HasTile(pos + Vector3Int.up);
                        bool down = floorTilemap.HasTile(pos + Vector3Int.down);
                        bool left = floorTilemap.HasTile(pos + Vector3Int.left);
                        bool right = floorTilemap.HasTile(pos + Vector3Int.right);

                        TileBase tile = null;

                        // Углы
                        if (down && right) tile = GetRandom(wallCornerTLTiles);
                        else if (down && left) tile = GetRandom(wallCornerTRTiles);
                        else if (up && right) tile = GetRandom(wallCornerBLTiles);
                        else if (up && left) tile = GetRandom(wallCornerBRTiles);
                        if (tile != null)
                        {
                            wallsTilemap.SetTile(pos, tile);
                            continue;
                        }

                        // T-образные стены
                        if (left && right && down && !up) tile = GetRandom(wallTTopTiles);
                        else if (left && right && up && !down) tile = GetRandom(wallTBottomTiles);
                        else if (up && down && right && !left) tile = GetRandom(wallTLeftTiles);
                        else if (up && down && left && !right) tile = GetRandom(wallTRightTiles);
                        if (tile != null)
                        {
                            wallsTilemap.SetTile(pos, tile);
                            continue;
                        }

                        // Края
                        else if (down) tile = GetRandom(wallTopTiles);
                        else if (up) tile = GetRandom(wallBottomTiles);
                        else if (right) tile = GetRandom(wallLeftTiles);
                        else if (left) tile = GetRandom(wallRightTiles);

                        if (tile != null)
                        {
                            wallsTilemap.SetTile(pos, tile);
                            continue;
                        }

                        if (floorTilemap.HasTile(pos + new Vector3Int(-1, 1, 0)) &&
                            !up && !left)
                        {
                            tile = GetRandom(wallOuterCornerTLTiles);
                        }
                        else if (floorTilemap.HasTile(pos + new Vector3Int(1, 1, 0)) &&
                                 !up && !right)
                        {
                            tile = GetRandom(wallOuterCornerTRTiles);
                        }
                        else if (floorTilemap.HasTile(pos + new Vector3Int(-1, -1, 0)) &&
                                 !down && !left)
                        {
                            tile = GetRandom(wallOuterCornerBLTiles);
                        }
                        else if (floorTilemap.HasTile(pos + new Vector3Int(1, -1, 0)) &&
                                 !down && !right)
                        {
                            tile = GetRandom(wallOuterCornerBRTiles);
                        }

                        if (tile != null)
                            wallsTilemap.SetTile(pos, tile);
                    }
                }
            }
        }

        private TileBase GetRandom(TileBase[] tiles)
        {
            if (tiles == null || tiles.Length == 0) return null;
            return tiles[Random.Range(0, tiles.Length)];
        }
    }
}