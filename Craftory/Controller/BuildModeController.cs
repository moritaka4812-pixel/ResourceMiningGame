using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Craftory.Maps.Buildings;
using Craftory.GameUI;
using Craftory.Maps;
using Craftory.Input;
using Craftory.Core;
using Craftory.Maps.Tiles;
using Craftory.Screens;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics;

namespace Craftory.Controller
{
    public class BuildModeController
    {
        public bool IsActive { get; private set; }

        private WorldUIFactory worldui;

        private Camera camera;
        private ToolPanel toolPanel;
        private MapManager mapManager;
        private BuildType currentBuildType;
        private List<Point> buildTargets = new(); //建設する位置の一時リスト
        private List<Point> invalidTargets = new(); //建設不可の一時リスト
        private bool[,] previewOccupied;
        private List<Point>[,] previewOwner;
        private Vector2 confirmButtonWorldPos;
        private BuildPlacementValidator validator;
        private Point? lastDragOrigin = null;
        private BuildingDirection direction;

        public WorldPanel confirmPanel;

        public BuildModeController(MapManager mapManager, ToolPanel toolPanel, Game1 game, Camera camera, GamePlayScreen screen)
        {
            this.camera = camera;
            this.mapManager = mapManager;
            this.toolPanel = toolPanel;

            this.worldui = new WorldUIFactory(game, camera);

            confirmPanel = worldui.CreateWorldPanel(120, 40);

            var okButton = worldui.CreateWorldTextButton("o", 0, 0, 40, 40);
            var cancelButton = worldui.CreateWorldTextButton("x", 40, 0, 40, 40);
            var rotateButton = worldui.CreateWorldTextButton("R", 80, 0, 40, 40);

            okButton.LeftClicked += () => screen.buildModeController.Confirm();
            cancelButton.LeftClicked += () => screen.buildModeController.Cancel();
            rotateButton.LeftClicked += RotateDirection;

            confirmPanel.AddChild(okButton);
            confirmPanel.AddChild(cancelButton);
            confirmPanel.AddChild(rotateButton);
        }

        public void Start(BuildType type)
        {
            IsActive = true;
            currentBuildType = type;
            buildTargets.Clear();
            invalidTargets.Clear();
            confirmPanel.Visible = false;
            direction = BuildingDirection.Up;
            previewOccupied = new bool[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];
            previewOwner = new List<Point>[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];
            for (int x = 0; x < mapManager.Map.MapSizeX; x++)
                for (int y = 0; y < mapManager.Map.MapSizeY; y++)
                    previewOwner[x, y] = new List<Point>();

            validator = new BuildPlacementValidator(mapManager.Map, previewOccupied);
        }

        public void Cancel()
        {
            confirmPanel.Visible = false;
            IsActive = false;
            buildTargets.Clear();
            invalidTargets.Clear();
            toolPanel.ClearActiveButton();
        }

        public void Confirm()
        {
            if (buildTargets.Count == 0) return;
            foreach (var target in buildTargets)
                mapManager.AddBuilding(currentBuildType, target, direction);

            confirmPanel.Visible = false;
            IsActive = false;
            buildTargets.Clear();
            invalidTargets.Clear();
            toolPanel.ClearActiveButton();
            mapManager.shadowGenerator.MarkDirty();
        }

        public void Update(MouseInput mouse)
        {
            var worldPos = camera.ScreenToWorld(mouse.Current.Position.ToVector2());
            if (confirmPanel.HitTestWorld(worldPos)) return;

            var tilePos = mapManager.Map.WorldToTile(worldPos);
            if (tilePos == null) return;

            var p = tilePos.Value;
            var tile = mapManager.Map.GetTile(p.X, p.Y);

            //左クリック
            if (mouse.LeftClicked())
            {
                HandleBuildTarget(p, tile, worldPos);
            }

            //左ドラッグ
            if (mouse.LeftDragging())
            {
                if(lastDragOrigin != p && //前のタイルと別のタイルをドラッグしている
                    !buildTargets.Contains(p) && //BuildTargetに含まれない
                    !invalidTargets.Contains(p)) //InvalidTargetにも含まれない
                {
                    bool canPlace = validator.CanPlace(BuildingRegistry.Data[currentBuildType], p); //配置が可能か

                    if (canPlace)
                    {
                        HandleBuildTarget(p, tile, worldPos);
                        lastDragOrigin = p; //ドラッグの前タイルを格納
                    }
                    else
                    {
                        lastDragOrigin = p; //追加するタイルではなかったが前タイルは格納
                    }
                }
            }
            else
            {
                lastDragOrigin = null; //マウス右ドラッグされていないならドラッグの前タイルはなし
            }

            confirmPanel.X = (int)confirmButtonWorldPos.X;
            confirmPanel.Y = (int)confirmButtonWorldPos.Y;
        }

        public void Draw(SpriteBatch sb)
        {
            var info = BuildingRegistry.Data[currentBuildType];
            var previewAnim = info.CreateTileAnimation(direction);

            foreach (var origin in buildTargets)
            {
                Vector2 worldPos = new Vector2(origin.X * 32, origin.Y * 32);

                DrawPreviewRotated(sb, previewAnim, worldPos, direction, Color.White * 0.5f);
            }
            foreach (var origin in invalidTargets)
            {
                Vector2 worldPos = new Vector2(origin.X * 32, origin.Y * 32);

                DrawPreviewRotated(sb, previewAnim, worldPos, direction, Color.White * 0.5f);
            }
            confirmPanel.DrawWorld(sb);
        }

        private void HandleBuildTarget(Point p, Tile tile, Vector2 worldPos)
        {
            confirmPanel.Visible = true;
            var info = BuildingRegistry.Data[currentBuildType];

            // pが既存のプレビュー建物の占有タイルか
            var owners = previewOwner[p.X, p.Y];
            if (owners.Count > 0)
            {
                //最後に追加された建物を優先して消去
                var origin = owners.Last();
                RemovePreviewBuilding(origin, info);
                return;
            }

            //以下新規追加の処理
            bool canPlace = validator.CanPlace(info, p); //タイルが設置可能かを判定

            if (canPlace) //建設可能
                buildTargets.Add(p);
            else //建設不可
                invalidTargets.Add(p);

            //仮想マップに追加
            foreach (var pos in info.GetArea(p))
            {
                previewOccupied[pos.X, pos.Y] = true;
                previewOwner[pos.X, pos.Y].Add(p);
            }

            confirmButtonWorldPos = worldPos + new Vector2(10, 10);
        }

        private void RemovePreviewBuilding(Point origin, BuildingInfo info) 
        {
            buildTargets.Remove(origin);
            invalidTargets.Remove(origin);

            for (int x = 0; x < info.SizeInTiles.X; x++)
            {
                for (int y = 0; y < info.SizeInTiles.Y; y++)
                {
                    var pos = new Point(origin.X + x, origin.Y + y);

                    previewOwner[pos.X, pos.Y].Remove(origin);

                    //他の建物が残っていればoccupiedのまま
                    previewOccupied[pos.X, pos.Y] = previewOwner[pos.X, pos.Y].Count > 0;
                }
            }
        }

        private void DrawPreviewRotated(SpriteBatch sb, TileAnimation anim, Vector2 worldPos, BuildingDirection dir, Color color)
        {
            var tex = anim.Texture;
            var frame = anim.GetCurrentFrameRect();

            float rotation = dir switch
            {
                BuildingDirection.Right => 0f,
                BuildingDirection.Down => MathF.PI / 2,
                BuildingDirection.Left => MathF.PI,
                BuildingDirection.Up => -MathF.PI / 2,
                _ => 0f
            };

            // スプライト中心
            Vector2 origin = new(frame.Width / 2f, frame.Height / 2f);

            // タイル左上 + origin = スプライト中心のワールド座標
            Vector2 pos = worldPos + origin;

            // ピクセルスナップは pos に対して行う
            pos.X = (float)Math.Floor(pos.X);
            pos.Y = (float)Math.Floor(pos.Y);

            sb.Draw(
                tex,
                pos,
                frame,
                color,
                rotation,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }

        private void RotateDirection()
        {
            direction = direction switch
            {
                BuildingDirection.Up => BuildingDirection.Right,
                BuildingDirection.Right => BuildingDirection.Down,
                BuildingDirection.Down => BuildingDirection.Left,
                BuildingDirection.Left => BuildingDirection.Up,
                _ => BuildingDirection.Up
            };
        }
    }
}
