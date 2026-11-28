using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019DC RID: 6620
	public class Comp_AtmosphericHeater : CompTempControl, IThingGlower
	{
		// Token: 0x17001D23 RID: 7459
		// (get) Token: 0x0600A6AA RID: 42666 RVA: 0x003B696C File Offset: 0x003B4B6C
		private GameCondition_UnnaturalHeat Condition
		{
			get
			{
				GameCondition_UnnaturalHeat gameCondition_UnnaturalHeat = this.parent.Map.gameConditionManager.GetActiveCondition(GameConditionDefOf.UnnaturalHeat) as GameCondition_UnnaturalHeat;
				if (gameCondition_UnnaturalHeat == null)
				{
					gameCondition_UnnaturalHeat = (GameCondition_UnnaturalHeat)GameConditionMaker.MakeCondition(GameConditionDefOf.UnnaturalHeat, -1);
					this.parent.Map.GameConditionManager.RegisterCondition(gameCondition_UnnaturalHeat);
					gameCondition_UnnaturalHeat.Permanent = true;
				}
				return gameCondition_UnnaturalHeat;
			}
		}

		// Token: 0x17001D24 RID: 7460
		// (get) Token: 0x0600A6AB RID: 42667 RVA: 0x003B69CB File Offset: 0x003B4BCB
		// (set) Token: 0x0600A6AC RID: 42668 RVA: 0x003B69D8 File Offset: 0x003B4BD8
		public override float TargetTemperature
		{
			get
			{
				return this.Condition.heaterTargetTemp;
			}
			set
			{
				this.Condition.heaterTargetTemp = value;
			}
		}

		// Token: 0x17001D25 RID: 7461
		// (get) Token: 0x0600A6AD RID: 42669 RVA: 0x003B69E6 File Offset: 0x003B4BE6
		private bool Powered
		{
			get
			{
				return this.powerTraderComp.PowerOn;
			}
		}

		// Token: 0x17001D26 RID: 7462
		// (get) Token: 0x0600A6AE RID: 42670 RVA: 0x003B69F3 File Offset: 0x003B4BF3
		public int TempOffset
		{
			get
			{
				if (!this.Powered || !this.refuelableComp.HasFuel || this.TargetTemperature <= this.parent.Map.mapTemperature.OutdoorTemp)
				{
					return 0;
				}
				return 10;
			}
		}

		// Token: 0x17001D27 RID: 7463
		// (get) Token: 0x0600A6AF RID: 42671 RVA: 0x003B6A2B File Offset: 0x003B4C2B
		private bool Working
		{
			get
			{
				return this.Powered && this.refuelableComp.HasFuel && this.TargetTemperature > this.parent.Map.mapTemperature.OutdoorTemp - 1f;
			}
		}

		// Token: 0x0600A6B0 RID: 42672 RVA: 0x003B6A67 File Offset: 0x003B4C67
		public bool ShouldBeLitNow()
		{
			return this.Working;
		}

		// Token: 0x0600A6B1 RID: 42673 RVA: 0x003B6A70 File Offset: 0x003B4C70
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Atmospheric heater"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerTraderComp = this.parent.GetComp<CompPowerTrader>();
			this.heatPusherComp = this.parent.GetComp<CompHeatPusher>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x0600A6B2 RID: 42674 RVA: 0x003B6AD0 File Offset: 0x003B4CD0
		public override void CompTick()
		{
			if (this.Working)
			{
				this.powerTraderComp.PowerOutput = -this.powerTraderComp.Props.PowerConsumption;
				this.refuelableComp.Notify_UsedThisTick();
				this.heatPusherComp.enabled = true;
			}
			else
			{
				this.powerTraderComp.PowerOutput = -this.powerTraderComp.Props.PowerConsumption * base.Props.lowPowerConsumptionFactor;
				this.heatPusherComp.enabled = false;
			}
			int num = (this.Working > false) ? 1 : 0;
			int? overrideGraphicIndex = this.parent.overrideGraphicIndex;
			int num2 = num;
			if (!(overrideGraphicIndex.GetValueOrDefault() == num2 & overrideGraphicIndex != null))
			{
				this.parent.overrideGraphicIndex = new int?(num);
				this.parent.DirtyMapMesh(this.parent.Map);
				CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
				if (compGlower != null)
				{
					compGlower.UpdateLit(this.parent.Map);
				}
			}
			this.operatingAtHighPower = this.Working;
		}

		// Token: 0x0600A6B3 RID: 42675 RVA: 0x003B6BCE File Offset: 0x003B4DCE
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Comp_AtmosphericHeater.<CompGetGizmosExtra>d__20 <CompGetGizmosExtra>d__ = new Comp_AtmosphericHeater.<CompGetGizmosExtra>d__20(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0400623B RID: 25147
		private const int DisabledTextureIndex = 0;

		// Token: 0x0400623C RID: 25148
		private const int EnabledTextureIndex = 1;

		// Token: 0x0400623D RID: 25149
		private const int MaxOffset = 10;

		// Token: 0x0400623E RID: 25150
		private CompPowerTrader powerTraderComp;

		// Token: 0x0400623F RID: 25151
		private CompHeatPusher heatPusherComp;

		// Token: 0x04006240 RID: 25152
		private CompRefuelable refuelableComp;

		// Token: 0x02003723 RID: 14115
		private class Command_GroupedTempChange : Command_Action
		{
			// Token: 0x06014272 RID: 82546 RVA: 0x00680576 File Offset: 0x0067E776
			public Command_GroupedTempChange(Comp_AtmosphericHeater heater, float offset)
			{
				this.heater = heater;
				this.offset = offset;
			}

			// Token: 0x06014273 RID: 82547 RVA: 0x0068058C File Offset: 0x0067E78C
			public override void ProcessInput(Event ev)
			{
			}

			// Token: 0x06014274 RID: 82548 RVA: 0x0068058E File Offset: 0x0067E78E
			public override void ProcessGroupInput(Event ev, List<Gizmo> group)
			{
				this.heater.InterfaceChangeTargetTemperature(this.offset);
			}

			// Token: 0x0400DC3B RID: 56379
			private Comp_AtmosphericHeater heater;

			// Token: 0x0400DC3C RID: 56380
			private float offset;
		}
	}
}
