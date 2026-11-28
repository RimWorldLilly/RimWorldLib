using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020004E1 RID: 1249
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060024B8 RID: 9400 RVA: 0x000DDB50 File Offset: 0x000DBD50
		// (set) Token: 0x060024B9 RID: 9401 RVA: 0x000DDB9C File Offset: 0x000DBD9C
		public ThingWithComps Primary
		{
			get
			{
				for (int i = 0; i < this.equipment.Count; i++)
				{
					if (this.equipment[i].def.equipmentType == EquipmentType.Primary)
					{
						return this.equipment[i];
					}
				}
				return null;
			}
			private set
			{
				if (this.Primary == value)
				{
					return;
				}
				if (value != null && value.def.equipmentType != EquipmentType.Primary)
				{
					Log.Error("Tried to set non-primary equipment as primary.");
					return;
				}
				if (this.Primary != null)
				{
					this.equipment.Remove(this.Primary);
				}
				if (value != null)
				{
					this.equipment.TryAdd(value, true);
				}
				if (this.pawn.drafter != null)
				{
					this.pawn.drafter.Notify_PrimaryWeaponChanged();
				}
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060024BA RID: 9402 RVA: 0x000DDC17 File Offset: 0x000DBE17
		public CompEquippable PrimaryEq
		{
			get
			{
				ThingWithComps primary = this.Primary;
				if (primary == null)
				{
					return null;
				}
				return primary.GetComp<CompEquippable>();
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060024BB RID: 9403 RVA: 0x000DDC2A File Offset: 0x000DBE2A
		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060024BC RID: 9404 RVA: 0x000DDC37 File Offset: 0x000DBE37
		public IEnumerable<Verb> AllEquipmentVerbs
		{
			get
			{
				Pawn_EquipmentTracker.<get_AllEquipmentVerbs>d__12 <get_AllEquipmentVerbs>d__ = new Pawn_EquipmentTracker.<get_AllEquipmentVerbs>d__12(-2);
				<get_AllEquipmentVerbs>d__.<>4__this = this;
				return <get_AllEquipmentVerbs>d__;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060024BD RID: 9405 RVA: 0x000DDC47 File Offset: 0x000DBE47
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000DDC4F File Offset: 0x000DBE4F
		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000DDC6C File Offset: 0x000DBE6C
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<ThingWithComps>>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_References.Look<Thing>(ref this.bondedWeapon, "bondedWeapon", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					foreach (Verb verb in allEquipmentListForReading[i].GetComp<CompEquippable>().AllVerbs)
					{
						verb.caster = this.pawn;
					}
				}
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000DDD18 File Offset: 0x000DBF18
		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				ThingWithComps thingWithComps = allEquipmentListForReading[i];
				if (thingWithComps.def.tickerType != TickerType.Normal)
				{
					thingWithComps.GetComp<CompEquippable>().verbTracker.VerbsTick();
				}
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000DDD63 File Offset: 0x000DBF63
		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000DDD70 File Offset: 0x000DBF70
		public void MakeRoomFor(ThingWithComps eq)
		{
			ThingWithComps thingWithComps;
			this.MakeRoomFor(eq, out thingWithComps);
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000DDD88 File Offset: 0x000DBF88
		public void MakeRoomFor(ThingWithComps eq, out ThingWithComps dropped)
		{
			dropped = null;
			if (eq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				if (this.TryDropEquipment(this.Primary, out dropped, this.pawn.Position, true))
				{
					if (dropped != null)
					{
						dropped.SetForbidden(false, true);
						return;
					}
				}
				else
				{
					Pawn pawn = this.pawn;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " couldn't make room for equipment " + ((eq != null) ? eq.ToString() : null));
				}
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000DDE05 File Offset: 0x000DC005
		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000DDE14 File Offset: 0x000DC014
		public bool TryDropEquipment(ThingWithComps eq, out ThingWithComps resultingEq, IntVec3 pos, bool forbid = true)
		{
			if (!pos.IsValid)
			{
				Pawn pawn = this.pawn;
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to drop " + ((eq != null) ? eq.ToString() : null) + " at invalid cell.");
				resultingEq = null;
				return false;
			}
			if (this.equipment.TryDrop(eq, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingEq, null, null))
			{
				if (resultingEq != null)
				{
					resultingEq.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x000DDE94 File Offset: 0x000DC094
		public void DropAllEquipment(IntVec3 pos, bool forbid = true, bool rememberPrimary = false)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				bool flag = this.equipment[i] == this.Primary;
				ThingWithComps droppedWeapon;
				if (this.TryDropEquipment(this.equipment[i], out droppedWeapon, pos, forbid) && rememberPrimary && flag)
				{
					this.pawn.mindState.droppedWeapon = droppedWeapon;
				}
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000DDEFB File Offset: 0x000DC0FB
		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000DDF0B File Offset: 0x000DC10B
		public void DestroyEquipment(ThingWithComps eq)
		{
			if (!this.equipment.Contains(eq))
			{
				Log.Warning("Tried to destroy equipment " + ((eq != null) ? eq.ToString() : null) + " but it's not here.");
				return;
			}
			this.Remove(eq);
			eq.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000DDF4B File Offset: 0x000DC14B
		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000DDF59 File Offset: 0x000DC159
		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x000DDF67 File Offset: 0x000DC167
		internal void Notify_PrimaryDestroyed()
		{
			if (this.Primary != null)
			{
				this.Remove(this.Primary);
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000DDF9C File Offset: 0x000DC19C
		public void AddEquipment(ThingWithComps newEq)
		{
			if (newEq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				string[] array = new string[6];
				array[0] = "Pawn ";
				array[1] = this.pawn.LabelCap;
				array[2] = " got primaryInt equipment ";
				array[3] = ((newEq != null) ? newEq.ToString() : null);
				array[4] = " while already having primaryInt equipment ";
				int num = 5;
				ThingWithComps primary = this.Primary;
				array[num] = ((primary != null) ? primary.ToString() : null);
				Log.Error(string.Concat(array));
				return;
			}
			if (this.equipment.TryAdd(newEq, true) && newEq.def.equipmentType == EquipmentType.Primary)
			{
				this.pawn.mindState.droppedWeapon = null;
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000DE049 File Offset: 0x000DC249
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_EquipmentTracker.<GetGizmos>d__30 <GetGizmos>d__ = new Pawn_EquipmentTracker.<GetGizmos>d__30(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000DE05C File Offset: 0x000DC25C
		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb verb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				verb.caster = this.pawn;
				verb.Notify_PickedUp();
			}
			eq.Notify_Equipped(this.pawn);
			if (ModsConfig.RoyaltyActive && eq.def.equipmentType == EquipmentType.Primary && this.bondedWeapon != null && !this.bondedWeapon.Destroyed)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_WieldedOtherWeapon();
				}
			}
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000DE108 File Offset: 0x000DC308
		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			eq.Notify_Unequipped(this.pawn);
			if (ModsConfig.RoyaltyActive)
			{
				CompBladelinkWeapon compBladelinkWeapon = eq.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_EquipmentLost(this.pawn);
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				CompUniqueWeapon compUniqueWeapon = eq.TryGetComp<CompUniqueWeapon>();
				if (compUniqueWeapon == null)
				{
					return;
				}
				compUniqueWeapon.Notify_EquipmentLost(this.pawn);
			}
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000DE15C File Offset: 0x000DC35C
		public void Notify_AbilityUsed(Ability ability)
		{
			CompEquippableAbility compEquippableAbility = this.PrimaryEq as CompEquippableAbility;
			if (compEquippableAbility != null && ability == compEquippableAbility.AbilityForReading)
			{
				compEquippableAbility.UsedOnce();
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000DE188 File Offset: 0x000DC388
		public void Notify_PawnSpawned()
		{
			if (this.pawn.BeingTransportedOnGravship)
			{
				return;
			}
			if (this.HasAnything() && this.pawn.Downed && !this.pawn.GetPosture().InBed())
			{
				if (this.pawn.kindDef.destroyGearOnDrop)
				{
					this.DestroyAllEquipment(DestroyMode.Vanish);
					return;
				}
				this.DropAllEquipment(this.pawn.Position, true, false);
			}
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000DE1F8 File Offset: 0x000DC3F8
		public void Notify_PawnDied()
		{
			if (ModsConfig.RoyaltyActive && this.bondedWeapon != null)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.UnCode();
				}
			}
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000DE22C File Offset: 0x000DC42C
		public void Notify_KilledPawn()
		{
			foreach (ThingWithComps thingWithComps in this.equipment)
			{
				thingWithComps.Notify_KilledPawn(this.pawn);
			}
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000DE284 File Offset: 0x000DC484
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000DE28C File Offset: 0x000DC48C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x000DE2A6 File Offset: 0x000DC4A6
		[CompilerGenerated]
		internal static IEnumerable<Gizmo> <GetGizmos>g__YieldGizmos|30_0(ThingWithComps eq, KeyBindingDef preferredHotKey = null)
		{
			Pawn_EquipmentTracker.<<GetGizmos>g__YieldGizmos|30_0>d <<GetGizmos>g__YieldGizmos|30_0>d = new Pawn_EquipmentTracker.<<GetGizmos>g__YieldGizmos|30_0>d(-2);
			<<GetGizmos>g__YieldGizmos|30_0>d.<>3__eq = eq;
			<<GetGizmos>g__YieldGizmos|30_0>d.<>3__preferredHotKey = preferredHotKey;
			return <<GetGizmos>g__YieldGizmos|30_0>d;
		}

		// Token: 0x04001ABC RID: 6844
		public Pawn pawn;

		// Token: 0x04001ABD RID: 6845
		private ThingOwner<ThingWithComps> equipment;

		// Token: 0x04001ABE RID: 6846
		public Thing bondedWeapon;

		// Token: 0x04001ABF RID: 6847
		private static List<KeyBindingDef> tmpKeybindings = new List<KeyBindingDef>();
	}
}
