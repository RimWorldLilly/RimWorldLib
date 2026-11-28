using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001763 RID: 5987
	public class TransportShipManager : IExposable
	{
		// Token: 0x1700197B RID: 6523
		// (get) Token: 0x060094FF RID: 38143 RVA: 0x00364510 File Offset: 0x00362710
		public List<TransportShip> AllTransportShips
		{
			get
			{
				return this.ships;
			}
		}

		// Token: 0x06009500 RID: 38144 RVA: 0x00364518 File Offset: 0x00362718
		public void RegisterShipObject(TransportShip s)
		{
			this.ships.Add(s);
		}

		// Token: 0x06009501 RID: 38145 RVA: 0x00364526 File Offset: 0x00362726
		public void DeregisterShipObject(TransportShip s)
		{
			if (s != null)
			{
				s.EndCurrentJob();
				this.ships.Remove(s);
			}
		}

		// Token: 0x06009502 RID: 38146 RVA: 0x00364540 File Offset: 0x00362740
		public void ShipObjectsTick()
		{
			for (int i = this.ships.Count - 1; i >= 0; i--)
			{
				this.ships[i].Tick();
			}
		}

		// Token: 0x06009503 RID: 38147 RVA: 0x00364578 File Offset: 0x00362778
		public void ExposeData()
		{
			Scribe_Collections.Look<TransportShip>(ref this.ships, "ships", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ships.RemoveAll((TransportShip x) => x == null) != 0)
				{
					Log.Error("Removed some null ship objects.");
				}
				this.ships.RemoveAll((TransportShip x) => x.shipThing == null);
			}
		}

		// Token: 0x040059CF RID: 22991
		private List<TransportShip> ships = new List<TransportShip>();
	}
}
