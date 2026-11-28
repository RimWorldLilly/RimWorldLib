using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AC RID: 4268
	public class QuestPart_AssignMechToMechanitor : QuestPart
	{
		// Token: 0x06006ACD RID: 27341 RVA: 0x002582D4 File Offset: 0x002564D4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && MechanitorUtility.IsMechanitor(this.mechanitor) && this.mechanitor.mechanitor.CanOverseeSubject(this.mech))
			{
				this.mechanitor.relations.AddDirectRelation(PawnRelationDefOf.Overseer, this.mech);
			}
		}

		// Token: 0x06006ACE RID: 27342 RVA: 0x0025833B File Offset: 0x0025653B
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.mech == pawn)
			{
				this.mech = null;
			}
			if (this.mechanitor == pawn)
			{
				this.mechanitor = null;
			}
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x0025835D File Offset: 0x0025655D
		public override bool QuestPartReserves(Pawn p)
		{
			return p == this.mech || p == this.mechanitor;
		}

		// Token: 0x06006AD0 RID: 27344 RVA: 0x00258373 File Offset: 0x00256573
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.mech, "mech", false);
			Scribe_References.Look<Pawn>(ref this.mechanitor, "mechanitor", false);
		}

		// Token: 0x040043E4 RID: 17380
		public Pawn mech;

		// Token: 0x040043E5 RID: 17381
		public Pawn mechanitor;

		// Token: 0x040043E6 RID: 17382
		public string inSignal;
	}
}
