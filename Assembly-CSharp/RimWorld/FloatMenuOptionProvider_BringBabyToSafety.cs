using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D14 RID: 7444
	public class FloatMenuOptionProvider_BringBabyToSafety : FloatMenuOptionProvider
	{
		// Token: 0x17002308 RID: 8968
		// (get) Token: 0x0600C08C RID: 49292 RVA: 0x00463908 File Offset: 0x00461B08
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002309 RID: 8969
		// (get) Token: 0x0600C08D RID: 49293 RVA: 0x0046390B File Offset: 0x00461B0B
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700230A RID: 8970
		// (get) Token: 0x0600C08E RID: 49294 RVA: 0x0046390E File Offset: 0x00461B0E
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700230B RID: 8971
		// (get) Token: 0x0600C08F RID: 49295 RVA: 0x00463911 File Offset: 0x00461B11
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700230C RID: 8972
		// (get) Token: 0x0600C090 RID: 49296 RVA: 0x00463914 File Offset: 0x00461B14
		public override bool CanTargetDespawned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C091 RID: 49297 RVA: 0x00463917 File Offset: 0x00461B17
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x0600C092 RID: 49298 RVA: 0x00463920 File Offset: 0x00461B20
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!HealthAIUtility.CanRescueNow(context.FirstSelectedPawn, clickedPawn, true))
			{
				return null;
			}
			if (clickedPawn.IsPrisonerOfColony || clickedPawn.IsSlaveOfColony || clickedPawn.IsColonyMech)
			{
				return null;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(clickedPawn, out breastfeedFailReason))
			{
				return null;
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PutSomewhereSafe".Translate(clickedPawn.LabelCap, clickedPawn), delegate()
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(ChildcareUtility.MakeBringBabyToSafetyJob(context.FirstSelectedPawn, clickedPawn), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
