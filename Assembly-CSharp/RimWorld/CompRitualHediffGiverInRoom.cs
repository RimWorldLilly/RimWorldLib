using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200209A RID: 8346
	public class CompRitualHediffGiverInRoom : ThingComp
	{
		// Token: 0x170025C8 RID: 9672
		// (get) Token: 0x0600D25A RID: 53850 RVA: 0x004D420C File Offset: 0x004D240C
		private CompProperties_RitualHediffGiverInRoom Props
		{
			get
			{
				return (CompProperties_RitualHediffGiverInRoom)this.props;
			}
		}

		// Token: 0x0600D25B RID: 53851 RVA: 0x004D421C File Offset: 0x004D241C
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(60) && this.parent.IsRitualTarget())
			{
				Room room = this.parent.GetRoom(RegionType.Set_All);
				if (room == null || room.TouchesMapEdge)
				{
					return;
				}
				foreach (Pawn pawn in this.parent.Map.mapPawns.AllPawnsSpawned)
				{
					if (pawn.RaceProps.IsFlesh && pawn.GetRoom(RegionType.Set_All) == room && this.parent.Position.InHorDistOf(pawn.Position, this.Props.minRadius))
					{
						Hediff hediff = HediffMaker.MakeHediff(this.Props.hediff, pawn, null);
						if (this.Props.severity > 0f)
						{
							hediff.Severity = this.Props.severity;
						}
						pawn.health.AddHediff(hediff, null, null, null);
						if (this.Props.resetLastRecreationalDrugTick && pawn.mindState != null)
						{
							pawn.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
						}
					}
				}
			}
		}

		// Token: 0x04008544 RID: 34116
		private const int CheckInterval = 60;
	}
}
