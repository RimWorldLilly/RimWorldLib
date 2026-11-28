using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E3 RID: 483
	public class BattleLogEntry_ExplosionImpact : LogEntry_DamageResult
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x000451F6 File Offset: 0x000433F6
		private string InitiatorName
		{
			get
			{
				if (this.initiatorPawn != null)
				{
					return this.initiatorPawn.LabelShort;
				}
				if (this.initiatorThing != null)
				{
					return this.initiatorThing.defName;
				}
				return "null";
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00045225 File Offset: 0x00043425
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn != null)
				{
					return this.recipientPawn.LabelShort;
				}
				if (this.recipientThing != null)
				{
					return this.recipientThing.defName;
				}
				return "null";
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00045254 File Offset: 0x00043454
		public BattleLogEntry_ExplosionImpact() : base(null)
		{
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00045260 File Offset: 0x00043460
		public BattleLogEntry_ExplosionImpact(Thing initiator, Thing recipient, ThingDef weaponDef, ThingDef projectileDef, DamageDef damageDef) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.damageDef = damageDef;
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x000452D5 File Offset: 0x000434D5
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x000452EB File Offset: 0x000434EB
		public override IEnumerable<Thing> GetConcerns()
		{
			BattleLogEntry_ExplosionImpact.<GetConcerns>d__14 <GetConcerns>d__ = new BattleLogEntry_ExplosionImpact.<GetConcerns>d__14(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000452FC File Offset: 0x000434FC
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.initiatorPawn && this.recipientPawn != null && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn));
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0004534C File Offset: 0x0004354C
		public override void ClickedFromPOV(Thing pov)
		{
			if (this.recipientPawn == null)
			{
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0004539D File Offset: 0x0004359D
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return null;
			}
			if (pov == null || pov == this.recipientPawn)
			{
				return LogEntry.Blood;
			}
			if (pov == this.initiatorPawn)
			{
				return LogEntry.BloodTarget;
			}
			return null;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x000453D0 File Offset: 0x000435D0
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000453EC File Offset: 0x000435EC
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(RulePackDefOf.Combat_ExplosionImpact);
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, false, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			else
			{
				result.Constants["INITIATOR_missing"] = "True";
			}
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, false, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			else
			{
				result.Constants["RECIPIENT_missing"] = "True";
			}
			result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			if (this.projectileDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("PROJECTILE", this.projectileDef));
			}
			if (this.damageDef != null && this.damageDef.combatLogRules != null)
			{
				result.Includes.Add(this.damageDef.combatLogRules);
			}
			return result;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0004555C File Offset: 0x0004375C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Defs.Look<DamageDef>(ref this.damageDef, "damageDef");
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x000455E1 File Offset: 0x000437E1
		public override string ToString()
		{
			return "BattleLogEntry_ExplosionImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x04000D90 RID: 3472
		private Pawn initiatorPawn;

		// Token: 0x04000D91 RID: 3473
		private ThingDef initiatorThing;

		// Token: 0x04000D92 RID: 3474
		private Pawn recipientPawn;

		// Token: 0x04000D93 RID: 3475
		private ThingDef recipientThing;

		// Token: 0x04000D94 RID: 3476
		private ThingDef weaponDef;

		// Token: 0x04000D95 RID: 3477
		private ThingDef projectileDef;

		// Token: 0x04000D96 RID: 3478
		private DamageDef damageDef;
	}
}
