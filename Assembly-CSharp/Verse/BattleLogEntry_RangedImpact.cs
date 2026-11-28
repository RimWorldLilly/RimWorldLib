using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E7 RID: 487
	public class BattleLogEntry_RangedImpact : LogEntry_DamageResult
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x000460A8 File Offset: 0x000442A8
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

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x000460D7 File Offset: 0x000442D7
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

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00046106 File Offset: 0x00044306
		public BattleLogEntry_RangedImpact() : base(null)
		{
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00046110 File Offset: 0x00044310
		public BattleLogEntry_RangedImpact(Thing initiator, Thing recipient, Thing originalTarget, ThingDef weaponDef, ThingDef projectileDef, ThingDef coverDef) : base(null)
		{
			Pawn pawn = initiator as Pawn;
			if (pawn != null)
			{
				this.initiatorPawn = pawn;
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			Pawn pawn2 = recipient as Pawn;
			if (pawn2 != null)
			{
				this.recipientPawn = pawn2;
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			Pawn pawn3 = originalTarget as Pawn;
			if (pawn3 != null)
			{
				this.originalTargetPawn = pawn3;
				this.originalTargetMobile = (!this.originalTargetPawn.Downed && !this.originalTargetPawn.Dead && this.originalTargetPawn.Awake());
			}
			else if (originalTarget != null)
			{
				this.originalTargetThing = originalTarget.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.coverDef = coverDef;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000461D0 File Offset: 0x000443D0
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn || t == this.originalTargetPawn;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000461EF File Offset: 0x000443EF
		public override IEnumerable<Thing> GetConcerns()
		{
			BattleLogEntry_RangedImpact.<GetConcerns>d__18 <GetConcerns>d__ = new BattleLogEntry_RangedImpact.<GetConcerns>d__18(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00046200 File Offset: 0x00044400
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipientPawn != null && ((pov == this.initiatorPawn && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn)));
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00046250 File Offset: 0x00044450
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

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000462A1 File Offset: 0x000444A1
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return null;
			}
			if (this.deflected)
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

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000462DE File Offset: 0x000444DE
		protected override BodyDef DamagedBody()
		{
			Pawn pawn = this.recipientPawn;
			if (pawn == null)
			{
				return null;
			}
			return pawn.RaceProps.body;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000462F8 File Offset: 0x000444F8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn != null || this.recipientThing != null)
			{
				result.Includes.Add(this.deflected ? RulePackDefOf.Combat_RangedDeflect : RulePackDefOf.Combat_RangedDamage);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedMiss);
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
			if (this.originalTargetPawn != this.recipientPawn || this.originalTargetThing != this.recipientThing)
			{
				if (this.originalTargetPawn != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForPawn("ORIGINALTARGET", this.originalTargetPawn, result.Constants, false, true));
					result.Constants["ORIGINALTARGET_mobile"] = this.originalTargetMobile.ToString();
				}
				else if (this.originalTargetThing != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef("ORIGINALTARGET", this.originalTargetThing));
				}
				else
				{
					result.Constants["ORIGINALTARGET_missing"] = "True";
				}
			}
			if (this.weaponDef != null)
			{
				result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			}
			else
			{
				result.Constants["WEAPON_missing"] = "True";
				if (this.projectileDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef("PROJECTILE", this.projectileDef));
				}
			}
			if (this.initiatorPawn != null && this.initiatorPawn.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			result.Constants["COVER_missing"] = ((this.coverDef != null) ? "False" : "True");
			if (this.coverDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("COVER", this.coverDef));
			}
			return result;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00046608 File Offset: 0x00044808
		public override bool ShowInCompactView()
		{
			if (!this.deflected)
			{
				if (this.recipientPawn != null)
				{
					return true;
				}
				if (this.originalTargetThing != null && this.originalTargetThing == this.recipientThing)
				{
					return true;
				}
			}
			int num = 1;
			if (this.weaponDef != null && !this.weaponDef.Verbs.NullOrEmpty<VerbProperties>())
			{
				num = this.weaponDef.Verbs[0].burstShotCount;
			}
			return Rand.ChanceSeeded(BattleLogEntry_RangedImpact.DisplayChanceOnMiss / (float)num, this.logID);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00046688 File Offset: 0x00044888
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.initiatorPawn != null && this.initiatorPawn.Discarded)
				{
					this.initiatorPawn = null;
				}
				if (this.recipientPawn != null && this.recipientPawn.Discarded)
				{
					this.recipientPawn = null;
				}
				if (this.originalTargetPawn != null && this.originalTargetPawn.Discarded)
				{
					this.originalTargetPawn = null;
				}
			}
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_References.Look<Pawn>(ref this.originalTargetPawn, "originalTargetPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.originalTargetThing, "originalTargetThing");
			Scribe_Values.Look<bool>(ref this.originalTargetMobile, "originalTargetMobile", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Defs.Look<ThingDef>(ref this.coverDef, "coverDef");
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0004679C File Offset: 0x0004499C
		public override string ToString()
		{
			return "BattleLogEntry_RangedImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x04000DAA RID: 3498
		private Pawn initiatorPawn;

		// Token: 0x04000DAB RID: 3499
		private ThingDef initiatorThing;

		// Token: 0x04000DAC RID: 3500
		private Pawn recipientPawn;

		// Token: 0x04000DAD RID: 3501
		private ThingDef recipientThing;

		// Token: 0x04000DAE RID: 3502
		private Pawn originalTargetPawn;

		// Token: 0x04000DAF RID: 3503
		private ThingDef originalTargetThing;

		// Token: 0x04000DB0 RID: 3504
		private bool originalTargetMobile;

		// Token: 0x04000DB1 RID: 3505
		private ThingDef weaponDef;

		// Token: 0x04000DB2 RID: 3506
		private ThingDef projectileDef;

		// Token: 0x04000DB3 RID: 3507
		private ThingDef coverDef;

		// Token: 0x04000DB4 RID: 3508
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.25f;
	}
}
