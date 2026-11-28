using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001083 RID: 4227
	public class QuestPart_IsDead : QuestPartActivable
	{
		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x060069E8 RID: 27112 RVA: 0x00254F77 File Offset: 0x00253177
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_IsDead.<get_QuestLookTargets>d__2 <get_QuestLookTargets>d__ = new QuestPart_IsDead.<get_QuestLookTargets>d__2(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x00254F87 File Offset: 0x00253187
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.Destroyed)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x060069EA RID: 27114 RVA: 0x00254FBA File Offset: 0x002531BA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x00254FD3 File Offset: 0x002531D3
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x060069EC RID: 27116 RVA: 0x00254FFC File Offset: 0x002531FC
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04004359 RID: 17241
		public Pawn pawn;
	}
}
