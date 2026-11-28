using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B78 RID: 7032
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x0600B48E RID: 46222 RVA: 0x003FABAE File Offset: 0x003F8DAE
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600B48F RID: 46223 RVA: 0x003FABC8 File Offset: 0x003F8DC8
		public void ExposeData()
		{
			Scribe_Collections.Look<PassingShip>(ref this.passingShips, "passingShips", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.passingShips.Count; i++)
				{
					this.passingShips[i].passingShipManager = this;
				}
			}
		}

		// Token: 0x0600B490 RID: 46224 RVA: 0x003FAC1B File Offset: 0x003F8E1B
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x0600B491 RID: 46225 RVA: 0x003FAC30 File Offset: 0x003F8E30
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x0600B492 RID: 46226 RVA: 0x003FAC48 File Offset: 0x003F8E48
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x0600B493 RID: 46227 RVA: 0x003FAC80 File Offset: 0x003F8E80
		public void RemoveAllShipsOfFaction(Faction faction)
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				if (this.passingShips[i].Faction == faction)
				{
					this.passingShips[i].Depart();
				}
			}
		}

		// Token: 0x0600B494 RID: 46228 RVA: 0x003FACCC File Offset: 0x003F8ECC
		internal void DebugSendAllShipsAway()
		{
			PassingShipManager.tmpPassingShips.Clear();
			PassingShipManager.tmpPassingShips.AddRange(this.passingShips);
			for (int i = 0; i < PassingShipManager.tmpPassingShips.Count; i++)
			{
				PassingShipManager.tmpPassingShips[i].Depart();
			}
			Messages.Message("All passing ships sent away.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x040067E9 RID: 26601
		public Map map;

		// Token: 0x040067EA RID: 26602
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x040067EB RID: 26603
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();
	}
}
