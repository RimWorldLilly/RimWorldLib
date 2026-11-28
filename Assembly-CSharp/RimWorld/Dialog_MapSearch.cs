using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB7 RID: 7351
	[StaticConstructorOnStartup]
	public class Dialog_MapSearch : Dialog_Search<Thing>
	{
		// Token: 0x1700225D RID: 8797
		// (get) Token: 0x0600BCB4 RID: 48308 RVA: 0x00438958 File Offset: 0x00436B58
		protected override List<Thing> SearchSet
		{
			get
			{
				return this.map.listerThings.AllThings;
			}
		}

		// Token: 0x1700225E RID: 8798
		// (get) Token: 0x0600BCB5 RID: 48309 RVA: 0x0043896A File Offset: 0x00436B6A
		protected override bool ShouldClose
		{
			get
			{
				return WorldRendererUtility.WorldSelected || Find.CurrentMap != this.map;
			}
		}

		// Token: 0x1700225F RID: 8799
		// (get) Token: 0x0600BCB6 RID: 48310 RVA: 0x00438985 File Offset: 0x00436B85
		protected override TaggedString SearchLabel
		{
			get
			{
				return "SearchTheMap".Translate();
			}
		}

		// Token: 0x0600BCB7 RID: 48311 RVA: 0x00438991 File Offset: 0x00436B91
		public Dialog_MapSearch(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BCB8 RID: 48312 RVA: 0x004389AC File Offset: 0x00436BAC
		public override void PostOpen()
		{
			base.PostOpen();
			ThingListChangedCallbacks thingListChangedCallbacks = this.map.thingListChangedCallbacks;
			thingListChangedCallbacks.onThingAdded = (Action<Thing>)Delegate.Combine(thingListChangedCallbacks.onThingAdded, new Action<Thing>(this.TryAddElement));
			ThingListChangedCallbacks thingListChangedCallbacks2 = this.map.thingListChangedCallbacks;
			thingListChangedCallbacks2.onThingRemoved = (Action<Thing>)Delegate.Combine(thingListChangedCallbacks2.onThingRemoved, new Action<Thing>(this.TryRemoveElement));
		}

		// Token: 0x0600BCB9 RID: 48313 RVA: 0x00438A1C File Offset: 0x00436C1C
		public override void PostClose()
		{
			base.PostClose();
			ThingListChangedCallbacks thingListChangedCallbacks = this.map.thingListChangedCallbacks;
			thingListChangedCallbacks.onThingAdded = (Action<Thing>)Delegate.Remove(thingListChangedCallbacks.onThingAdded, new Action<Thing>(this.TryAddElement));
			ThingListChangedCallbacks thingListChangedCallbacks2 = this.map.thingListChangedCallbacks;
			thingListChangedCallbacks2.onThingRemoved = (Action<Thing>)Delegate.Remove(thingListChangedCallbacks2.onThingRemoved, new Action<Thing>(this.TryRemoveElement));
		}

		// Token: 0x0600BCBA RID: 48314 RVA: 0x00438A8C File Offset: 0x00436C8C
		protected override void CheckAnyElementRemoved()
		{
			bool flag = false;
			for (int i = this.searchResults.Count - 1; i >= 0; i--)
			{
				Thing thing = this.searchResults.Values[i];
				if (thing == null || thing.Destroyed || thing.MapHeld != this.map || !this.ThingIsVisibleToPlayer(thing))
				{
					this.searchResults.RemoveAt(i);
					this.searchResultsSet.Remove(thing);
					flag = true;
				}
			}
			if (flag)
			{
				this.SetInitialSizeAndPosition();
			}
		}

		// Token: 0x0600BCBB RID: 48315 RVA: 0x00438B0C File Offset: 0x00436D0C
		protected override void DoIcon(Thing element, Rect iconRect)
		{
			Widgets.ThingIcon(iconRect, element, 1f, null, false, 1f, false);
		}

		// Token: 0x0600BCBC RID: 48316 RVA: 0x00438B35 File Offset: 0x00436D35
		protected override void DoLabel(Thing element, Rect labelRect)
		{
			Widgets.Label(labelRect, element.LabelCap);
		}

		// Token: 0x0600BCBD RID: 48317 RVA: 0x00438B44 File Offset: 0x00436D44
		protected override void DoExtraIcon(Thing element, Rect iconRect)
		{
			IThingHolder parentHolder = element.ParentHolder;
			if (((parentHolder != null) ? parentHolder.ParentHolder : null) is Pawn)
			{
				GUI.DrawTexture(iconRect.ContractedBy(2f), Dialog_MapSearch.InventoryIcon);
				return;
			}
			if (!(element.ParentHolder is Map))
			{
				GUI.DrawTexture(iconRect.ContractedBy(2f), Dialog_MapSearch.ContainerIcon);
			}
		}

		// Token: 0x0600BCBE RID: 48318 RVA: 0x00438BA4 File Offset: 0x00436DA4
		protected override void ClikedOnElement(Thing element)
		{
			if (element == null || element.Destroyed || element.MapHeld != this.map)
			{
				return;
			}
			CameraJumper.TryJump(element, CameraJumper.MovementMode.Pan);
			Find.Selector.ClearSelection();
			Thing obj = element;
			IThingHolder parentHolder = element.ParentHolder;
			if (parentHolder != null)
			{
				Pawn pawn = parentHolder.ParentHolder as Pawn;
				if (pawn != null)
				{
					Corpse corpse = pawn.Corpse;
					if (corpse != null)
					{
						obj = corpse;
					}
					else
					{
						obj = pawn;
					}
				}
				else
				{
					Thing thing = parentHolder as Thing;
					if (thing != null)
					{
						obj = thing;
					}
				}
			}
			Find.Selector.Select(obj, true, true);
		}

		// Token: 0x0600BCBF RID: 48319 RVA: 0x00438C2C File Offset: 0x00436E2C
		protected override bool ShouldSkipElement(Thing element)
		{
			if (element == null)
			{
				return true;
			}
			Corpse corpse = element as Corpse;
			if (corpse != null && corpse.Bugged)
			{
				return true;
			}
			MinifiedThing minifiedThing = element as MinifiedThing;
			return minifiedThing != null && minifiedThing.InnerThing == null;
		}

		// Token: 0x0600BCC0 RID: 48320 RVA: 0x00438C68 File Offset: 0x00436E68
		protected override void OnHighlightUpdate(Thing element)
		{
			GenDraw.DrawArrowPointingAt(element.PositionHeld.ToVector3Shifted(), false);
		}

		// Token: 0x0600BCC1 RID: 48321 RVA: 0x00438C8C File Offset: 0x00436E8C
		protected override void SetInitialSizeAndPosition()
		{
			this.scrollHeight = (float)this.searchResults.Count * 26f;
			Vector2 initialSize = this.InitialSize;
			initialSize.y = Mathf.Clamp(initialSize.y + this.scrollHeight, this.InitialSize.y, (float)UI.screenHeight / 2f);
			this.windowRect = new Rect((float)UI.screenWidth - initialSize.x, (float)UI.screenHeight - initialSize.y - 35f, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x0600BCC2 RID: 48322 RVA: 0x00438D24 File Offset: 0x00436F24
		private bool CanAddThing(Thing thing)
		{
			if (!this.quickSearchWidget.filter.Text.NullOrEmpty() && thing != null && thing.def.selectable && !thing.Destroyed && thing.def.showInSearch && thing.MapHeld == this.map && (DebugSettings.searchIgnoresRestrictions || !thing.PositionHeld.Fogged(thing.MapHeld)) && !this.searchResultsSet.Contains(thing))
			{
				Corpse corpse = thing as Corpse;
				if ((corpse == null || !corpse.Bugged) && (base.TextMatch(thing.LabelNoCount.StripTags()) || base.TextMatch(thing.def.label.StripTags())))
				{
					return this.ThingIsVisibleToPlayer(thing);
				}
			}
			return false;
		}

		// Token: 0x0600BCC3 RID: 48323 RVA: 0x00438DF8 File Offset: 0x00436FF8
		private bool ThingIsVisibleToPlayer(Thing thing)
		{
			if (DebugSettings.searchIgnoresRestrictions)
			{
				return true;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.IsHiddenFromPlayer())
			{
				return false;
			}
			IThingHolder parentHolder = thing.ParentHolder;
			if (parentHolder != null)
			{
				Pawn_InventoryTracker pawn_InventoryTracker = parentHolder as Pawn_InventoryTracker;
				if (pawn_InventoryTracker != null && pawn_InventoryTracker.pawn.IsHiddenFromPlayer())
				{
					return false;
				}
				Pawn_EquipmentTracker pawn_EquipmentTracker = parentHolder as Pawn_EquipmentTracker;
				if (pawn_EquipmentTracker != null && pawn_EquipmentTracker.pawn.IsHiddenFromPlayer())
				{
					return false;
				}
				Pawn_ApparelTracker pawn_ApparelTracker = parentHolder as Pawn_ApparelTracker;
				if (pawn_ApparelTracker != null && pawn_ApparelTracker.pawn.IsHiddenFromPlayer())
				{
					return false;
				}
				Pawn_CarryTracker pawn_CarryTracker = parentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null && pawn_CarryTracker.pawn.IsHiddenFromPlayer())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BCC4 RID: 48324 RVA: 0x00438E98 File Offset: 0x00437098
		protected override void TryAddElement(Thing element)
		{
			bool flag = false;
			foreach (Thing thing in this.ContentsFromThing(element))
			{
				this.searchResults.Add(thing.LabelNoParenthesis.ToLower(), thing);
				this.searchResultsSet.Add(thing);
				flag = true;
			}
			if (flag)
			{
				this.SetInitialSizeAndPosition();
			}
		}

		// Token: 0x0600BCC5 RID: 48325 RVA: 0x00438F18 File Offset: 0x00437118
		protected override void TryRemoveElement(Thing thing)
		{
			bool flag = false;
			foreach (Thing thing2 in this.ContentsFromThing(thing))
			{
				int num = this.searchResults.IndexOfValue(thing2);
				if (num >= 0)
				{
					this.searchResults.RemoveAt(num);
					this.searchResultsSet.Remove(thing2);
					flag = true;
				}
			}
			if (flag)
			{
				this.SetInitialSizeAndPosition();
			}
		}

		// Token: 0x0600BCC6 RID: 48326 RVA: 0x00438F9C File Offset: 0x0043719C
		private List<Thing> ContentsFromThing(Thing thing)
		{
			this.tmpContents.Clear();
			if (this.CanAddThing(thing))
			{
				this.tmpContents.Add(thing);
			}
			if (!thing.Faction.IsPlayerSafe() && !(thing is Corpse))
			{
				return this.tmpContents;
			}
			Corpse corpse = thing as Corpse;
			if (corpse != null && !corpse.Bugged)
			{
				thing = corpse.InnerPawn;
			}
			ISearchableContents searchableContents = thing as ISearchableContents;
			if (searchableContents != null)
			{
				ThingOwner searchableContents2 = searchableContents.SearchableContents;
				if (searchableContents2 != null)
				{
					foreach (Thing thing2 in ((IEnumerable<Thing>)searchableContents2))
					{
						if (this.CanAddThing(thing2))
						{
							this.tmpContents.Add(thing2);
						}
					}
				}
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && (pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsAnimal || pawn.Corpse != null))
			{
				if (pawn.equipment != null)
				{
					foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
					{
						if (this.CanAddThing(thingWithComps))
						{
							this.tmpContents.Add(thingWithComps);
						}
					}
				}
				if (pawn.apparel != null)
				{
					foreach (Apparel apparel in pawn.apparel.WornApparel)
					{
						if (this.CanAddThing(apparel))
						{
							this.tmpContents.Add(apparel);
						}
					}
				}
				if (pawn.inventory != null)
				{
					foreach (Thing thing3 in pawn.inventory.innerContainer)
					{
						if (this.CanAddThing(thing3))
						{
							this.tmpContents.Add(thing3);
						}
					}
				}
			}
			ThingWithComps thingWithComps2 = thing as ThingWithComps;
			if (thingWithComps2 != null)
			{
				foreach (ThingComp thingComp in thingWithComps2.AllComps)
				{
					ISearchableContents searchableContents3 = thingComp as ISearchableContents;
					if (searchableContents3 != null)
					{
						ThingOwner searchableContents4 = searchableContents3.SearchableContents;
						if (searchableContents4 != null)
						{
							foreach (Thing thing4 in ((IEnumerable<Thing>)searchableContents4))
							{
								if (this.CanAddThing(thing4))
								{
									this.tmpContents.Add(thing4);
								}
							}
						}
					}
				}
			}
			return this.tmpContents;
		}

		// Token: 0x04006BE8 RID: 27624
		private Map map;

		// Token: 0x04006BE9 RID: 27625
		private static readonly Texture2D InventoryIcon = ContentFinder<Texture2D>.Get("UI/Widgets/SearchBar_Inventory", true);

		// Token: 0x04006BEA RID: 27626
		private static readonly Texture2D ContainerIcon = ContentFinder<Texture2D>.Get("UI/Widgets/SearchBar_Container", true);

		// Token: 0x04006BEB RID: 27627
		private List<Thing> tmpContents = new List<Thing>();
	}
}
