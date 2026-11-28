using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107F RID: 4223
	public class QuestPart_FactionRelationKind : QuestPartActivable
	{
		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x060069D4 RID: 27092 RVA: 0x0025485E File Offset: 0x00252A5E
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_FactionRelationKind.<get_InvolvedFactions>d__4 <get_InvolvedFactions>d__ = new QuestPart_FactionRelationKind.<get_InvolvedFactions>d__4(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x00254870 File Offset: 0x00252A70
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.faction1 != null && this.faction2 != null && this.faction1.RelationKindWith(this.faction2) == this.relationKind)
			{
				base.Complete(this.faction1.Named("SUBJECT"));
			}
		}

		// Token: 0x060069D6 RID: 27094 RVA: 0x002548C2 File Offset: 0x00252AC2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x002548FE File Offset: 0x00252AFE
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.faction1 = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.faction2 = Faction.OfPlayer;
			this.relationKind = FactionRelationKind.Neutral;
		}

		// Token: 0x04004346 RID: 17222
		public Faction faction1;

		// Token: 0x04004347 RID: 17223
		public Faction faction2;

		// Token: 0x04004348 RID: 17224
		public FactionRelationKind relationKind;
	}
}
