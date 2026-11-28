using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200192A RID: 6442
	public class CompOxygenPusher : ThingComp
	{
		// Token: 0x17001BED RID: 7149
		// (get) Token: 0x0600A1A4 RID: 41380 RVA: 0x0039C692 File Offset: 0x0039A892
		private CompProperties_OxygenPusher Props
		{
			get
			{
				return (CompProperties_OxygenPusher)this.props;
			}
		}

		// Token: 0x17001BEE RID: 7150
		// (get) Token: 0x0600A1A5 RID: 41381 RVA: 0x0039C6A0 File Offset: 0x0039A8A0
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

		// Token: 0x0600A1A6 RID: 41382 RVA: 0x0039C6CC File Offset: 0x0039A8CC
		public override void CompTickRare()
		{
			if (this.Props.requiresPower && this.PowerTrader.Off)
			{
				return;
			}
			Room room = this.parent.GetRoom(RegionType.Set_All);
			float num = 100f / (float)room.CellCount * this.Props.airPerSecondPerHundredCells * 4.1666665f;
			room.Vacuum = room.UnsanitizedVacuum - num;
		}

		// Token: 0x04005F7E RID: 24446
		private const float IntervalToPerSecond = 4.1666665f;

		// Token: 0x04005F7F RID: 24447
		private CompPowerTrader intPowerTrader;
	}
}
