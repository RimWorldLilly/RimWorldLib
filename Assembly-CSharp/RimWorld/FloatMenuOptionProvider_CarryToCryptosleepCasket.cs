using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D1D RID: 7453
	public class FloatMenuOptionProvider_CarryToCryptosleepCasket : FloatMenuOptionProvider
	{
		// Token: 0x1700232C RID: 9004
		// (get) Token: 0x0600C0CF RID: 49359 RVA: 0x00464F9A File Offset: 0x0046319A
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700232D RID: 9005
		// (get) Token: 0x0600C0D0 RID: 49360 RVA: 0x00464F9D File Offset: 0x0046319D
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700232E RID: 9006
		// (get) Token: 0x0600C0D1 RID: 49361 RVA: 0x00464FA0 File Offset: 0x004631A0
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700232F RID: 9007
		// (get) Token: 0x0600C0D2 RID: 49362 RVA: 0x00464FA3 File Offset: 0x004631A3
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0D3 RID: 49363 RVA: 0x00464FA8 File Offset: 0x004631A8
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.Downed)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReserveAndReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				return null;
			}
			if (Building_CryptosleepCasket.FindCryptosleepCasketFor(clickedPawn, context.FirstSelectedPawn, true) == null)
			{
				return null;
			}
			TaggedString taggedString = "CarryToCryptosleepCasket".Translate(clickedPawn.LabelCap, clickedPawn);
			if (clickedPawn.IsQuestLodger())
			{
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString + " (" + "CryptosleepCasketGuestsNotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			if (clickedPawn.GetExtraHostFaction(null) != null)
			{
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString + " (" + "CryptosleepCasketGuestPrisonersNotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			Action action = delegate()
			{
				Building_CryptosleepCasket building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(clickedPawn, context.FirstSelectedPawn, false);
				if (building_CryptosleepCasket == null)
				{
					building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(clickedPawn, context.FirstSelectedPawn, true);
				}
				if (building_CryptosleepCasket == null)
				{
					Messages.Message("CannotCarryToCryptosleepCasket".Translate() + ": " + "NoCryptosleepCasket".Translate(), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.CarryToCryptosleepCasket, clickedPawn, building_CryptosleepCasket);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			};
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, action, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
