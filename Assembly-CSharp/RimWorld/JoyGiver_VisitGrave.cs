using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B83 RID: 2947
	public class JoyGiver_VisitGrave : JoyGiver
	{
		// Token: 0x0600558B RID: 21899 RVA: 0x001FD0E0 File Offset: 0x001FB2E0
		public override Job TryGiveJob(Pawn pawn)
		{
			JoyGiver_VisitGrave.<>c__DisplayClass0_0 CS$<>8__locals1 = new JoyGiver_VisitGrave.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.allowedOutside = JoyUtility.EnjoyableOutsideNow(CS$<>8__locals1.pawn, null);
			Thing t;
			if (!CS$<>8__locals1.pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).Where(new Func<Thing, bool>(CS$<>8__locals1.<TryGiveJob>g__Validator|0)).TryRandomElementByWeight(delegate(Thing x)
			{
				float lengthHorizontal = (x.Position - CS$<>8__locals1.pawn.Position).LengthHorizontal;
				return Mathf.Max(150f - lengthHorizontal, 5f);
			}, out t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}
	}
}
