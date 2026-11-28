using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ADD RID: 6877
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x17001FA7 RID: 8103
		// (get) Token: 0x0600AF8C RID: 44940 RVA: 0x003DCD92 File Offset: 0x003DAF92
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x17001FA8 RID: 8104
		// (get) Token: 0x0600AF8D RID: 44941 RVA: 0x003DCDB8 File Offset: 0x003DAFB8
		public bool CanWorkWithoutFuel
		{
			get
			{
				return this.refuelableComp == null;
			}
		}

		// Token: 0x0600AF8E RID: 44942 RVA: 0x003DCDC3 File Offset: 0x003DAFC3
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x0600AF8F RID: 44943 RVA: 0x003DCDD7 File Offset: 0x003DAFD7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x0600AF90 RID: 44944 RVA: 0x003DCDFC File Offset: 0x003DAFFC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.refuelableComp = base.GetComp<CompRefuelable>();
			this.breakdownableComp = base.GetComp<CompBreakdownable>();
			this.moteEmitterComp = base.GetComp<CompMoteEmitter>();
			if (base.BeingTransportedOnGravship)
			{
				return;
			}
			foreach (Bill bill in this.billStack)
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x0600AF91 RID: 44945 RVA: 0x003DCE88 File Offset: 0x003DB088
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
			if (this.moteEmitterComp != null)
			{
				if (!this.moteEmitterComp.MoteLive)
				{
					this.moteEmitterComp.Emit();
				}
				this.moteEmitterComp.Maintain();
			}
		}

		// Token: 0x17001FA9 RID: 8105
		// (get) Token: 0x0600AF92 RID: 44946 RVA: 0x003DCEC8 File Offset: 0x003DB0C8
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x17001FAA RID: 8106
		// (get) Token: 0x0600AF93 RID: 44947 RVA: 0x003DCED0 File Offset: 0x003DB0D0
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x17001FAB RID: 8107
		// (get) Token: 0x0600AF94 RID: 44948 RVA: 0x003DCED8 File Offset: 0x003DB0D8
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x0600AF95 RID: 44949 RVA: 0x003DCEE0 File Offset: 0x003DB0E0
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.CanWorkWithoutFuel || (this.refuelableComp != null && this.refuelableComp.HasFuel));
		}

		// Token: 0x0600AF96 RID: 44950 RVA: 0x003DCF36 File Offset: 0x003DB136
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}

		// Token: 0x0600AF97 RID: 44951 RVA: 0x003DCF6F File Offset: 0x003DB16F
		public virtual void Notify_BillDeleted(Bill bill)
		{
		}

		// Token: 0x0400655A RID: 25946
		public BillStack billStack;

		// Token: 0x0400655B RID: 25947
		private CompPowerTrader powerComp;

		// Token: 0x0400655C RID: 25948
		private CompRefuelable refuelableComp;

		// Token: 0x0400655D RID: 25949
		private CompBreakdownable breakdownableComp;

		// Token: 0x0400655E RID: 25950
		private CompMoteEmitter moteEmitterComp;
	}
}
