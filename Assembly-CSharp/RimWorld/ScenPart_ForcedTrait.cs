using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200178C RID: 6028
	public class ScenPart_ForcedTrait : ScenPart_PawnModifier
	{
		// Token: 0x06009633 RID: 38451 RVA: 0x00368E59 File Offset: 0x00367059
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraitDef>(ref this.trait, "trait");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
		}

		// Token: 0x06009634 RID: 38452 RVA: 0x00368E84 File Offset: 0x00367084
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			if (Widgets.ButtonText(scenPartRect.TopPart(0.333f), this.trait.DataAtDegree(this.degree).LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (TraitDef traitDef in from td in DefDatabase<TraitDef>.AllDefs
				orderby td.label
				select td)
				{
					foreach (TraitDegreeData localDeg2 in traitDef.degreeDatas)
					{
						TraitDef localDef = traitDef;
						TraitDegreeData localDeg = localDeg2;
						list.Add(new FloatMenuOption(localDeg.LabelCap, delegate()
						{
							this.trait = localDef;
							this.degree = localDeg.degree;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			base.DoPawnModifierEditInterface(scenPartRect.BottomPart(0.666f));
		}

		// Token: 0x06009635 RID: 38453 RVA: 0x00368FF4 File Offset: 0x003671F4
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveTrait".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.trait.DataAtDegree(this.degree).LabelCap).CapitalizeFirst();
		}

		// Token: 0x06009636 RID: 38454 RVA: 0x00369053 File Offset: 0x00367253
		public override void Randomize()
		{
			base.Randomize();
			this.trait = DefDatabase<TraitDef>.GetRandom();
			this.degree = this.trait.degreeDatas.RandomElement<TraitDegreeData>().degree;
		}

		// Token: 0x06009637 RID: 38455 RVA: 0x00369084 File Offset: 0x00367284
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_ForcedTrait scenPart_ForcedTrait = other as ScenPart_ForcedTrait;
			return scenPart_ForcedTrait == null || this.trait != scenPart_ForcedTrait.trait || !this.context.OverlapsWith(scenPart_ForcedTrait.context);
		}

		// Token: 0x06009638 RID: 38456 RVA: 0x003690C0 File Offset: 0x003672C0
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.story == null || pawn.story.traits == null || this.trait == null)
			{
				return;
			}
			if (pawn.story.traits.HasTrait(this.trait) && pawn.story.traits.DegreeOfTrait(this.trait) == this.degree)
			{
				return;
			}
			if (pawn.story.traits.HasTrait(this.trait))
			{
				pawn.story.traits.allTraits.RemoveAll((Trait tr) => tr.def == this.trait);
			}
			else
			{
				IEnumerable<Trait> source = from tr in pawn.story.traits.allTraits
				where !tr.ScenForced && !ScenPart_ForcedTrait.PawnHasTraitForcedByBackstory(pawn, tr.def)
				select tr;
				if (source.Any<Trait>())
				{
					Trait trait = (from tr in source
					where this.trait.ConflictsWith(tr.def)
					select tr).FirstOrDefault<Trait>();
					if (trait != null)
					{
						pawn.story.traits.allTraits.Remove(trait);
					}
					else
					{
						pawn.story.traits.allTraits.Remove(source.RandomElement<Trait>());
					}
				}
			}
			Trait trait2 = new Trait(this.trait, this.degree, true);
			pawn.story.traits.GainTrait(trait2, false);
			TraitUtility.ApplySkillGainFromTrait(pawn, trait2);
		}

		// Token: 0x06009639 RID: 38457 RVA: 0x00369254 File Offset: 0x00367454
		private static bool PawnHasTraitForcedByBackstory(Pawn pawn, TraitDef trait)
		{
			return (pawn.story.Childhood != null && pawn.story.Childhood.forcedTraits != null && pawn.story.Childhood.forcedTraits.Any((BackstoryTrait te) => te.def == trait)) || (pawn.story.Adulthood != null && pawn.story.Adulthood.forcedTraits != null && pawn.story.Adulthood.forcedTraits.Any((BackstoryTrait te) => te.def == trait));
		}

		// Token: 0x0600963A RID: 38458 RVA: 0x003692F7 File Offset: 0x003674F7
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.trait == null;
		}

		// Token: 0x0600963B RID: 38459 RVA: 0x0036930C File Offset: 0x0036750C
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.trait != null) ? this.trait.GetHashCode() : 0) ^ this.degree;
		}

		// Token: 0x04005A3C RID: 23100
		private TraitDef trait;

		// Token: 0x04005A3D RID: 23101
		private int degree;
	}
}
