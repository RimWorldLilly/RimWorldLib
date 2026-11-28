using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001F7 RID: 503
	public class PlayLogEntry_InteractionSinglePawn : LogEntry
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0004A94E File Offset: 0x00048B4E
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

		// Token: 0x06000E65 RID: 3685 RVA: 0x0004A969 File Offset: 0x00048B69
		public PlayLogEntry_InteractionSinglePawn() : base(null)
		{
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0004A972 File Offset: 0x00048B72
		public PlayLogEntry_InteractionSinglePawn(InteractionDef intDef, Pawn initiator, List<RulePackDef> extraSentencePacks) : base(null)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0004A9A8 File Offset: 0x00048BA8
		public override bool Concerns(Thing t)
		{
			return t == this.initiator;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0004A9B3 File Offset: 0x00048BB3
		public override IEnumerable<Thing> GetConcerns()
		{
			PlayLogEntry_InteractionSinglePawn.<GetConcerns>d__10 <GetConcerns>d__ = new PlayLogEntry_InteractionSinglePawn.<GetConcerns>d__10(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0004A9C3 File Offset: 0x00048BC3
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return pov == this.initiator && CameraJumper.CanJump(this.initiator);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0004A9E0 File Offset: 0x00048BE0
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0004AA02 File Offset: 0x00048C02
		public override Texture2D IconFromPOV(Thing pov)
		{
			return this.intDef.GetSymbol(this.initiatorFaction, this.initiatorIdeo);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0004AA1B File Offset: 0x00048C1B
		public override Color? IconColorFromPOV(Thing pov)
		{
			return this.intDef.GetSymbolColor(this.initiatorFaction);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0004AA2E File Offset: 0x00048C2E
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.initiatorFaction == faction)
			{
				this.initiatorFaction = null;
			}
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0004AA40 File Offset: 0x00048C40
		public override void Notify_IdeoRemoved(Ideo ideo)
		{
			if (this.initiatorIdeo == ideo)
			{
				this.initiatorIdeo = null;
			}
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0004AA52 File Offset: 0x00048C52
		public override string GetTipString()
		{
			return this.intDef.LabelCap + "\n" + base.GetTipString();
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0004AA7C File Offset: 0x00048C7C
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null)
			{
				Log.ErrorOnce("PlayLogEntry_InteractionSinglePawn has a null pawn reference.", 34422);
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
				text = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else
			{
				Log.ErrorOnce("Cannot display PlayLogEntry_InteractionSinglePawn from POV who isn't initiator.", 51251);
				text = this.ToString();
			}
			if (this.extraSentencePacks != null)
			{
				for (int i = 0; i < this.extraSentencePacks.Count; i++)
				{
					request.Clear();
					request.Includes.Add(this.extraSentencePacks[i]);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
					text = text + " " + GrammarResolver.Resolve(this.extraSentencePacks[i].FirstRuleKeyword, request, "extraSentencePack", forceLog, this.extraSentencePacks[i].FirstUntranslatedRuleKeyword, null, null, true);
				}
			}
			Rand.PopState();
			return text;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0004ABF4 File Offset: 0x00048DF4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<InteractionDef>(ref this.intDef, "intDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_Collections.Look<RulePackDef>(ref this.extraSentencePacks, "extras", LookMode.Undefined, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.initiatorFaction, "initiatorFaction", false);
			Scribe_References.Look<Ideo>(ref this.initiatorIdeo, "initiatorIdeo", false);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0004AC60 File Offset: 0x00048E60
		public override string ToString()
		{
			return this.intDef.label + ": " + this.InitiatorName;
		}

		// Token: 0x04000E3F RID: 3647
		protected InteractionDef intDef;

		// Token: 0x04000E40 RID: 3648
		protected Pawn initiator;

		// Token: 0x04000E41 RID: 3649
		protected List<RulePackDef> extraSentencePacks;

		// Token: 0x04000E42 RID: 3650
		public Faction initiatorFaction;

		// Token: 0x04000E43 RID: 3651
		public Ideo initiatorIdeo;
	}
}
