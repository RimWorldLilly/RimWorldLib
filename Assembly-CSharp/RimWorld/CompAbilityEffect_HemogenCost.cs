using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001418 RID: 5144
	public class CompAbilityEffect_HemogenCost : CompAbilityEffect
	{
		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x06007F33 RID: 32563 RVA: 0x002D7363 File Offset: 0x002D5563
		public new CompProperties_AbilityHemogenCost Props
		{
			get
			{
				return (CompProperties_AbilityHemogenCost)this.props;
			}
		}

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x06007F34 RID: 32564 RVA: 0x002D7370 File Offset: 0x002D5570
		private bool HasEnoughHemogen
		{
			get
			{
				Pawn_GeneTracker genes = this.parent.pawn.genes;
				Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
				return gene_Hemogen != null && gene_Hemogen.Value >= this.Props.hemogenCost;
			}
		}

		// Token: 0x06007F35 RID: 32565 RVA: 0x002D73B3 File Offset: 0x002D55B3
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GeneUtility.OffsetHemogen(this.parent.pawn, -this.Props.hemogenCost, true);
		}

		// Token: 0x06007F36 RID: 32566 RVA: 0x002D73DC File Offset: 0x002D55DC
		public override bool GizmoDisabled(out string reason)
		{
			Pawn_GeneTracker genes = this.parent.pawn.genes;
			Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
			if (gene_Hemogen == null)
			{
				reason = "AbilityDisabledNoHemogenGene".Translate(this.parent.pawn);
				return true;
			}
			if (gene_Hemogen.Value < this.Props.hemogenCost)
			{
				reason = "AbilityDisabledNoHemogen".Translate(this.parent.pawn);
				return true;
			}
			float num = this.TotalHemogenCostOfQueuedAbilities();
			float num2 = this.Props.hemogenCost + num;
			if (this.Props.hemogenCost > 1E-45f && num2 > gene_Hemogen.Value)
			{
				reason = "AbilityDisabledNoHemogen".Translate(this.parent.pawn);
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x06007F37 RID: 32567 RVA: 0x002D74B9 File Offset: 0x002D56B9
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return this.HasEnoughHemogen;
		}

		// Token: 0x06007F38 RID: 32568 RVA: 0x002D74C4 File Offset: 0x002D56C4
		private float TotalHemogenCostOfQueuedAbilities()
		{
			Pawn_JobTracker jobs = this.parent.pawn.jobs;
			object obj;
			if (jobs == null)
			{
				obj = null;
			}
			else
			{
				Job curJob = jobs.curJob;
				obj = ((curJob != null) ? curJob.verbToUse : null);
			}
			Verb_CastAbility verb_CastAbility = obj as Verb_CastAbility;
			float num;
			if (verb_CastAbility == null)
			{
				num = 0f;
			}
			else
			{
				Ability ability = verb_CastAbility.ability;
				num = ((ability != null) ? ability.HemogenCost() : 0f);
			}
			float num2 = num;
			if (this.parent.pawn.jobs != null)
			{
				for (int i = 0; i < this.parent.pawn.jobs.jobQueue.Count; i++)
				{
					Verb_CastAbility verb_CastAbility2 = this.parent.pawn.jobs.jobQueue[i].job.verbToUse as Verb_CastAbility;
					if (verb_CastAbility2 != null)
					{
						float num3 = num2;
						Ability ability2 = verb_CastAbility2.ability;
						num2 = num3 + ((ability2 != null) ? ability2.HemogenCost() : 0f);
					}
				}
			}
			return num2;
		}
	}
}
