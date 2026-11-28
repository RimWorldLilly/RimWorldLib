using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001B12 RID: 6930
	[StaticConstructorOnStartup]
	public class HumanOvum : ThingWithComps
	{
		// Token: 0x0600B12C RID: 45356 RVA: 0x003E6E42 File Offset: 0x003E5042
		public override IEnumerable<Gizmo> GetGizmos()
		{
			HumanOvum.<GetGizmos>d__3 <GetGizmos>d__ = new HumanOvum.<GetGizmos>d__3(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B12D RID: 45357 RVA: 0x003E6E54 File Offset: 0x003E5054
		public Thing ProduceEmbryo(Pawn father)
		{
			HumanEmbryo humanEmbryo = (HumanEmbryo)ThingMaker.MakeThing(ThingDefOf.HumanEmbryo, null);
			CompHasPawnSources comp = humanEmbryo.GetComp<CompHasPawnSources>();
			List<Pawn> pawnSources = base.GetComp<CompHasPawnSources>().pawnSources;
			if (!pawnSources.NullOrEmpty<Pawn>())
			{
				foreach (Pawn pawn in pawnSources)
				{
					comp.AddSource(pawn);
				}
			}
			comp.AddSource(father);
			if (!humanEmbryo.TryPopulateGenes())
			{
				humanEmbryo.Destroy(DestroyMode.Vanish);
				return null;
			}
			return humanEmbryo;
		}

		// Token: 0x0600B12E RID: 45358 RVA: 0x003E6EEC File Offset: 0x003E50EC
		public override Thing SplitOff(int count)
		{
			return base.SplitOff(count) as HumanOvum;
		}

		// Token: 0x0600B12F RID: 45359 RVA: 0x003E6EFA File Offset: 0x003E50FA
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			HumanOvum.<GetFloatMenuOptions>d__6 <GetFloatMenuOptions>d__ = new HumanOvum.<GetFloatMenuOptions>d__6(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600B130 RID: 45360 RVA: 0x003E6F14 File Offset: 0x003E5114
		private FloatMenuOption CanFertilizeFloatOption(Pawn pawn, bool pawnAsLabel)
		{
			AcceptanceReport acceptanceReport = this.CanFertilizeReport(pawn);
			if (acceptanceReport.Accepted)
			{
				Action <>9__2;
				return new FloatMenuOption(pawnAsLabel ? pawn.LabelShort : "FertilizeThing".Translate(this).ToString(), delegate()
				{
					Pawn pawn2 = this.GetComp<CompHasPawnSources>().pawnSources.FirstOrDefault<Pawn>();
					PawnRelationDef pawnRelationDef;
					float num = PregnancyUtility.InbredChanceFromParents(pawn, pawn2, out pawnRelationDef);
					if (num > 0f)
					{
						WindowStack windowStack = Find.WindowStack;
						TaggedString text = "FertilizeOvumInbredChance".Translate(pawn2.Named("MOTHER"), pawn.Named("FATHER"), pawnRelationDef.label.Named("RELATION"), num.ToStringPercent().Named("CHANCE"));
						Action confirmedAct;
						if ((confirmedAct = <>9__2) == null)
						{
							confirmedAct = (<>9__2 = delegate()
							{
								base.<CanFertilizeFloatOption>g__TakeJob|1();
							});
						}
						windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
						return;
					}
					base.<CanFertilizeFloatOption>g__TakeJob|1();
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				return new FloatMenuOption(pawnAsLabel ? "DisabledOption".Translate(pawn.LabelShort, acceptanceReport.Reason).ToString() : ("FertilizeCannotFloatOption".Translate().ToString() + ": " + acceptanceReport.Reason), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x0600B131 RID: 45361 RVA: 0x003E701C File Offset: 0x003E521C
		private AcceptanceReport CanFertilizeReport(Pawn pawn)
		{
			if (pawn.gender != Gender.Male)
			{
				return false;
			}
			if (pawn.IsQuestLodger())
			{
				return false;
			}
			if (!pawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if ((float)pawn.ageTracker.AgeBiologicalYears < 14f)
			{
				return "CannotMustBeAge".Translate(14f).CapitalizeFirst();
			}
			if (pawn.Sterile())
			{
				return "CannotSterile".Translate();
			}
			if (pawn.Downed || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return "Incapacitated".Translate().ToLower();
			}
			return true;
		}

		// Token: 0x0600B132 RID: 45362 RVA: 0x003E70F5 File Offset: 0x003E52F5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.fertilizingMan, "fertilizingMan", false);
		}

		// Token: 0x0400665F RID: 26207
		public static readonly CachedTexture FertilizeIcon = new CachedTexture("UI/Gizmos/Fertilize");

		// Token: 0x04006660 RID: 26208
		private Pawn fertilizingMan;

		// Token: 0x04006661 RID: 26209
		private static readonly Texture2D CancelFertilizeIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
