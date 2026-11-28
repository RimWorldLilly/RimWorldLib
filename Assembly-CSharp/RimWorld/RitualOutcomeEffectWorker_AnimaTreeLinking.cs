using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E6 RID: 5862
	public class RitualOutcomeEffectWorker_AnimaTreeLinking : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x060092AA RID: 37546 RVA: 0x00359850 File Offset: 0x00357A50
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060092AB RID: 37547 RVA: 0x00359853 File Offset: 0x00357A53
		public RitualOutcomeEffectWorker_AnimaTreeLinking()
		{
		}

		// Token: 0x060092AC RID: 37548 RVA: 0x0035985B File Offset: 0x00357A5B
		public RitualOutcomeEffectWorker_AnimaTreeLinking(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092AD RID: 37549 RVA: 0x00359864 File Offset: 0x00357A64
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			Pawn pawn = jobRitual.PawnWithRole("organizer");
			Thing thing = jobRitual.selectedTarget.Thing;
			object obj = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			int num = (int)RitualOutcomeEffectWorker_AnimaTreeLinking.RestoredGrassFromQuality.Evaluate(quality);
			object obj2 = obj;
			if (obj2 != null)
			{
				obj2.FinishLinkingRitual(pawn, num);
			}
			string text = "LetterTextLinkingRitualCompleted".Translate(pawn.Named("PAWN"), jobRitual.selectedTarget.Thing.Named("LINKABLE"));
			if (num > 0)
			{
				text += " " + "LetterTextLinkingRitualCompletedAnimaGrass".Translate(num);
			}
			text = text + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			Find.LetterStack.ReceiveLetter("LetterLabelLinkingRitualCompleted".Translate(), text, LetterDefOf.RitualOutcomePositive, new LookTargets(new TargetInfo[]
			{
				pawn,
				jobRitual.selectedTarget.Thing
			}), null, null, null, null, 0, true);
		}

		// Token: 0x040058C8 RID: 22728
		public static readonly SimpleCurve RestoredGrassFromQuality = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0f),
				true
			},
			{
				new CurvePoint(0.4f, 1f),
				true
			},
			{
				new CurvePoint(0.6f, 3f),
				true
			},
			{
				new CurvePoint(0.8f, 5f),
				true
			},
			{
				new CurvePoint(1f, 8f),
				true
			}
		};
	}
}
