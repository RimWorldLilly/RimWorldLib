using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CA1 RID: 3233
	public class PsychicRitualDef_SummonFleshbeasts : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B8F RID: 23439 RVA: 0x0021A53A File Offset: 0x0021873A
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonFleshbeastsCultist(this.InvokerRole));
			return list;
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x0021A558 File Offset: 0x00218758
		public override void CalculateMaxPower(PsychicRitualRoleAssignments assignments, List<QualityFactor> powerFactorsOut, out float power)
		{
			power = 0f;
			Pawn pawn = assignments.FirstAssignedPawn(this.InvokerRole);
			object obj;
			if (pawn == null)
			{
				obj = null;
			}
			else
			{
				Lord lord = pawn.GetLord();
				obj = ((lord != null) ? lord.LordJob : null);
			}
			LordJob_PsychicRitual lordJob_PsychicRitual = obj as LordJob_PsychicRitual;
			if (lordJob_PsychicRitual != null)
			{
				power = Mathf.Max(power, lordJob_PsychicRitual.points);
			}
		}

		// Token: 0x0400346A RID: 13418
		public SimpleCurve fleshbeastPointsFromThreatPointsCurve;
	}
}
