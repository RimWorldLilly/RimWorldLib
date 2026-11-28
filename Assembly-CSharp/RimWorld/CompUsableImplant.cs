using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019D2 RID: 6610
	public class CompUsableImplant : CompUsable
	{
		// Token: 0x0600A668 RID: 42600 RVA: 0x003B5728 File Offset: 0x003B3928
		protected override string FloatMenuOptionLabel(Pawn pawn)
		{
			CompUseEffect_InstallImplant compUseEffect_InstallImplant = this.parent.TryGetComp<CompUseEffect_InstallImplant>();
			if (compUseEffect_InstallImplant != null)
			{
				Hediff_Level hediff_Level = compUseEffect_InstallImplant.GetExistingImplant(pawn) as Hediff_Level;
				if (hediff_Level != null && compUseEffect_InstallImplant.Props.canUpgrade && (float)hediff_Level.level < hediff_Level.def.maxSeverity)
				{
					return "UpgradeImplant".Translate(hediff_Level.def.label, hediff_Level.level + 1);
				}
			}
			return base.FloatMenuOptionLabel(pawn);
		}

		// Token: 0x0600A669 RID: 42601 RVA: 0x003B57AC File Offset: 0x003B39AC
		public override void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget, bool forced = false)
		{
			CompUseEffect_InstallImplant useEffectImplant = this.parent.TryGetComp<CompUseEffect_InstallImplant>();
			Hediff_Level hediff_Level = useEffectImplant.GetExistingImplant(pawn) as Hediff_Level;
			TaggedString text = CompRoyalImplant.CheckForViolations(pawn, useEffectImplant.Props.hediffDef, (hediff_Level != null && useEffectImplant.Props.canUpgrade) ? 1 : 0);
			if (!text.NullOrEmpty())
			{
				Find.WindowStack.Add(new Dialog_MessageBox(text, "Yes".Translate(), delegate()
				{
					this.UseJobInternal(pawn, extraTarget, useEffectImplant.Props.hediffDef);
				}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			this.UseJobInternal(pawn, extraTarget, useEffectImplant.Props.hediffDef);
		}

		// Token: 0x0600A66A RID: 42602 RVA: 0x003B58A0 File Offset: 0x003B3AA0
		private void UseJobInternal(Pawn pawn, LocalTargetInfo extraTarget, HediffDef hediff)
		{
			base.TryStartUseJob(pawn, extraTarget, false);
			if (hediff == HediffDefOf.PsychicAmplifier && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) < 1E-45f)
			{
				Messages.Message("MessagePsylinkNoSensitivity".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
