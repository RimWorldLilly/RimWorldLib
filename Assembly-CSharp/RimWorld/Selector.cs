using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D8B RID: 7563
	public class Selector
	{
		// Token: 0x1700245F RID: 9311
		// (get) Token: 0x0600C410 RID: 50192 RVA: 0x0047BECB File Offset: 0x0047A0CB
		public static bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x17002460 RID: 9312
		// (get) Token: 0x0600C411 RID: 50193 RVA: 0x0047BEE5 File Offset: 0x0047A0E5
		public List<object> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x17002461 RID: 9313
		// (get) Token: 0x0600C412 RID: 50194 RVA: 0x0047BEED File Offset: 0x0047A0ED
		public List<object> SelectedObjectsListForReading
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x17002462 RID: 9314
		// (get) Token: 0x0600C413 RID: 50195 RVA: 0x0047BEF5 File Offset: 0x0047A0F5
		public Thing SingleSelectedThing
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				if (this.selected[0] is Thing)
				{
					return (Thing)this.selected[0];
				}
				return null;
			}
		}

		// Token: 0x17002463 RID: 9315
		// (get) Token: 0x0600C414 RID: 50196 RVA: 0x0047BF2D File Offset: 0x0047A12D
		public object FirstSelectedObject
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

		// Token: 0x17002464 RID: 9316
		// (get) Token: 0x0600C415 RID: 50197 RVA: 0x0047BF4A File Offset: 0x0047A14A
		public object SingleSelectedObject
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

		// Token: 0x17002465 RID: 9317
		// (get) Token: 0x0600C416 RID: 50198 RVA: 0x0047BF68 File Offset: 0x0047A168
		public List<Pawn> SelectedPawns
		{
			get
			{
				Selector.tmpSelectedPawns.Clear();
				for (int i = 0; i < this.selected.Count; i++)
				{
					Pawn pawn = this.selected[i] as Pawn;
					if (pawn != null)
					{
						Selector.tmpSelectedPawns.Add(pawn);
					}
				}
				return Selector.tmpSelectedPawns;
			}
		}

		// Token: 0x17002466 RID: 9318
		// (get) Token: 0x0600C417 RID: 50199 RVA: 0x0047BFBA File Offset: 0x0047A1BA
		public int NumSelected
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x17002467 RID: 9319
		// (get) Token: 0x0600C418 RID: 50200 RVA: 0x0047BFC7 File Offset: 0x0047A1C7
		// (set) Token: 0x0600C419 RID: 50201 RVA: 0x0047BFE9 File Offset: 0x0047A1E9
		public Zone SelectedZone
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0] as Zone;
			}
			set
			{
				this.ClearSelection();
				if (value != null)
				{
					this.Select(value, true, true);
				}
			}
		}

		// Token: 0x17002468 RID: 9320
		// (get) Token: 0x0600C41A RID: 50202 RVA: 0x0047BFFD File Offset: 0x0047A1FD
		// (set) Token: 0x0600C41B RID: 50203 RVA: 0x0047C01F File Offset: 0x0047A21F
		public Plan SelectedPlan
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0] as Plan;
			}
			set
			{
				this.ClearSelection();
				if (value != null)
				{
					this.Select(value, true, true);
				}
			}
		}

		// Token: 0x17002469 RID: 9321
		// (get) Token: 0x0600C41C RID: 50204 RVA: 0x0047C034 File Offset: 0x0047A234
		public bool AnyPawnSelected
		{
			get
			{
				for (int i = 0; i < this.selected.Count; i++)
				{
					if (this.selected[i] is Pawn)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600C41D RID: 50205 RVA: 0x0047C070 File Offset: 0x0047A270
		public void SelectorOnGUI()
		{
			this.HandleMapClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
			if (this.NumSelected > 0 && Find.MainTabsRoot.OpenTab == null && !WorldRendererUtility.WorldSelected)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, false);
			}
		}

		// Token: 0x0600C41E RID: 50206 RVA: 0x0047C0D9 File Offset: 0x0047A2D9
		public void SelectorOnGUI_BeforeMainTabs()
		{
			if (this.gotoController.Active)
			{
				this.gotoController.OnGUI();
			}
		}

		// Token: 0x0600C41F RID: 50207 RVA: 0x0047C0F4 File Offset: 0x0047A2F4
		private void HandleMapClicks()
		{
			if (Event.current.type == EventType.mouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MouseMapPosition();
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.SelectedPawns.Any<Pawn>())
				{
					List<FloatMenuOption> list = null;
					FloatMenuContext floatMenuContext = null;
					try
					{
						list = FloatMenuMakerMap.GetOptions(this.SelectedPawns, UI.MouseMapPosition(), out floatMenuContext);
					}
					catch (Exception ex)
					{
						string str = "Error trying to make float menu: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					if (!list.NullOrEmpty<FloatMenuOption>())
					{
						FloatMenuOption autoTakeOption = FloatMenuMakerMap.GetAutoTakeOption(list);
						if (floatMenuContext.IsMultiselect && floatMenuContext.ValidSelectedPawns.Count<Pawn>() > 1 && list.Count == 1 && list[0].isGoto)
						{
							this.HandleMultiselectGoto(floatMenuContext);
						}
						else if (autoTakeOption != null)
						{
							autoTakeOption.Chosen(true, null);
						}
						else
						{
							string title = (!floatMenuContext.IsMultiselect) ? floatMenuContext.FirstSelectedPawn.LabelCap : null;
							FloatMenuMap floatMenuMap = new FloatMenuMap(list, title, UI.MouseMapPosition());
							floatMenuMap.givesColonistOrders = true;
							Find.WindowStack.Add(floatMenuMap);
						}
					}
					else if (floatMenuContext != null && floatMenuContext.IsMultiselect)
					{
						this.HandleMultiselectGoto(floatMenuContext);
					}
					Event.current.Use();
				}
			}
			if (Event.current.rawType == EventType.mouseUp)
			{
				if (Event.current.button == 0)
				{
					if (this.dragBox.active)
					{
						this.dragBox.active = false;
						if (!this.dragBox.IsValid)
						{
							this.SelectUnderMouse();
						}
						else
						{
							this.SelectInsideDragBox();
						}
					}
				}
				else if (Event.current.button == 1 && this.gotoController.Active)
				{
					this.gotoController.FinalizeInteraction();
				}
				Event.current.Use();
			}
			if (this.gotoController.Active && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
			{
				this.gotoController.FinalizeInteraction();
			}
			if (this.gotoController.Active)
			{
				this.gotoController.ProcessInputEvents();
			}
		}

		// Token: 0x0600C420 RID: 50208 RVA: 0x0047C338 File Offset: 0x0047A538
		public bool IsSelected(object obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x0600C421 RID: 50209 RVA: 0x0047C346 File Offset: 0x0047A546
		public void ClearSelection()
		{
			SelectionDrawer.Clear();
			this.selected.Clear();
			this.gotoController.Deactivate();
		}

		// Token: 0x0600C422 RID: 50210 RVA: 0x0047C363 File Offset: 0x0047A563
		public void Deselect(object obj)
		{
			this.DeselectInternal(obj);
		}

		// Token: 0x0600C423 RID: 50211 RVA: 0x0047C36C File Offset: 0x0047A56C
		private void DeselectInternal(object obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x0600C424 RID: 50212 RVA: 0x0047C389 File Offset: 0x0047A589
		public void Select(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
		{
			this.SelectInternal(obj, playSound, forceDesignatorDeselect);
		}

		// Token: 0x0600C425 RID: 50213 RVA: 0x0047C394 File Offset: 0x0047A594
		private void SelectInternal(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.");
				return;
			}
			Thing thing = obj as Thing;
			if (thing == null && !(obj is Zone) && !(obj is Plan))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to select ");
				defaultInterpolatedStringHandler.AppendFormatted<object>(obj);
				defaultInterpolatedStringHandler.AppendLiteral(" which is neither a Thing, Zone, or Plan.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (thing != null && thing.Destroyed)
			{
				Log.Error("Cannot select destroyed thing.");
				return;
			}
			CompSelectProxy compSelectProxy = thing.TryGetComp<CompSelectProxy>();
			if (compSelectProxy != null && compSelectProxy.thingToSelect != null)
			{
				this.SelectInternal(compSelectProxy.thingToSelect, playSound, forceDesignatorDeselect);
				return;
			}
			Pawn pawn = obj as Pawn;
			if (pawn != null && pawn.IsWorldPawn())
			{
				Log.Error("Cannot select world pawns.");
				return;
			}
			if (forceDesignatorDeselect)
			{
				Find.DesignatorManager.Deselect();
			}
			if (this.SelectedZone != null ^ !(obj is Zone) ^ !(obj is Plan))
			{
				this.ClearSelection();
			}
			if (obj is Zone && this.SelectedZone == null)
			{
				this.ClearSelection();
			}
			if (obj is Plan && this.SelectedPlan == null)
			{
				this.ClearSelection();
			}
			Map map = null;
			if (thing != null)
			{
				map = thing.MapHeld;
			}
			else
			{
				Zone zone = obj as Zone;
				if (zone != null)
				{
					map = zone.Map;
				}
				else
				{
					Plan plan = obj as Plan;
					if (plan != null)
					{
						map = plan.Map;
					}
				}
			}
			for (int i = this.selected.Count - 1; i >= 0; i--)
			{
				Map map2 = null;
				Thing thing2 = this.selected[i] as Thing;
				if (thing2 != null)
				{
					map2 = thing2.MapHeld;
				}
				else
				{
					Zone zone2 = this.selected[i] as Zone;
					if (zone2 != null)
					{
						map2 = zone2.Map;
					}
					else
					{
						Plan plan2 = this.selected[i] as Plan;
						if (plan2 != null)
						{
							map2 = plan2.Map;
						}
					}
				}
				if (map2 != map)
				{
					this.Deselect(this.selected[i]);
				}
			}
			if (this.selected.Count >= 200)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (map != Find.CurrentMap)
				{
					Current.Game.CurrentMap = map;
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					IntVec3 cell = (thing != null) ? thing.PositionHeld : ((Zone)obj).Cells[0];
					Find.CameraDriver.JumpToCurrentMapLoc(cell);
				}
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				if (thing != null)
				{
					thing.Notify_ThingSelected();
				}
				SelectionDrawer.Notify_Selected(obj);
			}
		}

		// Token: 0x0600C426 RID: 50214 RVA: 0x0047C61D File Offset: 0x0047A81D
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
			this.gotoController.Deactivate();
		}

		// Token: 0x0600C427 RID: 50215 RVA: 0x0047C638 File Offset: 0x0047A838
		private void PlaySelectionSoundFor(object obj)
		{
			if (obj is Pawn && ((Pawn)obj).Faction == Faction.OfPlayer && ((Pawn)obj).RaceProps.Humanlike)
			{
				SoundDefOf.ColonistSelected.PlayOneShotOnCamera(null);
				return;
			}
			if (obj is Thing || obj is Zone || obj is Plan)
			{
				SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Can't determine selection sound for ");
			defaultInterpolatedStringHandler.AppendFormatted<object>(obj);
			Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600C428 RID: 50216 RVA: 0x0047C6CC File Offset: 0x0047A8CC
		private void SelectInsideDragBox()
		{
			Selector.<>c__DisplayClass42_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			if (!Selector.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			CS$<>8__locals1.selectedSomething = false;
			List<Thing> list = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
			for (int i = 0; i < list.Count; i++)
			{
				CS$<>8__locals1.selectedSomething = true;
				this.Select(list[i], true, true);
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			List<Caravan> list2 = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
			for (int j = 0; j < list2.Count; j++)
			{
				if (!CS$<>8__locals1.selectedSomething)
				{
					CameraJumper.TryJumpAndSelect(list2[j], CameraJumper.MovementMode.Pan);
					CS$<>8__locals1.selectedSomething = true;
				}
				else
				{
					Find.WorldSelector.Select(list2[j], true);
				}
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			CS$<>8__locals1.boxThings = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<Thing>();
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0(new Predicate<Thing>(Selector.<SelectInsideDragBox>g__IsColonist|42_1), new Action<List<Thing>>(SelectorUtility.SortInColonistBarOrder), ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0(new Predicate<Thing>(Selector.<SelectInsideDragBox>g__IsHumanlike|42_2), null, ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0(new Predicate<Thing>(Selector.<SelectInsideDragBox>g__IsResource|42_3), null, ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0(new Predicate<Thing>(Selector.<SelectInsideDragBox>g__IsPawn|42_4), null, ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|42_0((Thing t) => t.def.selectable, null, ref CS$<>8__locals1))
			{
				return;
			}
			foreach (Zone obj in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(this.dragBox.ScreenRect, null).ToList<Zone>())
			{
				CS$<>8__locals1.selectedSomething = true;
				this.Select(obj, true, true);
			}
			foreach (Plan obj2 in ThingSelectionUtility.MultiSelectablePlansInScreenRectDistinct(this.dragBox.ScreenRect, null).ToList<Plan>())
			{
				CS$<>8__locals1.selectedSomething = true;
				this.Select(obj2, true, true);
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			this.SelectUnderMouse();
		}

		// Token: 0x0600C429 RID: 50217 RVA: 0x0047C92C File Offset: 0x0047AB2C
		private IEnumerable<object> SelectableObjectsUnderMouse()
		{
			return new Selector.<SelectableObjectsUnderMouse>d__43(-2);
		}

		// Token: 0x0600C42A RID: 50218 RVA: 0x0047C935 File Offset: 0x0047AB35
		public static IEnumerable<object> SelectableObjectsAt(IntVec3 c, Map map)
		{
			Selector.<SelectableObjectsAt>d__44 <SelectableObjectsAt>d__ = new Selector.<SelectableObjectsAt>d__44(-2);
			<SelectableObjectsAt>d__.<>3__c = c;
			<SelectableObjectsAt>d__.<>3__map = map;
			return <SelectableObjectsAt>d__;
		}

		// Token: 0x0600C42B RID: 50219 RVA: 0x0047C94C File Offset: 0x0047AB4C
		private void SelectUnderMouse()
		{
			Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(UI.MousePositionOnUIInverted);
			if (caravan != null)
			{
				CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
				return;
			}
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			list.SortBy((object x) => (!(x is Building_Storage)) ? 1 : 0);
			if (list.Count == 0)
			{
				if (!Selector.ShiftIsHeld)
				{
					this.ClearSelection();
					return;
				}
			}
			else if (list.Count == 1)
			{
				object obj4 = list[0];
				if (!Selector.ShiftIsHeld)
				{
					this.ClearSelection();
					this.Select(obj4, true, true);
					return;
				}
				if (!this.selected.Contains(obj4))
				{
					this.Select(obj4, true, true);
					return;
				}
				this.Deselect(obj4);
				return;
			}
			else if (list.Count > 1)
			{
				object obj2 = list.FirstOrDefault((object obj) => this.selected.Contains(obj));
				if (obj2 != null)
				{
					if (!Selector.ShiftIsHeld)
					{
						int num = list.IndexOf(obj2) + 1;
						if (num >= list.Count)
						{
							num -= list.Count;
						}
						Thing thing = obj2 as Thing;
						if (thing != null && thing.def.category == ThingCategory.Item && thing.Spawned && thing.Position.GetItemCount(thing.Map) >= 2)
						{
							if (list.All(delegate(object x)
							{
								Thing thing2 = x as Thing;
								if (thing2 != null && thing2.def.category != ThingCategory.Item)
								{
									Building building2 = thing2 as Building;
									return building2 != null && building2.MaxItemsInCell > 1;
								}
								return true;
							}))
							{
								if (obj2 != list[0])
								{
									this.ClearSelection();
									this.Select(list[0], true, true);
									return;
								}
								List<object> list2 = list;
								Building building = list2[list2.Count - 1] as Building;
								if (building != null && building.MaxItemsInCell > 1)
								{
									this.ClearSelection();
									List<object> list3 = list;
									this.Select(list3[list3.Count - 1], true, true);
									return;
								}
								this.ClearSelection();
								this.Select(list[num], true, true);
								return;
							}
						}
						this.ClearSelection();
						this.Select(list[num], true, true);
						return;
					}
					using (List<object>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj3 = enumerator.Current;
							if (this.selected.Contains(obj3))
							{
								this.Deselect(obj3);
							}
						}
						return;
					}
				}
				if (!Selector.ShiftIsHeld)
				{
					this.ClearSelection();
				}
				this.Select(list[0], true, true);
			}
		}

		// Token: 0x0600C42C RID: 50220 RVA: 0x0047CBC4 File Offset: 0x0047ADC4
		public void SelectNextAt(IntVec3 c, Map map)
		{
			if (this.SelectedObjects.Count != 1)
			{
				Log.Error("Cannot select next at with < or > 1 selected.");
				return;
			}
			List<object> list = Selector.SelectableObjectsAt(c, map).ToList<object>();
			int num = list.IndexOf(this.SingleSelectedThing) + 1;
			if (num >= list.Count)
			{
				num -= list.Count;
			}
			this.ClearSelection();
			this.Select(list[num], true, true);
		}

		// Token: 0x0600C42D RID: 50221 RVA: 0x0047CC30 File Offset: 0x0047AE30
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			Selector.<>c__DisplayClass47_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			if (list.Count == 0)
			{
				return;
			}
			CS$<>8__locals1.clickedThing = (list.FirstOrDefault(delegate(object o)
			{
				Pawn pawn = o as Pawn;
				return pawn != null && pawn.Faction == Faction.OfPlayer && !pawn.IsPrisoner;
			}) as Thing);
			if (CS$<>8__locals1.clickedThing == null)
			{
				CS$<>8__locals1.clickedThing = (list.FirstOrDefault((object o) => o is Pawn) as Thing);
			}
			if (CS$<>8__locals1.clickedThing != null && !CS$<>8__locals1.clickedThing.Spawned)
			{
				CS$<>8__locals1.clickedThing = null;
			}
			if (CS$<>8__locals1.clickedThing == null)
			{
				CS$<>8__locals1.clickedThing = (list.FirstOrDefault(delegate(object o)
				{
					Thing thing2 = o as Thing;
					return thing2 != null && !thing2.def.neverMultiSelect;
				}) as Thing);
			}
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			if (CS$<>8__locals1.clickedThing != null)
			{
				Building edifice = CS$<>8__locals1.clickedThing.PositionHeld.GetEdifice(CS$<>8__locals1.clickedThing.MapHeld);
				if (edifice != null)
				{
					Building_Storage building_Storage = edifice as Building_Storage;
					if (building_Storage != null && building_Storage.GetSlotGroup().HeldThings.Contains(CS$<>8__locals1.clickedThing))
					{
						this.Deselect(edifice);
					}
				}
				foreach (object obj in ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(rect))
				{
					Thing thing = (Thing)obj;
					if (this.<SelectAllMatchingObjectUnderMouseOnScreen>g__Validator|47_3(thing.GetInnerIfMinified(), ref CS$<>8__locals1))
					{
						this.Select(thing, true, true);
					}
				}
				return;
			}
			Zone zone = list.FirstOrDefault(delegate(object o)
			{
				Zone zone2 = o as Zone;
				return zone2 != null && zone2.IsMultiselectable;
			}) as Zone;
			if (zone == null)
			{
				return;
			}
			foreach (Zone obj2 in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(rect, zone))
			{
				if (!this.IsSelected(obj2))
				{
					this.Select(obj2, true, true);
				}
			}
		}

		// Token: 0x0600C42E RID: 50222 RVA: 0x0047CE78 File Offset: 0x0047B078
		private void HandleMultiselectGoto(FloatMenuContext context)
		{
			Selector.tmpDraftedGotoPawns.Clear();
			foreach (Pawn pawn in context.allSelectedPawns)
			{
				if (FloatMenuMakerMap.ShouldGenerateFloatMenuForPawn(pawn) && pawn.Drafted)
				{
					Selector.tmpDraftedGotoPawns.Add(pawn);
				}
			}
			if (Selector.tmpDraftedGotoPawns.Count == 1)
			{
				if (FloatMenuOptionProvider_DraftedMove.PawnCanGoto(Selector.tmpDraftedGotoPawns[0], context.ClickedCell))
				{
					FloatMenuOptionProvider_DraftedMove.PawnGotoAction(context.ClickedCell, Selector.tmpDraftedGotoPawns[0], context.ClickedCell);
				}
			}
			else
			{
				IntVec3 mouseCell = CellFinder.StandableCellNear(context.ClickedCell, context.map, 2.9f, null);
				if (mouseCell.IsValid)
				{
					this.gotoController.StartInteraction(mouseCell);
					foreach (Pawn pawn2 in Selector.tmpDraftedGotoPawns)
					{
						this.gotoController.AddPawn(pawn2);
					}
				}
			}
			Selector.tmpDraftedGotoPawns.Clear();
		}

		// Token: 0x0600C431 RID: 50225 RVA: 0x0047D004 File Offset: 0x0047B204
		[CompilerGenerated]
		private bool <SelectInsideDragBox>g__SelectWhere|42_0(Predicate<Thing> predicate, Action<List<Thing>> postProcessor = null, ref Selector.<>c__DisplayClass42_0 A_3)
		{
			Selector.tmp_selectedBoxThings.Clear();
			IEnumerable<Thing> boxThings = A_3.boxThings;
			Func<Thing, bool> <>9__6;
			Func<Thing, bool> predicate2;
			if ((predicate2 = <>9__6) == null)
			{
				predicate2 = (<>9__6 = ((Thing t) => predicate(t)));
			}
			foreach (Thing item in boxThings.Where(predicate2))
			{
				Selector.tmp_selectedBoxThings.Add(item);
			}
			if (Selector.tmp_selectedBoxThings.Any<Thing>())
			{
				if (postProcessor != null)
				{
					postProcessor(Selector.tmp_selectedBoxThings);
				}
				foreach (Thing obj in Selector.tmp_selectedBoxThings)
				{
					this.Select(obj, true, true);
					A_3.selectedSomething = true;
				}
			}
			Selector.tmp_selectedBoxThings.Clear();
			return A_3.selectedSomething;
		}

		// Token: 0x0600C432 RID: 50226 RVA: 0x0047D108 File Offset: 0x0047B308
		[CompilerGenerated]
		internal static bool <SelectInsideDragBox>g__IsColonist|42_1(Thing t)
		{
			return t.def.category == ThingCategory.Pawn && (((Pawn)t).RaceProps.Humanlike || (ModsConfig.BiotechActive && ((Pawn)t).RaceProps.IsMechanoid)) && t.Faction == Faction.OfPlayer;
		}

		// Token: 0x0600C433 RID: 50227 RVA: 0x0047D15D File Offset: 0x0047B35D
		[CompilerGenerated]
		internal static bool <SelectInsideDragBox>g__IsHumanlike|42_2(Thing t)
		{
			return t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike;
		}

		// Token: 0x0600C434 RID: 50228 RVA: 0x0047D17F File Offset: 0x0047B37F
		[CompilerGenerated]
		internal static bool <SelectInsideDragBox>g__IsResource|42_3(Thing t)
		{
			return t.def.CountAsResource;
		}

		// Token: 0x0600C435 RID: 50229 RVA: 0x0047D18C File Offset: 0x0047B38C
		[CompilerGenerated]
		internal static bool <SelectInsideDragBox>g__IsPawn|42_4(Thing t)
		{
			return t.def.category == ThingCategory.Pawn;
		}

		// Token: 0x0600C437 RID: 50231 RVA: 0x0047D1AC File Offset: 0x0047B3AC
		[CompilerGenerated]
		private bool <SelectAllMatchingObjectUnderMouseOnScreen>g__Validator|47_3(Thing t, ref Selector.<>c__DisplayClass47_0 A_2)
		{
			Thing innerIfMinified = A_2.clickedThing.GetInnerIfMinified();
			if (this.IsSelected(t) || t.Faction != innerIfMinified.Faction)
			{
				return false;
			}
			Pawn pawn = innerIfMinified as Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = t as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.HostFaction != pawn.HostFaction)
					{
						return false;
					}
					Pawn_MutantTracker mutant = pawn2.mutant;
					MutantDef mutantDef = (mutant != null) ? mutant.Def : null;
					Pawn_MutantTracker mutant2 = pawn.mutant;
					return mutantDef == ((mutant2 != null) ? mutant2.Def : null) && SelectorUtility.IsEquivalentRace(pawn2, pawn);
				}
			}
			return t.def == innerIfMinified.def;
		}

		// Token: 0x04007012 RID: 28690
		public DragBox dragBox = new DragBox();

		// Token: 0x04007013 RID: 28691
		public MultiPawnGotoController gotoController = new MultiPawnGotoController();

		// Token: 0x04007014 RID: 28692
		private List<object> selected = new List<object>();

		// Token: 0x04007015 RID: 28693
		public const float PawnSelectRadius = 1f;

		// Token: 0x04007016 RID: 28694
		private const int MaxNumSelected = 200;

		// Token: 0x04007017 RID: 28695
		private static readonly List<Pawn> tmpSelectedPawns = new List<Pawn>();

		// Token: 0x04007018 RID: 28696
		private static List<Thing> tmp_selectedBoxThings = new List<Thing>();

		// Token: 0x04007019 RID: 28697
		private static readonly List<Pawn> tmpDraftedGotoPawns = new List<Pawn>();
	}
}
