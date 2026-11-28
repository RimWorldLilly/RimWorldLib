using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001E26 RID: 7718
	[StaticConstructorOnStartup]
	public class WorldTargeter
	{
		// Token: 0x1700250A RID: 9482
		// (get) Token: 0x0600C88E RID: 51342 RVA: 0x00498CE9 File Offset: 0x00496EE9
		public bool IsTargeting
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x1700250B RID: 9483
		// (get) Token: 0x0600C88F RID: 51343 RVA: 0x00498CF4 File Offset: 0x00496EF4
		public PlanetTile ClosestLayerTile
		{
			get
			{
				return this.closestLayerTile;
			}
		}

		// Token: 0x0600C890 RID: 51344 RVA: 0x00498CFC File Offset: 0x00496EFC
		public void BeginTargeting(Func<GlobalTargetInfo, bool> action, bool canTargetTiles, Texture2D mouseAttachment = null, bool closeWorldTabWhenFinished = false, Action onUpdate = null, Func<GlobalTargetInfo, TaggedString> extraLabelGetter = null, Func<GlobalTargetInfo, bool> canSelectTarget = null, PlanetTile? originForClosest = null, bool showCancelButton = false)
		{
			this.action = action;
			this.canTargetTiles = canTargetTiles;
			this.mouseAttachment = mouseAttachment;
			this.closeWorldTabWhenFinished = closeWorldTabWhenFinished;
			this.onUpdate = onUpdate;
			this.extraLabelGetter = extraLabelGetter;
			this.canSelectTarget = canSelectTarget;
			this.originForClosest = (originForClosest ?? PlanetTile.Invalid);
			this.showCancelButton = showCancelButton;
		}

		// Token: 0x0600C891 RID: 51345 RVA: 0x00498D68 File Offset: 0x00496F68
		public void StopTargeting()
		{
			if (this.closeWorldTabWhenFinished)
			{
				CameraJumper.TryHideWorld();
			}
			this.action = null;
			this.canTargetTiles = false;
			this.mouseAttachment = null;
			this.closeWorldTabWhenFinished = false;
			this.onUpdate = null;
			this.extraLabelGetter = null;
			this.originForClosest = PlanetTile.Invalid;
			this.closestLayerTile = PlanetTile.Invalid;
			this.showCancelButton = false;
			this.cachedLayer = null;
			this.cachedOrigin = PlanetTile.Invalid;
			this.cachedClosest = PlanetTile.Invalid;
		}

		// Token: 0x0600C892 RID: 51346 RVA: 0x00498DE8 File Offset: 0x00496FE8
		public void ProcessInputEvents()
		{
			if (Event.current.type == EventType.mouseDown)
			{
				if (Event.current.button == 0 && this.IsTargeting)
				{
					GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
					if ((this.canSelectTarget == null || this.canSelectTarget(arg)) && this.action(arg))
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.IsTargeting)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsTargeting)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.StopTargeting();
				Event.current.Use();
			}
		}

		// Token: 0x0600C893 RID: 51347 RVA: 0x00498EC0 File Offset: 0x004970C0
		public void TargeterOnGUI()
		{
			if (!this.IsTargeting || Mouse.IsInputBlockedNow)
			{
				return;
			}
			Vector2 mousePosition = Event.current.mousePosition;
			Texture2D image = this.mouseAttachment ? this.mouseAttachment : TexCommand.Attack;
			Rect position = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			GUI.DrawTexture(position, image);
			if (this.originForClosest.Valid && this.originForClosest.Layer != PlanetLayer.Selected)
			{
				if (this.cachedLayer != PlanetLayer.Selected || this.cachedOrigin != this.originForClosest)
				{
					this.cachedLayer = PlanetLayer.Selected;
					this.cachedOrigin = this.originForClosest;
					this.closestLayerTile = (this.cachedClosest = PlanetLayer.Selected.GetClosestTile_NewTemp(this.originForClosest, false));
				}
				else
				{
					this.closestLayerTile = this.cachedClosest;
				}
			}
			else
			{
				this.closestLayerTile = PlanetTile.Invalid;
			}
			if (this.extraLabelGetter == null)
			{
				return;
			}
			GUI.color = Color.white;
			TaggedString taggedString = this.extraLabelGetter(this.CurrentTargetUnderMouse());
			if (!taggedString.NullOrEmpty())
			{
				Color color = GUI.color;
				GUI.color = Color.white;
				Rect rect = new Rect(position.xMax, position.y, 9999f, 100f);
				Vector2 vector = Text.CalcSize(taggedString);
				GUI.DrawTexture(new Rect(rect.x - vector.x * 0.1f, rect.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
				GUI.color = color;
				Widgets.Label(rect, taggedString);
			}
			GUI.color = Color.white;
			if (this.showCancelButton)
			{
				Rect rect2 = new Rect((float)UI.screenWidth / 2f - WorldTargeter.ButtonSize.x / 2f - 8f, (float)UI.screenHeight - (WorldTargeter.ButtonSize.y + 8f) + -50f, WorldTargeter.ButtonSize.x + 16f, WorldTargeter.ButtonSize.y + 16f);
				Rect rect3 = rect2.ContractedBy(8f);
				Widgets.DrawWindowBackground(rect2);
				if (Widgets.ButtonText(rect3, "Cancel".Translate(), true, true, true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.StopTargeting();
				}
			}
		}

		// Token: 0x0600C894 RID: 51348 RVA: 0x00499140 File Offset: 0x00497340
		public void TargeterUpdate()
		{
			if (!this.IsTargeting)
			{
				return;
			}
			Vector3 pos = Vector3.zero;
			GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
			if (arg.HasWorldObject)
			{
				pos = arg.WorldObject.DrawPos;
			}
			else if (arg.Tile.Valid)
			{
				pos = Find.WorldGrid.GetTileCenter(arg.Tile);
			}
			if (arg.IsValid && !Mouse.IsInputBlockedNow && (this.canSelectTarget == null || this.canSelectTarget(arg)))
			{
				WorldRendererUtility.DrawQuadTangentialToPlanet(pos, 0.8f * Find.WorldGrid.AverageTileSize, 0.05f, WorldMaterials.CurTargetingMat, 0f, false, false, null);
			}
			if (this.onUpdate != null)
			{
				this.onUpdate();
			}
		}

		// Token: 0x0600C895 RID: 51349 RVA: 0x004991FF File Offset: 0x004973FF
		public bool IsTargetedNow(WorldObject o, List<WorldObject> worldObjectsUnderMouse = null)
		{
			if (!this.IsTargeting)
			{
				return false;
			}
			if (worldObjectsUnderMouse == null)
			{
				worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			}
			return worldObjectsUnderMouse.Any<WorldObject>() && o == worldObjectsUnderMouse[0];
		}

		// Token: 0x0600C896 RID: 51350 RVA: 0x00499230 File Offset: 0x00497430
		private GlobalTargetInfo CurrentTargetUnderMouse()
		{
			if (!this.IsTargeting)
			{
				return GlobalTargetInfo.Invalid;
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			if (list.Any<WorldObject>())
			{
				return list[0];
			}
			if (!this.canTargetTiles)
			{
				return GlobalTargetInfo.Invalid;
			}
			PlanetTile tile = GenWorld.MouseTile(false);
			if (tile.Valid)
			{
				return new GlobalTargetInfo(tile);
			}
			return GlobalTargetInfo.Invalid;
		}

		// Token: 0x04007234 RID: 29236
		private Func<GlobalTargetInfo, bool> action;

		// Token: 0x04007235 RID: 29237
		private bool canTargetTiles;

		// Token: 0x04007236 RID: 29238
		private bool showCancelButton;

		// Token: 0x04007237 RID: 29239
		private Texture2D mouseAttachment;

		// Token: 0x04007238 RID: 29240
		public bool closeWorldTabWhenFinished;

		// Token: 0x04007239 RID: 29241
		private PlanetTile originForClosest;

		// Token: 0x0400723A RID: 29242
		private Action onUpdate;

		// Token: 0x0400723B RID: 29243
		private Func<GlobalTargetInfo, TaggedString> extraLabelGetter;

		// Token: 0x0400723C RID: 29244
		private Func<GlobalTargetInfo, bool> canSelectTarget;

		// Token: 0x0400723D RID: 29245
		private PlanetTile closestLayerTile = PlanetTile.Invalid;

		// Token: 0x0400723E RID: 29246
		private const float BaseFeedbackTexSize = 0.8f;

		// Token: 0x0400723F RID: 29247
		private static readonly Vector2 ButtonSize = new Vector2(150f, 38f);

		// Token: 0x04007240 RID: 29248
		private const int BottomPanelYOffset = -50;

		// Token: 0x04007241 RID: 29249
		private const int Padding = 8;

		// Token: 0x04007242 RID: 29250
		private PlanetLayer cachedLayer;

		// Token: 0x04007243 RID: 29251
		private PlanetTile cachedOrigin;

		// Token: 0x04007244 RID: 29252
		private PlanetTile cachedClosest;
	}
}
