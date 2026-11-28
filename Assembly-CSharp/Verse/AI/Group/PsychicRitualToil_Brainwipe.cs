using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000921 RID: 2337
	public class PsychicRitualToil_Brainwipe : PsychicRitualToil
	{
		// Token: 0x0600477A RID: 18298 RVA: 0x001BFDB1 File Offset: 0x001BDFB1
		protected PsychicRitualToil_Brainwipe()
		{
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x001BFDB9 File Offset: 0x001BDFB9
		public PsychicRitualToil_Brainwipe(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x001BFDD0 File Offset: 0x001BDFD0
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			PsychicRitualDef_Brainwipe psychicRitualDef_Brainwipe = (PsychicRitualDef_Brainwipe)psychicRitual.def;
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
			this.comaDurationTicks = Mathf.RoundToInt(psychicRitualDef_Brainwipe.comaDurationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent) * 60000f);
			if (pawn != null && pawn2 != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, pawn2);
			}
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x001BFE48 File Offset: 0x001BE048
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
		{
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BrainwipeComa, target, null);
			HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null)
			{
				hediffComp_Disappears.SetDuration(this.comaDurationTicks);
			}
			target.health.AddHediff(hediff, null, null, null);
			bool flag = target.Inhumanized();
			if (flag)
			{
				target.Rehumanize();
			}
			bool recruitable = target.guest.Recruitable;
			target.guest.Recruitable = true;
			MemoryThoughtHandler memories = target.needs.mood.thoughts.memories;
			List<Thought_Memory> list = new List<Thought_Memory>();
			List<Thought_Memory> list2 = new List<Thought_Memory>();
			foreach (Thought_Memory thought_Memory in memories.Memories)
			{
				if (thought_Memory.MoodOffset() != 0f && thought_Memory.DurationTicks > 0)
				{
					list.Add(thought_Memory);
				}
				if (thought_Memory is ISocialThought)
				{
					list2.Add(thought_Memory);
				}
			}
			foreach (Thought_Memory th in list)
			{
				memories.RemoveMemory(th);
			}
			foreach (Thought_Memory th2 in list2)
			{
				memories.RemoveMemory(th2);
			}
			target.guest.resistance = 0f;
			target.guest.will = 0f;
			if (ModsConfig.IdeologyActive)
			{
				target.ideo.OffsetCertainty(-target.ideo.Certainty);
			}
			TaggedString taggedString = "BrainwipeCompleteText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL"), target.Named("TARGET"));
			taggedString += "\n\n" + "BrainwipeTargetComa".Translate(target.Named("TARGET"), this.comaDurationTicks.ToStringTicksToDays("F1")).CapitalizeFirst();
			if (!recruitable)
			{
				taggedString += "\n\n" + "BrainwipeUnwaveringlyLoyal".Translate(target.Named("TARGET"));
			}
			if (flag)
			{
				taggedString += "\n\n" + "BrainwipeRehumanized".Translate(target.Named("TARGET"));
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), taggedString, LetterDefOf.NeutralEvent, new LookTargets(new TargetInfo[]
			{
				invoker,
				target
			}), null, null, null, null, 0, true);
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x001C0138 File Offset: 0x001BE338
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_Values.Look<int>(ref this.comaDurationTicks, "comaDurationTicks", 0, false);
		}

		// Token: 0x04002E4A RID: 11850
		public PsychicRitualRoleDef targetRole;

		// Token: 0x04002E4B RID: 11851
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E4C RID: 11852
		public int comaDurationTicks;
	}
}
