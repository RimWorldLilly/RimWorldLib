using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DE RID: 4318
	public class QuestPart_ReleaseParalyzedAnimals : QuestPart
	{
		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06006BF1 RID: 27633 RVA: 0x0025EB0B File Offset: 0x0025CD0B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_ReleaseParalyzedAnimals.<get_QuestLookTargets>d__2 <get_QuestLookTargets>d__ = new QuestPart_ReleaseParalyzedAnimals.<get_QuestLookTargets>d__2(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006BF2 RID: 27634 RVA: 0x0025EB1B File Offset: 0x0025CD1B
		private bool IsParalyzed(Pawn pawn)
		{
			return pawn.health.hediffSet.HasHediff(HediffDefOf.Abasia, false);
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x0025EB34 File Offset: 0x0025CD34
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i].RaceProps.Animal && this.IsParalyzed(this.pawns[i]) && this.pawns[i].Faction != null)
				{
					this.pawns[i].SetFaction(null, null);
				}
			}
		}

		// Token: 0x06006BF4 RID: 27636 RVA: 0x0025EBB0 File Offset: 0x0025CDB0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006BF5 RID: 27637 RVA: 0x0025EC0C File Offset: 0x0025CE0C
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040044C7 RID: 17607
		public List<Pawn> pawns = new List<Pawn>();
	}
}
