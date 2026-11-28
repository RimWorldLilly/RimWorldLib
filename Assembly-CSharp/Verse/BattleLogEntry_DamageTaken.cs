using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E1 RID: 481
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x00044E0E File Offset: 0x0004300E
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn == null)
				{
					return "null";
				}
				return this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00044E29 File Offset: 0x00043029
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00044E32 File Offset: 0x00043032
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00044E50 File Offset: 0x00043050
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00044E66 File Offset: 0x00043066
		public override IEnumerable<Thing> GetConcerns()
		{
			BattleLogEntry_DamageTaken.<GetConcerns>d__8 <GetConcerns>d__ = new BattleLogEntry_DamageTaken.<GetConcerns>d__8(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00044E76 File Offset: 0x00043076
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return CameraJumper.CanJump(this.recipientPawn);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00044E88 File Offset: 0x00043088
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn, CameraJumper.MovementMode.Pan);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00044E9B File Offset: 0x0004309B
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00044EA2 File Offset: 0x000430A2
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00044EC0 File Offset: 0x000430C0
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709);
			}
			result.Includes.Add(this.ruleDef);
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, false, true));
			return result;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00044F23 File Offset: 0x00043123
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00044F5D File Offset: 0x0004315D
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}

		// Token: 0x04000D88 RID: 3464
		private Pawn initiatorPawn;

		// Token: 0x04000D89 RID: 3465
		private Pawn recipientPawn;

		// Token: 0x04000D8A RID: 3466
		private RulePackDef ruleDef;
	}
}
