using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000812 RID: 2066
	public class Verb_CastTargetEffectLances : Verb_CastTargetEffect
	{
		// Token: 0x06004109 RID: 16649 RVA: 0x0019D2A8 File Offset: 0x0019B4A8
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.verbProps.targetParams.CanTarget(target.ToTargetInfo(this.caster.Map), null))
			{
				Pawn pawn = target.Pawn;
				if (pawn == null)
				{
					base.OnGUI(target);
					return;
				}
				bool flag = target.Pawn.kindDef.isBoss;
				using (IEnumerator<CompTargetEffect> enumerator = base.EquipmentSource.GetComps<CompTargetEffect>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanApplyOn(pawn))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
					if (!string.IsNullOrEmpty(this.verbProps.invalidTargetPawn))
					{
						Widgets.MouseAttachedLabel(this.verbProps.invalidTargetPawn.CapitalizeFirst(), 0f, -20f, null);
						return;
					}
				}
				else if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
				{
					GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
					Widgets.MouseAttachedLabel("CannotShootPawnIsPsychicallyDeaf".Translate(pawn), 0f, -20f, null);
					return;
				}
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x0019D3F8 File Offset: 0x0019B5F8
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (target.Pawn.kindDef.isBoss)
				{
					return false;
				}
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
				{
					return false;
				}
				using (IEnumerator<CompTargetEffect> enumerator = base.EquipmentSource.GetComps<CompTargetEffect>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanApplyOn(target.Pawn))
						{
							return false;
						}
					}
				}
			}
			return base.ValidateTarget(target, showMessages);
		}
	}
}
