using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C3 RID: 5059
	public class Zone_Fishing : Zone
	{
		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x06007D5A RID: 32090 RVA: 0x002CD707 File Offset: 0x002CB907
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x06007D5B RID: 32091 RVA: 0x002CD70A File Offset: 0x002CB90A
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextFishingZoneColor();
			}
		}

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x06007D5C RID: 32092 RVA: 0x002CD711 File Offset: 0x002CB911
		public bool Allowed
		{
			get
			{
				return this.allowed;
			}
		}

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x06007D5D RID: 32093 RVA: 0x002CD719 File Offset: 0x002CB919
		public bool ShouldFishNow
		{
			get
			{
				return this.Allowed && this.OverTargetPopulation && !this.PausedDueToResourceCount && this.UnderTargetResourceCount;
			}
		}

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x06007D5E RID: 32094 RVA: 0x002CD73B File Offset: 0x002CB93B
		private bool AllFishableCellsFrozenNow
		{
			get
			{
				return this.cells.FindIndex(delegate(IntVec3 c)
				{
					TerrainDef terrain = c.GetTerrain(base.Map);
					return terrain == null || !terrain.temporary || terrain != TerrainDefOf.ThinIce;
				}) < 0;
			}
		}

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x06007D5F RID: 32095 RVA: 0x002CD758 File Offset: 0x002CB958
		private bool UnderTargetResourceCount
		{
			get
			{
				bool result;
				switch (this.repeatMode)
				{
				case FishRepeatMode.RepeatCount:
					result = (this.repeatCount > 0);
					break;
				case FishRepeatMode.TargetCount:
					result = (this.OwnedFishCount < this.targetCount);
					break;
				case FishRepeatMode.DoForever:
					result = true;
					break;
				default:
					result = (this.OwnedFishCount < this.repeatCount);
					break;
				}
				return result;
			}
		}

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x06007D60 RID: 32096 RVA: 0x002CD7B1 File Offset: 0x002CB9B1
		public int OwnedFishCount
		{
			get
			{
				return base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Fish).Sum((Thing t) => t.stackCount) + this.CarriedFishCount();
			}
		}

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x06007D61 RID: 32097 RVA: 0x002CD7F0 File Offset: 0x002CB9F0
		private bool OverTargetPopulation
		{
			get
			{
				return base.Map.TileInfo.MaxFishPopulation > 0f && this.cells.Count != 0 && this.FishType != WaterBodyType.None && base.Map.waterBodyTracker.PopulationPercentAt(base.Cells[0]) > this.targetPopulationPct;
			}
		}

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x06007D62 RID: 32098 RVA: 0x002CD84F File Offset: 0x002CBA4F
		public WaterBodyType FishType
		{
			get
			{
				if (this.cells.Count == 0)
				{
					return WaterBodyType.None;
				}
				return base.Cells[0].GetWaterBodyType(base.Map);
			}
		}

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x06007D63 RID: 32099 RVA: 0x002CD877 File Offset: 0x002CBA77
		public bool AllFishableCellsFrozen
		{
			get
			{
				if (Find.TickManager.TicksGame < this.cachedFrozenTicks + 60)
				{
					return this.cachedIsFrozen;
				}
				this.cachedFrozenTicks = Find.TickManager.TicksGame;
				this.cachedIsFrozen = this.AllFishableCellsFrozenNow;
				return this.cachedIsFrozen;
			}
		}

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x06007D64 RID: 32100 RVA: 0x002CD8B7 File Offset: 0x002CBAB7
		public bool PausedDueToResourceCount
		{
			get
			{
				if (Find.TickManager.TicksGame < this.lastPauseCheckTicks + 60)
				{
					return this.pausedDueToResourceCount;
				}
				this.RecheckPausedDueToResourceCount();
				return this.pausedDueToResourceCount;
			}
		}

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x06007D65 RID: 32101 RVA: 0x002CD8E4 File Offset: 0x002CBAE4
		public bool HasAnyFishableCells
		{
			get
			{
				for (int i = 0; i < this.cells.Count; i++)
				{
					if (this.IsFishable(this.cells[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x06007D66 RID: 32102 RVA: 0x002CD920 File Offset: 0x002CBB20
		public List<IntVec3> FishbleCells
		{
			get
			{
				this.tmpFishableCells.Clear();
				for (int i = 0; i < this.cells.Count; i++)
				{
					IntVec3 intVec = this.cells[i];
					if (this.IsFishable(intVec))
					{
						this.tmpFishableCells.Add(intVec);
					}
				}
				return this.tmpFishableCells;
			}
		}

		// Token: 0x06007D67 RID: 32103 RVA: 0x002CD976 File Offset: 0x002CBB76
		public bool IsFishable(IntVec3 cell)
		{
			return cell.GetTerrain(base.Map).IsWater && cell.GetWaterBodyType(base.Map) == this.FishType;
		}

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x06007D68 RID: 32104 RVA: 0x002CD9A4 File Offset: 0x002CBBA4
		public IntVec3 RandomFishableCell
		{
			get
			{
				IntVec3 result;
				if (this.FishbleCells.TryRandomElement(out result))
				{
					return result;
				}
				return IntVec3.Invalid;
			}
		}

		// Token: 0x06007D69 RID: 32105 RVA: 0x002CD9C8 File Offset: 0x002CBBC8
		public Zone_Fishing()
		{
		}

		// Token: 0x06007D6A RID: 32106 RVA: 0x002CDA38 File Offset: 0x002CBC38
		public Zone_Fishing(ZoneManager zoneManager) : base("Zone_Fishing".Translate(), zoneManager)
		{
		}

		// Token: 0x06007D6B RID: 32107 RVA: 0x002CDAB8 File Offset: 0x002CBCB8
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.ITabs;
		}

		// Token: 0x06007D6C RID: 32108 RVA: 0x002CDAC0 File Offset: 0x002CBCC0
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			return new Zone_Fishing.<GetZoneAddGizmos>d__47(-2);
		}

		// Token: 0x06007D6D RID: 32109 RVA: 0x002CDACC File Offset: 0x002CBCCC
		private int CarriedFishCount()
		{
			int num = 0;
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Thing carriedThing = pawn.carryTracker.CarriedThing;
				if (carriedThing != null && ThingRequestGroup.Fish.Includes(carriedThing.def) && !carriedThing.Spawned)
				{
					num += carriedThing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x06007D6E RID: 32110 RVA: 0x002CDB54 File Offset: 0x002CBD54
		public void Notify_Fished()
		{
			if (this.repeatMode == FishRepeatMode.RepeatCount)
			{
				this.repeatCount--;
			}
			this.RecheckPausedDueToResourceCount();
		}

		// Token: 0x06007D6F RID: 32111 RVA: 0x002CDB74 File Offset: 0x002CBD74
		public void RecheckPausedDueToResourceCount()
		{
			this.lastPauseCheckTicks = Find.TickManager.TicksGame;
			if (this.repeatMode != FishRepeatMode.TargetCount || !this.pauseWhenSatisfied)
			{
				this.pausedDueToResourceCount = false;
				return;
			}
			if (this.pausedDueToResourceCount)
			{
				this.pausedDueToResourceCount = (this.OwnedFishCount > this.unpauseAtCount);
				return;
			}
			this.pausedDueToResourceCount = (this.OwnedFishCount >= this.targetCount);
		}

		// Token: 0x06007D70 RID: 32112 RVA: 0x002CDBDE File Offset: 0x002CBDDE
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Zone_Fishing.<GetFloatMenuOptions>d__51 <GetFloatMenuOptions>d__ = new Zone_Fishing.<GetFloatMenuOptions>d__51(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x06007D71 RID: 32113 RVA: 0x002CDBF5 File Offset: 0x002CBDF5
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Zone_Fishing.<GetGizmos>d__52 <GetGizmos>d__ = new Zone_Fishing.<GetGizmos>d__52(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06007D72 RID: 32114 RVA: 0x002CDC08 File Offset: 0x002CBE08
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			float num = base.Map.waterBodyTracker.FishPopulationAt(base.Cells[0]);
			string str = text;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FishPopulation".Translate().CapitalizeFirst());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(num, "F0");
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(FishingUtility.FishPopulationLabel(num));
			defaultInterpolatedStringHandler.AppendLiteral(")");
			text = str + defaultInterpolatedStringHandler.ToStringAndClear();
			GameCondition gameCondition;
			if (base.Map.gameConditionManager.FishPopulationOffsetFactorPerDay(base.Map, out gameCondition) < 0f && gameCondition != null)
			{
				string str2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("\n");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("FishPopulationDecline".Translate().CapitalizeFirst());
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted(gameCondition.LabelCap);
				text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			if (!this.ShouldFishNow)
			{
				if (!this.UnderTargetResourceCount)
				{
					text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotOverTargetResourceCount".Translate() + ")";
				}
				else if (this.PausedDueToResourceCount)
				{
					text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "Paused".Translate() + ")";
				}
				else if (!this.OverTargetPopulation)
				{
					text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotUnderTargetPopulation".Translate() + ")";
				}
				else if (!this.Allowed)
				{
					text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "ForbiddenLower".Translate() + ")";
				}
			}
			else if (this.AllFishableCellsFrozen)
			{
				text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotFrozen".Translate() + ")";
			}
			else if (!this.FishbleCells.Any<IntVec3>())
			{
				text += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotBlocked".Translate() + ")";
			}
			return text;
		}

		// Token: 0x06007D73 RID: 32115 RVA: 0x002CDF50 File Offset: 0x002CC150
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.allowed, "allowed", true, false);
			Scribe_Values.Look<FishRepeatMode>(ref this.repeatMode, "repeatMode", FishRepeatMode.TargetCount, false);
			Scribe_Values.Look<int>(ref this.repeatCount, "repeatCount", 1, false);
			Scribe_Values.Look<int>(ref this.targetCount, "targetCount", 100, false);
			Scribe_Values.Look<float>(ref this.targetPopulationPct, "targetPopulationPct", 0.5f, false);
			Scribe_Values.Look<bool>(ref this.pauseWhenSatisfied, "pauseWhenSatisfied", false, false);
			Scribe_Values.Look<int>(ref this.unpauseAtCount, "unpauseAtCount", 50, false);
			Scribe_Values.Look<bool>(ref this.pausedDueToResourceCount, "pausedDueToResourceCount", false, false);
		}

		// Token: 0x04004EFF RID: 20223
		private bool allowed = true;

		// Token: 0x04004F00 RID: 20224
		public int repeatCount = 1;

		// Token: 0x04004F01 RID: 20225
		public int targetCount = 100;

		// Token: 0x04004F02 RID: 20226
		public float targetPopulationPct = 0.5f;

		// Token: 0x04004F03 RID: 20227
		public FishRepeatMode repeatMode = FishRepeatMode.TargetCount;

		// Token: 0x04004F04 RID: 20228
		public bool pauseWhenSatisfied;

		// Token: 0x04004F05 RID: 20229
		public int unpauseAtCount;

		// Token: 0x04004F06 RID: 20230
		private bool pausedDueToResourceCount;

		// Token: 0x04004F07 RID: 20231
		private bool cachedIsFrozen;

		// Token: 0x04004F08 RID: 20232
		private int cachedFrozenTicks = int.MinValue;

		// Token: 0x04004F09 RID: 20233
		private int lastPauseCheckTicks = int.MinValue;

		// Token: 0x04004F0A RID: 20234
		private readonly ITab[] ITabs = new ITab[]
		{
			new ITab_Fishing()
		};

		// Token: 0x04004F0B RID: 20235
		private const string FishTexPath = "UI/Designators/ZoneCreate_Fishing";

		// Token: 0x04004F0C RID: 20236
		private const int FrozenCheckInterval = 60;

		// Token: 0x04004F0D RID: 20237
		private List<IntVec3> tmpFishableCells = new List<IntVec3>();
	}
}
