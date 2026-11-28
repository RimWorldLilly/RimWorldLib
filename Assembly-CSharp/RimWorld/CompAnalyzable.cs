using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017EF RID: 6127
	public abstract class CompAnalyzable : CompInteractable
	{
		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x060098E7 RID: 39143 RVA: 0x0037605C File Offset: 0x0037425C
		public new CompProperties_Analyzable Props
		{
			get
			{
				return (CompProperties_Analyzable)this.props;
			}
		}

		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x060098E8 RID: 39144 RVA: 0x00376069 File Offset: 0x00374269
		public Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x060098E9 RID: 39145
		public abstract int AnalysisID { get; }

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x060098EA RID: 39146 RVA: 0x00376076 File Offset: 0x00374276
		public override string ExposeKey
		{
			get
			{
				return "Analyzable";
			}
		}

		// Token: 0x060098EB RID: 39147 RVA: 0x0037607D File Offset: 0x0037427D
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !Find.AnalysisManager.HasAnalysisWithID(this.AnalysisID))
			{
				Find.AnalysisManager.AddAnalysisTask(this.AnalysisID, this.Props.analysisRequiredRange.RandomInRange);
			}
		}

		// Token: 0x060098EC RID: 39148 RVA: 0x003760BC File Offset: 0x003742BC
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Building_ResearchBench building_ResearchBench = null;
			if (base.ValidateTarget(target, false) && (this.Props.canStudyInPlace || StudyUtility.TryFindResearchBench(target.Pawn, out building_ResearchBench)))
			{
				ThingWithComps parent = this.parent;
				CompForbiddable compForbiddable = (parent != null) ? parent.TryGetComp<CompForbiddable>() : null;
				if (compForbiddable != null)
				{
					compForbiddable.Forbidden = false;
				}
				target.Pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.AnalyzeItem, this.parent, building_ResearchBench, (building_ResearchBench != null) ? building_ResearchBench.Position : IntVec3.Invalid), new JobTag?(JobTag.Misc), false);
			}
		}

		// Token: 0x060098ED RID: 39149 RVA: 0x0037615C File Offset: 0x0037435C
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			AnalysisDetails analysisDetails;
			Find.AnalysisManager.TryGetAnalysisProgress(this.AnalysisID, out analysisDetails);
			if (!this.Props.allowRepeatAnalysis && analysisDetails != null && analysisDetails.Satisfied)
			{
				return "AlreadyAnalyzed".Translate();
			}
			if (activateBy != null && StatDefOf.ResearchSpeed.Worker.IsDisabledFor(activateBy))
			{
				return "Incapable".Translate();
			}
			Building_ResearchBench building_ResearchBench;
			if (activateBy != null && !this.Props.canStudyInPlace && !StudyUtility.TryFindResearchBench(activateBy, out building_ResearchBench))
			{
				return "NoResearchBench".Translate();
			}
			if (activateBy == null && !this.Props.canStudyInPlace && !this.parent.MapHeld.listerBuildings.ColonistsHaveResearchBench())
			{
				return "NoResearchBench".Translate();
			}
			return true;
		}

		// Token: 0x060098EE RID: 39150 RVA: 0x00376244 File Offset: 0x00374444
		public virtual void OnAnalyzed(Pawn pawn)
		{
			AnalysisDetails details;
			if (!Find.AnalysisManager.TryIncrementAnalysisProgress(this.AnalysisID, out details) && Find.AnalysisManager.IsAnalysisComplete(this.AnalysisID))
			{
				this.SendLetter(this.Props.repeatCompletedLetterLabel, this.Props.repeatCompletedLetter, this.Props.repeatCompletedLetterDef, pawn);
			}
			else
			{
				this.SendAppropriateProgressLetter(pawn, details);
			}
			if (this.Props.destroyedOnAnalyzed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060098EF RID: 39151 RVA: 0x003762C4 File Offset: 0x003744C4
		private void SendAppropriateProgressLetter(Pawn pawn, AnalysisDetails details)
		{
			if (details.Satisfied)
			{
				this.SendLetter(this.Props.completedLetterLabel, this.Props.completedLetter, this.Props.completedLetterDef, pawn);
				return;
			}
			string str = "";
			if (this.Props.showProgress)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(details.timesDone);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(details.required);
				str = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			string letter = (details.timesDone <= this.Props.progressedLetters.Count) ? this.Props.progressedLetters[details.timesDone - 1] : this.Props.progressedLetters.Last<string>();
			this.SendLetter(this.Props.progressedLetterLabel + str, letter, this.Props.progressedLetterDef, pawn);
		}

		// Token: 0x060098F0 RID: 39152 RVA: 0x003763C0 File Offset: 0x003745C0
		private void SendLetter(string label, string letter, LetterDef def, Pawn pawn)
		{
			if (string.IsNullOrEmpty(label) || string.IsNullOrEmpty(letter))
			{
				return;
			}
			string formattedLetterString = this.GetFormattedLetterString(label, pawn);
			string formattedLetterString2 = this.GetFormattedLetterString(letter, pawn);
			Find.LetterStack.ReceiveLetter(formattedLetterString, formattedLetterString2, def, null, 0, true);
		}

		// Token: 0x060098F1 RID: 39153 RVA: 0x0037640C File Offset: 0x0037460C
		private string GetFormattedLetterString(string text, Pawn pawn)
		{
			if (this.ExtraNamedArg != null)
			{
				return text.Formatted(pawn.Named("PAWN"), this.ExtraNamedArg.Value).Resolve();
			}
			return text.Formatted(pawn.Named("PAWN")).Resolve();
		}

		// Token: 0x060098F2 RID: 39154 RVA: 0x0037646A File Offset: 0x0037466A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompAnalyzable.<CompGetGizmosExtra>d__15 <CompGetGizmosExtra>d__ = new CompAnalyzable.<CompGetGizmosExtra>d__15(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}
	}
}
