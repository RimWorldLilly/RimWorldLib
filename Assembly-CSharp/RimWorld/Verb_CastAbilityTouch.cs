using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200147A RID: 5242
	public class Verb_CastAbilityTouch : Verb_CastAbility
	{
		// Token: 0x060080A6 RID: 32934 RVA: 0x002DE01E File Offset: 0x002DC21E
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && this.IsApplicableTo(target, false))
			{
				GenDraw.DrawTargetHighlight(target);
				this.ability.DrawEffectPreviews(target);
			}
		}

		// Token: 0x060080A7 RID: 32935 RVA: 0x002DE045 File Offset: 0x002DC245
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.ValidateTarget(target, false))
			{
				GenUI.DrawMouseAttachment(this.UIIcon);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
			base.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x060080A8 RID: 32936 RVA: 0x002DE070 File Offset: 0x002DC270
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!this.IsApplicableTo(target, showMessages))
			{
				return false;
			}
			for (int i = 0; i < this.ability.EffectComps.Count; i++)
			{
				if (!this.ability.EffectComps[i].Valid(target, showMessages))
				{
					return false;
				}
			}
			return true;
		}
	}
}
