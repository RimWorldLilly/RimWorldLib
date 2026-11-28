using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023FF RID: 9215
	public class WorldSelector
	{
		// Token: 0x170028BB RID: 10427
		// (get) Token: 0x0600E675 RID: 58997 RVA: 0x0055BA83 File Offset: 0x00559C83
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x170028BC RID: 10428
		// (get) Token: 0x0600E676 RID: 58998 RVA: 0x0055BA9D File Offset: 0x00559C9D
		// (set) Token: 0x0600E677 RID: 58999 RVA: 0x0055BAA5 File Offset: 0x00559CA5
		public PlanetTile SelectedTile
		{
			get
			{
				return this.selectedTile;
			}
			set
			{
				if (this.selectedTile.Layer != value.Layer)
				{
					this.SelectedLayer = value.Layer;
				}
				this.selectedTile = value;
			}
		}

		// Token: 0x170028BD RID: 10429
		// (get) Token: 0x0600E678 RID: 59000 RVA: 0x0055BACF File Offset: 0x00559CCF
		// (set) Token: 0x0600E679 RID: 59001 RVA: 0x0055BAEC File Offset: 0x00559CEC
		public PlanetLayer SelectedLayer
		{
			get
			{
				if (this.selectedLayer == null)
				{
					return Find.WorldGrid.Surface;
				}
				return this.selectedLayer;
			}
			set
			{
				if (this.selectedLayer != value)
				{
					this.selectedTile = PlanetTile.Invalid;
					this.SelectedObjects.Clear();
				}
				this.selectedLayer = value;
				if (this.selectedLayer.Def.isSpace)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Orbit, OpportunityType.GoodToKnow);
				}
			}
		}

		// Token: 0x170028BE RID: 10430
		// (get) Token: 0x0600E67A RID: 59002 RVA: 0x0055BB3C File Offset: 0x00559D3C
		public List<WorldObject> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x170028BF RID: 10431
		// (get) Token: 0x0600E67B RID: 59003 RVA: 0x0055BB44 File Offset: 0x00559D44
		public WorldObject SingleSelectedObject
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		// Token: 0x170028C0 RID: 10432
		// (get) Token: 0x0600E67C RID: 59004 RVA: 0x0055BB62 File Offset: 0x00559D62
		public WorldObject FirstSelectedObject
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		// Token: 0x170028C1 RID: 10433
		// (get) Token: 0x0600E67D RID: 59005 RVA: 0x0055BB7F File Offset: 0x00559D7F
		public int NumSelectedObjects
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x170028C2 RID: 10434
		// (get) Token: 0x0600E67E RID: 59006 RVA: 0x0055BB8C File Offset: 0x00559D8C
		public bool AnyObjectOrTileSelected
		{
			get
			{
				return this.NumSelectedObjects != 0 || this.selectedTile.Valid;
			}
		}

		// Token: 0x170028C3 RID: 10435
		// (get) Token: 0x0600E67F RID: 59007 RVA: 0x0055BBA4 File Offset: 0x00559DA4
		public bool AnyCaravanSelected
		{
			get
			{
				for (int i = 0; i < this.selected.Count; i++)
				{
					if (this.selected[i] is Caravan)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600E680 RID: 59008 RVA: 0x0055BBDD File Offset: 0x00559DDD
		public void WorldSelectorOnGUI()
		{
			this.HandleWorldClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
		}

		// Token: 0x0600E681 RID: 59009 RVA: 0x0055BC10 File Offset: 0x00559E10
		private void HandleWorldClicks()
		{
			if (Event.current.type == EventType.mouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MousePositionOnUIInverted;
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.selected.Count > 0)
				{
					if (this.selected.Count == 1 && this.selected[0] is Caravan)
					{
						Caravan caravan = (Caravan)this.selected[0];
						if (caravan.IsPlayerControlled && !FloatMenuMakerWorld.TryMakeFloatMenu(caravan))
						{
							this.AutoOrderToTile(caravan, GenWorld.MouseTile(false));
						}
					}
					else
					{
						for (int i = 0; i < this.selected.Count; i++)
						{
							Caravan caravan2 = this.selected[i] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled)
							{
								this.AutoOrderToTile(caravan2, GenWorld.MouseTile(false));
							}
						}
					}
					Event.current.Use();
				}
			}
			if (Event.current.rawType == EventType.mouseUp)
			{
				if (Event.current.button == 0 && this.dragBox.active)
				{
					this.dragBox.active = false;
					if (!this.dragBox.IsValid)
					{
						this.SelectUnderMouse(true);
					}
					else
					{
						this.SelectInsideDragBox();
					}
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600E682 RID: 59010 RVA: 0x0055BD93 File Offset: 0x00559F93
		public bool IsSelected(WorldObject obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x0600E683 RID: 59011 RVA: 0x0055BDA1 File Offset: 0x00559FA1
		public void ClearSelection()
		{
			WorldSelectionDrawer.Clear();
			this.selected.Clear();
			this.selectedTile = PlanetTile.Invalid;
		}

		// Token: 0x0600E684 RID: 59012 RVA: 0x0055BDBE File Offset: 0x00559FBE
		public void Deselect(WorldObject obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x0600E685 RID: 59013 RVA: 0x0055BDDC File Offset: 0x00559FDC
		public void Select(WorldObject obj, bool playSound = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.");
				return;
			}
			this.selectedTile = PlanetTile.Invalid;
			if (this.selected.Count >= 80)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				WorldSelectionDrawer.Notify_Selected(obj);
			}
		}

		// Token: 0x0600E686 RID: 59014 RVA: 0x0055BE37 File Offset: 0x0055A037
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		// Token: 0x0600E687 RID: 59015 RVA: 0x0055BE45 File Offset: 0x0055A045
		private void PlaySelectionSoundFor(WorldObject obj)
		{
			SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600E688 RID: 59016 RVA: 0x0055BE54 File Offset: 0x0055A054
		private void SelectInsideDragBox()
		{
			if (!this.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			bool flag = false;
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Caravan> list = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
				for (int i = 0; i < list.Count; i++)
				{
					flag = true;
					this.Select(list[i], true);
				}
			}
			if (!flag && Current.ProgramState == ProgramState.Playing)
			{
				List<Thing> list2 = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
				for (int j = 0; j < list2.Count; j++)
				{
					if (!flag)
					{
						CameraJumper.TryJumpAndSelect(list2[j], CameraJumper.MovementMode.Pan);
						flag = true;
					}
					else
					{
						Find.Selector.Select(list2[j], true, true);
					}
				}
			}
			if (!flag)
			{
				List<WorldObject> list3 = WorldObjectSelectionUtility.MultiSelectableWorldObjectsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<WorldObject>();
				if (list3.Any((WorldObject x) => x is Caravan))
				{
					list3.RemoveAll((WorldObject x) => !(x is Caravan));
					if (list3.Any((WorldObject x) => x.Faction == Faction.OfPlayer))
					{
						list3.RemoveAll((WorldObject x) => x.Faction != Faction.OfPlayer);
					}
				}
				for (int k = 0; k < list3.Count; k++)
				{
					flag = true;
					this.Select(list3[k], true);
				}
			}
			if (!flag)
			{
				bool canSelectTile = this.dragBox.Diagonal < 30f;
				this.SelectUnderMouse(canSelectTile);
			}
		}

		// Token: 0x0600E689 RID: 59017 RVA: 0x0055C018 File Offset: 0x0055A218
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse()
		{
			bool flag;
			bool flag2;
			return this.SelectableObjectsUnderMouse(out flag, out flag2);
		}

		// Token: 0x0600E68A RID: 59018 RVA: 0x0055C030 File Offset: 0x0055A230
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse(out bool clickedDirectlyOnCaravan, out bool usedColonistBar)
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(mousePositionOnUIInverted);
				if (caravan != null)
				{
					clickedDirectlyOnCaravan = true;
					usedColonistBar = true;
					return Gen.YieldSingle<WorldObject>(caravan);
				}
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			clickedDirectlyOnCaravan = false;
			if (list.Count > 0 && list[0] is Caravan && list[0].DistanceToMouse(UI.MousePositionOnUI) < GenWorldUI.CaravanDirectClickRadius)
			{
				clickedDirectlyOnCaravan = true;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					WorldObject worldObject = list[i];
					if (worldObject is Caravan && worldObject.DistanceToMouse(UI.MousePositionOnUI) > GenWorldUI.CaravanDirectClickRadius)
					{
						list.Remove(worldObject);
					}
				}
			}
			usedColonistBar = false;
			return list;
		}

		// Token: 0x0600E68B RID: 59019 RVA: 0x0055C0EC File Offset: 0x0055A2EC
		public static IEnumerable<WorldObject> SelectableObjectsAt(PlanetTile tile)
		{
			WorldSelector.<SelectableObjectsAt>d__37 <SelectableObjectsAt>d__ = new WorldSelector.<SelectableObjectsAt>d__37(-2);
			<SelectableObjectsAt>d__.<>3__tile = tile;
			return <SelectableObjectsAt>d__;
		}

		// Token: 0x0600E68C RID: 59020 RVA: 0x0055C0FC File Offset: 0x0055A2FC
		private void SelectUnderMouse(bool canSelectTile = true)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Thing thing = Find.ColonistBar.ColonistOrCorpseAt(UI.MousePositionOnUIInverted);
				Pawn pawn = thing as Pawn;
				if (thing != null && (pawn == null || !pawn.IsCaravanMember()))
				{
					if (thing.Spawned)
					{
						CameraJumper.TryJumpAndSelect(thing, CameraJumper.MovementMode.Pan);
						return;
					}
					CameraJumper.TryJump(thing, CameraJumper.MovementMode.Pan);
					return;
				}
			}
			bool flag;
			bool flag2;
			List<WorldObject> list = this.SelectableObjectsUnderMouse(out flag, out flag2).ToList<WorldObject>();
			if (flag2 || (flag && list.Count >= 2))
			{
				canSelectTile = false;
			}
			if (list.Count == 0)
			{
				if (!this.ShiftIsHeld)
				{
					PlanetTile lhs = this.selectedTile;
					this.ClearSelection();
					if (canSelectTile)
					{
						this.selectedTile = GenWorld.MouseTile(false);
						if (lhs != this.selectedTile && this.selectedTile.Valid)
						{
							SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
							return;
						}
					}
				}
			}
			else
			{
				if (list.FirstOrDefault((WorldObject obj) => this.selected.Contains(obj)) != null)
				{
					if (!this.ShiftIsHeld)
					{
						PlanetTile tile = canSelectTile ? GenWorld.MouseTile(false) : PlanetTile.Invalid;
						this.SelectFirstOrNextFrom(list, tile);
						return;
					}
					using (List<WorldObject>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							WorldObject worldObject = enumerator.Current;
							if (this.selected.Contains(worldObject))
							{
								this.Deselect(worldObject);
							}
						}
						return;
					}
				}
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
				}
				this.Select(list[0], true);
			}
		}

		// Token: 0x0600E68D RID: 59021 RVA: 0x0055C288 File Offset: 0x0055A488
		public void SelectFirstOrNextAt(PlanetTile tile)
		{
			this.SelectFirstOrNextFrom(WorldSelector.SelectableObjectsAt(tile).ToList<WorldObject>(), tile);
		}

		// Token: 0x0600E68E RID: 59022 RVA: 0x0055C29C File Offset: 0x0055A49C
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<WorldObject> list = this.SelectableObjectsUnderMouse().ToList<WorldObject>();
			if (list.Count == 0)
			{
				return;
			}
			Type type = list[0].GetType();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (!(type != allWorldObjects[i].GetType()) && (allWorldObjects[i] == list[0] || allWorldObjects[i].AllMatchingObjectsOnScreenMatchesWith(list[0])) && allWorldObjects[i].VisibleToCameraNow())
				{
					this.Select(allWorldObjects[i], true);
				}
			}
		}

		// Token: 0x0600E68F RID: 59023 RVA: 0x0055C33C File Offset: 0x0055A53C
		private void AutoOrderToTile(Caravan c, PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return;
			}
			if (c.autoJoinable && CaravanExitMapUtility.AnyoneTryingToJoinCaravan(c))
			{
				CaravanExitMapUtility.OpenSomeoneTryingToJoinCaravanDialog(c, delegate
				{
					this.AutoOrderToTileNow(c, tile);
				});
				return;
			}
			this.AutoOrderToTileNow(c, tile);
		}

		// Token: 0x0600E690 RID: 59024 RVA: 0x0055C3B8 File Offset: 0x0055A5B8
		private void AutoOrderToTileNow(Caravan c, PlanetTile tile)
		{
			if (!tile.Valid || (tile == c.Tile && !c.pather.Moving))
			{
				return;
			}
			PlanetTile planetTile = CaravanUtility.BestGotoDestNear(tile, c);
			if (planetTile.Valid)
			{
				c.pather.StartPath(planetTile, null, true, true);
				c.gotoMote.OrderedToTile(planetTile);
				SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600E691 RID: 59025 RVA: 0x0055C424 File Offset: 0x0055A624
		private void SelectFirstOrNextFrom(List<WorldObject> objects, PlanetTile tile)
		{
			int num = objects.FindIndex((WorldObject x) => this.selected.Contains(x));
			PlanetTile planetTile = PlanetTile.Invalid;
			int num2 = -1;
			if (num != -1)
			{
				if (num == objects.Count - 1 || this.selected.Count >= 2)
				{
					if (this.selected.Count >= 2)
					{
						num2 = 0;
					}
					else if (tile.Valid)
					{
						planetTile = tile;
					}
					else
					{
						num2 = 0;
					}
				}
				else
				{
					num2 = num + 1;
				}
			}
			else if (objects.Count == 0)
			{
				planetTile = tile;
			}
			else
			{
				num2 = 0;
			}
			this.ClearSelection();
			if (num2 >= 0)
			{
				this.Select(objects[num2], true);
			}
			this.selectedTile = planetTile;
		}

		// Token: 0x04009270 RID: 37488
		public readonly WorldDragBox dragBox = new WorldDragBox();

		// Token: 0x04009271 RID: 37489
		private readonly List<WorldObject> selected = new List<WorldObject>();

		// Token: 0x04009272 RID: 37490
		private PlanetLayer selectedLayer;

		// Token: 0x04009273 RID: 37491
		private PlanetTile selectedTile = PlanetTile.Invalid;

		// Token: 0x04009274 RID: 37492
		private const int MaxNumSelected = 80;

		// Token: 0x04009275 RID: 37493
		private const float MaxDragBoxDiagonalToSelectTile = 30f;
	}
}
