using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F8 RID: 8696
	public abstract class QuestNode_RaceProperty : QuestNode
	{
		// Token: 0x0600D809 RID: 55305 RVA: 0x005021E4 File Offset: 0x005003E4
		protected override bool TestRunInt(Slate slate)
		{
			if (this.Matches(this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D80A RID: 55306 RVA: 0x00502234 File Offset: 0x00500434
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.Matches(this.value.GetValue(slate)))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x0600D80B RID: 55307 RVA: 0x00502284 File Offset: 0x00500484
		private bool Matches(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is PawnKindDef)
			{
				return this.Matches(((PawnKindDef)obj).RaceProps);
			}
			if (obj is ThingDef)
			{
				return ((ThingDef)obj).race != null && this.Matches(((ThingDef)obj).race);
			}
			if (obj is Pawn)
			{
				return this.Matches(((Pawn)obj).RaceProps);
			}
			if (obj is Faction)
			{
				return ((Faction)obj).def.basicMemberKind != null && this.Matches(((Faction)obj).def.basicMemberKind);
			}
			if (obj is IEnumerable<Pawn>)
			{
				return ((IEnumerable<Pawn>)obj).Any<Pawn>() && ((IEnumerable<Pawn>)obj).All((Pawn x) => this.Matches(x.RaceProps));
			}
			if (obj is IEnumerable<Thing>)
			{
				return ((IEnumerable<Thing>)obj).Any<Thing>() && ((IEnumerable<Thing>)obj).All((Thing x) => x is Pawn && this.Matches(((Pawn)x).RaceProps));
			}
			if (obj is IEnumerable<object>)
			{
				return ((IEnumerable<object>)obj).Any<object>() && ((IEnumerable<object>)obj).All((object x) => x is Pawn && this.Matches(((Pawn)x).RaceProps));
			}
			return obj is string && !((string)obj).NullOrEmpty() && this.Matches(DefDatabase<PawnKindDef>.GetNamed((string)obj, true).RaceProps);
		}

		// Token: 0x0600D80C RID: 55308
		protected abstract bool Matches(RaceProperties raceProperties);

		// Token: 0x04008969 RID: 35177
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x0400896A RID: 35178
		public QuestNode node;

		// Token: 0x0400896B RID: 35179
		public QuestNode elseNode;
	}
}
