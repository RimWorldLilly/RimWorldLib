using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9B RID: 7579
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		// Token: 0x17002486 RID: 9350
		// (get) Token: 0x0600C4A2 RID: 50338 RVA: 0x00480112 File Offset: 0x0047E312
		// (set) Token: 0x0600C4A3 RID: 50339 RVA: 0x0048011A File Offset: 0x0047E31A
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x17002487 RID: 9351
		// (get) Token: 0x0600C4A4 RID: 50340 RVA: 0x00480123 File Offset: 0x0047E323
		// (set) Token: 0x0600C4A5 RID: 50341 RVA: 0x0048012B File Offset: 0x0047E32B
		public float RecentHeight
		{
			get
			{
				return this.recentHeight;
			}
			set
			{
				this.recentHeight = value;
			}
		}

		// Token: 0x17002488 RID: 9352
		// (get) Token: 0x0600C4A6 RID: 50342 RVA: 0x00480134 File Offset: 0x0047E334
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002489 RID: 9353
		// (get) Token: 0x0600C4A7 RID: 50343 RVA: 0x0048013B File Offset: 0x0047E33B
		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x1700248A RID: 9354
		// (get) Token: 0x0600C4A8 RID: 50344 RVA: 0x00480143 File Offset: 0x0047E343
		private List<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x1700248B RID: 9355
		// (get) Token: 0x0600C4A9 RID: 50345 RVA: 0x0048014F File Offset: 0x0047E34F
		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x1700248C RID: 9356
		// (get) Token: 0x0600C4AA RID: 50346 RVA: 0x0048015B File Offset: 0x0047E35B
		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		// Token: 0x1700248D RID: 9357
		// (get) Token: 0x0600C4AB RID: 50347 RVA: 0x00480167 File Offset: 0x0047E367
		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		// Token: 0x1700248E RID: 9358
		// (get) Token: 0x0600C4AC RID: 50348 RVA: 0x00480173 File Offset: 0x0047E373
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - 165f - 35f;
			}
		}

		// Token: 0x1700248F RID: 9359
		// (get) Token: 0x0600C4AD RID: 50349 RVA: 0x00480187 File Offset: 0x0047E387
		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		// Token: 0x17002490 RID: 9360
		// (get) Token: 0x0600C4AE RID: 50350 RVA: 0x00480194 File Offset: 0x0047E394
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell)) && (Find.Selector.SelectedPlan == null || Find.Selector.SelectedPlan.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		// Token: 0x17002491 RID: 9361
		// (get) Token: 0x0600C4AF RID: 50351 RVA: 0x004801F0 File Offset: 0x0047E3F0
		public bool ShouldShowPaneContents
		{
			get
			{
				StorageGroup storageGroup;
				return (this.SelThing == null || !this.SelThing.def.hideInspect || DebugSettings.showHiddenInfo) && (this.TryGetSelectedStorageGroup(out storageGroup) || this.NumSelected == 1);
			}
		}

		// Token: 0x17002492 RID: 9362
		// (get) Token: 0x0600C4B0 RID: 50352 RVA: 0x00480238 File Offset: 0x0047E438
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (Find.ScreenshotModeHandler.Active)
				{
					return null;
				}
				if (this.NumSelected == 1)
				{
					if (this.SelThing != null)
					{
						if (this.SelThing.def.inspectorTabsResolved == null)
						{
							IStorageGroupMember storageGroupMember = this.SelThing as IStorageGroupMember;
							if (storageGroupMember == null || !storageGroupMember.DrawStorageTab)
							{
								goto IL_54;
							}
						}
						return this.SelThing.GetInspectTabs();
					}
					IL_54:
					if (this.SelZone != null)
					{
						return this.SelZone.GetInspectTabs();
					}
				}
				else if (this.Selected.Count > 1)
				{
					if (this.Selected.All((object s) => s is IStorageGroupMember))
					{
						Thing thing = this.Selected.First<object>() as Thing;
						if (thing == null)
						{
							return null;
						}
						return thing.GetInspectTabs();
					}
				}
				return null;
			}
		}

		// Token: 0x0600C4B1 RID: 50353 RVA: 0x00480304 File Offset: 0x0047E504
		public MainTabWindow_Inspect()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.drawInScreenshotMode = false;
			this.soundClose = SoundDefOf.TabClose;
		}

		// Token: 0x0600C4B2 RID: 50354 RVA: 0x0048032C File Offset: 0x0047E52C
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
			Designator selectedDesignator = Find.DesignatorManager.SelectedDesignator;
			if (selectedDesignator != null && (this.AnythingSelected || selectedDesignator.AlwaysDoGuiControls))
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, this.PaneTopY);
			}
		}

		// Token: 0x0600C4B3 RID: 50355 RVA: 0x00480380 File Offset: 0x0047E580
		public override void DoWindowContents(Rect inRect)
		{
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
			if (this.lastSelectedThing != this.SelThing)
			{
				this.SetInitialSizeAndPosition();
				this.lastSelectedThing = this.SelThing;
				return;
			}
			if (this.RequestedTabSize != this.lastTabSize)
			{
				this.SetInitialSizeAndPosition();
				this.lastTabSize = this.RequestedTabSize;
			}
		}

		// Token: 0x0600C4B4 RID: 50356 RVA: 0x004803DA File Offset: 0x0047E5DA
		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		// Token: 0x0600C4B5 RID: 50357 RVA: 0x004803E8 File Offset: 0x0047E5E8
		public void DoPaneContents(Rect rect)
		{
			StorageGroup group;
			if (this.TryGetSelectedStorageGroup(out group))
			{
				InspectPaneFiller.DoPaneContentsForStorageGroup(group, rect);
				return;
			}
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		// Token: 0x0600C4B6 RID: 50358 RVA: 0x0048041C File Offset: 0x0047E61C
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			StorageGroup renamable;
			if (this.NumSelected == 1)
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing != null)
				{
					float num = rect.width - 48f;
					Widgets.InfoCardButton(num, 0f, Find.Selector.SingleSelectedThing);
					lineEndWidth += 24f;
					Pawn p = singleSelectedThing as Pawn;
					if (p != null)
					{
						if (p.playerSettings != null && p.playerSettings.UsesConfigurableHostilityResponse)
						{
							num -= 24f;
							HostilityResponseModeUtility.DrawResponseButton(new Rect(num, 0f, 24f, 24f), p, false);
							lineEndWidth += 24f;
						}
						if ((p.Faction == Faction.OfPlayer && p.RaceProps.Animal && p.RaceProps.hideTrainingTab) || (ModsConfig.BiotechActive && p.IsColonyMech))
						{
							num -= 30f;
							RenameUIUtility.DrawRenameButton(new Rect(num, 0f, 30f, 30f), p);
							lineEndWidth += 30f;
						}
						if (p.guilt != null && p.guilt.IsGuilty)
						{
							num -= 26f;
							Rect rect2 = new Rect(num, 0f, 26f, 26f);
							GUI.DrawTexture(rect2, TexUI.GuiltyTex);
							TooltipHandler.TipRegion(rect2, () => p.guilt.Tip, 6321223);
							lineEndWidth += 26f;
							return;
						}
					}
					else
					{
						IStorageGroupMember storageGroupMember = singleSelectedThing as IStorageGroupMember;
						if (storageGroupMember != null && storageGroupMember.ShowRenameButton)
						{
							num -= 30f;
							Rect rect3 = new Rect(num, 0f, 30f, 30f);
							if (storageGroupMember.Group != null)
							{
								RenameUIUtility.DrawRenameButton(rect3, storageGroupMember.Group);
							}
							else if (singleSelectedThing.Spawned)
							{
								RenameUIUtility.DrawRenameButton(rect3, storageGroupMember);
							}
							lineEndWidth += 30f;
							return;
						}
						CompAnimalPenMarker marker;
						if (singleSelectedThing.Spawned && singleSelectedThing.Faction == Faction.OfPlayer && singleSelectedThing.TryGetComp(out marker))
						{
							num -= 30f;
							RenameUIUtility.DrawRenameButton(new Rect(num, 0f, 30f, 30f), marker);
							lineEndWidth += 30f;
							return;
						}
						CompPilotConsole compPilotConsole;
						if (singleSelectedThing.Spawned && singleSelectedThing.Faction == Faction.OfPlayer && singleSelectedThing.TryGetComp(out compPilotConsole) && compPilotConsole.engine != null)
						{
							num -= 30f;
							RenameUIUtility.DrawRenameButton(new Rect(num, 0f, 30f, 30f), compPilotConsole.engine);
							lineEndWidth += 30f;
							return;
						}
						IRenameable renameable = singleSelectedThing as IRenameable;
						if (renameable != null)
						{
							num -= 30f;
							RenameUIUtility.DrawRenameButton(new Rect(num, 0f, 30f, 30f), renameable);
							lineEndWidth += 30f;
							return;
						}
					}
				}
				else
				{
					if (Find.Selector.SelectedZone != null)
					{
						Rect rect4 = new Rect(rect.width - 30f, 0f, 30f, 30f);
						if (this.ShouldShowSelectNextInCellButton)
						{
							rect4.x -= 24f;
						}
						RenameUIUtility.DrawRenameButton(rect4, Find.Selector.SelectedZone);
						lineEndWidth += 30f;
						return;
					}
					if (Find.Selector.SelectedPlan != null)
					{
						Rect rect5 = new Rect(rect.width - 30f, 0f, 30f, 30f);
						if (this.ShouldShowSelectNextInCellButton)
						{
							rect5.x -= 24f;
						}
						RenameUIUtility.DrawRenameButton(rect5, Find.Selector.SelectedPlan);
						lineEndWidth += 30f;
						return;
					}
				}
			}
			else if (this.TryGetSelectedStorageGroup(out renamable))
			{
				float x = rect.width - 30f;
				RenameUIUtility.DrawRenameButton(new Rect(x, 0f, 30f, 30f), renamable);
				lineEndWidth += 30f;
			}
		}

		// Token: 0x0600C4B7 RID: 50359 RVA: 0x00480828 File Offset: 0x0047EA28
		private bool TryGetSelectedStorageGroup(out StorageGroup group)
		{
			bool flag = true;
			group = null;
			if (Find.Selector.SelectedObjects.Count <= 1)
			{
				group = null;
				return false;
			}
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				IStorageGroupMember storageGroupMember = obj as IStorageGroupMember;
				if (storageGroupMember == null)
				{
					flag = false;
					break;
				}
				if (group == null)
				{
					group = storageGroupMember.Group;
				}
				if (storageGroupMember.Group != group || storageGroupMember.Group == null)
				{
					flag = false;
					break;
				}
			}
			return flag && group != null;
		}

		// Token: 0x0600C4B8 RID: 50360 RVA: 0x004808CC File Offset: 0x0047EACC
		public void SelectNextInCell()
		{
			if (this.NumSelected != 1)
			{
				return;
			}
			Selector selector = Find.Selector;
			if ((selector.SelectedZone == null || selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell)) && (selector.SelectedPlan == null || selector.SelectedPlan.ContainsCell(MainTabWindow_Inspect.lastSelectCell)))
			{
				if (selector.SelectedZone == null && selector.SelectedPlan == null)
				{
					MainTabWindow_Inspect.lastSelectCell = selector.SingleSelectedThing.PositionHeld;
				}
				Map map;
				if (selector.SingleSelectedThing != null)
				{
					map = selector.SingleSelectedThing.MapHeld;
				}
				else if (selector.SelectedZone != null)
				{
					map = selector.SelectedZone.Map;
				}
				else
				{
					if (selector.SelectedPlan == null)
					{
						Log.Error("Attempted to selected next thing in cell but case was not handled.");
						return;
					}
					map = selector.SelectedPlan.Map;
				}
				selector.SelectNextAt(MainTabWindow_Inspect.lastSelectCell, map);
			}
		}

		// Token: 0x0600C4B9 RID: 50361 RVA: 0x00480999 File Offset: 0x0047EB99
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
		}

		// Token: 0x0600C4BA RID: 50362 RVA: 0x004809A7 File Offset: 0x0047EBA7
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x0600C4BB RID: 50363 RVA: 0x004809B0 File Offset: 0x0047EBB0
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x0400705E RID: 28766
		private Type openTabType;

		// Token: 0x0400705F RID: 28767
		private float recentHeight;

		// Token: 0x04007060 RID: 28768
		private static IntVec3 lastSelectCell;

		// Token: 0x04007061 RID: 28769
		private Thing lastSelectedThing;

		// Token: 0x04007062 RID: 28770
		private Vector2 lastTabSize;

		// Token: 0x04007063 RID: 28771
		private const float GuiltyTexSize = 26f;
	}
}
