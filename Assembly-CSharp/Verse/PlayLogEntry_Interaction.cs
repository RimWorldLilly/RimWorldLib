using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001F6 RID: 502
	public class PlayLogEntry_Interaction : LogEntry
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0004A489 File Offset: 0x00048689
		protected string InitiatorName
		{
			get
			{
				if (this.initiator == null)
				{
					return "null";
				}
				return this.initiator.LabelShort;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0004A4A4 File Offset: 0x000486A4
		private string RecipientName
		{
			get
			{
				if (this.recipient == null)
				{
					return "null";
				}
				return this.recipient.LabelShort;
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0004A4BF File Offset: 0x000486BF
		public PlayLogEntry_Interaction() : base(null)
		{
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0004A4C8 File Offset: 0x000486C8
		public PlayLogEntry_Interaction(InteractionDef intDef, Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks) : base(null)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipient = recipient;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0004A506 File Offset: 0x00048706
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipient;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0004A51C File Offset: 0x0004871C
		public override IEnumerable<Thing> GetConcerns()
		{
			PlayLogEntry_Interaction.<GetConcerns>d__13 <GetConcerns>d__ = new PlayLogEntry_Interaction.<GetConcerns>d__13(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0004A52C File Offset: 0x0004872C
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.recipient && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.recipient));
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0004A566 File Offset: 0x00048766
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.recipient, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.recipient)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0004A5A3 File Offset: 0x000487A3
		public override Texture2D IconFromPOV(Thing pov)
		{
			return this.intDef.GetSymbol(this.initiatorFaction, this.initiatorIdeo);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0004A5BC File Offset: 0x000487BC
		public override Color? IconColorFromPOV(Thing pov)
		{
			return this.intDef.GetSymbolColor(this.initiatorFaction);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0004A5CF File Offset: 0x000487CF
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.initiatorFaction == faction)
			{
				this.initiatorFaction = null;
			}
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0004A5E1 File Offset: 0x000487E1
		public override void Notify_IdeoRemoved(Ideo ideo)
		{
			if (this.initiatorIdeo == ideo)
			{
				this.initiatorIdeo = null;
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0004A5F3 File Offset: 0x000487F3
		public override string GetTipString()
		{
			return this.intDef.LabelCap + "\n" + base.GetTipString();
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0004A61C File Offset: 0x0004881C
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null || this.recipient == null)
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
				return "[" + this.intDef.label + " error: null pawn reference]";
			}
			Rand.PushState();
			Rand.Seed = this.logID;
			GrammarRequest request = base.GenerateGrammarRequest();
			string text;
			if (pov == this.initiator)
			{
				request.IncludesBare.Add(this.intDef.logRulesInitiator);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
				text = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else if (pov == this.recipient)
			{
				if (this.intDef.logRulesRecipient != null)
				{
					request.IncludesBare.Add(this.intDef.logRulesRecipient);
				}
				else
				{
					request.IncludesBare.Add(this.intDef.logRulesInitiator);
				}
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
				text = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog, null, null, null, true);
			}
			else
			{
				Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
				text = this.ToString();
			}
			if (this.extraSentencePacks != null)
			{
				for (int i = 0; i < this.extraSentencePacks.Count; i++)
				{
					request.Clear();
					request.Includes.Add(this.extraSentencePacks[i]);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
					text = text + " " + GrammarResolver.Resolve(this.extraSentencePacks[i].FirstRuleKeyword, request, "extraSentencePack", forceLog, this.extraSentencePacks[i].FirstUntranslatedRuleKeyword, null, null, true);
				}
			}
			Rand.PopState();
			return text;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0004A894 File Offset: 0x00048A94
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<InteractionDef>(ref this.intDef, "intDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipient, "recipient", true);
			Scribe_Collections.Look<RulePackDef>(ref this.extraSentencePacks, "extras", LookMode.Undefined, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.initiatorFaction, "initiatorFaction", false);
			Scribe_References.Look<Ideo>(ref this.initiatorIdeo, "initiatorIdeo", false);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0004A911 File Offset: 0x00048B11
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.intDef.label,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x04000E39 RID: 3641
		protected InteractionDef intDef;

		// Token: 0x04000E3A RID: 3642
		protected Pawn initiator;

		// Token: 0x04000E3B RID: 3643
		protected Pawn recipient;

		// Token: 0x04000E3C RID: 3644
		protected List<RulePackDef> extraSentencePacks;

		// Token: 0x04000E3D RID: 3645
		public Faction initiatorFaction;

		// Token: 0x04000E3E RID: 3646
		public Ideo initiatorIdeo;
	}
}
