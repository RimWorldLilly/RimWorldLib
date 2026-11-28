using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD2 RID: 3026
	public class JobGiver_CreateAndEnterCocoon : JobGiver_CreateAndEnterDryadHolder
	{
		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x0600570C RID: 22284 RVA: 0x00204ED5 File Offset: 0x002030D5
		public override JobDef JobDef
		{
			get
			{
				return JobDefOf.CreateAndEnterCocoon;
			}
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x00204EDC File Offset: 0x002030DC
		public override bool ExtraValidator(Pawn pawn, CompTreeConnection connectionComp)
		{
			return connectionComp.DryadKind != pawn.kindDef || base.ExtraValidator(pawn, connectionComp);
		}
	}
}
