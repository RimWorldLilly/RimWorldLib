using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E44 RID: 3652
	public class ThoughtWorker_PsychicEmanatorSoothe : ThoughtWorker
	{
		// Token: 0x0600617F RID: 24959 RVA: 0x002331EC File Offset: 0x002313EC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.PsychicEmanator);
			for (int i = 0; i < list.Count; i++)
			{
				CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
				if ((compPowerTrader == null || compPowerTrader.PowerOn) && p.Position.InHorDistOf(list[i].Position, 15f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003604 RID: 13828
		private const float Radius = 15f;
	}
}
