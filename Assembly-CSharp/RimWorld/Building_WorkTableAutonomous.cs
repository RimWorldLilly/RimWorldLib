using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ADF RID: 6879
	[StaticConstructorOnStartup]
	public class Building_WorkTableAutonomous : Building_WorkTable, IThingHolder, INotifyHauledTo
	{
		// Token: 0x17001FAC RID: 8108
		// (get) Token: 0x0600AF9A RID: 44954 RVA: 0x003DCFA7 File Offset: 0x003DB1A7
		// (set) Token: 0x0600AF9B RID: 44955 RVA: 0x003DCFAF File Offset: 0x003DB1AF
		public Bill_Autonomous ActiveBill
		{
			get
			{
				return this.activeBill;
			}
			set
			{
				if (this.activeBill != value)
				{
					this.activeBill = value;
				}
			}
		}

		// Token: 0x17001FAD RID: 8109
		// (get) Token: 0x0600AF9C RID: 44956 RVA: 0x003DCFC4 File Offset: 0x003DB1C4
		public float CurrentBillFormingPercent
		{
			get
			{
				if (this.activeBill == null || this.activeBill.State != FormingState.Forming)
				{
					return 0f;
				}
				return 1f - this.activeBill.formingTicks / (float)this.activeBill.recipe.formingTicks;
			}
		}

		// Token: 0x17001FAE RID: 8110
		// (get) Token: 0x0600AF9D RID: 44957 RVA: 0x003DD011 File Offset: 0x003DB211
		public GenDraw.FillableBarRequest BarDrawData
		{
			get
			{
				return this.def.building.BarDrawDataFor(base.Rotation);
			}
		}

		// Token: 0x0600AF9E RID: 44958 RVA: 0x003DD029 File Offset: 0x003DB229
		public Building_WorkTableAutonomous()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600AF9F RID: 44959 RVA: 0x003DD03D File Offset: 0x003DB23D
		public virtual void Notify_StartForming(Pawn billDoer)
		{
		}

		// Token: 0x0600AFA0 RID: 44960 RVA: 0x003DD040 File Offset: 0x003DB240
		public virtual void Notify_FormingCompleted()
		{
			Thing thing = this.activeBill.CreateProducts();
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (thing != null)
			{
				this.innerContainer.TryAdd(thing, true);
			}
		}

		// Token: 0x0600AFA1 RID: 44961 RVA: 0x003DD076 File Offset: 0x003DB276
		public override void Notify_BillDeleted(Bill bill)
		{
			if (this.activeBill == bill)
			{
				this.EjectContents();
				this.activeBill = null;
			}
		}

		// Token: 0x0600AFA2 RID: 44962 RVA: 0x003DD08E File Offset: 0x003DB28E
		public virtual void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
		}

		// Token: 0x0600AFA3 RID: 44963 RVA: 0x003DD090 File Offset: 0x003DB290
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Bill_Autonomous bill_Autonomous = this.activeBill;
			if (bill_Autonomous != null)
			{
				bill_Autonomous.Reset();
			}
			this.EjectContents();
			base.Destroy(mode);
		}

		// Token: 0x0600AFA4 RID: 44964 RVA: 0x003DD0B0 File Offset: 0x003DB2B0
		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
			Bill_Autonomous bill_Autonomous = this.activeBill;
			if (bill_Autonomous == null)
			{
				return;
			}
			bill_Autonomous.Reset();
		}

		// Token: 0x0600AFA5 RID: 44965 RVA: 0x003DD0DE File Offset: 0x003DB2DE
		public virtual bool CanWork()
		{
			return true;
		}

		// Token: 0x0600AFA6 RID: 44966 RVA: 0x003DD0E1 File Offset: 0x003DB2E1
		protected override void Tick()
		{
			base.Tick();
			if (this.activeBill != null && this.CanWork())
			{
				this.activeBill.BillTick();
			}
		}

		// Token: 0x0600AFA7 RID: 44967 RVA: 0x003DD104 File Offset: 0x003DB304
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_References.Look<Bill_Autonomous>(ref this.activeBill, "activeBill", false);
		}

		// Token: 0x0600AFA8 RID: 44968 RVA: 0x003DD138 File Offset: 0x003DB338
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			string inspectStringExtra = this.GetInspectStringExtra();
			if (!inspectStringExtra.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectStringExtra);
			}
			if (this.CanWork() && this.activeBill != null)
			{
				this.activeBill.AppendInspectionData(stringBuilder);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600AFA9 RID: 44969 RVA: 0x003DD1A0 File Offset: 0x003DB3A0
		protected virtual string GetInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600AFAA RID: 44970 RVA: 0x003DD1A3 File Offset: 0x003DB3A3
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_WorkTableAutonomous.<GetGizmos>d__21 <GetGizmos>d__ = new Building_WorkTableAutonomous.<GetGizmos>d__21(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AFAB RID: 44971 RVA: 0x003DD1B3 File Offset: 0x003DB3B3
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600AFAC RID: 44972 RVA: 0x003DD1BB File Offset: 0x003DB3BB
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04006560 RID: 25952
		public ThingOwner innerContainer;

		// Token: 0x04006561 RID: 25953
		protected Bill_Autonomous activeBill;
	}
}
