using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A35 RID: 2613
	public class JobDriver_Fish : JobDriver
	{
		// Token: 0x06004EE3 RID: 20195 RVA: 0x001DF1C4 File Offset: 0x001DD3C4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed, false);
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x001DF221 File Offset: 0x001DD421
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Fish.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_Fish.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x001DF231 File Offset: 0x001DD431
		private Toil CompleteFishingToil()
		{
			Toil toil = ToilMaker.MakeToil("CompleteFishingToil");
			toil.initAction = delegate()
			{
				IntVec3 cell = this.job.GetTarget(TargetIndex.A).Cell;
				List<NegativeFishingOutcomeDef> negativeFishingOutcomes = FishingUtility.GetNegativeFishingOutcomes(this.pawn, cell);
				bool flag;
				List<Thing> catchesFor = FishingUtility.GetCatchesFor(this.pawn, cell, false, out flag);
				if (negativeFishingOutcomes.Any<NegativeFishingOutcomeDef>())
				{
					NegativeFishingOutcomeDef negativeFishingOutcomeDef = negativeFishingOutcomes.RandomElement<NegativeFishingOutcomeDef>();
					this.pawn.Map.waterBodyTracker.lastNegativeCatchTick = Find.TickManager.TicksGame;
					if (negativeFishingOutcomeDef.damageDef != null)
					{
						DamageInfo dinfo = new DamageInfo(negativeFishingOutcomeDef.damageDef, (float)negativeFishingOutcomeDef.damageAmountRange.RandomInRange, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
						dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
						this.pawn.TakeDamage(dinfo);
					}
					if (negativeFishingOutcomeDef.addsHediff != null)
					{
						Hediff hediff = this.pawn.health.AddHediff(negativeFishingOutcomeDef.addsHediff, null, null, null);
						if (negativeFishingOutcomeDef.hediffSeverity > 0f)
						{
							hediff.Severity = negativeFishingOutcomeDef.hediffSeverity;
						}
					}
					Find.LetterStack.ReceiveLetter(negativeFishingOutcomeDef.letterLabel, negativeFishingOutcomeDef.letterText.Formatted(this.pawn.Named("PAWN")), negativeFishingOutcomeDef.letterDef, this.pawn, null, null, null, null, 0, true);
				}
				else if (catchesFor.Any<Thing>())
				{
					bool flag2 = false;
					int num = catchesFor.Sum((Thing x) => x.stackCount);
					foreach (Thing thing in catchesFor)
					{
						flag2 |= GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
					}
					if (flag2)
					{
						if (flag)
						{
							this.pawn.Map.waterBodyTracker.lastRareCatchTick = Find.TickManager.TicksGame;
							Find.LetterStack.ReceiveLetter("LetterLabelRareCatch".Translate(), "LetterTextRareCatch".Translate(this.pawn.Named("PAWN")) + ":\n" + (from x in catchesFor
							select x.LabelCap).ToLineList("  - ", false), LetterDefOf.PositiveEvent, catchesFor, null, null, null, null, 0, true);
						}
						else
						{
							this.pawn.Map.waterBodyTracker.Notify_Fished(cell, (float)num);
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SlaughteredFish, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
						}
					}
				}
				Zone_Fishing zone_Fishing = cell.GetZone(this.pawn.Map) as Zone_Fishing;
				if (zone_Fishing == null)
				{
					return;
				}
				zone_Fishing.Notify_Fished();
			};
			toil.PlaySoundAtStart(SoundDefOf.Interact_CatchFish);
			return toil;
		}

		// Token: 0x040030D4 RID: 12500
		private const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x040030D5 RID: 12501
		private const TargetIndex StandInd = TargetIndex.B;
	}
}
