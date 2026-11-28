using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F9 RID: 4345
	public class QuestPart_SituationalThought : QuestPartActivable
	{
		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06006C6C RID: 27756 RVA: 0x002605FC File Offset: 0x0025E7FC
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_SituationalThought.<get_QuestLookTargets>d__5 <get_QuestLookTargets>d__ = new QuestPart_SituationalThought.<get_QuestLookTargets>d__5(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006C6D RID: 27757 RVA: 0x0026060C File Offset: 0x0025E80C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.stage, "stage", 0, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x00260664 File Offset: 0x0025E864
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThoughtDefOf.DecreeUnmet;
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x06006C6F RID: 27759 RVA: 0x00260687 File Offset: 0x0025E887
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04004501 RID: 17665
		public ThoughtDef def;

		// Token: 0x04004502 RID: 17666
		public Pawn pawn;

		// Token: 0x04004503 RID: 17667
		public int stage;

		// Token: 0x04004504 RID: 17668
		public int delayTicks;
	}
}
