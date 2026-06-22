using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using ResourceMiningGame.Maps.Buildings;
using ResourceMiningGame.GameUI;
using ResourceMiningGame.Maps;
using ResourceMiningGame.Input;
using ResourceMiningGame.Core;
using ResourceMiningGame.Maps.Tiles;
using ResourceMiningGame.Maps.Buildings;
using ResourceMiningGame.Screens;
using System.ComponentModel.DataAnnotations;

namespace ResourceMiningGame.Controller
{
    public class BuildModeController
    {
        public bool IsActive { get; private set; }

        private WorldUIFactory worldui;

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

        public WorldPanel confirmPanel;

        public BuildModeController(MapManager mapManager, ToolPanel toolPanel, Game1 game, Camera camera, GamePlayScreen screen)
        {
            this.mapManager = mapManager;
            this.toolPanel = toolPanel;

            this.worldui = new WorldUIFactory(game, camera);

            confirmPanel = worldui.CreateWorldPanel(80, 40);

            var okButton = worldui.CreateWorldTextButton("o", 0, 0, 40, 40);
            var cancelButton = worldui.CreateWorldTextButton("x", 40, 0, 40, 40);

            okButton.LeftClicked += () => screen.buildModeController.Confirm();
            cancelButton.LeftClicked += () => screen.buildModeController.Cancel();

            confirmPanel.AddChild(okButton);
            confirmPanel.AddChild(cancelButton);
        }

        public void Start(BuildType type)
        {
            IsActive = true;
            currentBuildType = type;
            buildTargets.Clear();
            invalidTargets.Clear();
            confirmPanel.Visible = false;
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
                mapManager.AddBuilding(currentBuildType, target);

            confirmPanel.Visible = false;
            IsActive = false;
            buildTargets.Clear();
            invalidTargets.Clear();
            toolPanel.ClearActiveButton();
            mapManager.shadowGenerator.MarkDirty();
        }

        public void Update(MouseInput mouse, Camera camera)
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
            var previewAnim = info.CreateTileAnimation();

            foreach (var origin in buildTargets)
            {
                Vector2 worldPos = new Vector2(origin.X * 32, origin.Y * 32);

                previewAnim.Draw(sb, worldPos, Color.White * 0.5f);
            }
            foreach (var origin in invalidTargets)
            {
                Vector2 worldPos = new Vector2(origin.X * 32, origin.Y * 32);

                previewAnim.Draw(sb, worldPos, Color.Red * 0.4f);
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

    }
}
