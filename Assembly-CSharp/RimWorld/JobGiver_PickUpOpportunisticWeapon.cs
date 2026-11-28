using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AF5 RID: 2805
	public class JobGiver_PickUpOpportunisticWeapon : ThinkNode_JobGiver
	{
		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06005396 RID: 21398 RVA: 0x001F1C20 File Offset: 0x001EFE20
		private float MinMeleeWeaponDPSThreshold
		{
			get
			{
				List<Tool> tools = ThingDefOf.Human.tools;
				float num = 0f;
				for (int i = 0; i < tools.Count; i++)
				{
					if (tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.LeftHand || tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.RightHand)
					{
						num = tools[i].power / tools[i].cooldownTime;
						break;
					}
				}
				return num + 2f;
			}
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x001F1C98 File Offset: 0x001EFE98
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PickUpOpportunisticWeapon jobGiver_PickUpOpportunisticWeapon = (JobGiver_PickUpOpportunisticWeapon)base.DeepCopy(resolve);
			jobGiver_PickUpOpportunisticWeapon.preferBuildingDestroyers = this.preferBuildingDestroyers;
			jobGiver_PickUpOpportunisticWeapon.pickUpUtilityItems = this.pickUpUtilityItems;
			return jobGiver_PickUpOpportunisticWeapon;
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x001F1CC0 File Offset: 0x001EFEC0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment == null && pawn.apparel == null)
			{
				return null;
			}
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			if (pawn.GetRegion(RegionType.Set_Passable) == null)
			{
				return null;
			}
			if (pawn.equipment != null && !this.AlreadySatisfiedWithCurrentWeapon(pawn))
			{
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 8f, (Thing x) => pawn.CanReserve(x, 1, -1, null, false) && !x.IsBurning() && this.ShouldEquipWeapon(x, pawn), null, 0, 15, false, RegionType.Set_Passable, false, true);
				if (thing != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.Equip, thing);
					job.expiryInterval = 60;
					return job;
				}
				if (pawn.equipment.Primary != null && !SlaveRebellionUtility.WeaponUsableInRebellion(pawn.equipment.Primary))
				{
					return JobMaker.MakeJob(JobDefOf.DropEquipment, pawn.equipment.Primary);
				}
			}
			Pawn_EquipmentTracker equipment = pawn.equipment;
			bool flag;
			if (equipment == null)
			{
				flag = false;
			}
			else
			{
				ThingWithComps primary = equipment.Primary;
				bool? flag2;
				if (primary == null)
				{
					flag2 = null;
				}
				else
				{
					ThingDef def = primary.def;
					flag2 = ((def != null) ? new bool?(def.IsRangedWeapon) : null);
				}
				bool? flag3 = flag2;
				flag = flag3.GetValueOrDefault();
			}
			if (flag)
			{
				foreach (Apparel apparel in pawn.apparel.WornApparel)
				{
					if (apparel.def == ThingDefOf.Apparel_ShieldBelt)
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.RemoveApparel, apparel);
						job2.expiryInterval = 60;
						return job2;
					}
				}
			}
			if (this.pickUpUtilityItems && pawn.apparel != null && this.WouldPickupUtilityItem(pawn))
			{
				Thing thing2 = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Apparel), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 8f, (Thing x) => pawn.CanReserve(x, 1, -1, null, false) && !x.IsBurning() && this.ShouldEquipUtilityItem(x, pawn), null, 0, 15, false, RegionType.Set_Passable, false, true);
				if (thing2 != null)
				{
					Job job3 = JobMaker.MakeJob(JobDefOf.Wear, thing2);
					job3.expiryInterval = 60;
					return job3;
				}
			}
			return null;
		}

		// Token: 0x06005399 RID: 21401 RVA: 0x001F1F84 File Offset: 0x001F0184
		private bool AlreadySatisfiedWithCurrentWeapon(Pawn pawn)
		{
			ThingWithComps primary = pawn.equipment.Primary;
			if (primary == null)
			{
				return false;
			}
			if (this.preferBuildingDestroyers)
			{
				if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
				{
					return false;
				}
			}
			else if (!primary.def.IsRangedWeapon || !SlaveRebellionUtility.WeaponUsableInRebellion(primary))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x001F1FE0 File Offset: 0x001F01E0
		private bool ShouldEquipWeapon(Thing newWep, Pawn pawn)
		{
			return (!newWep.def.IsRangedWeapon || !pawn.WorkTagIsDisabled(WorkTags.Shooting)) && (EquipmentUtility.CanEquip(newWep, pawn) && this.GetWeaponScore(newWep) > this.GetWeaponScore(pawn.equipment.Primary)) && SlaveRebellionUtility.WeaponUsableInRebellion(newWep);
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x001F2034 File Offset: 0x001F0234
		private int GetWeaponScore(Thing wep)
		{
			if (wep == null)
			{
				return 0;
			}
			if (wep.def.IsMeleeWeapon && wep.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, true, -1) < this.MinMeleeWeaponDPSThreshold)
			{
				return 0;
			}
			if (this.preferBuildingDestroyers && wep.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
			{
				return 3;
			}
			if (wep.def.IsRangedWeapon)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x0600539C RID: 21404 RVA: 0x001F209B File Offset: 0x001F029B
		private bool WouldPickupUtilityItem(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			return ((equipment != null) ? equipment.Primary : null) == null && pawn.apparel.FirstApparelVerb == null;
		}

		// Token: 0x0600539D RID: 21405 RVA: 0x001F20C4 File Offset: 0x001F02C4
		private bool ShouldEquipUtilityItem(Thing thing, Pawn pawn)
		{
			Apparel apparel = thing as Apparel;
			return apparel != null && apparel.def.apparel.ai_pickUpOpportunistically && (EquipmentUtility.CanEquip(apparel, pawn) && ApparelUtility.HasPartsToWear(pawn, apparel.def)) && !pawn.apparel.WouldReplaceLockedApparel(apparel);
		}

		// Token: 0x04003227 RID: 12839
		private bool preferBuildingDestroyers;

		// Token: 0x04003228 RID: 12840
		private bool pickUpUtilityItems;
	}
}
