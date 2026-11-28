using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AD8 RID: 6872
	public class Bill_Production : Bill, IRenameable
	{
		// Token: 0x17001F94 RID: 8084
		// (get) Token: 0x0600AF3F RID: 44863 RVA: 0x003DB044 File Offset: 0x003D9244
		protected override string StatusString
		{
			get
			{
				if (this.paused)
				{
					return " " + "Paused".Translate().CapitalizeFirst();
				}
				return "";
			}
		}

		// Token: 0x17001F95 RID: 8085
		// (get) Token: 0x0600AF40 RID: 44864 RVA: 0x003DB080 File Offset: 0x003D9280
		protected override float StatusLineMinHeight
		{
			get
			{
				if (!this.CanUnpause())
				{
					return 0f;
				}
				return 24f;
			}
		}

		// Token: 0x17001F96 RID: 8086
		// (get) Token: 0x0600AF41 RID: 44865 RVA: 0x003DB098 File Offset: 0x003D9298
		public string RepeatInfoText
		{
			get
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					return "Forever".Translate();
				}
				if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					return this.repeatCount.ToString() + "x";
				}
				if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					return this.recipe.WorkerCounter.CountProducts(this).ToString() + "/" + this.targetCount.ToString();
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17001F97 RID: 8087
		// (get) Token: 0x0600AF42 RID: 44866 RVA: 0x003DB126 File Offset: 0x003D9326
		// (set) Token: 0x0600AF43 RID: 44867 RVA: 0x003DB138 File Offset: 0x003D9338
		public string RenamableLabel
		{
			get
			{
				return this.playerCustomName ?? this.BaseLabel;
			}
			set
			{
				this.playerCustomName = value;
			}
		}

		// Token: 0x17001F98 RID: 8088
		// (get) Token: 0x0600AF44 RID: 44868 RVA: 0x003DB141 File Offset: 0x003D9341
		public string BaseLabel
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x17001F99 RID: 8089
		// (get) Token: 0x0600AF45 RID: 44869 RVA: 0x003DB149 File Offset: 0x003D9349
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17001F9A RID: 8090
		// (get) Token: 0x0600AF46 RID: 44870 RVA: 0x003DB151 File Offset: 0x003D9351
		public override string LabelCap
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x0600AF47 RID: 44871 RVA: 0x003DB15C File Offset: 0x003D935C
		public Bill_Production()
		{
		}

		// Token: 0x0600AF48 RID: 44872 RVA: 0x003DB1B4 File Offset: 0x003D93B4
		public Bill_Production(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600AF49 RID: 44873 RVA: 0x003DB20C File Offset: 0x003D940C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<BillRepeatModeDef>(ref this.repeatMode, "repeatMode");
			Scribe_Values.Look<int>(ref this.repeatCount, "repeatCount", 0, false);
			Scribe_Defs.Look<BillStoreModeDef>(ref this.storeMode, "storeMode");
			Scribe_Values.Look<string>(ref this.playerCustomName, "playerCustomName", null, false);
			Scribe_Values.Look<int>(ref this.targetCount, "targetCount", 0, false);
			Scribe_Values.Look<bool>(ref this.pauseWhenSatisfied, "pauseWhenSatisfied", false, false);
			Scribe_Values.Look<int>(ref this.unpauseWhenYouHave, "unpauseWhenYouHave", 0, false);
			Scribe_Values.Look<bool>(ref this.includeEquipped, "includeEquipped", false, false);
			Scribe_Values.Look<bool>(ref this.includeTainted, "includeTainted", false, false);
			Scribe_Values.Look<FloatRange>(ref this.hpRange, "hpRange", FloatRange.ZeroToOne, false);
			Scribe_Values.Look<QualityRange>(ref this.qualityRange, "qualityRange", QualityRange.All, false);
			Scribe_Values.Look<bool>(ref this.limitToAllowedStuff, "limitToAllowedStuff", false, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Bill_Production.SaveSlotReferencable(this.storeGroup, "storeGroup");
				Bill_Production.SaveSlotReferencable(this.includeGroup, "includeGroup");
			}
			else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Bill_Production.LoadSlotReferencable(ref this.storeGroup, "storeGroup");
				Bill_Production.LoadSlotReferencable(ref this.includeGroup, "includeGroup");
			}
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			if (this.repeatMode == null)
			{
				this.repeatMode = BillRepeatModeDefOf.RepeatCount;
			}
			if (this.storeMode == null)
			{
				this.storeMode = BillStoreModeDefOf.BestStockpile;
			}
		}

		// Token: 0x0600AF4A RID: 44874 RVA: 0x003DB390 File Offset: 0x003D9590
		private static void SaveSlotReferencable(ISlotGroup slot, string key)
		{
			ILoadReferenceable loadReferenceable = null;
			ILoadReferenceable loadReferenceable2 = slot as ILoadReferenceable;
			if (loadReferenceable2 != null)
			{
				loadReferenceable = loadReferenceable2;
			}
			else
			{
				SlotGroup slotGroup = slot as SlotGroup;
				if (slotGroup != null)
				{
					ILoadReferenceable loadReferenceable3 = slotGroup.parent as ILoadReferenceable;
					if (loadReferenceable3 != null)
					{
						loadReferenceable = loadReferenceable3;
					}
				}
			}
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, key, false);
		}

		// Token: 0x0600AF4B RID: 44875 RVA: 0x003DB3D4 File Offset: 0x003D95D4
		private static void LoadSlotReferencable(ref ISlotGroup slot, string key)
		{
			ILoadReferenceable loadReferenceable = null;
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, key, false);
			ISlotGroup slotGroup = loadReferenceable as ISlotGroup;
			if (slotGroup != null)
			{
				slot = slotGroup;
				return;
			}
			ISlotGroupParent slotGroupParent = loadReferenceable as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				slot = slotGroupParent.GetSlotGroup();
			}
		}

		// Token: 0x0600AF4C RID: 44876 RVA: 0x003DB40C File Offset: 0x003D960C
		public override BillStoreModeDef GetStoreMode()
		{
			return this.storeMode;
		}

		// Token: 0x0600AF4D RID: 44877 RVA: 0x003DB414 File Offset: 0x003D9614
		public override ISlotGroup GetSlotGroup()
		{
			return this.storeGroup;
		}

		// Token: 0x0600AF4E RID: 44878 RVA: 0x003DB41C File Offset: 0x003D961C
		public void SetIncludeGroup(ISlotGroup group)
		{
			this.includeGroup = group;
		}

		// Token: 0x0600AF4F RID: 44879 RVA: 0x003DB425 File Offset: 0x003D9625
		public ISlotGroup GetIncludeSlotGroup()
		{
			return this.includeGroup;
		}

		// Token: 0x0600AF50 RID: 44880 RVA: 0x003DB42D File Offset: 0x003D962D
		public override void SetStoreMode(BillStoreModeDef mode, ISlotGroup group = null)
		{
			this.storeGroup = group;
			this.storeMode = mode;
			if (this.storeMode == BillStoreModeDefOf.SpecificStockpile != (group != null))
			{
				Log.ErrorOnce("Inconsistent bill StoreMode data set", 75645354);
			}
		}

		// Token: 0x0600AF51 RID: 44881 RVA: 0x003DB460 File Offset: 0x003D9660
		public override bool ShouldDoNow()
		{
			if (this.repeatMode != BillRepeatModeDefOf.TargetCount)
			{
				this.paused = false;
			}
			if (this.suspended)
			{
				return false;
			}
			if (this.repeatMode == BillRepeatModeDefOf.Forever)
			{
				return true;
			}
			if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				return this.repeatCount > 0;
			}
			if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				int num = this.recipe.WorkerCounter.CountProducts(this);
				if (this.pauseWhenSatisfied && num >= this.targetCount)
				{
					this.paused = true;
				}
				if (num <= this.unpauseWhenYouHave || !this.pauseWhenSatisfied)
				{
					this.paused = false;
				}
				return !this.paused && num < this.targetCount;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600AF52 RID: 44882 RVA: 0x003DB51C File Offset: 0x003D971C
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				if (this.repeatCount > 0)
				{
					this.repeatCount--;
				}
				if (this.repeatCount == 0)
				{
					Messages.Message("MessageBillComplete".Translate(this.LabelCap), (Thing)this.billStack.billGiver, MessageTypeDefOf.TaskCompletion, true);
				}
			}
			this.recipe.Worker.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x0600AF53 RID: 44883 RVA: 0x003DB5A1 File Offset: 0x003D97A1
		protected virtual Window GetBillDialog()
		{
			return new Dialog_BillConfig(this, ((Thing)this.billStack.billGiver).Position);
		}

		// Token: 0x0600AF54 RID: 44884 RVA: 0x003DB5C0 File Offset: 0x003D97C0
		protected override void DoConfigInterface(Rect baseRect, Color baseColor)
		{
			Rect rect = new Rect(28f, 32f, 100f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.65f);
			Widgets.Label(rect, this.RepeatInfoText);
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Details".Translate() + "...", null, true, true, true, null))
			{
				Find.WindowStack.Add(this.GetBillDialog());
			}
			if (widgetRow.ButtonText(this.repeatMode.LabelCap.Resolve().PadRight(20), null, true, true, true, null))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this);
			}
			if (widgetRow.ButtonIcon(TexButton.Plus, null, null, null, null, true, -1f))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
					this.targetCount += num;
					this.unpauseWhenYouHave += num;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					this.repeatCount += GenUI.CurrentAdjustmentMultiplier();
				}
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount.ToString());
				}
			}
			if (widgetRow.ButtonIcon(TexButton.Minus, null, null, null, null, true, -1f))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num2 = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
					this.targetCount = Mathf.Max(0, this.targetCount - num2);
					this.unpauseWhenYouHave = Mathf.Max(0, this.unpauseWhenYouHave - num2);
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					this.repeatCount = Mathf.Max(0, this.repeatCount - GenUI.CurrentAdjustmentMultiplier());
				}
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount.ToString());
				}
			}
		}

		// Token: 0x0600AF55 RID: 44885 RVA: 0x003DB8B0 File Offset: 0x003D9AB0
		private bool CanUnpause()
		{
			return this.repeatMode == BillRepeatModeDefOf.TargetCount && this.paused && this.pauseWhenSatisfied && this.recipe.WorkerCounter.CountProducts(this) < this.targetCount;
		}

		// Token: 0x0600AF56 RID: 44886 RVA: 0x003DB8EC File Offset: 0x003D9AEC
		public override void DoStatusLineInterface(Rect rect)
		{
			if (this.paused && new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenUp, 99999f, 4f).ButtonText("Unpause".Translate(), null, true, true, true, null))
			{
				this.paused = false;
			}
		}

		// Token: 0x0600AF57 RID: 44887 RVA: 0x003DB94C File Offset: 0x003D9B4C
		public override void ValidateSettings()
		{
			base.ValidateSettings();
			this.ValidateGroup(ref this.storeGroup);
			this.ValidateGroup(ref this.includeGroup);
			if (this.storeGroup == null && this.storeMode == BillStoreModeDefOf.SpecificStockpile)
			{
				this.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
			}
		}

		// Token: 0x0600AF58 RID: 44888 RVA: 0x003DB998 File Offset: 0x003D9B98
		private void ValidateGroup(ref ISlotGroup slot)
		{
			if (slot == null)
			{
				return;
			}
			SlotGroup slotGroup = slot as SlotGroup;
			if (slotGroup != null)
			{
				Zone_Stockpile zone_Stockpile = slotGroup.parent as Zone_Stockpile;
				if (zone_Stockpile != null && !this.IsZoneValid(zone_Stockpile))
				{
					slot = null;
					return;
				}
				Building_Storage building_Storage = slotGroup.parent as Building_Storage;
				if (building_Storage != null && !this.IsBuildingValid(building_Storage))
				{
					slot = null;
					return;
				}
			}
			else
			{
				StorageGroup storageGroup = slot as StorageGroup;
				if (storageGroup != null && !this.IsStorageGroupValid(storageGroup))
				{
					slot = null;
				}
			}
		}

		// Token: 0x0600AF59 RID: 44889 RVA: 0x003DBA04 File Offset: 0x003D9C04
		private bool IsZoneValid(Zone_Stockpile zone)
		{
			if (zone == null)
			{
				return false;
			}
			int id = zone.ID;
			if (!base.Map.zoneManager.AllZones.Contains(zone))
			{
				zone = (base.Map.zoneManager.AllZones.FirstOrDefault((Zone x) => x.ID == id) as Zone_Stockpile);
			}
			if (zone == null)
			{
				return false;
			}
			if (!zone.zoneManager.AllZones.Contains(zone))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationIncludeZoneDeleted".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), zone.label), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				return false;
			}
			if (base.Map != null && !base.Map.zoneManager.AllZones.Contains(zone))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationIncludeZoneUnavailable".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), zone.label), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600AF5A RID: 44890 RVA: 0x003DBB78 File Offset: 0x003D9D78
		private bool IsBuildingValid(Building_Storage storage)
		{
			if (storage == null)
			{
				return true;
			}
			if (base.Map != null && !base.Map.haulDestinationManager.AllGroups.Contains(storage.slotGroup))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationIncludeBuildingDeleted".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), storage.LabelCap), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600AF5B RID: 44891 RVA: 0x003DBC1C File Offset: 0x003D9E1C
		private bool IsStorageGroupValid(StorageGroup group)
		{
			if (group == null)
			{
				return true;
			}
			if (base.Map != null && !base.Map.storageGroups.HasStorageGroup(group))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationIncludeStorageGroupDeleted".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), group.RenamableLabel), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600AF5C RID: 44892 RVA: 0x003DBCB4 File Offset: 0x003D9EB4
		public override Bill Clone()
		{
			Bill_Production bill_Production = (Bill_Production)base.Clone();
			bill_Production.repeatMode = this.repeatMode;
			bill_Production.repeatCount = this.repeatCount;
			bill_Production.storeMode = this.storeMode;
			bill_Production.storeGroup = this.storeGroup;
			bill_Production.playerCustomName = this.playerCustomName;
			bill_Production.targetCount = this.targetCount;
			bill_Production.pauseWhenSatisfied = this.pauseWhenSatisfied;
			bill_Production.unpauseWhenYouHave = this.unpauseWhenYouHave;
			bill_Production.includeEquipped = this.includeEquipped;
			bill_Production.includeTainted = this.includeTainted;
			bill_Production.includeGroup = this.includeGroup;
			bill_Production.hpRange = this.hpRange;
			bill_Production.qualityRange = this.qualityRange;
			bill_Production.limitToAllowedStuff = this.limitToAllowedStuff;
			bill_Production.paused = this.paused;
			return bill_Production;
		}

		// Token: 0x04006542 RID: 25922
		public BillRepeatModeDef repeatMode = BillRepeatModeDefOf.RepeatCount;

		// Token: 0x04006543 RID: 25923
		public int repeatCount = 1;

		// Token: 0x04006544 RID: 25924
		private BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;

		// Token: 0x04006545 RID: 25925
		private ISlotGroup storeGroup;

		// Token: 0x04006546 RID: 25926
		private string playerCustomName;

		// Token: 0x04006547 RID: 25927
		public int targetCount = 10;

		// Token: 0x04006548 RID: 25928
		public bool pauseWhenSatisfied;

		// Token: 0x04006549 RID: 25929
		public int unpauseWhenYouHave = 5;

		// Token: 0x0400654A RID: 25930
		public bool includeEquipped;

		// Token: 0x0400654B RID: 25931
		public bool includeTainted;

		// Token: 0x0400654C RID: 25932
		public FloatRange hpRange = FloatRange.ZeroToOne;

		// Token: 0x0400654D RID: 25933
		public QualityRange qualityRange = QualityRange.All;

		// Token: 0x0400654E RID: 25934
		public bool limitToAllowedStuff;

		// Token: 0x0400654F RID: 25935
		private ISlotGroup includeGroup;

		// Token: 0x04006550 RID: 25936
		public bool paused;
	}
}
