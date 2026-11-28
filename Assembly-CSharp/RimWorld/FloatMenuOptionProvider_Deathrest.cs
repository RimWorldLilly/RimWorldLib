using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D21 RID: 7457
	public class FloatMenuOptionProvider_Deathrest : FloatMenuOptionProvider
	{
		// Token: 0x1700233C RID: 9020
		// (get) Token: 0x0600C0ED RID: 49389 RVA: 0x004655A4 File Offset: 0x004637A4
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700233D RID: 9021
		// (get) Token: 0x0600C0EE RID: 49390 RVA: 0x004655A7 File Offset: 0x004637A7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700233E RID: 9022
		// (get) Token: 0x0600C0EF RID: 49391 RVA: 0x004655AA File Offset: 0x004637AA
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C0F0 RID: 49392 RVA: 0x004655AD File Offset: 0x004637AD
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive && context.FirstSelectedPawn.CanDeathrest();
		}

		// Token: 0x0600C0F1 RID: 49393 RVA: 0x004655C8 File Offset: 0x004637C8
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Building_Bed bed = clickedThing as Building_Bed;
			if (bed == null || !bed.def.building.bed_humanlike)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(bed, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotDeathrest".Translate().CapitalizeFirst() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			AcceptanceReport acceptanceReport = bed.CompAssignableToPawn.CanAssignTo(context.FirstSelectedPawn);
			if (!acceptanceReport.Accepted)
			{
				return new FloatMenuOption("CannotDeathrest".Translate().CapitalizeFirst() + ": " + acceptanceReport.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if ((!bed.CompAssignableToPawn.HasFreeSlot || !RestUtility.BedOwnerWillShare(bed, context.FirstSelectedPawn, new GuestStatus?(context.FirstSelectedPawn.guest.GuestStatus))) && !bed.IsOwner(context.FirstSelectedPawn))
			{
				return new FloatMenuOption("CannotDeathrest".Translate().CapitalizeFirst() + ": " + "AssignedToOtherPawn".Translate(bed).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			bool flag = false;
			using (CellRect.Enumerator enumerator = bed.OccupiedRect().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetRoof(bed.Map) == null)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return new FloatMenuOption("CannotDeathrest".Translate().CapitalizeFirst() + ": " + "ThingIsSkyExposed".Translate(bed).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!RestUtility.IsValidBedFor(bed, context.FirstSelectedPawn, context.FirstSelectedPawn, true, false, false, context.FirstSelectedPawn.GuestStatus))
			{
				return null;
			}
			return new FloatMenuOption("StartDeathrest".Translate(), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.Deathrest, bed);
				job.forceSleep = true;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
		}
	}
}
