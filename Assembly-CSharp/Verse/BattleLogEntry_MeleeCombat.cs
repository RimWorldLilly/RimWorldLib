using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E5 RID: 485
	public class BattleLogEntry_MeleeCombat : LogEntry_DamageResult
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00045665 File Offset: 0x00043865
		private string InitiatorName
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

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00045680 File Offset: 0x00043880
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

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x0004569B File Offset: 0x0004389B
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x000456A3 File Offset: 0x000438A3
		public RulePackDef RuleDef
		{
			get
			{
				return this.ruleDef;
			}
			set
			{
				this.ruleDef = value;
				base.ResetCache();
			}
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x000456B2 File Offset: 0x000438B2
		public BattleLogEntry_MeleeCombat() : base(null)
		{
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000456BC File Offset: 0x000438BC
		public BattleLogEntry_MeleeCombat(RulePackDef ruleDef, bool alwaysShowInCompact, Pawn initiator, Thing recipient, ImplementOwnerTypeDef implementType, string toolLabel, ThingDef ownerEquipmentDef = null, HediffDef ownerHediffDef = null, LogEntryDef def = null) : base(def)
		{
			this.ruleDef = ruleDef;
			this.alwaysShowInCompact = alwaysShowInCompact;
			this.initiator = initiator;
			this.implementType = implementType;
			this.ownerEquipmentDef = ownerEquipmentDef;
			this.ownerHediffDef = ownerHediffDef;
			this.toolLabel = toolLabel;
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (ownerEquipmentDef != null && ownerHediffDef != null)
			{
				Log.ErrorOnce(string.Format("Combat log owned by both equipment {0} and hediff {1}, may produce unexpected results", ownerEquipmentDef.label, ownerHediffDef.label), 96474669);
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00045759 File Offset: 0x00043959
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipientPawn;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0004576F File Offset: 0x0004396F
		public override IEnumerable<Thing> GetConcerns()
		{
			BattleLogEntry_MeleeCombat.<GetConcerns>d__20 <GetConcerns>d__ = new BattleLogEntry_MeleeCombat.<GetConcerns>d__20(-2);
			<GetConcerns>d__.<>4__this = this;
			return <GetConcerns>d__;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00045780 File Offset: 0x00043980
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.initiator && this.recipientPawn != null && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiator));
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000457D0 File Offset: 0x000439D0
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator && this.recipientPawn != null)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			if (this.recipientPawn != null)
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0004582C File Offset: 0x00043A2C
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return this.def.iconMissTex;
			}
			if (this.deflected)
			{
				return this.def.iconMissTex;
			}
			if (pov == null || pov == this.recipientPawn)
			{
				return this.def.iconDamagedTex;
			}
			if (pov == this.initiator)
			{
				return this.def.iconDamagedFromInstigatorTex;
			}
			return this.def.iconDamagedTex;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0004589E File Offset: 0x00043A9E
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x000458BC File Offset: 0x00043ABC
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants, false, true));
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, false, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			result.Includes.Add(this.ruleDef);
			if (!this.toolLabel.NullOrEmpty())
			{
				result.Rules.Add(new Rule_String("TOOL_label", this.toolLabel));
				result.Rules.Add(new Rule_String("TOOL_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.toolLabel, false, false)));
				result.Rules.Add(new Rule_String("TOOL_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.toolLabel, false, false)));
				result.Constants["TOOL_gender"] = LanguageDatabase.activeLanguage.ResolveGender(this.toolLabel, null, Gender.Male).ToString();
			}
			if (this.implementType != null && !this.implementType.implementOwnerRuleName.NullOrEmpty())
			{
				if (this.ownerEquipmentDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerEquipmentDef));
				}
				else if (this.ownerHediffDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerHediffDef));
				}
			}
			if (this.initiator != null && this.initiator.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiator.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
			}
			if (this.implementType != null && !this.implementType.implementOwnerTypeValue.NullOrEmpty())
			{
				result.Constants["IMPLEMENTOWNER_type"] = this.implementType.implementOwnerTypeValue;
			}
			return result;
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00045B38 File Offset: 0x00043D38
		public override bool ShowInCompactView()
		{
			return this.alwaysShowInCompact || Rand.ChanceSeeded(BattleLogEntry_MeleeCombat.DisplayChanceOnMiss, this.logID);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00045B54 File Offset: 0x00043D54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
			Scribe_Values.Look<bool>(ref this.alwaysShowInCompact, "alwaysShowInCompact", false, false);
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ImplementOwnerTypeDef>(ref this.implementType, "implementType");
			Scribe_Defs.Look<ThingDef>(ref this.ownerEquipmentDef, "ownerDef");
			Scribe_Values.Look<string>(ref this.toolLabel, "toolLabel", null, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00045BF3 File Offset: 0x00043DF3
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.ruleDef.defName,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x04000D98 RID: 3480
		private RulePackDef ruleDef;

		// Token: 0x04000D99 RID: 3481
		private Pawn initiator;

		// Token: 0x04000D9A RID: 3482
		private Pawn recipientPawn;

		// Token: 0x04000D9B RID: 3483
		private ThingDef recipientThing;

		// Token: 0x04000D9C RID: 3484
		private ImplementOwnerTypeDef implementType;

		// Token: 0x04000D9D RID: 3485
		private ThingDef ownerEquipmentDef;

		// Token: 0x04000D9E RID: 3486
		private HediffDef ownerHediffDef;

		// Token: 0x04000D9F RID: 3487
		private string toolLabel;

		// Token: 0x04000DA0 RID: 3488
		public bool alwaysShowInCompact;

		// Token: 0x04000DA1 RID: 3489
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.5f;
	}
}
