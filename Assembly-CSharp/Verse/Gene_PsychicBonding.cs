using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003E2 RID: 994
	public class Gene_PsychicBonding : Gene
	{
		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001D41 RID: 7489 RVA: 0x000B28F8 File Offset: 0x000B0AF8
		public bool CanBondToNewPawn
		{
			get
			{
				if (this.bondedPawn != null)
				{
					return false;
				}
				if (this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicBondTorn, false))
				{
					return false;
				}
				Pawn_NeedsTracker needs = this.pawn.needs;
				bool flag;
				if (needs == null)
				{
					flag = (null != null);
				}
				else
				{
					Need_Mood mood = needs.mood;
					if (mood == null)
					{
						flag = (null != null);
					}
					else
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts == null)
						{
							flag = (null != null);
						}
						else
						{
							MemoryThoughtHandler memories = thoughts.memories;
							flag = (((memories != null) ? memories.GetFirstMemoryOfDef(ThoughtDefOf.PsychicBondTorn) : null) != null);
						}
					}
				}
				return !flag;
			}
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000B2974 File Offset: 0x000B0B74
		public override void PostAdd()
		{
			base.PostAdd();
			Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond != null)
			{
				this.bondedPawn = (Pawn)hediff_PsychicBond.target;
			}
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000B29BC File Offset: 0x000B0BBC
		public override void PostRemove()
		{
			base.PostRemove();
			this.Notify_MyOrPartnersGeneRemoved();
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x000B29CC File Offset: 0x000B0BCC
		public void BondTo(Pawn newBond)
		{
			if (!ModLister.CheckBiotech("Psychic bonding"))
			{
				return;
			}
			if (newBond == null)
			{
				return;
			}
			if (this.bondedPawn == newBond)
			{
				return;
			}
			if (this.bondedPawn != null)
			{
				Log.Error("Tried to bond to more than one pawn.");
				return;
			}
			this.bondedPawn = newBond;
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.RemoveMemoriesOfDefIf(ThoughtDefOf.PsychicBondTorn, (Thought_Memory m) => m.otherPawn == this.bondedPawn);
						}
					}
				}
			}
			Pawn_NeedsTracker needs2 = this.bondedPawn.needs;
			if (needs2 != null)
			{
				Need_Mood mood2 = needs2.mood;
				if (mood2 != null)
				{
					ThoughtHandler thoughts2 = mood2.thoughts;
					if (thoughts2 != null)
					{
						MemoryThoughtHandler memories2 = thoughts2.memories;
						if (memories2 != null)
						{
							memories2.RemoveMemoriesOfDefIf(ThoughtDefOf.PsychicBondTorn, (Thought_Memory m) => m.otherPawn == this.pawn);
						}
					}
				}
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = this.bondedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false);
			if (firstHediffOfDef2 != null)
			{
				this.bondedPawn.health.RemoveHediff(firstHediffOfDef2);
			}
			Hediff_PsychicBond hediff_PsychicBond = (Hediff_PsychicBond)HediffMaker.MakeHediff(HediffDefOf.PsychicBond, this.pawn, null);
			hediff_PsychicBond.target = this.bondedPawn;
			this.pawn.health.AddHediff(hediff_PsychicBond, null, null, null);
			Pawn_GeneTracker genes = this.bondedPawn.genes;
			Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			if (gene_PsychicBonding != null)
			{
				gene_PsychicBonding.BondTo(this.pawn);
				return;
			}
			Hediff_PsychicBond hediff_PsychicBond2 = (Hediff_PsychicBond)HediffMaker.MakeHediff(HediffDefOf.PsychicBond, this.bondedPawn, null);
			hediff_PsychicBond2.target = this.pawn;
			this.bondedPawn.health.AddHediff(hediff_PsychicBond2, null, null, null);
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000B2BAC File Offset: 0x000B0DAC
		public void RemoveBond()
		{
			if (this.bondedPawn == null)
			{
				return;
			}
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.PsychicBondTorn, this.bondedPawn, null);
						}
					}
				}
			}
			Pawn_NeedsTracker needs2 = this.bondedPawn.needs;
			if (needs2 != null)
			{
				Need_Mood mood2 = needs2.mood;
				if (mood2 != null)
				{
					ThoughtHandler thoughts2 = mood2.thoughts;
					if (thoughts2 != null)
					{
						MemoryThoughtHandler memories2 = thoughts2.memories;
						if (memories2 != null)
						{
							memories2.TryGainMemory(ThoughtDefOf.PsychicBondTorn, this.pawn, null);
						}
					}
				}
			}
			Pawn pawn = this.bondedPawn;
			this.bondedPawn = null;
			Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond != null && hediff_PsychicBond.target == pawn)
			{
				this.pawn.health.RemoveHediff(hediff_PsychicBond);
			}
			Hediff_PsychicBond hediff_PsychicBond2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond2 != null)
			{
				pawn.health.RemoveHediff(hediff_PsychicBond2);
			}
			Pawn_GeneTracker genes = pawn.genes;
			if (genes != null)
			{
				Gene_PsychicBonding firstGeneOfType = genes.GetFirstGeneOfType<Gene_PsychicBonding>();
				if (firstGeneOfType != null)
				{
					firstGeneOfType.RemoveBond();
				}
			}
			if (!this.pawn.Dead)
			{
				if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false) == null)
				{
					Hediff_PsychicBondTorn hediff_PsychicBondTorn = (Hediff_PsychicBondTorn)HediffMaker.MakeHediff(HediffDefOf.PsychicBondTorn, pawn, null);
					hediff_PsychicBondTorn.target = this.pawn;
					pawn.health.AddHediff(hediff_PsychicBondTorn, null, null, null);
				}
				MentalBreakDef mentalBreakDef;
				if ((from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
				where d.intensity == MentalBreakIntensity.Extreme && d.Worker.BreakCanOccur(this.pawn)
				select d).TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out mentalBreakDef))
				{
					mentalBreakDef.Worker.TryStart(this.pawn, "MentalStateReason_BondedHumanDeath".Translate(pawn), false);
				}
			}
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000B2D90 File Offset: 0x000B0F90
		public void Notify_MyOrPartnersGeneRemoved()
		{
			if (this.bondedPawn != null)
			{
				Pawn_GeneTracker genes = this.bondedPawn.genes;
				if (((genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null) == null)
				{
					Pawn pawn = this.bondedPawn;
					this.bondedPawn = null;
					Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
					if (hediff_PsychicBond != null && hediff_PsychicBond.target == pawn)
					{
						this.pawn.health.RemoveHediff(hediff_PsychicBond);
					}
					Hediff_PsychicBond hediff_PsychicBond2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
					if (hediff_PsychicBond2 != null)
					{
						pawn.health.RemoveHediff(hediff_PsychicBond2);
					}
					Pawn_GeneTracker genes2 = pawn.genes;
					if (genes2 == null)
					{
						return;
					}
					Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
					if (firstGeneOfType == null)
					{
						return;
					}
					firstGeneOfType.Notify_MyOrPartnersGeneRemoved();
					return;
				}
			}
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000B2E50 File Offset: 0x000B1050
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Gene_PsychicBonding.<GetGizmos>d__8 <GetGizmos>d__ = new Gene_PsychicBonding.<GetGizmos>d__8(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000B2E60 File Offset: 0x000B1060
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.bondedPawn, "bondedPawn", false);
		}

		// Token: 0x04001771 RID: 6001
		private Pawn bondedPawn;
	}
}
