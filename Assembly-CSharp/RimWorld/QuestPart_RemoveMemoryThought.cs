using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E0 RID: 4320
	public class QuestPart_RemoveMemoryThought : QuestPart
	{
		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06006BFB RID: 27643 RVA: 0x0025ED31 File Offset: 0x0025CF31
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_RemoveMemoryThought.<get_QuestLookTargets>d__7 <get_QuestLookTargets>d__ = new QuestPart_RemoveMemoryThought.<get_QuestLookTargets>d__7(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006BFC RID: 27644 RVA: 0x0025ED44 File Offset: 0x0025CF44
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				if (this.count != null)
				{
					for (int i = 0; i < this.count.Value; i++)
					{
						Thought_Memory thought_Memory = this.pawn.needs.mood.thoughts.memories.Memories.FirstOrDefault((Thought_Memory m) => this.def == m.def && (this.otherPawn == null || m.otherPawn == this.otherPawn));
						if (thought_Memory == null)
						{
							return;
						}
						this.pawn.needs.mood.thoughts.memories.RemoveMemory(thought_Memory);
					}
					return;
				}
				if (this.otherPawn == null)
				{
					this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.def);
					return;
				}
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(this.def, this.otherPawn);
			}
		}

		// Token: 0x06006BFD RID: 27645 RVA: 0x0025EE60 File Offset: 0x0025D060
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.addToLookTargets, "addToLookTargets", false, false);
			Scribe_Values.Look<int?>(ref this.count, "count", null, false);
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06006BFE RID: 27646 RVA: 0x0025EEE3 File Offset: 0x0025D0E3
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x06006BFF RID: 27647 RVA: 0x0025EF0F File Offset: 0x0025D10F
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

		// Token: 0x040044CA RID: 17610
		public string inSignal;

		// Token: 0x040044CB RID: 17611
		public ThoughtDef def;

		// Token: 0x040044CC RID: 17612
		public Pawn pawn;

		// Token: 0x040044CD RID: 17613
		public Pawn otherPawn;

		// Token: 0x040044CE RID: 17614
		public int? count;

		// Token: 0x040044CF RID: 17615
		public bool addToLookTargets = true;
	}
}
