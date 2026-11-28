using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC9 RID: 6857
	public class Building_SleepSuppressor : Building
	{
		// Token: 0x17001F50 RID: 8016
		// (get) Token: 0x0600AE7C RID: 44668 RVA: 0x003D7208 File Offset: 0x003D5408
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.powerTraderComp) == null)
				{
					result = (this.powerTraderComp = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17001F51 RID: 8017
		// (get) Token: 0x0600AE7D RID: 44669 RVA: 0x003D7230 File Offset: 0x003D5430
		private CompNoiseSource NoiseSourceComp
		{
			get
			{
				CompNoiseSource result;
				if ((result = this.noiseSourceComp) == null)
				{
					result = (this.noiseSourceComp = base.GetComp<CompNoiseSource>());
				}
				return result;
			}
		}

		// Token: 0x0600AE7E RID: 44670 RVA: 0x003D7256 File Offset: 0x003D5456
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Sleep suppressor"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600AE7F RID: 44671 RVA: 0x003D7274 File Offset: 0x003D5474
		protected override void Tick()
		{
			base.Tick();
			if (!this.IsHashIntervalTick(30))
			{
				return;
			}
			if (!this.PowerTraderComp.PowerOn)
			{
				return;
			}
			List<Pawn> allHumanlikeSpawned = base.Map.mapPawns.AllHumanlikeSpawned;
			for (int i = 0; i < allHumanlikeSpawned.Count; i++)
			{
				Pawn pawn = allHumanlikeSpawned[i];
				if (pawn.RaceProps.Humanlike && base.Position.InHorDistOf(pawn.PositionHeld, this.NoiseSourceComp.Props.radius))
				{
					((Hediff_SleepSuppression)pawn.health.GetOrAddHediff(HediffDefOf.SleepSuppression, null, null, null)).lastTickInRangeOfSuppressor = GenTicks.TicksGame;
				}
			}
		}

		// Token: 0x0600AE80 RID: 44672 RVA: 0x003D7328 File Offset: 0x003D5528
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!this.PowerTraderComp.PowerOn)
			{
				return text;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "SleepSuppressorEffectDescription".Translate().Resolve().CapitalizeFirst();
		}

		// Token: 0x040064EC RID: 25836
		private const int UpdateHediffInterval = 30;

		// Token: 0x040064ED RID: 25837
		private CompPowerTrader powerTraderComp;

		// Token: 0x040064EE RID: 25838
		private CompNoiseSource noiseSourceComp;
	}
}
