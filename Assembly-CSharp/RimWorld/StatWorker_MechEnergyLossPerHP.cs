using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD3 RID: 8147
	public class StatWorker_MechEnergyLossPerHP : StatWorker
	{
		// Token: 0x0600CEBA RID: 52922 RVA: 0x004C18C8 File Offset: 0x004BFAC8
		public override string ValueToString(float val, bool finalized, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			return base.ValueToString(val * 100f, finalized, numberSense);
		}
	}
}
