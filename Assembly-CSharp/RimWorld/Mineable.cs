using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A61 RID: 6753
	public class Mineable : Building
	{
		// Token: 0x0600A9A9 RID: 43433 RVA: 0x003C264C File Offset: 0x003C084C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.yieldPct, "yieldPct", 0f, false);
		}

		// Token: 0x0600A9AA RID: 43434 RVA: 0x003C266C File Offset: 0x003C086C
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (this.def.building.mineableThing != null && this.def.building.mineableYieldWasteable && dinfo.Def == DamageDefOf.Mining && dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.Notify_TookMiningDamage(GenMath.RoundRandom(dinfo.Amount), (Pawn)dinfo.Instigator);
			}
			absorbed = false;
		}

		// Token: 0x0600A9AB RID: 43435 RVA: 0x003C26EC File Offset: 0x003C08EC
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			if (dinfo != null && dinfo.Value.Instigator != null && dinfo.Value.Def != DamageDefOf.Mining)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.DestroyedMineable, dinfo.Value.Instigator.Named(HistoryEventArgsNames.Doer)), true);
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x0600A9AC RID: 43436 RVA: 0x003C2760 File Offset: 0x003C0960
		public void DestroyMined(Pawn pawn)
		{
			Map map = base.Map;
			base.Destroy(DestroyMode.KillFinalize);
			this.TrySpawnYield(map, true, pawn);
			if (pawn != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mined, pawn.Named(HistoryEventArgsNames.Doer)), true);
			}
		}

		// Token: 0x0600A9AD RID: 43437 RVA: 0x003C27A8 File Offset: 0x003C09A8
		public override void Destroy(DestroyMode mode)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				this.TrySpawnYield(map, false, null);
			}
		}

		// Token: 0x0600A9AE RID: 43438 RVA: 0x003C27D0 File Offset: 0x003C09D0
		[Obsolete("Will be removeds.")]
		private void TrySpawnYield(Map map, float yieldChance, bool moteOnWaste, Pawn pawn)
		{
			this.TrySpawnYield(map, moteOnWaste, pawn);
		}

		// Token: 0x0600A9AF RID: 43439 RVA: 0x003C27DC File Offset: 0x003C09DC
		private void TrySpawnYield(Map map, bool moteOnWaste, Pawn pawn)
		{
			Mineable.<>c__DisplayClass7_0 CS$<>8__locals1 = new Mineable.<>c__DisplayClass7_0();
			CS$<>8__locals1.pawn = pawn;
			if (this.def.building.mineableThing == null)
			{
				return;
			}
			if (Rand.Value > this.def.building.mineableDropChance)
			{
				return;
			}
			int num = Mathf.Max(1, this.def.building.EffectiveMineableYield);
			if (this.def.building.mineableYieldWasteable)
			{
				num = Mathf.Max(1, GenMath.RoundRandom((float)num * this.yieldPct));
			}
			Thing thing = ThingMaker.MakeThing(this.def.building.mineableThing, null);
			thing.stackCount = num;
			GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, new Action<Thing, int>(CS$<>8__locals1.<TrySpawnYield>g__ForbidIfNecessary|0), null, null, 1);
		}

		// Token: 0x0600A9B0 RID: 43440 RVA: 0x003C28A4 File Offset: 0x003C0AA4
		public void Notify_TookMiningDamage(int amount, Pawn miner)
		{
			float num = (float)Mathf.Min(amount, this.HitPoints) / (float)base.MaxHitPoints;
			float num2 = 1f;
			if (miner != null)
			{
				num2 = miner.GetStatValue(StatDefOf.MiningYield, true, -1);
			}
			this.yieldPct += num * num2;
		}

		// Token: 0x04006334 RID: 25396
		private float yieldPct;
	}
}
