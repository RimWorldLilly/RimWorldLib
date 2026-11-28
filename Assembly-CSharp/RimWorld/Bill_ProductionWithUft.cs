using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ADA RID: 6874
	public class Bill_ProductionWithUft : Bill_Production
	{
		// Token: 0x17001F9C RID: 8092
		// (get) Token: 0x0600AF63 RID: 44899 RVA: 0x003DBFD8 File Offset: 0x003DA1D8
		protected override string StatusString
		{
			get
			{
				if (this.BoundWorker == null)
				{
					return (base.StatusString ?? "").Trim();
				}
				return ("BoundWorkerIs".Translate(this.BoundWorker.LabelShort, this.BoundWorker) + base.StatusString).Trim();
			}
		}

		// Token: 0x17001F9D RID: 8093
		// (get) Token: 0x0600AF64 RID: 44900 RVA: 0x003DC040 File Offset: 0x003DA240
		public Pawn BoundWorker
		{
			get
			{
				if (this.boundUftInt == null)
				{
					return null;
				}
				Pawn creator = this.boundUftInt.Creator;
				if (creator == null || creator.Downed || creator.HostFaction != null || creator.Destroyed || !creator.Spawned)
				{
					this.boundUftInt = null;
					return null;
				}
				Thing thing = this.billStack.billGiver as Thing;
				if (thing != null)
				{
					WorkTypeDef workTypeDef = null;
					List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].fixedBillGiverDefs != null && allDefsListForReading[i].fixedBillGiverDefs.Contains(thing.def))
						{
							workTypeDef = allDefsListForReading[i].workType;
							break;
						}
					}
					if (workTypeDef != null && !creator.workSettings.WorkIsActive(workTypeDef))
					{
						this.boundUftInt = null;
						return null;
					}
				}
				return creator;
			}
		}

		// Token: 0x17001F9E RID: 8094
		// (get) Token: 0x0600AF65 RID: 44901 RVA: 0x003DC114 File Offset: 0x003DA314
		public UnfinishedThing BoundUft
		{
			get
			{
				return this.boundUftInt;
			}
		}

		// Token: 0x0600AF66 RID: 44902 RVA: 0x003DC11C File Offset: 0x003DA31C
		public void SetBoundUft(UnfinishedThing value, bool setOtherLink = true)
		{
			if (value == this.boundUftInt)
			{
				return;
			}
			if (value != null && value.Destroyed)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to bound destroyed UnfinishedThing ");
				defaultInterpolatedStringHandler.AppendFormatted<UnfinishedThing>(value);
				defaultInterpolatedStringHandler.AppendLiteral(" to bill ");
				defaultInterpolatedStringHandler.AppendFormatted<Bill_ProductionWithUft>(this);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			UnfinishedThing unfinishedThing = this.boundUftInt;
			this.boundUftInt = value;
			if (setOtherLink)
			{
				if (unfinishedThing != null && unfinishedThing.BoundBill == this)
				{
					unfinishedThing.BoundBill = null;
				}
				if (value != null && value.BoundBill != this)
				{
					this.boundUftInt.BoundBill = this;
				}
			}
		}

		// Token: 0x0600AF67 RID: 44903 RVA: 0x003DC1C5 File Offset: 0x003DA3C5
		public Bill_ProductionWithUft()
		{
		}

		// Token: 0x0600AF68 RID: 44904 RVA: 0x003DC1CD File Offset: 0x003DA3CD
		public Bill_ProductionWithUft(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600AF69 RID: 44905 RVA: 0x003DC1D7 File Offset: 0x003DA3D7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnfinishedThing>(ref this.boundUftInt, "boundUft", false);
		}

		// Token: 0x0600AF6A RID: 44906 RVA: 0x003DC1F0 File Offset: 0x003DA3F0
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			this.ClearBoundUft();
			base.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x0600AF6B RID: 44907 RVA: 0x003DC200 File Offset: 0x003DA400
		public void ClearBoundUft()
		{
			this.boundUftInt = null;
		}

		// Token: 0x0600AF6C RID: 44908 RVA: 0x003DC209 File Offset: 0x003DA409
		public override Bill Clone()
		{
			return (Bill_Production)base.Clone();
		}

		// Token: 0x04006551 RID: 25937
		private UnfinishedThing boundUftInt;
	}
}
