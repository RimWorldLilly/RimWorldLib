using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A5 RID: 4261
	public class QuestPart_AddMemoryThought : QuestPart
	{
		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06006AA0 RID: 27296 RVA: 0x002579DB File Offset: 0x00255BDB
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_AddMemoryThought.<get_QuestLookTargets>d__6 <get_QuestLookTargets>d__ = new QuestPart_AddMemoryThought.<get_QuestLookTargets>d__6(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006AA1 RID: 27297 RVA: 0x002579EC File Offset: 0x00255BEC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def, this.otherPawn, null);
			}
		}

		// Token: 0x06006AA2 RID: 27298 RVA: 0x00257A54 File Offset: 0x00255C54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.addToLookTargets, "addToLookTargets", false, false);
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06006AA3 RID: 27299 RVA: 0x00257ABD File Offset: 0x00255CBD
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x00257AE9 File Offset: 0x00255CE9
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
			if (this.otherPawn == replace)
			{
				this.otherPawn = with;
			}
		}

		// Token: 0x040043C4 RID: 17348
		public string inSignal;

		// Token: 0x040043C5 RID: 17349
		public ThoughtDef def;

		// Token: 0x040043C6 RID: 17350
		public Pawn pawn;

		// Token: 0x040043C7 RID: 17351
		public Pawn otherPawn;

		// Token: 0x040043C8 RID: 17352
		public bool addToLookTargets = true;
	}
}
