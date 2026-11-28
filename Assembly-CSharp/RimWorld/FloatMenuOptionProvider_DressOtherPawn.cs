using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D26 RID: 7462
	public class FloatMenuOptionProvider_DressOtherPawn : FloatMenuOptionProvider
	{
		// Token: 0x17002353 RID: 9043
		// (get) Token: 0x0600C11F RID: 49439 RVA: 0x00466574 File Offset: 0x00464774
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002354 RID: 9044
		// (get) Token: 0x0600C120 RID: 49440 RVA: 0x00466577 File Offset: 0x00464777
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002355 RID: 9045
		// (get) Token: 0x0600C121 RID: 49441 RVA: 0x0046657A File Offset: 0x0046477A
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C122 RID: 49442 RVA: 0x00466580 File Offset: 0x00464780
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Apparel apparel = clickedThing as Apparel;
			if (apparel == null)
			{
				return null;
			}
			string cannotForceTargetText = "CannotForceTargetToWear";
			string key = "ForceTargetToWear";
			if (apparel.def.apparel.LastLayer.IsUtilityLayer)
			{
				cannotForceTargetText = "CannotForceTargetToEquipApparel";
				key = "ForceTargetToEquipApparel";
			}
			if (!context.FirstSelectedPawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (apparel.IsBurning())
			{
				return new FloatMenuOption(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key.Translate(apparel.LabelShort, apparel), delegate()
			{
				bool queueOrder = KeyBindingDefOf.QueueOrder.IsDownEvent;
				Find.Targeter.BeginTargeting(TargetingParameters.ForForceWear(context.FirstSelectedPawn), delegate(LocalTargetInfo target)
				{
					Pawn targetPawn;
					bool queueOrder;
					if (!target.TryGetPawn(out targetPawn))
					{
						if (ModsConfig.OdysseyActive)
						{
							Building_OutfitStand building_OutfitStand = target.Thing as Building_OutfitStand;
							if (building_OutfitStand != null)
							{
								if (!building_OutfitStand.CanEverStoreThing(apparel))
								{
									Messages.Message("CannotStoreThingOnTarget".Translate(apparel.Named("THING"), building_OutfitStand.Named("TARGET")), MessageTypeDefOf.RejectInput, false);
									return;
								}
								Pawn_JobTracker jobs = context.FirstSelectedPawn.jobs;
								Job job = JobMaker.MakeJob(JobDefOf.PutApparelOnOutfitStand, apparel, building_OutfitStand);
								queueOrder = queueOrder;
								jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), queueOrder);
							}
						}
						return;
					}
					if (targetPawn.apparel.WouldReplaceLockedApparel(apparel))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					if (targetPawn.IsMutant && targetPawn.mutant.Def.disableApparel)
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + targetPawn.mutant.Def.LabelCap, targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					if (!ApparelUtility.HasPartsToWear(targetPawn, apparel.def))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate().CapitalizeFirst(), targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					string t;
					if (!EquipmentUtility.CanEquip(apparel, targetPawn, out t, true))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + t, targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					Action action = delegate()
					{
						apparel.SetForbidden(false, true);
						Job job2 = JobMaker.MakeJob(JobDefOf.ForceTargetWear, targetPawn, apparel);
						Pawn_JobTracker jobs2 = context.FirstSelectedPawn.jobs;
						Job job3 = job2;
						bool queueOrder2 = queueOrder;
						jobs2.TryTakeOrderedJob(job3, new JobTag?(JobTag.Misc), queueOrder2);
					};
					Apparel apparelReplacedByNewApparel = ApparelUtility.GetApparelReplacedByNewApparel(targetPawn, apparel);
					if (apparelReplacedByNewApparel == null || !ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(targetPawn, apparelReplacedByNewApparel, action))
					{
						action();
					}
				}, null, null, null, true);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, apparel, "ReservedBy", null);
		}
	}
}
