using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD0 RID: 6864
	public class Building_VacBarrier : Building_SupportedDoor
	{
		// Token: 0x17001F62 RID: 8034
		// (get) Token: 0x0600AEBE RID: 44734 RVA: 0x003D8918 File Offset: 0x003D6B18
		private CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.intPowerTrader) == null)
				{
					result = (this.intPowerTrader = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17001F63 RID: 8035
		// (get) Token: 0x0600AEBF RID: 44735 RVA: 0x003D8940 File Offset: 0x003D6B40
		private VacuumComponent Vacuum
		{
			get
			{
				VacuumComponent result;
				if ((result = this.intVacuum) == null)
				{
					Map mapHeld = base.MapHeld;
					result = (this.intVacuum = ((mapHeld != null) ? mapHeld.GetComponent<VacuumComponent>() : null));
				}
				return result;
			}
		}

		// Token: 0x17001F64 RID: 8036
		// (get) Token: 0x0600AEC0 RID: 44736 RVA: 0x003D8972 File Offset: 0x003D6B72
		private bool BarrierOn
		{
			get
			{
				return this.PowerTrader.PowerOn;
			}
		}

		// Token: 0x17001F65 RID: 8037
		// (get) Token: 0x0600AEC1 RID: 44737 RVA: 0x003D897F File Offset: 0x003D6B7F
		public override bool FreePassage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F66 RID: 8038
		// (get) Token: 0x0600AEC2 RID: 44738 RVA: 0x003D8982 File Offset: 0x003D6B82
		public override bool ExchangeVacuum
		{
			get
			{
				return !this.PowerTrader.PowerOn;
			}
		}

		// Token: 0x17001F67 RID: 8039
		// (get) Token: 0x0600AEC3 RID: 44739 RVA: 0x003D8992 File Offset: 0x003D6B92
		protected override float TempEqualizeRate
		{
			get
			{
				if (!this.BarrierOn)
				{
					return 14f;
				}
				return 0f;
			}
		}

		// Token: 0x17001F68 RID: 8040
		// (get) Token: 0x0600AEC4 RID: 44740 RVA: 0x003D89A7 File Offset: 0x003D6BA7
		protected override bool AlwaysOpen
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F69 RID: 8041
		// (get) Token: 0x0600AEC5 RID: 44741 RVA: 0x003D89AA File Offset: 0x003D6BAA
		protected override float OpenPct
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001F6A RID: 8042
		// (get) Token: 0x0600AEC6 RID: 44742 RVA: 0x003D89B1 File Offset: 0x003D6BB1
		protected override bool CanDrawMovers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600AEC7 RID: 44743 RVA: 0x003D89B4 File Offset: 0x003D6BB4
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff")
			{
				VacuumComponent vacuum = this.Vacuum;
				if (vacuum == null)
				{
					return;
				}
				vacuum.Dirty();
			}
		}

		// Token: 0x0600AEC8 RID: 44744 RVA: 0x003D89E0 File Offset: 0x003D6BE0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.BarrierOn)
			{
				this.Graphic.Draw(drawLoc, flip ? base.Rotation.Opposite : base.Rotation, this, 0f);
			}
		}

		// Token: 0x0400650A RID: 25866
		private CompPowerTrader intPowerTrader;

		// Token: 0x0400650B RID: 25867
		private VacuumComponent intVacuum;
	}
}
