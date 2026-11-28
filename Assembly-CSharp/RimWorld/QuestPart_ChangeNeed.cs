using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B0 RID: 4272
	public class QuestPart_ChangeNeed : QuestPart
	{
		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06006ADF RID: 27359 RVA: 0x00258814 File Offset: 0x00256A14
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_ChangeNeed.<get_QuestLookTargets>d__5 <get_QuestLookTargets>d__ = new QuestPart_ChangeNeed.<get_QuestLookTargets>d__5(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006AE0 RID: 27360 RVA: 0x00258824 File Offset: 0x00256A24
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Need need;
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null && this.pawn.needs.TryGetNeed(this.need, out need))
			{
				need.CurLevel += this.offset;
			}
		}

		// Token: 0x06006AE1 RID: 27361 RVA: 0x00258890 File Offset: 0x00256A90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<float>(ref this.offset, "offset", 0f, false);
		}

		// Token: 0x06006AE2 RID: 27362 RVA: 0x002588EC File Offset: 0x00256AEC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.need = NeedDefOf.Food;
			this.offset = 0.5f;
			if (Find.AnyPlayerHomeMap != null)
			{
				Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x06006AE3 RID: 27363 RVA: 0x00258926 File Offset: 0x00256B26
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x040043F1 RID: 17393
		public string inSignal;

		// Token: 0x040043F2 RID: 17394
		public Pawn pawn;

		// Token: 0x040043F3 RID: 17395
		public NeedDef need;

		// Token: 0x040043F4 RID: 17396
		public float offset;
	}
}
