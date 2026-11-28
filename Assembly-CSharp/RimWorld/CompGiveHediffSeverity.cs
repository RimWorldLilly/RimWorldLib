using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A4 RID: 6308
	public class CompGiveHediffSeverity : ThingComp
	{
		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x06009DFA RID: 40442 RVA: 0x0038B70A File Offset: 0x0038990A
		private CompProperties_GiveHediffSeverity Props
		{
			get
			{
				return (CompProperties_GiveHediffSeverity)this.props;
			}
		}

		// Token: 0x06009DFB RID: 40443 RVA: 0x0038B717 File Offset: 0x00389917
		private bool AppliesTo(Pawn pawn)
		{
			return pawn.GetRoom(RegionType.Set_All) == this.parent.GetRoom(RegionType.Set_All) && (this.Props.allowMechs || pawn.RaceProps.IsFlesh);
		}

		// Token: 0x06009DFC RID: 40444 RVA: 0x0038B750 File Offset: 0x00389950
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (!this.parent.IsHashIntervalTick(87))
			{
				return;
			}
			CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			if (compRefuelable != null && !compRefuelable.HasFuel)
			{
				return;
			}
			if (compPowerTrader != null && !compPowerTrader.PowerOn)
			{
				return;
			}
			int num = GenRadial.NumCellsInRadius(this.Props.range);
			for (int i = 0; i < num; i++)
			{
				List<Thing> thingList = (this.parent.Position + GenRadial.RadialPattern[i]).GetThingList(this.parent.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Pawn pawn = thingList[j] as Pawn;
					if (pawn != null && this.AppliesTo(pawn))
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false);
						float num2 = this.Props.severityPerSecond * 1.45f;
						if (firstHediffOfDef != null)
						{
							firstHediffOfDef.Severity += num2;
						}
						else
						{
							pawn.health.AddHediff(this.Props.hediff, null, null, null).Severity = num2;
						}
						if (this.Props.drugExposure)
						{
							pawn.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
							Pawn_NeedsTracker needs = pawn.needs;
							if (((needs != null) ? needs.drugsDesire : null) != null)
							{
								pawn.needs.drugsDesire.CurLevel += 0.14500001f;
							}
							ChemicalDef chemical = this.Props.chemical;
							if (((chemical != null) ? chemical.addictionHediff : null) != null && pawn.needs != null)
							{
								HediffDef addictionHediffDef = this.Props.chemical.addictionHediff;
								Need need = pawn.needs.AllNeeds.Find((Need x) => x.def == addictionHediffDef.chemicalNeed);
								if (need != null)
								{
									need.CurLevel += 0.14500001f;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04005D7A RID: 23930
		private const int UpdateInterval = 87;

		// Token: 0x04005D7B RID: 23931
		private const float ChemicalSatisfactionPerSecond = 0.1f;
	}
}
