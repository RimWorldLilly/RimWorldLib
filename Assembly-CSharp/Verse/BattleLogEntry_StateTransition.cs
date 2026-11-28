using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E8 RID: 488
	public class BattleLogEntry_StateTransition : LogEntry
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x000467C5 File Offset: 0x000449C5
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

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000467E0 File Offset: 0x000449E0
		public BattleLogEntry_StateTransition() : base(null)
		{
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000467EC File Offset: 0x000449EC
		public BattleLogEntry_StateTransition(Thing subject, RulePackDef transitionDef, Pawn initiator, Hediff culpritHediff, BodyPartRecord culpritTargetDef) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			this.transitionDef = transitionDef;
			this.initiator = initiator;
			if (culpritHediff != null)
			{
				this.culpritHediffDef = culpritHediff.def;
				if (culpritHediff.Part != null)
				{
					this.culpritHediffTargetPart = culpritHediff.Part;
				}
			}
			this.culpritTargetPart = culpritTargetDef;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00046862 File Offset: 0x00044A62
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiator;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00046878 File Offset: 0x00044A78
		public override IEnumerable<Thing> GetConcerns()
		{
			BattleLogEntry_StateTransition.<GetConcerns>d__12 <GetConcerns>d__ = new BattleLogEntry_StateTransition.<GetConcerns>d__12(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00046888 File Offset: 0x00044A88
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x000468C2 File Offset: 0x00044AC2
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00046900 File Offset: 0x00044B00
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (pov == null || pov == this.subjectPawn)
			{
				if (this.transitionDef != RulePackDefOf.Transition_Downed)
				{
					return LogEntry.Skull;
				}
				return LogEntry.Downed;
			}
			else
			{
				if (pov != this.initiator)
				{
					return null;
				}
				if (this.transitionDef != RulePackDefOf.Transition_Downed)
				{
					return LogEntry.SkullTarget;
				}
				return LogEntry.DownedTarget;
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00046958 File Offset: 0x00044B58
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, false, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			result.Includes.Add(this.transitionDef);
			if (this.initiator != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants, false, true));
			}
			if (this.culpritHediffDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForHediffDef("CULPRITHEDIFF", this.culpritHediffDef, this.culpritHediffTargetPart));
			}
			if (this.culpritHediffTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_target", this.culpritHediffTargetPart));
			}
			if (this.culpritTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_originaltarget", this.culpritTargetPart));
			}
			return result;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00046A74 File Offset: 0x00044C74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.transitionDef, "transitionDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_Defs.Look<HediffDef>(ref this.culpritHediffDef, "culpritHediffDef");
			Scribe_BodyParts.Look(ref this.culpritHediffTargetPart, "culpritHediffTargetPart", null);
			Scribe_BodyParts.Look(ref this.culpritTargetPart, "culpritTargetPart", null);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00046AFB File Offset: 0x00044CFB
		public override string ToString()
		{
			string defName = this.transitionDef.defName;
			string str = ": ";
			Pawn pawn = this.subjectPawn;
			return defName + str + ((pawn != null) ? pawn.ToString() : null);
		}

		// Token: 0x04000DB5 RID: 3509
		private RulePackDef transitionDef;

		// Token: 0x04000DB6 RID: 3510
		private Pawn subjectPawn;

		// Token: 0x04000DB7 RID: 3511
		private ThingDef subjectThing;

		// Token: 0x04000DB8 RID: 3512
		private Pawn initiator;

		// Token: 0x04000DB9 RID: 3513
		private HediffDef culpritHediffDef;

		// Token: 0x04000DBA RID: 3514
		private BodyPartRecord culpritHediffTargetPart;

		// Token: 0x04000DBB RID: 3515
		private BodyPartRecord culpritTargetPart;
	}
}
