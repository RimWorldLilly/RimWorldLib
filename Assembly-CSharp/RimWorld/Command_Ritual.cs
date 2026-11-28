using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001655 RID: 5717
	[StaticConstructorOnStartup]
	public class Command_Ritual : Command
	{
		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x06008FCC RID: 36812 RVA: 0x0034D954 File Offset: 0x0034BB54
		private static Texture2D PenaltyArrowTex
		{
			get
			{
				if (Command_Ritual.penaltyArrowTex == null)
				{
					Command_Ritual.penaltyArrowTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/QualityPenalty", true);
				}
				return Command_Ritual.penaltyArrowTex;
			}
		}

		// Token: 0x06008FCD RID: 36813 RVA: 0x0034D978 File Offset: 0x0034BB78
		public Command_Ritual(Precept_Ritual ritual, TargetInfo targetInfo, RitualObligation forObligation = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			this.ritual = ritual;
			this.targetInfo = targetInfo;
			this.forcedForRole = forcedForRole;
			this.obligation = forObligation;
			this.defaultLabel = ritual.GetBeginRitualText(this.obligation);
			this.defaultDesc = ritual.def.description;
			foreach (RitualObligationTrigger ritualObligationTrigger in ritual.obligationTriggers)
			{
				if (ritualObligationTrigger.TriggerExtraDesc != null)
				{
					this.defaultDesc = this.defaultDesc + "\n\n" + ritualObligationTrigger.TriggerExtraDesc;
				}
			}
			this.groupKey = (ritual.canMergeGizmosFromDifferentIdeos ? -1 : ritual.ideo.id);
			this.icon = ritual.Icon;
			if (!ritual.def.mergeRitualGizmosFromAllIdeos && !ritual.def.iconIgnoresIdeoColor)
			{
				this.defaultIconColor = ritual.ideo.Color;
			}
			if (!this.disabled)
			{
				this.ValidateDisabledState();
			}
		}

		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x06008FCE RID: 36814 RVA: 0x0034DAA4 File Offset: 0x0034BCA4
		public override string Desc
		{
			get
			{
				return this.ritual.TipLabel.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.ritual.TipMainPart();
			}
		}

		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x06008FCF RID: 36815 RVA: 0x0034DAD0 File Offset: 0x0034BCD0
		public override string DescPostfix
		{
			get
			{
				return this.ritual.TipExtraPart();
			}
		}

		// Token: 0x06008FD0 RID: 36816 RVA: 0x0034DAE0 File Offset: 0x0034BCE0
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			base.DrawIcon(rect, buttonMat, parms);
			if (this.ritual.RepeatPenaltyActive)
			{
				float value = Mathf.InverseLerp(1200000f, 0f, (float)this.ritual.TicksSinceLastPerformed);
				Widgets.FillableBar(rect.ContractedBy(1f), Mathf.Clamp01(value), Command_Ritual.CooldownBarTex, null, false);
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.UpperCenter;
				float num = (float)(1200000 - this.ritual.TicksSinceLastPerformed) / 60000f;
				if (num >= 1f)
				{
					num = (float)Mathf.RoundToInt(num);
				}
				else
				{
					num = (float)((int)(num * 10f)) / 10f;
				}
				Widgets.Label(rect, "PeriodDays".Translate(num));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.DrawTexture(new Rect(rect.xMax - (float)this.PenaltyIconSize.x, rect.yMin + 4f, (float)this.PenaltyIconSize.x, (float)this.PenaltyIconSize.z), Command_Ritual.PenaltyArrowTex);
			}
		}

		// Token: 0x06008FD1 RID: 36817 RVA: 0x0034DBEB File Offset: 0x0034BDEB
		protected override GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			if (!this.disabled)
			{
				this.ValidateDisabledState();
			}
			return base.GizmoOnGUIInt(butRect, parms);
		}

		// Token: 0x06008FD2 RID: 36818 RVA: 0x0034DC04 File Offset: 0x0034BE04
		private void ValidateDisabledState()
		{
			string text = this.ritual.behavior.CanStartRitualNow(this.targetInfo, this.ritual, null, null);
			if (!text.NullOrEmpty())
			{
				this.disabled = true;
				this.disabledReason = text;
				return;
			}
			if (this.ritual.abilityOnCooldownUntilTick > Find.TickManager.TicksGame)
			{
				this.disabled = true;
				this.disabledReason = "AbilityOnCooldown".Translate((this.ritual.abilityOnCooldownUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
				return;
			}
			if (this.ritual.def.sourcePawnRoleDef != null && this.ritual.def.sourceAbilityDef != null)
			{
				Precept_Role precept_Role = this.ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == this.ritual.def.sourcePawnRoleDef);
				if (precept_Role != null)
				{
					Precept_RoleSingle precept_RoleSingle = precept_Role as Precept_RoleSingle;
					if (precept_RoleSingle != null && precept_RoleSingle.ChosenPawnSingle() != null)
					{
						Ability ability = precept_RoleSingle.AbilitiesForReading.FirstOrDefault((Ability a) => a.def == this.ritual.def.sourceAbilityDef);
						if (ability != null)
						{
							this.disabled = ability.GizmoDisabled(out this.disabledReason);
						}
					}
				}
			}
		}

		// Token: 0x06008FD3 RID: 36819 RVA: 0x0034DD2E File Offset: 0x0034BF2E
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			RitualBehaviorWorker behavior = this.ritual.behavior;
			if (behavior == null)
			{
				return;
			}
			behavior.DrawPreviewOnTarget(this.targetInfo);
		}

		// Token: 0x06008FD4 RID: 36820 RVA: 0x0034DD51 File Offset: 0x0034BF51
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.ritual.ShowRitualBeginWindow(this.targetInfo, null, null, this.forcedForRole);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
		}

		// Token: 0x040057D0 RID: 22480
		private Precept_Ritual ritual;

		// Token: 0x040057D1 RID: 22481
		private RitualObligation obligation;

		// Token: 0x040057D2 RID: 22482
		private TargetInfo targetInfo;

		// Token: 0x040057D3 RID: 22483
		private Dictionary<string, Pawn> forcedForRole;

		// Token: 0x040057D4 RID: 22484
		private readonly IntVec2 PenaltyIconSize = new IntVec2(16, 16);

		// Token: 0x040057D5 RID: 22485
		private static readonly Texture2D CooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(170, 150, 0, 60));

		// Token: 0x040057D6 RID: 22486
		private static Texture2D penaltyArrowTex;
	}
}
