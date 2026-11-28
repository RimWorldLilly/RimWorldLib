using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB0 RID: 6832
	public class Building_FrenzyInducer : Building
	{
		// Token: 0x17001EE4 RID: 7908
		// (get) Token: 0x0600ACF4 RID: 44276 RVA: 0x003CF47C File Offset: 0x003CD67C
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

		// Token: 0x0600ACF5 RID: 44277 RVA: 0x003CF4A2 File Offset: 0x003CD6A2
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Frenzy inducer"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600ACF6 RID: 44278 RVA: 0x003CF4C0 File Offset: 0x003CD6C0
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
				if (pawn.RaceProps.Humanlike && base.Position.InHorDistOf(pawn.PositionHeld, 8.9f))
				{
					((Hediff_FrenzyField)pawn.health.GetOrAddHediff(HediffDefOf.FrenzyField, null, null, null)).lastTickInRangeOfInducer = GenTicks.TicksGame;
				}
			}
		}

		// Token: 0x0600ACF7 RID: 44279 RVA: 0x003CF56C File Offset: 0x003CD76C
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
			return text + "FrenzyInducerEffectDescription".Translate().Resolve().CapitalizeFirst();
		}

		// Token: 0x04006455 RID: 25685
		private const int UpdateHediffInterval = 30;

		// Token: 0x04006456 RID: 25686
		private const float Radius = 8.9f;

		// Token: 0x04006457 RID: 25687
		private CompPowerTrader powerTraderComp;
	}
}
