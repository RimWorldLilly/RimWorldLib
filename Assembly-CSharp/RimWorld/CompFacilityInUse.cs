using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187D RID: 6269
	public class CompFacilityInUse : ThingComp
	{
		// Token: 0x17001ADC RID: 6876
		// (get) Token: 0x06009D00 RID: 40192 RVA: 0x00387B01 File Offset: 0x00385D01
		public CompProperties_FacilityInUse Props
		{
			get
			{
				return this.props as CompProperties_FacilityInUse;
			}
		}

		// Token: 0x17001ADD RID: 6877
		// (get) Token: 0x06009D01 RID: 40193 RVA: 0x00387B10 File Offset: 0x00385D10
		public CompPowerTrader PowerTrader
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

		// Token: 0x06009D02 RID: 40194 RVA: 0x00387B3B File Offset: 0x00385D3B
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			Effecter effecter = this.effecterInUse;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecterInUse = null;
		}

		// Token: 0x06009D03 RID: 40195 RVA: 0x00387B5D File Offset: 0x00385D5D
		public override void CompTickRare()
		{
			this.CompTick();
		}

		// Token: 0x06009D04 RID: 40196 RVA: 0x00387B65 File Offset: 0x00385D65
		public override void CompTickLong()
		{
			this.CompTick();
		}

		// Token: 0x06009D05 RID: 40197 RVA: 0x00387B70 File Offset: 0x00385D70
		public override void CompTick()
		{
			CompFacility compFacility = this.parent.TryGetComp<CompFacility>();
			List<Thing> list = (compFacility != null) ? compFacility.LinkedBuildings : null;
			if (list == null)
			{
				return;
			}
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			Thing thing = null;
			foreach (Thing thing2 in list)
			{
				if (compPowerTrader.PowerOn && this.BuildingInUse(thing2))
				{
					thing = thing2;
					break;
				}
			}
			bool flag = thing != null;
			this.operatingAtHighPower = false;
			if (this.Props.inUsePowerConsumption != null)
			{
				float num = compPowerTrader.Props.PowerConsumption;
				if (flag)
				{
					num = this.Props.inUsePowerConsumption.Value;
					this.operatingAtHighPower = true;
				}
				compPowerTrader.PowerOutput = -num;
			}
			if (this.Props.effectInUse != null)
			{
				if (flag)
				{
					if (this.effecterInUse == null)
					{
						this.effecterInUse = this.Props.effectInUse.Spawn();
						this.effecterInUse.Trigger(this.parent, thing, -1);
					}
					this.effecterInUse.EffectTick(this.parent, thing);
				}
				if (!flag && this.effecterInUse != null)
				{
					this.effecterInUse.Cleanup();
					this.effecterInUse = null;
				}
			}
		}

		// Token: 0x06009D06 RID: 40198 RVA: 0x00387CD0 File Offset: 0x00385ED0
		private bool BuildingInUse(Thing building)
		{
			Building_Bed building_Bed = building as Building_Bed;
			return building_Bed != null && building_Bed.AnyOccupants;
		}

		// Token: 0x06009D07 RID: 40199 RVA: 0x00387CF4 File Offset: 0x00385EF4
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string str;
			if (this.PowerTrader.Off)
			{
				str = "PowerConsumptionOff".Translate();
			}
			else if (!this.operatingAtHighPower)
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
			stringBuilder.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			return stringBuilder.ToString();
		}

		// Token: 0x04005D13 RID: 23827
		[Unsaved(false)]
		private bool operatingAtHighPower;

		// Token: 0x04005D14 RID: 23828
		[Unsaved(false)]
		private Effecter effecterInUse;

		// Token: 0x04005D15 RID: 23829
		[Unsaved(false)]
		private CompPowerTrader intPowerTrader;
	}
}
