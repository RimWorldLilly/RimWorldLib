using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001084 RID: 4228
	public class QuestPart_IsPrisoner : QuestPartActivable
	{
		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x060069EF RID: 27119 RVA: 0x0025501E File Offset: 0x0025321E
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_IsPrisoner.<get_QuestLookTargets>d__2 <get_QuestLookTargets>d__ = new QuestPart_IsPrisoner.<get_QuestLookTargets>d__2(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x060069F0 RID: 27120 RVA: 0x0025502E File Offset: 0x0025322E
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.IsPrisoner)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x060069F1 RID: 27121 RVA: 0x00255061 File Offset: 0x00253261
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x060069F2 RID: 27122 RVA: 0x0025507A File Offset: 0x0025327A
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x060069F3 RID: 27123 RVA: 0x002550A3 File Offset: 0x002532A3
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x0400435A RID: 17242
		public Pawn pawn;
	}
}
