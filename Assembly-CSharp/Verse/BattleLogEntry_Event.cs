using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E2 RID: 482
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x00044F6F File Offset: 0x0004316F
		private string SubjectName
		{
			get
			{
				if (this.subjectPawn == null)
				{
					return "null";
				}
				return this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00044F8A File Offset: 0x0004318A
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00044F94 File Offset: 0x00043194
		public BattleLogEntry_Event(Thing subject, RulePackDef eventDef, Thing initiator) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			this.eventDef = eventDef;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00044FF9 File Offset: 0x000431F9
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0004500F File Offset: 0x0004320F
		public override IEnumerable<Thing> GetConcerns()
		{
			BattleLogEntry_Event.<GetConcerns>d__10 <GetConcerns>d__ = new BattleLogEntry_Event.<GetConcerns>d__10(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0004501F File Offset: 0x0004321F
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiatorPawn)) || (pov == this.initiatorPawn && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00045059 File Offset: 0x00043259
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00045098 File Offset: 0x00043298
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(this.eventDef);
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, false, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, false, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			return result;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00045168 File Offset: 0x00043368
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000451CD File Offset: 0x000433CD
		public override string ToString()
		{
			string defName = this.eventDef.defName;
			string str = ": ";
			Pawn pawn = this.subjectPawn;
			return defName + str + ((pawn != null) ? pawn.ToString() : null);
		}

		// Token: 0x04000D8B RID: 3467
		protected RulePackDef eventDef;

		// Token: 0x04000D8C RID: 3468
		protected Pawn subjectPawn;

		// Token: 0x04000D8D RID: 3469
		protected ThingDef subjectThing;

		// Token: 0x04000D8E RID: 3470
		protected Pawn initiatorPawn;

		// Token: 0x04000D8F RID: 3471
		protected ThingDef initiatorThing;
	}
}
