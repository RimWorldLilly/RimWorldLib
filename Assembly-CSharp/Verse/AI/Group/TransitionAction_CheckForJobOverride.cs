using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x0200095D RID: 2397
	public class TransitionAction_CheckForJobOverride : TransitionAction
	{
		// Token: 0x0600495D RID: 18781 RVA: 0x001CA35C File Offset: 0x001C855C
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i].CurJob != null)
				{
					ownedPawns[i].jobs.CheckForJobOverride(0f, true);
				}
			}
		}
	}
}
