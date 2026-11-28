using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001F8 RID: 504
	public class PlayLogEntry_InteractionWithMany : PlayLogEntry_Interaction
	{
		// Token: 0x06000E73 RID: 3699 RVA: 0x0004AC7D File Offset: 0x00048E7D
		public PlayLogEntry_InteractionWithMany()
		{
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0004AC85 File Offset: 0x00048E85
		public PlayLogEntry_InteractionWithMany(InteractionDef intDef, Pawn initiator, List<Pawn> recipients, List<RulePackDef> extraSentencePacks)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipients = recipients;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0004ACC4 File Offset: 0x00048EC4
		public override bool Concerns(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && (t == this.initiator || this.recipients.Contains(pawn));
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0004ACF4 File Offset: 0x00048EF4
		public override IEnumerable<Thing> GetConcerns()
		{
			PlayLogEntry_InteractionWithMany.<GetConcerns>d__4 <GetConcerns>d__ = new PlayLogEntry_InteractionWithMany.<GetConcerns>d__4(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0004AD04 File Offset: 0x00048F04
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null || this.recipients == null || this.recipients.Contains(null))
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
				return "[" + this.intDef.label + " error: null pawn reference]";
			}
			Rand.PushState();
			Rand.Seed = this.logID;
			GrammarRequest request = base.GenerateGrammarRequest();
			string result;
			if (pov == this.initiator)
			{
				request.IncludesBare.Add(this.intDef.logRulesInitiator);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				result = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else
			{
				Pawn pawn = pov as Pawn;
				if (pawn != null && this.recipients.Contains(pawn))
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
					request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", pawn, request.Constants, true, true));
					result = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog, null, null, null, true);
				}
				else
				{
					Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
					result = this.ToString();
				}
			}
			Rand.PopState();
			return result;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0004AEA4 File Offset: 0x000490A4
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipients.Contains(pov as Pawn) && CameraJumper.CanJump(this.initiator);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0004AECC File Offset: 0x000490CC
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.recipients.RandomElement<Pawn>(), CameraJumper.MovementMode.Pan);
				return;
			}
			if (this.recipients.Contains(pov as Pawn))
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0004AF23 File Offset: 0x00049123
		public override string ToString()
		{
			return this.intDef.label + ": " + base.InitiatorName + "-> Many";
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0004AF48 File Offset: 0x00049148
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.recipients, "recipients", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.recipients.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04000E44 RID: 3652
		private List<Pawn> recipients;
	}
}
