using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E6 RID: 486
	public class BattleLogEntry_RangedFire : LogEntry
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x00045C3C File Offset: 0x00043E3C
		private string InitiatorName
		{
			get
			{
				if (this.initiatorPawn == null)
				{
					return "null";
				}
				return this.initiatorPawn.LabelShort;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00045C57 File Offset: 0x00043E57
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

		// Token: 0x06000D95 RID: 3477 RVA: 0x00045C72 File Offset: 0x00043E72
		public BattleLogEntry_RangedFire() : base(null)
		{
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00045C7C File Offset: 0x00043E7C
		public BattleLogEntry_RangedFire(Thing initiator, Thing target, ThingDef weaponDef, ThingDef projectileDef, bool burst) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (target is Pawn)
			{
				this.recipientPawn = (target as Pawn);
			}
			else if (target != null)
			{
				this.recipientThing = target.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.burst = burst;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00045CF1 File Offset: 0x00043EF1
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00045D07 File Offset: 0x00043F07
		public override IEnumerable<Thing> GetConcerns()
		{
			BattleLogEntry_RangedFire.<GetConcerns>d__15 <GetConcerns>d__ = new BattleLogEntry_RangedFire.<GetConcerns>d__15(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00045D18 File Offset: 0x00043F18
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipientPawn != null && ((pov == this.initiatorPawn && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn)));
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00045D68 File Offset: 0x00043F68
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

		// Token: 0x06000D9B RID: 3483 RVA: 0x00045DBC File Offset: 0x00043FBC
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.initiatorPawn == null && this.initiatorThing == null)
			{
				Log.ErrorOnce("BattleLogEntry_RangedFire has a null initiator.", 60465709);
			}
			if (this.weaponDef != null && this.weaponDef.Verbs[0].rangedFireRulepack != null)
			{
				result.Includes.Add(this.weaponDef.Verbs[0].rangedFireRulepack);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedFire);
			}
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
			if (this.initiatorPawn != null && this.initiatorPawn.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			result.Constants["BURST"] = this.burst.ToString();
			return result;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00045FE3 File Offset: 0x000441E3
		public override bool ShowInCompactView()
		{
			return Rand.ChanceSeeded(BattleLogEntry_RangedFire.DisplayChance, this.logID);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00045FF8 File Offset: 0x000441F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Values.Look<bool>(ref this.burst, "burst", false, false);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0004607F File Offset: 0x0004427F
		public override string ToString()
		{
			return "BattleLogEntry_RangedFire: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x04000DA2 RID: 3490
		private Pawn initiatorPawn;

		// Token: 0x04000DA3 RID: 3491
		private ThingDef initiatorThing;

		// Token: 0x04000DA4 RID: 3492
		private Pawn recipientPawn;

		// Token: 0x04000DA5 RID: 3493
		private ThingDef recipientThing;

		// Token: 0x04000DA6 RID: 3494
		private ThingDef weaponDef;

		// Token: 0x04000DA7 RID: 3495
		private ThingDef projectileDef;

		// Token: 0x04000DA8 RID: 3496
		private bool burst;

		// Token: 0x04000DA9 RID: 3497
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChance = 0.25f;
	}
}
