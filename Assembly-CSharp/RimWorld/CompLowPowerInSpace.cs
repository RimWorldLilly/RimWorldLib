using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001359 RID: 4953
	public class CompLowPowerInSpace : ThingComp
	{
		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x06007AA2 RID: 31394 RVA: 0x002BAC86 File Offset: 0x002B8E86
		private CompProperties_LowPowerUnlessVacuum Props
		{
			get
			{
				return (CompProperties_LowPowerUnlessVacuum)this.props;
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x06007AA3 RID: 31395 RVA: 0x002BAC94 File Offset: 0x002B8E94
		private CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.intPowerTrader) == null)
				{
					result = (this.intPowerTrader = this.parent.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x06007AA4 RID: 31396 RVA: 0x002BACBF File Offset: 0x002B8EBF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.CheckUpdatePowerMode();
		}

		// Token: 0x06007AA5 RID: 31397 RVA: 0x002BACC7 File Offset: 0x002B8EC7
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CheckUpdatePowerMode();
			}
		}

		// Token: 0x06007AA6 RID: 31398 RVA: 0x002BACE1 File Offset: 0x002B8EE1
		public override void CompTickRare()
		{
			this.CheckUpdatePowerMode();
		}

		// Token: 0x06007AA7 RID: 31399 RVA: 0x002BACEC File Offset: 0x002B8EEC
		private void CheckUpdatePowerMode()
		{
			if (this.PowerTrader.Off)
			{
				return;
			}
			this.lowPowerMode = !this.NeedsPower();
			if (this.lowPowerMode)
			{
				this.PowerTrader.PowerOutput = -this.PowerTrader.Props.PowerConsumption * this.Props.lowPowerConsumptionFactor;
				return;
			}
			this.PowerTrader.PowerOutput = -this.PowerTrader.Props.PowerConsumption;
		}

		// Token: 0x06007AA8 RID: 31400 RVA: 0x002BAD64 File Offset: 0x002B8F64
		private bool NeedsPower()
		{
			if (this.Props.checkRoomVacuum)
			{
				Room room = this.parent.GetRoom(RegionType.Set_All);
				if (room.ExposedToSpace)
				{
					return false;
				}
				if (Mathf.Approximately(room.Vacuum, 0f))
				{
					return false;
				}
			}
			return this.parent.Map.Biome.inVacuum;
		}

		// Token: 0x06007AA9 RID: 31401 RVA: 0x002BADC4 File Offset: 0x002B8FC4
		public override string CompInspectStringExtra()
		{
			string str;
			if (this.PowerTrader.Off)
			{
				str = "PowerConsumptionOff".Translate();
			}
			else if (this.lowPowerMode)
			{
				str = "PowerConsumptionLow".Translate();
			}
			else
			{
				str = "PowerConsumptionHigh".Translate();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PowerConsumptionMode".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(str.CapitalizeFirst());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04004D74 RID: 19828
		private bool lowPowerMode;

		// Token: 0x04004D75 RID: 19829
		private CompPowerTrader intPowerTrader;
	}
}
