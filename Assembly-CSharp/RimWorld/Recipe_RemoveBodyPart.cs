using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A9 RID: 5289
	public class Recipe_RemoveBodyPart : Recipe_Surgery
	{
		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x0600818D RID: 33165 RVA: 0x002E3B6E File Offset: 0x002E1D6E
		protected virtual bool SpawnPartsWhenRemoved
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600818E RID: 33166 RVA: 0x002E3B71 File Offset: 0x002E1D71
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_RemoveBodyPart.<GetPartsToApplyOn>d__2 <GetPartsToApplyOn>d__ = new Recipe_RemoveBodyPart.<GetPartsToApplyOn>d__2(-2);
			<GetPartsToApplyOn>d__.<>3__pawn = pawn;
			return <GetPartsToApplyOn>d__;
		}

		// Token: 0x0600818F RID: 33167 RVA: 0x002E3B81 File Offset: 0x002E1D81
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return ((pawn.Faction != billDoerFaction && pawn.Faction != null) || pawn.IsQuestLodger()) && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x06008190 RID: 33168 RVA: 0x002E3BAC File Offset: 0x002E1DAC
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				Hediff directlyAddedPartFor = pawn.health.hediffSet.GetDirectlyAddedPartFor(part);
				if (directlyAddedPartFor != null)
				{
					directlyAddedPartFor.Notify_SurgicallyRemoved(billDoer);
				}
				if (this.SpawnPartsWhenRemoved)
				{
					MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
					MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
				}
			}
			this.DamagePart(pawn, part);
			pawn.Drawer.renderer.SetAllGraphicsDirty();
			if (flag)
			{
				this.ApplyThoughts(pawn, billDoer);
			}
			if (flag2)
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
			}
		}

		// Token: 0x06008191 RID: 33169 RVA: 0x002E3C80 File Offset: 0x002E1E80
		public virtual void DamagePart(Pawn pawn, BodyPartRecord part)
		{
			pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, part, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
		}

		// Token: 0x06008192 RID: 33170 RVA: 0x002E3CB7 File Offset: 0x002E1EB7
		public virtual void ApplyThoughts(Pawn pawn, Pawn billDoer)
		{
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.OrganHarvesting);
				return;
			}
			ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn, billDoer);
		}

		// Token: 0x06008193 RID: 33171 RVA: 0x002E3CD4 File Offset: 0x002E1ED4
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			if (!part.def.removeRecipeLabelOverride.NullOrEmpty())
			{
				return part.def.removeRecipeLabelOverride;
			}
			if (pawn.RaceProps.IsMechanoid || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				Hediff hediff;
				if (pawn.health.hediffSet.TryGetDirectlyAddedPartFor(part, out hediff))
				{
					return "RemovePart".Translate(hediff.Label);
				}
				return RecipeDefOf.RemoveBodyPart.label;
			}
			else
			{
				BodyPartRemovalIntent bodyPartRemovalIntent = HealthUtility.PartRemovalIntent(pawn, part);
				if (bodyPartRemovalIntent == BodyPartRemovalIntent.Harvest)
				{
					return "HarvestOrgan".Translate();
				}
				if (bodyPartRemovalIntent != BodyPartRemovalIntent.Amputate)
				{
					throw new InvalidOperationException();
				}
				if (part.depth == BodyPartDepth.Inside || part.def.socketed)
				{
					return "RemoveOrgan".Translate();
				}
				return "Amputate".Translate();
			}
		}
	}
}
