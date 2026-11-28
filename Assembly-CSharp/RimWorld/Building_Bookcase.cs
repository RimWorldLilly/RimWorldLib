using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A78 RID: 6776
	public class Building_Bookcase : Building, IThingHolderEvents<Book>, IHaulEnroute, ILoadReferenceable, IStorageGroupMember, IHaulDestination, IStoreSettingsParent, IHaulSource, IThingHolder, ISearchableContents, IBeautyContainer
	{
		// Token: 0x17001E10 RID: 7696
		// (get) Token: 0x0600AA7B RID: 43643 RVA: 0x003C5B38 File Offset: 0x003C3D38
		private Graphic BookendGraphicEast
		{
			get
			{
				Graphic result;
				if ((result = this.bookendGraphicEastInt) == null)
				{
					result = (this.bookendGraphicEastInt = this.def.building.bookendGraphicEast.GraphicColoredFor(this));
				}
				return result;
			}
		}

		// Token: 0x17001E11 RID: 7697
		// (get) Token: 0x0600AA7C RID: 43644 RVA: 0x003C5B70 File Offset: 0x003C3D70
		private Graphic BookendGraphicNorth
		{
			get
			{
				Graphic result;
				if ((result = this.bookendGraphicNorthInt) == null)
				{
					result = (this.bookendGraphicNorthInt = this.def.building.bookendGraphicNorth.GraphicColoredFor(this));
				}
				return result;
			}
		}

		// Token: 0x17001E12 RID: 7698
		// (get) Token: 0x0600AA7D RID: 43645 RVA: 0x003C5BA6 File Offset: 0x003C3DA6
		public int MaximumBooks
		{
			get
			{
				return this.def.building.maxItemsInCell * this.def.size.Area;
			}
		}

		// Token: 0x17001E13 RID: 7699
		// (get) Token: 0x0600AA7E RID: 43646 RVA: 0x003C5BC9 File Offset: 0x003C3DC9
		public IReadOnlyList<Book> HeldBooks
		{
			get
			{
				return this.innerContainer.InnerListForReading;
			}
		}

		// Token: 0x17001E14 RID: 7700
		// (get) Token: 0x0600AA7F RID: 43647 RVA: 0x003C5BD6 File Offset: 0x003C3DD6
		public float ReadingBonus
		{
			get
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room == null)
				{
					return 0f;
				}
				return room.GetStat(RoomStatDefOf.ReadingBonus);
			}
		}

		// Token: 0x17001E15 RID: 7701
		// (get) Token: 0x0600AA80 RID: 43648 RVA: 0x003C5BF4 File Offset: 0x003C3DF4
		protected virtual Vector3[] RotOffsets
		{
			get
			{
				return Building_Bookcase.StandardBookshelfRotOffsets;
			}
		}

		// Token: 0x17001E16 RID: 7702
		// (get) Token: 0x0600AA81 RID: 43649 RVA: 0x003C5BFB File Offset: 0x003C3DFB
		public IEnumerable<float> CellsFilledPercentage
		{
			get
			{
				Building_Bookcase.<get_CellsFilledPercentage>d__23 <get_CellsFilledPercentage>d__ = new Building_Bookcase.<get_CellsFilledPercentage>d__23(-2);
				<get_CellsFilledPercentage>d__.<>4__this = this;
				return <get_CellsFilledPercentage>d__;
			}
		}

		// Token: 0x17001E17 RID: 7703
		// (get) Token: 0x0600AA82 RID: 43650 RVA: 0x003C5C0B File Offset: 0x003C3E0B
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x0600AA83 RID: 43651 RVA: 0x003C5C13 File Offset: 0x003C3E13
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600AA84 RID: 43652 RVA: 0x003C5C21 File Offset: 0x003C3E21
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x17001E18 RID: 7704
		// (get) Token: 0x0600AA85 RID: 43653 RVA: 0x003C5C29 File Offset: 0x003C3E29
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AA86 RID: 43654 RVA: 0x003C5C2C File Offset: 0x003C3E2C
		public StorageSettings GetStoreSettings()
		{
			if (this.storageGroup != null)
			{
				return this.storageGroup.GetStoreSettings();
			}
			return this.settings;
		}

		// Token: 0x0600AA87 RID: 43655 RVA: 0x003C5C48 File Offset: 0x003C3E48
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x17001E19 RID: 7705
		// (get) Token: 0x0600AA88 RID: 43656 RVA: 0x003C5C5A File Offset: 0x003C3E5A
		public bool HaulSourceEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E1A RID: 7706
		// (get) Token: 0x0600AA89 RID: 43657 RVA: 0x003C5C5D File Offset: 0x003C3E5D
		public bool HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AA8A RID: 43658 RVA: 0x003C5C60 File Offset: 0x003C3E60
		public bool Accepts(Thing t)
		{
			if (this.HeldBooks.Count >= this.MaximumBooks)
			{
				Book book = t as Book;
				if (book == null)
				{
					return false;
				}
				if (!this.innerContainer.InnerListForReading.Contains(book))
				{
					return false;
				}
			}
			return this.GetStoreSettings().AllowedToAccept(t) && this.innerContainer.CanAcceptAnyOf(t, true);
		}

		// Token: 0x0600AA8B RID: 43659 RVA: 0x003C5CBE File Offset: 0x003C3EBE
		public int SpaceRemainingFor(ThingDef _)
		{
			return this.MaximumBooks - this.HeldBooks.Count;
		}

		// Token: 0x0600AA8C RID: 43660 RVA: 0x003C5CD2 File Offset: 0x003C3ED2
		public void Notify_SettingsChanged()
		{
			if (!base.Spawned)
			{
				return;
			}
			base.MapHeld.listerHaulables.Notify_HaulSourceChanged(this);
		}

		// Token: 0x17001E1B RID: 7707
		// (get) Token: 0x0600AA8D RID: 43661 RVA: 0x003C5CEE File Offset: 0x003C3EEE
		// (set) Token: 0x0600AA8E RID: 43662 RVA: 0x003C5CF6 File Offset: 0x003C3EF6
		StorageGroup IStorageGroupMember.Group
		{
			get
			{
				return this.storageGroup;
			}
			set
			{
				this.storageGroup = value;
			}
		}

		// Token: 0x17001E1C RID: 7708
		// (get) Token: 0x0600AA8F RID: 43663 RVA: 0x003C5CFF File Offset: 0x003C3EFF
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return base.Spawned;
			}
		}

		// Token: 0x17001E1D RID: 7709
		// (get) Token: 0x0600AA90 RID: 43664 RVA: 0x003C5D07 File Offset: 0x003C3F07
		Map IStorageGroupMember.Map
		{
			get
			{
				return base.MapHeld;
			}
		}

		// Token: 0x17001E1E RID: 7710
		// (get) Token: 0x0600AA91 RID: 43665 RVA: 0x003C5D0F File Offset: 0x003C3F0F
		string IStorageGroupMember.StorageGroupTag
		{
			get
			{
				return this.def.building.storageGroupTag;
			}
		}

		// Token: 0x17001E1F RID: 7711
		// (get) Token: 0x0600AA92 RID: 43666 RVA: 0x003C5D21 File Offset: 0x003C3F21
		StorageSettings IStorageGroupMember.StoreSettings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x17001E20 RID: 7712
		// (get) Token: 0x0600AA93 RID: 43667 RVA: 0x003C5D29 File Offset: 0x003C3F29
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.GetParentStoreSettings();
			}
		}

		// Token: 0x17001E21 RID: 7713
		// (get) Token: 0x0600AA94 RID: 43668 RVA: 0x003C5D31 File Offset: 0x003C3F31
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x17001E22 RID: 7714
		// (get) Token: 0x0600AA95 RID: 43669 RVA: 0x003C5D39 File Offset: 0x003C3F39
		bool IStorageGroupMember.DrawStorageTab
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E23 RID: 7715
		// (get) Token: 0x0600AA96 RID: 43670 RVA: 0x003C5D3C File Offset: 0x003C3F3C
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17001E24 RID: 7716
		// (get) Token: 0x0600AA97 RID: 43671 RVA: 0x003C5D4B File Offset: 0x003C3F4B
		public float BeautyOffset
		{
			get
			{
				if (this.cachedBeauty < 0f)
				{
					this.cachedBeauty = this.HeldBooks.Aggregate(0f, (float beauty, Book book) => beauty + book.GetBeauty(this.GetRoom(RegionType.Set_All).PsychologicallyOutdoors));
				}
				return this.cachedBeauty - 0.001f;
			}
		}

		// Token: 0x17001E25 RID: 7717
		// (get) Token: 0x0600AA98 RID: 43672 RVA: 0x003C5D88 File Offset: 0x003C3F88
		string IBeautyContainer.BeautyOffsetExplanation
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ContainedBookBeauty".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(this.BeautyOffset.ToStringWithSign("0.##"));
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x0600AA99 RID: 43673 RVA: 0x003C5DD9 File Offset: 0x003C3FD9
		public void Notify_ItemAdded(Book item)
		{
			this.DirtyRoomStats();
			this.cachedBeauty = -1f;
			base.MapHeld.listerHaulables.Notify_AddedThing(item);
		}

		// Token: 0x0600AA9A RID: 43674 RVA: 0x003C5DFD File Offset: 0x003C3FFD
		public void Notify_ItemRemoved(Book item)
		{
			this.DirtyRoomStats();
			this.cachedBeauty = -1f;
		}

		// Token: 0x0600AA9B RID: 43675 RVA: 0x003C5E10 File Offset: 0x003C4010
		public Building_Bookcase()
		{
			this.innerContainer = new ThingOwner<Book>(this, false, LookMode.Deep, true);
		}

		// Token: 0x0600AA9C RID: 43676 RVA: 0x003C5E34 File Offset: 0x003C4034
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.storageGroup != null && map != this.storageGroup.Map)
			{
				StorageSettings storeSettings = this.storageGroup.GetStoreSettings();
				this.storageGroup.RemoveMember(this, true);
				this.storageGroup = null;
				this.settings.CopyFrom(storeSettings);
			}
		}

		// Token: 0x0600AA9D RID: 43677 RVA: 0x003C5E8C File Offset: 0x003C408C
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
			this.DirtyRoomStats();
		}

		// Token: 0x0600AA9E RID: 43678 RVA: 0x003C5EE0 File Offset: 0x003C40E0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			drawLoc -= Altitudes.AltIncVect * 2f;
			base.DrawAt(drawLoc, flip);
			Rot4 rot = base.Rotation.Rotated(RotationDirection.Counterclockwise);
			float num = (base.Rotation == Rot4.North || base.Rotation == Rot4.South) ? 0.155f : 0.16f;
			Vector3 a = rot.FacingCell.ToVector3() * num;
			Vector3 b = rot.FacingCell.ToVector3() * ((float)(-(float)this.MaximumBooks) * num * 0.5f);
			Vector3 b2 = this.RotOffsets[base.Rotation.AsInt];
			for (int i = 0; i < this.HeldBooks.Count; i++)
			{
				Book book = this.HeldBooks[i];
				Rot4 opposite = base.Rotation.Opposite;
				if (opposite == Rot4.East || opposite == Rot4.West)
				{
					opposite = opposite.Opposite;
				}
				Vector3 loc = drawLoc + b + b2 + Building_Bookcase.DrawOffset + a * (float)i;
				book.VerticalGraphic.Draw(loc, opposite, this, 0f);
			}
			if (base.Rotation != Rot4.South)
			{
				if (base.Rotation != Rot4.North && this.def.building.bookendGraphicEast != null)
				{
					this.BookendGraphicEast.Draw(drawLoc + Building_Bookcase.DrawOffsetBookcaseEnd, Rot4.North, this, 0f);
					return;
				}
				if (base.Rotation == Rot4.North && this.def.building.bookendGraphicNorth != null)
				{
					this.BookendGraphicNorth.Draw(drawLoc + Building_Bookcase.DrawOffsetBookcaseEnd, Rot4.North, this, 0f);
				}
			}
		}

		// Token: 0x0600AA9F RID: 43679 RVA: 0x003C60E8 File Offset: 0x003C42E8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.storageGroup != null)
			{
				StorageGroup storageGroup = this.storageGroup;
				if (storageGroup != null)
				{
					storageGroup.RemoveMember(this, true);
				}
				this.storageGroup = null;
			}
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600AAA0 RID: 43680 RVA: 0x003C6140 File Offset: 0x003C4340
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (Find.Selector.SingleSelectedThing == this)
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room != null && room.ProperRoom)
				{
					room.DrawFieldEdges();
				}
			}
			StorageGroupUtility.DrawSelectionOverlaysFor(this);
		}

		// Token: 0x0600AAA1 RID: 43681 RVA: 0x003C6180 File Offset: 0x003C4380
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Spawned)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				if (this.storageGroup != null)
				{
					string str = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("StorageGroupLabel".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(this.storageGroup.RenamableLabel.CapitalizeFirst());
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					text = str + defaultInterpolatedStringHandler.ToStringAndClear();
					if (this.storageGroup.MemberCount > 1)
					{
						text += "(" + "NumBuildings".Translate(this.storageGroup.MemberCount).CapitalizeFirst() + ")\n";
					}
					else
					{
						text += "(" + "OneBuilding".Translate() + ")\n";
					}
				}
				string str2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 3);
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("BookshelfStoredInspect".Translate());
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(this.HeldBooks.Count);
				defaultInterpolatedStringHandler2.AppendLiteral(" / ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(this.MaximumBooks);
				text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
				string str3 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("\n");
				defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("BookshelfReadingBonusInspect".Translate());
				defaultInterpolatedStringHandler3.AppendLiteral(": ");
				defaultInterpolatedStringHandler3.AppendFormatted(Mathf.Max(this.ReadingBonus - 1f, 0f).ToStringPercent());
				text = str3 + defaultInterpolatedStringHandler3.ToStringAndClear();
				if (this.IsOutside())
				{
					string str4 = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler4.AppendLiteral(" (");
					defaultInterpolatedStringHandler4.AppendFormatted<TaggedString>("Outdoors".Translate());
					defaultInterpolatedStringHandler4.AppendLiteral(")");
					text = str4 + defaultInterpolatedStringHandler4.ToStringAndClear().Colorize(Color.gray).ResolveTags();
				}
			}
			return text;
		}

		// Token: 0x0600AAA2 RID: 43682 RVA: 0x003C63AA File Offset: 0x003C45AA
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Bookcase.<GetGizmos>d__72 <GetGizmos>d__ = new Building_Bookcase.<GetGizmos>d__72(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AAA3 RID: 43683 RVA: 0x003C63BA File Offset: 0x003C45BA
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_Bookcase.<GetFloatMenuOptions>d__73 <GetFloatMenuOptions>d__ = new Building_Bookcase.<GetFloatMenuOptions>d__73(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600AAA4 RID: 43684 RVA: 0x003C63D1 File Offset: 0x003C45D1
		private void DirtyRoomStats()
		{
			Room room = this.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return;
			}
			room.Notify_ContainedThingSpawnedOrDespawned(this);
		}

		// Token: 0x0600AAA5 RID: 43685 RVA: 0x003C63E6 File Offset: 0x003C45E6
		public override void Notify_ColorChanged()
		{
			this.bookendGraphicEastInt = null;
			this.bookendGraphicNorthInt = null;
			base.Notify_ColorChanged();
		}

		// Token: 0x0600AAA6 RID: 43686 RVA: 0x003C63FC File Offset: 0x003C45FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner<Book>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
		}

		// Token: 0x0400639C RID: 25500
		private ThingOwner<Book> innerContainer;

		// Token: 0x0400639D RID: 25501
		private StorageSettings settings;

		// Token: 0x0400639E RID: 25502
		private StorageGroup storageGroup;

		// Token: 0x0400639F RID: 25503
		private Graphic bookendGraphicEastInt;

		// Token: 0x040063A0 RID: 25504
		private Graphic bookendGraphicNorthInt;

		// Token: 0x040063A1 RID: 25505
		private static readonly Vector3 DrawOffset = new Vector3(0f, 0.018292684f, 0f);

		// Token: 0x040063A2 RID: 25506
		private static readonly Vector3 DrawOffsetBookcaseEnd = new Vector3(0f, 0.07317074f, 0f);

		// Token: 0x040063A3 RID: 25507
		private const float BookWidthEastWest = 0.16f;

		// Token: 0x040063A4 RID: 25508
		private const float BookWidthNorthSouth = 0.155f;

		// Token: 0x040063A5 RID: 25509
		private static readonly Vector3[] StandardBookshelfRotOffsets = new Vector3[]
		{
			new Vector3(-0.081f, 0f, 0f),
			new Vector3(-0.082f, 0f, 0.02f),
			new Vector3(0.08f, 0f, 0.08f),
			new Vector3(0.082f, 0f, -0.15f)
		};

		// Token: 0x040063A6 RID: 25510
		private float cachedBeauty = -1f;
	}
}
