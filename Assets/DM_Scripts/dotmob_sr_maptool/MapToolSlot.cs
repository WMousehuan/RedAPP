/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

namespace dotmob.sr.maptool
{
	public class MapToolSlot
	{
		public BoardPosition boardPosition;

		private readonly string[] chipColors = new string[6]
		{
			"Red",
			"Orange",
			"Yellow",
			"Green",
			"Blue",
			"Purple"
		};

		public int chipID;

		public ChipType chipType;

		public DropDirection dropDirection;

		private int generatorIndex;

		private int generatorSpecialIndex;

		private int numberChocolateIndex;

		public IBlockType iBlockType;

		public bool isBringDownEnd;

		public bool isBringDownStart;

		public bool isDropLock;

		public bool isGenerator;

		public bool isGeneratorSpecial;

		public bool isNumberChocolate;

		public bool IsNull;

		public bool isRail;

		private GameObject objBackTile;

		private GameObject objBringDownStartOrEnd;

		private GameObject objChip;

		private GameObject objClothButton;

		private GameObject objDropDirection;

		private GameObject objGenerator;

		private GameObject objJellyTile;

		private GameObject objKnot;

		private GameObject objMilkTile;

		private GameObject objObstacle;

		private readonly GameObject objParent;

		private GameObject objRailImage;

		private GameObject objRailInfo;

		private GameObject objRailNextPosition;

		private GameObject objRescueGingerMan;

		private GameObject objRibbon;

		private GameObject objRoadEnterOrExit;

		private GameObject objSafeObs;

		private GameObject objTunnel;

		private GameObject objTutorial;

		private GameObject objYarn;

		private GameObject objWallH;

		private GameObject objWallV;

		private int obsLayerNo;

		private int railNextX = -1;

		private int railNextY = -1;

		[HideInInspector]
		public Text TextGeneratorIndex;

		[HideInInspector]
		public Text TextNumberChocolateIndex;

		public int GeneratorIndex
		{
			get
			{
				return generatorIndex;
			}
			set
			{
				if ((bool)TextGeneratorIndex)
				{
					TextGeneratorIndex.text = value.ToString();
				}
				generatorIndex = value;
			}
		}

		public int GeneratorSpecialIndex
		{
			get
			{
				return generatorSpecialIndex;
			}
			set
			{
				if ((bool)TextGeneratorIndex)
				{
					TextGeneratorIndex.text = value.ToString();
				}
				generatorSpecialIndex = value;
			}
		}

		public int NumberChocolateIndex
		{
			get
			{
				return numberChocolateIndex;
			}
			set
			{
				if ((bool)TextNumberChocolateIndex)
				{
					TextNumberChocolateIndex.text = value.ToString();
				}
				numberChocolateIndex = value;
			}
		}

		public MapToolSlot(BoardPosition pos, GameObject objParent)
		{
			boardPosition = pos;
			this.objParent = objParent;
			Reset();
		}

		private void SetBlock(ChipType _chipType, int _chipID, IBlockType _iblockType = IBlockType.None)
		{
			chipType = _chipType;
			chipID = _chipID;
			iBlockType = _iblockType;
			if (iBlockType != 0)
			{
				chipType = ChipType.None;
			}
		}

		public void Reset()
		{
			dropDirection = DropDirection.Down;
			isDropLock = false;
			isGenerator = false;
			isGeneratorSpecial = false;
			isNumberChocolate = false;
			isBringDownEnd = (isBringDownStart = false);
			isRail = false;
			numberChocolateIndex = 0;
			SetNull(newSetValue: false);
			if (objChip != null)
			{
				chipType = ChipType.None;
				chipID = 0;
			}
			RemoveAllObject();
			if ((bool)objDropDirection)
			{
				UnityEngine.Object.DestroyImmediate(objDropDirection);
				objDropDirection = null;
			}
			CreateChildObject(MonoSingleton<MapToolManager>.Instance.SpriteDropDirection[0], out objDropDirection);
			Image component = objDropDirection.GetComponent<Image>();
			component.color = new Color(1f, 1f, 1f, 0.5f);
			component.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
		}

		private void RemoveAllObject()
		{
			if ((bool)objRoadEnterOrExit)
			{
				UnityEngine.Object.Destroy(objRoadEnterOrExit);
				objRoadEnterOrExit = null;
			}
			if ((bool)objGenerator)
			{
				UnityEngine.Object.Destroy(objGenerator);
				objGenerator = null;
			}
			if ((bool)objChip)
			{
				UnityEngine.Object.Destroy(objChip);
				objChip = null;
			}
			if ((bool)objObstacle)
			{
				UnityEngine.Object.Destroy(objObstacle);
				objObstacle = null;
				RemoveNumberChocolate();
			}
			if ((bool)objBackTile)
			{
				UnityEngine.Object.Destroy(objBackTile);
				objBackTile = null;
			}
			if ((bool)objJellyTile)
			{
				UnityEngine.Object.Destroy(objJellyTile);
				objJellyTile = null;
			}
			if ((bool)objMilkTile)
			{
				UnityEngine.Object.Destroy(objMilkTile);
				objMilkTile = null;
			}
			if ((bool)objTunnel)
			{
				UnityEngine.Object.Destroy(objTunnel);
				objTunnel = null;
			}
			if ((bool)objRibbon)
			{
				UnityEngine.Object.Destroy(objRibbon);
				objRibbon = null;
			}
			if ((bool)objKnot)
			{
				UnityEngine.Object.Destroy(objKnot);
				objKnot = null;
			}
			if ((bool)objYarn)
			{
				UnityEngine.Object.Destroy(objYarn);
				objYarn = null;
			}
			if ((bool)objClothButton)
			{
				UnityEngine.Object.Destroy(objClothButton);
				objClothButton = null;
			}
			if ((bool)objRescueGingerMan)
			{
				UnityEngine.Object.Destroy(objRescueGingerMan);
				objRescueGingerMan = null;
			}
			if ((bool)objBringDownStartOrEnd)
			{
				UnityEngine.Object.Destroy(objBringDownStartOrEnd);
				objBringDownStartOrEnd = null;
			}
			if ((bool)objRailInfo)
			{
				UnityEngine.Object.Destroy(objRailInfo);
				objRailInfo = null;
			}
			if ((bool)objRailImage)
			{
				UnityEngine.Object.Destroy(objRailImage);
				objRailImage = null;
			}
			if ((bool)objRailNextPosition)
			{
				UnityEngine.Object.Destroy(objRailNextPosition);
				objRailNextPosition = null;
			}
			if ((bool)objTutorial)
			{
				UnityEngine.Object.Destroy(objTutorial);
				objTutorial = null;
			}
			if ((bool)objSafeObs)
			{
				UnityEngine.Object.Destroy(objSafeObs);
				objSafeObs = null;
			}
			if ((bool)objWallH)
			{
				UnityEngine.Object.Destroy(objWallH);
				objWallH = null;
			}
			if ((bool)objWallV)
			{
				UnityEngine.Object.Destroy(objWallV);
				objWallV = null;
			}
			RemoveObs();
		}

		private GameObject CreateChildObject(Sprite copyFromSprite, out GameObject targetObject)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = objParent.transform;
			gameObject.transform.localPosition = Vector3.zero;
			Image image = gameObject.AddComponent<Image>();
			targetObject = gameObject;
			gameObject.transform.localScale = objParent.transform.localScale * 0.9f;
			image.sprite = copyFromSprite;
			image.raycastTarget = false;
			ResetSibling();
			return gameObject;
		}

		private void ResetSibling()
		{
			int num = 0;
			if ((bool)objRailImage)
			{
				objRailImage.transform.SetSiblingIndex(num++);
			}
			if ((bool)objRailInfo)
			{
				objRailInfo.transform.SetSiblingIndex(num++);
			}
			if ((bool)objJellyTile)
			{
				objJellyTile.transform.SetSiblingIndex(num++);
			}
			if ((bool)objMilkTile)
			{
				objMilkTile.transform.SetSiblingIndex(num++);
			}
			if ((bool)objBackTile)
			{
				objBackTile.transform.SetSiblingIndex(num++);
			}
			if ((bool)objObstacle && obsLayerNo == 0)
			{
				objObstacle.transform.SetSiblingIndex(num++);
			}
			if ((bool)objChip)
			{
				objChip.transform.SetSiblingIndex(num++);
			}
			if ((bool)objObstacle && obsLayerNo == 1)
			{
				objObstacle.transform.SetSiblingIndex(num++);
			}
			if ((bool)objObstacle && obsLayerNo == 2)
			{
				objObstacle.transform.SetSiblingIndex(num++);
			}
			if ((bool)objTunnel)
			{
				objTunnel.transform.SetSiblingIndex(num++);
			}
			if ((bool)objRibbon)
			{
				objRibbon.transform.SetSiblingIndex(num++);
			}
			if ((bool)objKnot)
			{
				objKnot.transform.SetSiblingIndex(num++);
			}
			if ((bool)objYarn)
			{
				objYarn.transform.SetSiblingIndex(num++);
			}
			if ((bool)objClothButton)
			{
				objClothButton.transform.SetSiblingIndex(num++);
			}
			if ((bool)objDropDirection)
			{
				objDropDirection.transform.SetSiblingIndex(num++);
			}
			if ((bool)objRailNextPosition)
			{
				objRailNextPosition.transform.SetSiblingIndex(num++);
			}
			if ((bool)objRoadEnterOrExit)
			{
				objRoadEnterOrExit.transform.SetSiblingIndex(num++);
			}
			if ((bool)objGenerator)
			{
				objGenerator.transform.SetSiblingIndex(num++);
			}
			if ((bool)objBringDownStartOrEnd)
			{
				objBringDownStartOrEnd.transform.SetSiblingIndex(num++);
			}
			if ((bool)objTutorial)
			{
				objTutorial.transform.SetSiblingIndex(num++);
			}
			if ((bool)objSafeObs)
			{
				objSafeObs.transform.SetSiblingIndex(num++);
			}
			if ((bool)objWallH)
			{
				objWallH.transform.SetSiblingIndex(num++);
			}
			if ((bool)objWallV)
			{
				objWallV.transform.SetSiblingIndex(num++);
			}
		}

		public void SetActiveDirectionLayer(bool isActive)
		{
			if ((bool)objDropDirection)
			{
				objDropDirection.SetActive(!IsNull && isActive);
			}
		}

		public void ChangeNull()
		{
			SetNull(!IsNull);
		}

		public void SetNull(bool newSetValue)
		{
			IsNull = newSetValue;
			if (IsNull)
			{
				objParent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
				chipType = ChipType.None;
				dropDirection = DropDirection.Down;
				RemoveAllObject();
				if ((bool)objDropDirection)
				{
					objDropDirection.SetActive(value: false);
				}
			}
			else
			{
				objParent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				if ((bool)objDropDirection)
				{
					objDropDirection.SetActive(value: true);
				}
			}
		}

		public void RemoveRockCandyTile()
		{
			if (!IsNull && (bool)objBackTile)
			{
				UnityEngine.Object.DestroyImmediate(objBackTile);
				objBackTile = null;
			}
		}

		public void SetRockCandyTile(int level)
		{
			if (!IsNull)
			{
				if ((bool)objBackTile)
				{
					objBackTile.GetComponent<Image>().sprite = MonoSingleton<MapToolManager>.Instance.GetBlockSprite("B" + level);
					return;
				}
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("B" + level), out objBackTile);
				objBackTile.transform.localScale = objParent.transform.localScale;
			}
		}

		public void RemoveJellyTile()
		{
			if ((bool)objJellyTile)
			{
				UnityEngine.Object.Destroy(objJellyTile);
				objJellyTile = null;
			}
		}

		public void SetJellyTile()
		{
			if (objJellyTile == null)
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("J"), out objJellyTile);
			}
		}

		public void RemoveMilkTile()
		{
			if ((bool)objMilkTile)
			{
				UnityEngine.Object.Destroy(objMilkTile);
				objMilkTile = null;
			}
		}

		public void SetMilkTile()
		{
			if (objMilkTile == null)
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("MK"), out objMilkTile);
			}
		}

		public void RemoveTunnel()
		{
			if ((bool)objTunnel)
			{
				UnityEngine.Object.Destroy(objTunnel);
				objTunnel = null;
			}
		}

		public bool SetTunnel(string tunnelImageKey)
		{
			if ((bool)objTunnel)
			{
				UnityEngine.Object.DestroyImmediate(objTunnel);
				objTunnel = null;
				return false;
			}
			CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(tunnelImageKey), out objTunnel);
			return true;
		}

		public void RemoveRibbon()
		{
			if ((bool)objRibbon)
			{
				UnityEngine.Object.Destroy(objRibbon);
				objRibbon = null;
				RemoveObs();
			}
		}

		public bool SetRibbon(string ribbonImageKey)
		{
			if ((bool)objRibbon)
			{
				UnityEngine.Object.DestroyImmediate(objRibbon);
				objRibbon = null;
				return false;
			}
			CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(ribbonImageKey), out objRibbon);
			return true;
		}

		public string GetRibbonName()
		{
			if ((bool)objRibbon && (bool)objRibbon.GetComponent<Image>() && (bool)objRibbon.GetComponent<Image>().sprite)
			{
				return objRibbon.GetComponent<Image>().sprite.name;
			}
			return null;
		}

		public void RemoveKnot()
		{
			if ((bool)objKnot)
			{
				UnityEngine.Object.Destroy(objKnot);
				objKnot = null;
				RemoveObs();
			}
		}

		public bool SetKnot(string knotImageKey)
		{
			if ((bool)objKnot)
			{
				UnityEngine.Object.DestroyImmediate(objKnot);
				objKnot = null;
				return false;
			}
			if (GetRibbonName() == "H" || GetRibbonName() == "V")
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(knotImageKey), out objKnot);
				return true;
			}
			return false;
		}

		public void RemoveYarn()
		{
			if ((bool)objYarn)
			{
				UnityEngine.Object.Destroy(objYarn);
				objYarn = null;
				RemoveObs();
			}
		}

		public bool SetYarn(string yarnImageKey)
		{
			if ((bool)objYarn)
			{
				UnityEngine.Object.DestroyImmediate(objYarn);
				objYarn = null;
				return false;
			}
			CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(yarnImageKey), out objYarn);
			return true;
		}

		public void RemoveClothButton()
		{
			if ((bool)objClothButton)
			{
				UnityEngine.Object.Destroy(objClothButton);
				objClothButton = null;
				RemoveObs();
			}
		}

		public bool SetClothButton(string buttonImageKey)
		{
			if ((bool)objClothButton)
			{
				UnityEngine.Object.DestroyImmediate(objClothButton);
				objClothButton = null;
				return false;
			}
			if ((bool)objYarn && (bool)objYarn.GetComponent<Image>() && (bool)objYarn.GetComponent<Image>().sprite && objYarn.GetComponent<Image>().sprite.name == "yarn")
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(buttonImageKey), out objClothButton);
			}
			return true;
		}

		public void RemoveObs()
		{
			if (!IsNull && (bool)objObstacle)
			{
				UnityEngine.Object.DestroyImmediate(objObstacle);
				objObstacle = null;
				MapData.main.CurrentMapBoardData.SetBlock(boardPosition.x, boardPosition.y, IBlockType.None);
				RemoveNumberChocolate();
			}
		}

		public void SetObs(IBlockType blockType)
		{
			if (IsNull)
			{
				return;
			}
			string blockJsonFormat = MapData.GetBlockJsonFormat(blockType);
			if ((bool)objObstacle)
			{
				if (!(objObstacle.name == blockJsonFormat))
				{
					RemoveObs();
				}
			}
			else if (MapData.IsNumberChocolate(blockType))
			{
				obsLayerNo = MapData.GetBlockLayerNo(blockType);
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(blockType), out objObstacle);
				objObstacle.name = blockJsonFormat;
				if (obsLayerNo == 1)
				{
					SetEraseChip();
				}
				if ((bool)objObstacle)
				{
					GameObject gameObject = Object.Instantiate(MonoSingleton<MapToolManager>.Instance.PrefabGeneratorIndex);
					if ((bool)gameObject)
					{
						gameObject.transform.SetParent(objObstacle.transform, worldPositionStays: false);
						gameObject.transform.localPosition = new Vector3(-32f, -32f, 0f);
						TextNumberChocolateIndex = gameObject.GetComponent<Text>();
						NumberChocolateIndex = MonoSingleton<MapToolManager>.Instance.AddNumberChocolateInOrder(this);
					}
					isNumberChocolate = true;
				}
			}
			else
			{
				obsLayerNo = MapData.GetBlockLayerNo(blockType);
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(blockType), out objObstacle);
				objObstacle.name = blockJsonFormat;
				if (obsLayerNo == 1)
				{
					SetEraseChip();
				}
			}
		}

		public void SetWall(bool isH)
		{
			if (isH)
			{
				if (objWallH == null)
				{
					CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("WH"), out objWallH);
					objWallH.transform.localPosition = new Vector3(0f, 50f);
				}
				else
				{
					RemoveWall(isH: true);
				}
			}
			else if (objWallV == null)
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("WV"), out objWallV);
				objWallV.transform.localPosition = new Vector3(50f, 0f);
			}
			else
			{
				RemoveWall(isH: false);
			}
		}

		public void RemoveWall(bool isH)
		{
			if (isH)
			{
				if ((bool)objWallH)
				{
					UnityEngine.Object.Destroy(objWallH);
					objWallH = null;
				}
			}
			else if ((bool)objWallV)
			{
				UnityEngine.Object.Destroy(objWallV);
				objWallV = null;
			}
		}

		public void SetDirection(DropDirection dir, bool isLock)
		{
			if (!IsNull)
			{
				objDropDirection.SetActive(value: true);
				if (dropDirection != dir || isDropLock != isLock)
				{
					dropDirection = dir;
					isDropLock = isLock;
					Image component = objDropDirection.GetComponent<Image>();
					component.sprite = MonoSingleton<MapToolManager>.Instance.SpriteDropDirection[(int)dropDirection];
					component.color = ((!isDropLock) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 0.5f));
				}
			}
		}

		public bool SetGenerator()
		{
			if (IsNull)
			{
				return false;
			}
			if (isGenerator)
			{
				if ((bool)objGenerator)
				{
					UnityEngine.Object.DestroyImmediate(objGenerator);
					objGenerator = null;
				}
				isGenerator = false;
				MonoSingleton<MapToolManager>.Instance.RemoveGeneratorList(generatorIndex);
			}
			else if (isGeneratorSpecial)
			{
				if ((bool)objGenerator)
				{
					UnityEngine.Object.DestroyImmediate(objGenerator);
					objGenerator = null;
				}
				isGeneratorSpecial = false;
				MonoSingleton<MapToolManager>.Instance.RemoveGeneratorSpecialList(generatorSpecialIndex);
			}
			else
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("G"), out objGenerator);
				if ((bool)objGenerator)
				{
					objGenerator.GetComponent<RectTransform>().sizeDelta = new Vector2(37f, 37f);
					objGenerator.transform.localPosition = new Vector3(-32f, 32f, 0f);
					GameObject gameObject = Object.Instantiate(MonoSingleton<MapToolManager>.Instance.PrefabGeneratorIndex);
					if ((bool)gameObject)
					{
						gameObject.transform.SetParent(objGenerator.transform, worldPositionStays: false);
						gameObject.transform.localPosition = new Vector3(21f, -9f, 0f);
						TextGeneratorIndex = gameObject.GetComponent<Text>();
						GeneratorIndex = MonoSingleton<MapToolManager>.Instance.AddGeneratorList(this);
					}
				}
				isGenerator = true;
			}
			return true;
		}

		public bool SetGeneratorSpecial()
		{
			if (IsNull)
			{
				return false;
			}
			if (isGeneratorSpecial)
			{
				if ((bool)objGenerator)
				{
					UnityEngine.Object.DestroyImmediate(objGenerator);
					objGenerator = null;
				}
				isGeneratorSpecial = false;
				MonoSingleton<MapToolManager>.Instance.RemoveGeneratorSpecialList(generatorSpecialIndex);
			}
			else if (isGenerator)
			{
				if ((bool)objGenerator)
				{
					UnityEngine.Object.DestroyImmediate(objGenerator);
					objGenerator = null;
				}
				isGenerator = false;
				MonoSingleton<MapToolManager>.Instance.RemoveGeneratorList(generatorIndex);
			}
			else
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("GS"), out objGenerator);
				if ((bool)objGenerator)
				{
					objGenerator.GetComponent<RectTransform>().sizeDelta = new Vector2(37f, 37f);
					objGenerator.transform.localPosition = new Vector3(-32f, 32f, 0f);
					GameObject gameObject = Object.Instantiate(MonoSingleton<MapToolManager>.Instance.PrefabGeneratorIndex);
					if ((bool)gameObject)
					{
						gameObject.transform.SetParent(objGenerator.transform, worldPositionStays: false);
						gameObject.transform.localPosition = new Vector3(21f, -9f, 0f);
						TextGeneratorIndex = gameObject.GetComponent<Text>();
						GeneratorSpecialIndex = MonoSingleton<MapToolManager>.Instance.AddGeneratorSpecialList(this);
					}
				}
				isGeneratorSpecial = true;
			}
			return true;
		}

		public bool SetRailImage(string railImageKey)
		{
			if ((bool)objRailImage)
			{
				UnityEngine.Object.DestroyImmediate(objRailImage);
				if ((bool)objRailInfo)
				{
					objRailInfo.GetComponent<Image>().enabled = true;
				}
				objRailImage = null;
				return false;
			}
			CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(railImageKey), out objRailImage);
			if ((bool)objRailInfo && (bool)objRailInfo.GetComponent<Image>())
			{
				objRailInfo.GetComponent<Image>().enabled = false;
			}
			return true;
		}

		public bool SetRailInfo()
		{
			if (IsNull)
			{
				return false;
			}
			if (isRail)
			{
				if ((bool)objRailInfo)
				{
					UnityEngine.Object.DestroyImmediate(objRailInfo);
				}
				if ((bool)objRailImage)
				{
					UnityEngine.Object.DestroyImmediate(objRailImage);
				}
				if ((bool)objRailNextPosition)
				{
					UnityEngine.Object.DestroyImmediate(objRailNextPosition);
				}
				objRailInfo = (objRailImage = (objRailNextPosition = null));
				isRail = false;
				return false;
			}
			CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("Rail"), out objRailInfo);
			isRail = true;
			railNextX = (railNextY = -1);
			return true;
		}

		public void SetNextRail(int x, int y)
		{
			railNextX = x;
			railNextY = y;
			if (!objRailInfo)
			{
				return;
			}
			if (objRailNextPosition == null)
			{
				objRailNextPosition = Object.Instantiate(MonoSingleton<MapToolManager>.Instance.PrefabRailNextText);
				if ((bool)objRailNextPosition)
				{
					objRailNextPosition.transform.SetParent(objParent.transform, worldPositionStays: false);
					objRailNextPosition.transform.localPosition = new Vector3(0f, -34f, 0f);
				}
			}
			if ((bool)objRailNextPosition)
			{
				objRailNextPosition.GetComponent<Text>().text = $"({railNextX},{railNextY})";
			}
		}

		public bool SetBringDown(bool isStart)
		{
			if (IsNull)
			{
				return false;
			}
			if ((isBringDownStart && isStart) || (isBringDownEnd && !isStart))
			{
				if ((bool)objBringDownStartOrEnd)
				{
					UnityEngine.Object.DestroyImmediate(objBringDownStartOrEnd);
					objBringDownStartOrEnd = null;
				}
				isBringDownStart = false;
				isBringDownEnd = false;
			}
			else
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite((!isStart) ? "BDE" : "BDS"), out objBringDownStartOrEnd);
				if ((bool)objBringDownStartOrEnd)
				{
					objBringDownStartOrEnd.GetComponent<RectTransform>().sizeDelta = new Vector2(37f, 37f);
					objBringDownStartOrEnd.transform.localPosition = new Vector3(32f, 32f, 0f);
				}
				isBringDownStart = isStart;
				isBringDownEnd = !isStart;
			}
			return true;
		}

		public bool SetTutorial()
		{
			if (objTutorial != null)
			{
				UnityEngine.Object.DestroyImmediate(objTutorial);
				objTutorial = null;
				return false;
			}
			CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("Tutorial"), out objTutorial);
			if ((bool)objTutorial)
			{
				objTutorial.GetComponent<RectTransform>().sizeDelta = new Vector2(37f, 37f);
				objTutorial.transform.localPosition = new Vector3(32f, -32f, 0f);
			}
			return true;
		}

		public bool SetSafeObs()
		{
			if (objSafeObs != null)
			{
				UnityEngine.Object.DestroyImmediate(objSafeObs);
				objSafeObs = null;
				return false;
			}
			CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite("Safe"), out objSafeObs);
			if ((bool)objSafeObs)
			{
				objSafeObs.GetComponent<RectTransform>().sizeDelta = new Vector2(37f, 37f);
				objSafeObs.transform.localPosition = new Vector3(-32f, -32f, 0f);
			}
			return true;
		}

		public void RemoveRoadGate()
		{
			if ((bool)objRoadEnterOrExit)
			{
				UnityEngine.Object.DestroyImmediate(objRoadEnterOrExit);
				objRoadEnterOrExit = null;
			}
		}

		public void SetRoadGate(bool isGateEnter)
		{
			if (!IsNull)
			{
				if ((bool)objRoadEnterOrExit)
				{
					UnityEngine.Object.DestroyImmediate(objRoadEnterOrExit);
					objRoadEnterOrExit = null;
				}
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite((!isGateEnter) ? "GateExit" : "GateEnter"), out objRoadEnterOrExit);
				objRoadEnterOrExit.GetComponent<RectTransform>().sizeDelta = new Vector2(37f, 37f);
				objRoadEnterOrExit.transform.localPosition = new Vector3(0f, 32f, 0f);
			}
		}

		public void RemoveRescueGingerMan()
		{
			if ((bool)objRescueGingerMan)
			{
				UnityEngine.Object.DestroyImmediate(objRescueGingerMan);
				objRescueGingerMan = null;
			}
		}

		public void SetRescueGingerMan(string sizeName)
		{
			if (MonoSingleton<MapToolManager>.Instance.dicPrefabRescueGingerMan.ContainsKey(sizeName))
			{
				if ((bool)objRescueGingerMan)
				{
					UnityEngine.Object.DestroyImmediate(objRescueGingerMan);
					objRescueGingerMan = null;
				}
				objRescueGingerMan = Object.Instantiate(MonoSingleton<MapToolManager>.Instance.dicPrefabRescueGingerMan[sizeName]);
				objRescueGingerMan.transform.SetParent(MonoSingleton<MapToolManager>.Instance.ObjParentRescueGingerMan.transform, worldPositionStays: false);
				objRescueGingerMan.transform.position = objParent.transform.position;
				objRescueGingerMan.transform.localPosition += new Vector3(-50f, 50f, 0f);
			}
		}

		private int GetChipIdByColorName(string name)
		{
			for (int i = 0; i < chipColors.Length; i++)
			{
				if (name == chipColors[i])
				{
					return i + 1;
				}
			}
			return 0;
		}

		public void SetEraseChip()
		{
			chipType = ChipType.None;
			chipID = 0;
			NewSetChip();
		}

		public void SetChip(ChipType newChipType, int newChipID)
		{
			if (!IsNull && (newChipType != chipType || newChipID != chipID))
			{
				if (newChipType == ChipType.SimpleChip && newChipID == -1)
				{
					chipType = ChipType.Empty;
				}
				else
				{
					chipType = newChipType;
				}
				chipID = newChipID;
				NewSetChip();
			}
		}

		private void NewSetChip()
		{
			if (objChip != null)
			{
				UnityEngine.Object.DestroyImmediate(objChip);
				objChip = null;
			}
			SetBlock(chipType, chipID);
			if (chipType != 0 && (chipType != ChipType.SimpleChip || (chipID != 0 && chipID != -1)))
			{
				CreateChildObject(MonoSingleton<MapToolManager>.Instance.GetBlockSprite(chipType, chipID), out objChip);
			}
		}

		private void RemoveNumberChocolate()
		{
			if (isNumberChocolate)
			{
				MonoSingleton<MapToolManager>.Instance.DeleteNumberChocolateInOrder(this);
				isNumberChocolate = false;
			}
		}
	}
}
