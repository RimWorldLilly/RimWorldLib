using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CE RID: 5070
	public class Command_AbilitySpeech : Command_Ability
	{
		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x06007E40 RID: 32320 RVA: 0x002D21B4 File Offset: 0x002D03B4
		protected Precept_Ritual Ritual
		{
			get
			{
				if (this.ritualCached == null)
				{
					CompAbilityEffect_StartRitual compAbilityEffect_StartRitual = this.ability.CompOfType<CompAbilityEffect_StartRitual>();
					if (compAbilityEffect_StartRitual != null)
					{
						this.ritualCached = compAbilityEffect_StartRitual.Ritual;
					}
				}
				return this.ritualCached;
			}
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x06007E41 RID: 32321 RVA: 0x002D21EC File Offset: 0x002D03EC
		public override string Tooltip
		{
			get
			{
				if (this.Ritual == null)
				{
					return "";
				}
				string text = this.Ritual.Label.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.Ritual.def.description.Formatted(this.ability.pawn.Named("ORGANIZER")).Resolve() + "\n";
				if (this.ability.CooldownTicksRemaining > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"AbilitySpeechCooldown".Translate().Resolve(),
						": ",
						this.ability.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false)
					});
				}
				if (this.Ritual.outcomeEffect != null)
				{
					text = text + "\n" + this.Ritual.outcomeEffect.ExtraAlertParagraph(this.Ritual);
				}
				text = string.Concat(new string[]
				{
					text,
					"\n\n",
					("AbilitySpeechTargetsLabel".Translate().Resolve() + ":").Colorize(ColoredText.TipSectionTitleColor),
					"\n",
					this.Ritual.targetFilter.GetTargetInfos(this.ability.pawn).ToLineList(" -  ", true)
				});
				return text.CapitalizeFirst();
			}
		}

		// Token: 0x06007E42 RID: 32322 RVA: 0x002D2368 File Offset: 0x002D0568
		public Command_AbilitySpeech(Ability ability, Pawn pawn) : base(ability, pawn)
		{
			Precept_Ritual ritual = this.Ritual;
			this.defaultLabel = ((ritual != null) ? ritual.GetBeginRitualText(null) : "");
			this.icon = ((ability.def.iconPath == null) ? this.Ritual.Icon : ContentFinder<Texture2D>.Get(ability.def.iconPath, true));
		}

		// Token: 0x04004F3D RID: 20285
		private Precept_Ritual ritualCached;
	}
}
