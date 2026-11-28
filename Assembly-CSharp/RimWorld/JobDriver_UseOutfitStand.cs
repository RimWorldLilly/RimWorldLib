using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AAF RID: 2735
	public class JobDriver_UseOutfitStand : JobDriver
	{
		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x0600526A RID: 21098 RVA: 0x001EB42A File Offset: 0x001E962A
		private Building_OutfitStand OutfitStand
		{
			get
			{
				return (Building_OutfitStand)this.job.targetA.Thing;
			}
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x001EB441 File Offset: 0x001E9641
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Collections.Look<Apparel>(ref this.wornApparelToTransferToStand, "wornApparelToTransferToStand", LookMode.Reference);
			Scribe_Collections.Look<Apparel>(ref this.standApparelToTransferToPawn, "standApparelToTransferToPawn", LookMode.Reference);
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x001EB47D File Offset: 0x001E967D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x001EB4A0 File Offset: 0x001E96A0
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.standApparelToTransferToPawn = new HashSet<Apparel>();
			this.wornApparelToTransferToStand = new HashSet<Apparel>();
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			IEnumerable<Thing> heldItems = this.OutfitStand.HeldItems;
			this.duration = 0;
			foreach (Thing thing in heldItems)
			{
				Apparel apparel = thing as Apparel;
				if (apparel != null && apparel.PawnCanWear(this.pawn, false) && ApparelUtility.HasPartsToWear(this.pawn, apparel.def) && (!CompBiocodable.IsBiocoded(apparel) || CompBiocodable.IsBiocodedFor(apparel, this.pawn)))
				{
					bool flag = true;
					foreach (Apparel apparel2 in wornApparel)
					{
						if (!ApparelUtility.CanWearTogether(apparel.def, apparel2.def, this.pawn.RaceProps.body))
						{
							if (this.pawn.apparel.IsLocked(apparel2))
							{
								flag = false;
								break;
							}
							this.duration += (int)(apparel2.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
							this.wornApparelToTransferToStand.Add(apparel2);
						}
					}
					if (flag)
					{
						this.standApparelToTransferToPawn.Add(apparel);
						this.duration += (int)(apparel.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
					}
				}
			}
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x001EB660 File Offset: 0x001E9860
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UseOutfitStand.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_UseOutfitStand.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x001EB670 File Offset: 0x001E9870
		private void DoTransfer()
		{
			foreach (Apparel ap in this.wornApparelToTransferToStand)
			{
				this.pawn.apparel.Remove(ap);
			}
			foreach (Apparel apparel in this.standApparelToTransferToPawn)
			{
				this.OutfitStand.RemoveApparel(apparel);
				this.pawn.apparel.Wear(apparel, true, false);
				this.pawn.outfits.forcedHandler.SetForced(apparel, true);
			}
			foreach (Apparel apparel2 in this.wornApparelToTransferToStand)
			{
				this.OutfitStand.AddApparel(apparel2);
			}
			ThingWithComps heldWeapon = this.OutfitStand.HeldWeapon;
			if (heldWeapon != null && JobDriver_UseOutfitStand.PawnCanWieldWeapon(heldWeapon, this.pawn) && this.OutfitStand.RemoveHeldWeapon(heldWeapon))
			{
				ThingWithComps thingWithComps;
				this.pawn.equipment.MakeRoomFor(heldWeapon, out thingWithComps);
				this.pawn.equipment.AddEquipment(heldWeapon);
				if (thingWithComps != null)
				{
					thingWithComps.DeSpawn(DestroyMode.Vanish);
					this.OutfitStand.TryAddHeldWeapon(thingWithComps);
				}
			}
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x001EB7F4 File Offset: 0x001E99F4
		private static bool PawnCanWieldWeapon(Thing weapon, Pawn pawn)
		{
			string text;
			return (!weapon.def.IsWeapon || !pawn.WorkTagIsDisabled(WorkTags.Violent)) && (!weapon.def.IsRangedWeapon || !pawn.WorkTagIsDisabled(WorkTags.Shooting)) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && (!pawn.IsQuestLodger() || EquipmentUtility.QuestLodgerCanEquip(weapon, pawn)) && EquipmentUtility.CanEquip(weapon, pawn, out text, true);
		}

		// Token: 0x040031D0 RID: 12752
		private int duration;

		// Token: 0x040031D1 RID: 12753
		private HashSet<Apparel> wornApparelToTransferToStand;

		// Token: 0x040031D2 RID: 12754
		private HashSet<Apparel> standApparelToTransferToPawn;
	}
}
