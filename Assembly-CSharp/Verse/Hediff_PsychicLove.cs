using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004C2 RID: 1218
	public class Hediff_PsychicLove : HediffWithTarget
	{
		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x000C9B5C File Offset: 0x000C7D5C
		public override string LabelBase
		{
			get
			{
				string[] array = new string[5];
				array[0] = base.LabelBase;
				array[1] = " ";
				array[2] = this.def.targetPrefix;
				array[3] = " ";
				int num = 4;
				Thing target = this.target;
				array[num] = ((target != null) ? target.LabelShortCap : null);
				return string.Concat(array);
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000C9BB0 File Offset: 0x000C7DB0
		public override void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
			if (otherPawn == this.target && (relationDef == PawnRelationDefOf.Lover || relationDef == PawnRelationDefOf.Fiance || relationDef == PawnRelationDefOf.Spouse))
			{
				this.pawn.health.RemoveHediff(this);
			}
		}
	}
}
