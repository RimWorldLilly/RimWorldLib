using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CA2 RID: 3234
	public class PsychicRitualDef_SummonFleshbeastsPlayer : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B92 RID: 23442 RVA: 0x0021A5B0 File Offset: 0x002187B0
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_SummonFleshbeastsPlayer(this.InvokerRole));
			return list;
		}

		// Token: 0x0400346B RID: 13419
		public SimpleCurve fleshbeastCombatPointsFromQualityCurve;
	}
}
