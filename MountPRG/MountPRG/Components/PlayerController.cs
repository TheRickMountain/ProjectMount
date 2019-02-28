using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public enum PlayerState
    {
        IDLE,
        MOVE
    }

    public class PlayerController : Component
    {
        private Tile currTile;
        private Tile nextTile;
        private Tile destTile;

        private Tile newDestTile;

        private PathAStar pathAStar;

        private float movementPerc;
        private float speed = 4f;

        private AnimatedSprite sprite;
        private PlayerState playerState = PlayerState.IDLE;

        private bool inStorage;

        public PlayerController(AnimatedSprite sprite)
            : base(true, false)
        {
            this.sprite = sprite;
        }

        public override void Initialize()
        {
            currTile = destTile = nextTile = GamePlayState.TileMap
                .GetTile((int)(Entity.X / TileMap.TILE_SIZE), (int)(Entity.Y / TileMap.TILE_SIZE));
        }

        public override void Update(GameTime gameTime)
        {
            if(InputManager.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.G))
            {
                GUIManager.ActiveInventoryGUI.AddItem(ItemDatabase.GetItemById(ItemDatabase.WOOD), 100);
            }

            // Пробуем открыть сундук если игрок рядом с ним
            if(InputManager.GetMouseButtonDown(MouseInput.LeftButton))
            {
                if (!inStorage)
                {
                    int x = GamePlayState.Camera.GetCellX();
                    int y = GamePlayState.Camera.GetCellY();

                    Tile tile = GamePlayState.TileMap.GetTile(x, y);
                    if (tile != null)
                    {
                        Entity entity = tile.Entity;
                        if (entity != null)
                        {
                            // Если игрок рядом с предметом в одном из 4-х направлений, то открываем его
                            if (currTile == GamePlayState.TileMap.GetTile(x - 1, y) ||
                                    currTile == GamePlayState.TileMap.GetTile(x + 1, y) ||
                                    currTile == GamePlayState.TileMap.GetTile(x, y - 1) ||
                                    currTile == GamePlayState.TileMap.GetTile(x, y + 1))
                            {
                                // Открываем сундук
                                if (entity.Get<Storage>() != null)
                                {
                                    GUIManager.StorageGUI.Open(entity.Get<Storage>());
                                    inStorage = true;
                                } // Подбираем предмет
                                else if (entity.Get<Gatherable>() != null)
                                {
                                    if (GUIManager.ActiveInventoryGUI.AddItem(entity.Get<Gatherable>().Item, 1) == 0)
                                    {
                                        ResourceBank.ChopSong.Play();
                                        GamePlayState.TileMap.RemoveEntity(x, y);
                                    }
                                }
                            }
                        }
                    }


                }
                else
                {
                    Slot actInvSlot = GUIManager.ActiveInventoryGUI.getSelectedSlot();
                    Slot strSlot = GUIManager.StorageGUI.getSelectedSlot();
                    if(actInvSlot != null && actInvSlot.HasItem)
                    {
                        int itemsLeft = GUIManager.StorageGUI.AddItem(actInvSlot.Item, actInvSlot.Count);
                        if (itemsLeft == 0)
                        {
                            actInvSlot.Clear();
                        }
                        else
                        {
                            actInvSlot.AddItem(actInvSlot.Item, itemsLeft);
                        }
                        
                    }
                    else if(strSlot != null && strSlot.HasItem)
                    {
                        int itemsLeft = GUIManager.ActiveInventoryGUI.AddItem(strSlot.Item, strSlot.Count);
                        if (itemsLeft == 0)
                        {
                            strSlot.Clear();
                        }
                        else
                        {
                            strSlot.AddItem(strSlot.Item, itemsLeft);
                        }
                    }
                }
            }

            if (InputManager.GetMouseButtonDown(MouseInput.RightButton))
            {
                if (!inStorage)
                {
                    Slot actInvSlot = GUIManager.ActiveInventoryGUI.getSelectedSlot();
                    if (actInvSlot != null)
                    {
                        if (actInvSlot.HasItem)
                        {
                            if (actInvSlot.Item.Consumable)
                            {
                                actInvSlot.Clear();
                            }
                        }
                    }
                    else
                    {
                        int x = GamePlayState.Camera.GetCellX();
                        int y = GamePlayState.Camera.GetCellY();
                        Tile tile = GamePlayState.TileMap.GetTile(x, y);
                        if (tile != null && tile.IsWalkable)
                        {
                            if (playerState == PlayerState.MOVE)
                            {
                                newDestTile = tile;
                            }
                            else
                            {
                                SetDestTile(tile,
                                        GamePlayState.TileMap.GetTileGraph().Nodes,
                                        GamePlayState.TileMap);
                            }
                        }
                    }
                }
                else
                {
                    /* Если было нажато правой кнопкой по инвентарю, то предмет из выделенного слота будет
                     * съеден, если по инвентарю нажато не было, ты инвентарь закроется*/
                    Slot actInvSlot = GUIManager.ActiveInventoryGUI.getSelectedSlot();
                    Slot strSlot = GUIManager.StorageGUI.getSelectedSlot();
                    if (actInvSlot != null)
                    {
                        if(actInvSlot.HasItem)
                        {
                            if (actInvSlot.Item.Consumable)
                            {
                                actInvSlot.Clear();
                            }
                        } 
                    }
                    else if (strSlot != null)
                    {
                        if (strSlot.HasItem)
                        {
                            if(strSlot.Item.Consumable)
                            {
                                strSlot.Clear();
                            }      
                        }
                    }
                    else
                    {
                        inStorage = false;
                        GUIManager.StorageGUI.Close();
                    }

                    
                }
            }

            if (currTile.Equals(destTile))
            {
                pathAStar = null;
                playerState = PlayerState.IDLE;
            }

            if (pathAStar != null)
            {

                if (nextTile.Equals(currTile))
                    nextTile = pathAStar.NextTile;
                    

                float distToTravel = MathUtils.Distance(currTile.X, currTile.Y, nextTile.X, nextTile.Y);

                float distThisFrame = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                float percThisFrame = distThisFrame / distToTravel;

                movementPerc += percThisFrame;
                if (movementPerc >= 1)
                {
                    currTile = nextTile;
                    
                    if(newDestTile != null)
                    {
                        SetDestTile(currTile, newDestTile,
                            GamePlayState.TileMap.GetTileGraph().Nodes,
                                GamePlayState.TileMap);
                        newDestTile = null;
                    }

                    movementPerc = 0;
                }

                Entity.X = MathUtils.Lerp(currTile.X, nextTile.X, movementPerc) * TileMap.TILE_SIZE;
                Entity.Y = MathUtils.Lerp(currTile.Y, nextTile.Y, movementPerc) * TileMap.TILE_SIZE;

                playerState = PlayerState.MOVE;
            }

            if (playerState == PlayerState.IDLE)
                sprite.ResetAnimation();
        }

        public void SetDestTile(Tile tile, Dictionary<Tile, Node<Tile>> nodes, TileMap tilemap)
        {
            if (tile.IsWalkable)
            {
                currTile = nextTile = tilemap.GetTile((int)(Entity.X / TileMap.TILE_SIZE), 
                    (int)(Entity.Y / TileMap.TILE_SIZE));
                pathAStar = new PathAStar(currTile, tile, nodes, tilemap);
                if (pathAStar.Length != -1)
                    destTile = tile;
                else
                    pathAStar = null;
            }
        }

        public void SetDestTile(Tile cTile, Tile dTile, Dictionary<Tile, Node<Tile>> nodes, TileMap tilemap)
        {
            if (dTile.IsWalkable)
            {
                currTile = nextTile = cTile;
                pathAStar = new PathAStar(currTile, dTile, nodes, tilemap);
                if (pathAStar.Length != -1)
                    destTile = dTile;
                else
                    pathAStar = null;
            }
        }
    }
}
