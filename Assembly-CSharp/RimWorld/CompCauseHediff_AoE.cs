using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001849 RID: 6217
	public class CompCauseHediff_AoE : ThingComp
	{
		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x06009B9D RID: 39837 RVA: 0x003817B8 File Offset: 0x0037F9B8
		public CompProperties_CauseHediff_AoE Props
		{
			get
			{
				return (CompProperties_CauseHediff_AoE)this.props;
			}
		}

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x06009B9E RID: 39838 RVA: 0x003817C5 File Offset: 0x0037F9C5
		private CompPowerTrader PowerTrader
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>();
			}
		}

		// Token: 0x06009B9F RID: 39839 RVA: 0x003817D4 File Offset: 0x0037F9D4
		private bool IsPawnAffected(Pawn target)
		{
			return (this.PowerTrader == null || this.PowerTrader.PowerOn) && !target.Dead && target.health != null && (target != this.parent || this.Props.canTargetSelf) && (!this.Props.ignoreMechs || !target.RaceProps.IsMechanoid) && (!this.Props.onlyTargetMechs || target.RaceProps.IsMechanoid) && target.PositionHeld.DistanceTo(this.parent.PositionHeld) <= this.range;
		}

		// Token: 0x06009BA0 RID: 39840 RVA: 0x0038187E File Offset: 0x0037FA7E
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.range = this.Props.range;
		}

		// Token: 0x06009BA1 RID: 39841 RVA: 0x00381898 File Offset: 0x0037FA98
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.range, "range", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.range <= 0f)
			{
				this.range = this.Props.range;
			}
		}

		// Token: 0x06009BA2 RID: 39842 RVA: 0x003818E8 File Offset: 0x0037FAE8
		public override void CompTick()
		{
			this.MaintainSustainer();
			if (!this.parent.IsHashIntervalTick(this.Props.checkInterval))
			{
				return;
			}
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null && !compPowerTrader.PowerOn)
			{
				return;
			}
			this.lastIntervalActive = false;
			if (!this.parent.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (this.IsPawnAffected(pawn))
				{
					this.GiveOrUpdateHediff(pawn);
				}
				Pawn pawn2 = pawn.carryTracker.CarriedThing as Pawn;
				if (pawn2 != null && this.IsPawnAffected(pawn2))
				{
					this.GiveOrUpdateHediff(pawn2);
				}
			}
		}

		// Token: 0x06009BA3 RID: 39843 RVA: 0x003819C0 File Offset: 0x0037FBC0
		private void GiveOrUpdateHediff(Pawn target)
		{
			Hediff hediff = target.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false);
			if (hediff == null)
			{
				hediff = target.health.AddHediff(this.Props.hediff, target.health.hediffSet.GetBrain(), null, null);
				hediff.Severity = 1f;
				HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
				if (hediffComp_Link != null)
				{
					hediffComp_Link.drawConnection = false;
					hediffComp_Link.other = this.parent;
				}
			}
			HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears == null)
			{
				Log.ErrorOnce("CompCauseHediff_AoE has a hediff in props which does not have a HediffComp_Disappears", 78945945);
			}
			else
			{
				hediffComp_Disappears.ticksToDisappear = this.Props.checkInterval + 5;
			}
			this.lastIntervalActive = true;
		}

		// Token: 0x06009BA4 RID: 39844 RVA: 0x00381A7C File Offset: 0x0037FC7C
		private void MaintainSustainer()
		{
			if (this.lastIntervalActive && this.Props.activeSound != null)
			{
				if (this.activeSustainer == null || this.activeSustainer.Ended)
				{
					this.activeSustainer = this.Props.activeSound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.parent), MaintenanceType.None));
				}
				this.activeSustainer.Maintain();
				return;
			}
			if (this.activeSustainer != null)
			{
				this.activeSustainer.End();
				this.activeSustainer = null;
			}
		}

		// Token: 0x06009BA5 RID: 39845 RVA: 0x00381B00 File Offset: 0x0037FD00
		public override void PostDraw()
		{
			if (!this.Props.drawLines)
			{
				return;
			}
			int num = Mathf.Max(this.parent.Map.Size.x, this.parent.Map.Size.y);
			if (Find.Selector.SelectedObjectsListForReading.Contains(this.parent) && this.range < (float)num)
			{
				foreach (Pawn pawn in this.parent.Map.mapPawns.AllPawnsSpawned)
				{
					if (this.IsPawnAffected(pawn))
					{
						GenDraw.DrawLineBetween(pawn.DrawPos, this.parent.DrawPos);
					}
				}
			}
		}

		// Token: 0x04005C8F RID: 23695
		public float range;

		// Token: 0x04005C90 RID: 23696
		private Sustainer activeSustainer;

		// Token: 0x04005C91 RID: 23697
		private bool lastIntervalActive;
	}
}
