using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004E0 RID: 1248
	public class Pawn_CarryTracker : IThingHolder, IExposable
	{
		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x000DD628 File Offset: 0x000DB828
		public Thing CarriedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x060024A6 RID: 9382 RVA: 0x000DD645 File Offset: 0x000DB845
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x000DD65E File Offset: 0x000DB85E
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000DD666 File Offset: 0x000DB866
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep, true);
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000DD684 File Offset: 0x000DB884
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000DD6A0 File Offset: 0x000DB8A0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000DD6A8 File Offset: 0x000DB8A8
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000DD6B8 File Offset: 0x000DB8B8
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000DD6E4 File Offset: 0x000DB8E4
		public int MaxStackSpaceEver(ThingDef td)
		{
			int b = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true, -1) / td.VolumePerUnit);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000DD71C File Offset: 0x000DB91C
		public bool TryStartCarry(Thing item)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				string str = "Dead/downed/deathresting pawn ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " tried to start carry item.");
				return false;
			}
			if (this.innerContainer.TryAdd(item, true))
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				return true;
			}
			return false;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x000DD7AC File Offset: 0x000DB9AC
		public int TryStartCarry(Thing item, int count, bool reserve = true)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				string str = "Dead/downed/deathresting pawn ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " tried to start carry " + item.ToStringSafe<Thing>());
				return 0;
			}
			count = Mathf.Min(count, this.AvailableStackSpace(item.def));
			count = Mathf.Min(count, item.stackCount);
			bool flag = Find.Selector.IsSelected(item);
			Thing thing = item.SplitOff(count);
			int num = this.innerContainer.TryAdd(thing, count, true);
			if (num > 0 && thing != item)
			{
				this.TryUpdateTransferables(thing);
			}
			if (num > 0)
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				if (reserve)
				{
					this.pawn.Reserve(this.CarriedThing, this.pawn.CurJob, 1, -1, null, true, false);
				}
				if (flag)
				{
					if (!thing.Destroyed)
					{
						Find.Selector.Select(thing, true, true);
					}
					Find.Selector.Select(this.CarriedThing, true, true);
				}
				this.pawn.MapHeld.resourceCounter.UpdateResourceCounts();
			}
			return num;
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000DD8F0 File Offset: 0x000DBAF0
		private void TryUpdateTransferables(Thing splitStack)
		{
			if (splitStack == null)
			{
				return;
			}
			Pawn_JobTracker jobs = this.pawn.jobs;
			JobDriver_HaulToTransporter jobDriver_HaulToTransporter = ((jobs != null) ? jobs.curDriver : null) as JobDriver_HaulToTransporter;
			if (jobDriver_HaulToTransporter != null)
			{
				CompTransporter transporter = jobDriver_HaulToTransporter.Transporter;
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(splitStack, (transporter != null) ? transporter.leftToLoad : null, TransferAsOneMode.PodsOrCaravanPacking);
				if (transferableOneWay != null && !transferableOneWay.things.Contains(splitStack) && transferableOneWay.MaxCount + splitStack.stackCount <= transferableOneWay.CountToTransfer)
				{
					transferableOneWay.things.Add(splitStack);
				}
			}
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000DD970 File Offset: 0x000DBB70
		public bool TryDropCarriedThing(IntVec3 dropLoc, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				this.pawn.MapHeld.resourceCounter.UpdateResourceCounts();
				return true;
			}
			return false;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000DD9E0 File Offset: 0x000DBBE0
		public bool TryDropCarriedThing(IntVec3 dropLoc, int count, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, count, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				this.pawn.MapHeld.resourceCounter.UpdateResourceCounts();
				return true;
			}
			return false;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000DDA54 File Offset: 0x000DBC54
		public int CarriedCount(ThingDef def)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.def == def)
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000DDAB8 File Offset: 0x000DBCB8
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000DDAC8 File Offset: 0x000DBCC8
		public void CarryHandsTickInterval(int delta)
		{
			Pawn pawn = this.CarriedThing as Pawn;
			if (pawn != null && pawn.DevelopmentalStage.Baby())
			{
				Pawn_IdeoTracker ideo = pawn.ideo;
				if (ideo == null)
				{
					return;
				}
				ideo.IncreaseIdeoExposureIfBabyTick(this.pawn.Ideo, delta);
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000DDB0D File Offset: 0x000DBD0D
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_CarryTracker.<GetGizmos>d__22 <GetGizmos>d__ = new Pawn_CarryTracker.<GetGizmos>d__22(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x04001ABA RID: 6842
		public Pawn pawn;

		// Token: 0x04001ABB RID: 6843
		public ThingOwner<Thing> innerContainer;
	}
}
