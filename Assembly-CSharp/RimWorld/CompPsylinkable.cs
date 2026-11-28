using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001948 RID: 6472
	public class CompPsylinkable : ThingComp
	{
		// Token: 0x17001C14 RID: 7188
		// (get) Token: 0x0600A258 RID: 41560 RVA: 0x003A056B File Offset: 0x0039E76B
		public CompProperties_Psylinkable Props
		{
			get
			{
				return (CompProperties_Psylinkable)this.props;
			}
		}

		// Token: 0x17001C15 RID: 7189
		// (get) Token: 0x0600A259 RID: 41561 RVA: 0x003A0578 File Offset: 0x0039E778
		public CompSpawnSubplant CompSubplant
		{
			get
			{
				return this.parent.TryGetComp<CompSpawnSubplant>();
			}
		}

		// Token: 0x0600A25A RID: 41562 RVA: 0x003A0588 File Offset: 0x0039E788
		private IEnumerable<Pawn> GetPawnsThatCanPsylink(int level = -1)
		{
			return from p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists
			where this.Props.requiredFocus.CanPawnUse(p) && this.GetRequiredPlantCount(p) <= this.CompSubplant.SubplantsForReading.Count && (level == -1 || p.GetPsylinkLevel() == level)
			select p;
		}

		// Token: 0x0600A25B RID: 41563 RVA: 0x003A05BF File Offset: 0x0039E7BF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.CompSubplant.onGrassGrown = new Action(this.OnGrassGrown);
		}

		// Token: 0x0600A25C RID: 41564 RVA: 0x003A05D8 File Offset: 0x0039E7D8
		private void OnGrassGrown()
		{
			bool flag = false;
			foreach (Pawn item in this.GetPawnsThatCanPsylink(-1))
			{
				if (!this.pawnsThatCanPsylinkLastGrassGrow.Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
				string text = "";
				for (int i = 0; i < this.Props.requiredSubplantCountPerPsylinkLevel.Count; i++)
				{
					IEnumerable<string> enumerable = from p in this.GetPawnsThatCanPsylink(i)
					select p.LabelShort;
					if (enumerable.Count<string>() > 0)
					{
						text = string.Concat(new string[]
						{
							text,
							"- " + "Level".Translate().CapitalizeFirst() + " ",
							(i + 1).ToString(),
							": ",
							this.Props.requiredSubplantCountPerPsylinkLevel[i].ToString(),
							" ",
							compSpawnSubplant.Props.subplant.label,
							" (",
							enumerable.ToCommaList(false, false),
							")\n"
						});
					}
				}
				Find.LetterStack.ReceiveLetter(this.Props.enoughPlantsLetterLabel, this.Props.enoughPlantsLetterText.Formatted(compSpawnSubplant.SubplantsForReading.Count, text.TrimEndNewlines()), LetterDefOf.NeutralEvent, new LookTargets(this.GetPawnsThatCanPsylink(-1)), null, null, null, null, 0, true);
			}
			this.pawnsThatCanPsylinkLastGrassGrow.Clear();
			this.pawnsThatCanPsylinkLastGrassGrow.AddRange(this.GetPawnsThatCanPsylink(-1));
		}

		// Token: 0x0600A25D RID: 41565 RVA: 0x003A07D8 File Offset: 0x0039E9D8
		public int GetRequiredPlantCount(Pawn pawn)
		{
			int psylinkLevel = pawn.GetPsylinkLevel();
			if (this.parent.TryGetComp<CompSpawnSubplant>() == null)
			{
				Log.Warning("CompPsylinkable with requiredSubplantCountPerPsylinkLevel set on a Thing without CompSpawnSubplant!");
				return -1;
			}
			int result;
			if (this.Props.requiredSubplantCountPerPsylinkLevel.Count <= psylinkLevel)
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel.Last<int>();
			}
			else
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel[psylinkLevel];
			}
			return result;
		}

		// Token: 0x0600A25E RID: 41566 RVA: 0x003A0840 File Offset: 0x0039EA40
		public AcceptanceReport CanPsylink(Pawn pawn, LocalTargetInfo? knownSpot = null, bool checkSpot = true)
		{
			if (pawn.Dead || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int requiredPlantCount = this.GetRequiredPlantCount(pawn);
			if (requiredPlantCount == -1)
			{
				return false;
			}
			if (!this.Props.requiredFocus.CanPawnUse(pawn))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedFocus".Translate(this.Props.requiredFocus.label));
			}
			if (pawn.GetPsylinkLevel() >= pawn.GetMaxPsylinkLevel())
			{
				return new AcceptanceReport("BeginLinkingRitualMaxPsylinkLevel".Translate());
			}
			if (!pawn.Map.reservationManager.CanReserve(pawn, this.parent, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(this.parent, pawn, null);
				return new AcceptanceReport((pawn2 == null) ? "Reserved".Translate() : "ReservedBy".Translate(pawn.LabelShort, pawn2));
			}
			if (compSpawnSubplant.SubplantsForReading.Count < requiredPlantCount)
			{
				return new AcceptanceReport("BeginLinkingRitualNeedSubplants".Translate(requiredPlantCount.ToString(), compSpawnSubplant.Props.subplant.label, compSpawnSubplant.SubplantsForReading.Count.ToString()));
			}
			if (checkSpot)
			{
				LocalTargetInfo localTargetInfo;
				if (knownSpot != null)
				{
					if (!this.CanUseSpot(pawn, knownSpot.Value))
					{
						return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
					}
				}
				else if (!this.TryFindLinkSpot(pawn, out localTargetInfo))
				{
					return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600A25F RID: 41567 RVA: 0x003A0A10 File Offset: 0x0039EC10
		public bool TryFindLinkSpot(Pawn pawn, out LocalTargetInfo spot)
		{
			spot = MeditationUtility.FindMeditationSpot(pawn).spot;
			if (this.CanUseSpot(pawn, spot))
			{
				return true;
			}
			int num = GenRadial.NumCellsInRadius(2.9f);
			int num2 = GenRadial.NumCellsInRadius(3.9f);
			for (int i = num; i < num2; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (this.CanUseSpot(pawn, c))
				{
					spot = c;
					return true;
				}
			}
			spot = IntVec3.Zero;
			return false;
		}

		// Token: 0x0600A260 RID: 41568 RVA: 0x003A0AAC File Offset: 0x0039ECAC
		private bool CanUseSpot(Pawn pawn, LocalTargetInfo spot)
		{
			IntVec3 cell = spot.Cell;
			return cell.DistanceTo(this.parent.Position) <= 3.9f && cell.Standable(this.parent.Map) && GenSight.LineOfSight(cell, this.parent.Position, this.parent.Map) && pawn.CanReach(spot, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x0600A261 RID: 41569 RVA: 0x003A0B21 File Offset: 0x0039ED21
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			CompPsylinkable.<CompFloatMenuOptions>d__13 <CompFloatMenuOptions>d__ = new CompPsylinkable.<CompFloatMenuOptions>d__13(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__pawn = pawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A262 RID: 41570 RVA: 0x003A0B38 File Offset: 0x0039ED38
		public void FinishLinkingRitual(Pawn pawn, int plantsToKeep)
		{
			if (!ModLister.CheckRoyalty("Psylinkable"))
			{
				return;
			}
			FleckMaker.Static(this.parent.Position, pawn.Map, FleckDefOf.PsycastAreaEffect, 10f);
			SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(this.parent));
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int num = this.GetRequiredPlantCount(pawn) - plantsToKeep;
			List<Thing> list = (from p in compSpawnSubplant.SubplantsForReading
			orderby p.Position.DistanceTo(this.parent.Position) descending
			select p).ToList<Thing>();
			int num2 = 0;
			while (num2 < num && num2 < list.Count)
			{
				list[num2].Destroy(DestroyMode.Vanish);
				num2++;
			}
			compSpawnSubplant.Cleanup();
			pawn.ChangePsylinkLevel(1, true);
			Find.History.Notify_PsylinkAvailable();
		}

		// Token: 0x0600A263 RID: 41571 RVA: 0x003A0BFC File Offset: 0x0039EDFC
		public override void PostExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsThatCanPsylinkLastGrassGrow, "pawnsThatCanPsylinkLastGrassGrow", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawnsThatCanPsylinkLastGrassGrow.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04005FED RID: 24557
		private List<Pawn> pawnsThatCanPsylinkLastGrassGrow = new List<Pawn>();

		// Token: 0x04005FEE RID: 24558
		public const float MaxDistance = 3.9f;
	}
}
