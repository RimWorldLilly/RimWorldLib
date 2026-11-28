using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D3B RID: 7483
	public class FloatMenuOptionProvider_Relic : FloatMenuOptionProvider
	{
		// Token: 0x1700239A RID: 9114
		// (get) Token: 0x0600C19C RID: 49564 RVA: 0x0046828F File Offset: 0x0046648F
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700239B RID: 9115
		// (get) Token: 0x0600C19D RID: 49565 RVA: 0x00468292 File Offset: 0x00466492
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700239C RID: 9116
		// (get) Token: 0x0600C19E RID: 49566 RVA: 0x00468295 File Offset: 0x00466495
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700239D RID: 9117
		// (get) Token: 0x0600C19F RID: 49567 RVA: 0x00468298 File Offset: 0x00466498
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1A0 RID: 49568 RVA: 0x0046829B File Offset: 0x0046649B
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x0600C1A1 RID: 49569 RVA: 0x004682A2 File Offset: 0x004664A2
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_Relic.<GetOptionsFor>d__9 <GetOptionsFor>d__ = new FloatMenuOptionProvider_Relic.<GetOptionsFor>d__9(-2);
			<GetOptionsFor>d__.<>4__this = this;
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x0600C1A2 RID: 49570 RVA: 0x004682C0 File Offset: 0x004664C0
		private FloatMenuOption InstallRelic(Thing containerThing, Pawn pawn)
		{
			IEnumerable<Thing> searchSet = from x in containerThing.Map.listerThings.ThingsOfDef(ThingDefOf.Reliquary)
			where x.TryGetComp<CompRelicContainer>().ContainedThing == null
			select x;
			Thing thing = GenClosest.ClosestThing_Global_Reachable(containerThing.Position, containerThing.Map, searchSet, PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => pawn.CanReserve(t, 1, -1, null, false), null, false);
			if (thing == null)
			{
				return new FloatMenuOption("InstallInReliquary".Translate() + " (" + "NoEmptyReliquary".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Job job = JobMaker.MakeJob(JobDefOf.InstallRelic, containerThing, thing, thing.InteractionCell);
			job.count = 1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("InstallInReliquary".Translate(), delegate()
			{
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, new LocalTargetInfo(containerThing), "ReservedBy", null);
		}

		// Token: 0x0600C1A3 RID: 49571 RVA: 0x0046840C File Offset: 0x0046660C
		private FloatMenuOption ExtractRelic(CompRelicContainer container, Pawn pawn)
		{
			TaggedString taggedString = "ExtractRelic".Translate(container.ContainedThing.Label);
			IntVec3 c;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(container.ContainedThing, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out c, out haulDestination, true))
			{
				return new FloatMenuOption(taggedString + " (" + HaulAIUtility.NoEmptyPlaceLowerTrans + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Job job = JobMaker.MakeJob(JobDefOf.ExtractRelic, container.parent, container.ContainedThing, c);
			job.count = 1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, new LocalTargetInfo(container.parent), "ReservedBy", null);
		}

		// Token: 0x0600C1A4 RID: 49572 RVA: 0x00468520 File Offset: 0x00466720
		private bool InstallRelicFromContainer(Thing containerThing, Pawn pawn, out FloatMenuOption option)
		{
			option = null;
			IEnumerable<Thing> enumerable = from x in pawn.Map.listerThings.AllThings
			where CompRelicContainer.IsRelic(x) && pawn.CanReach(x, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn)
			select x;
			if (!enumerable.Any<Thing>())
			{
				option = new FloatMenuOption("NoRelicToInstall".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			using (IEnumerator<Thing> enumerator = enumerable.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing thing = enumerator.Current;
					Job job = JobMaker.MakeJob(JobDefOf.InstallRelic, thing, containerThing, containerThing.InteractionCell);
					job.count = 1;
					option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("InstallRelic".Translate(thing.Label), delegate()
					{
						pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, new LocalTargetInfo(containerThing), "ReservedBy", null);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C1A5 RID: 49573 RVA: 0x00468670 File Offset: 0x00466870
		private FloatMenuOption ExtractToInventory(CompRelicContainer container, Pawn pawn)
		{
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ExtractRelicToInventory".Translate(container.ContainedThing.Label, 300.ToStringTicksToPeriod(true, false, true, true, false)), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.ExtractToInventory, container.parent, container.ContainedThing, container.parent.InteractionCell);
				job.count = 1;
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, new LocalTargetInfo(container.parent), "ReservedBy", null);
		}
	}
}
