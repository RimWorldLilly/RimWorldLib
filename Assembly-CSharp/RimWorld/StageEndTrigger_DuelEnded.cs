using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200166B RID: 5739
	public class StageEndTrigger_DuelEnded : StageEndTrigger_AnyPawnDead
	{
		// Token: 0x0600901B RID: 36891 RVA: 0x0034EFF0 File Offset: 0x0034D1F0
		protected override bool Trigger(LordJob_Ritual ritual)
		{
			if (base.Trigger(ritual))
			{
				return true;
			}
			foreach (string roleId in this.roleIds)
			{
				using (IEnumerator<Pawn> enumerator2 = ritual.assignments.AssignedPawns(roleId).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Downed)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
