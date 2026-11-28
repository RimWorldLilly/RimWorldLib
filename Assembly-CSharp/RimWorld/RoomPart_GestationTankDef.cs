using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F94 RID: 3988
	public class RoomPart_GestationTankDef : RoomPartDef
	{
		// Token: 0x060065C6 RID: 26054 RVA: 0x002438FE File Offset: 0x00241AFE
		public RoomPart_GestationTankDef()
		{
			this.workerClass = typeof(RoomPart_GestationTanks);
		}

		// Token: 0x04003CDD RID: 15581
		public List<RoomPart_GestationTankDef.TankOption> options = new List<RoomPart_GestationTankDef.TankOption>();

		// Token: 0x02002F6B RID: 12139
		public enum State
		{
			// Token: 0x0400C0C3 RID: 49347
			Dormant,
			// Token: 0x0400C0C4 RID: 49348
			Proximity,
			// Token: 0x0400C0C5 RID: 49349
			Empty
		}

		// Token: 0x02002F6C RID: 12140
		public class TankOption
		{
			// Token: 0x06011EBE RID: 73406 RVA: 0x0061D937 File Offset: 0x0061BB37
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "state", "weight");
			}

			// Token: 0x0400C0C6 RID: 49350
			public RoomPart_GestationTankDef.State state;

			// Token: 0x0400C0C7 RID: 49351
			public float weight = 1f;
		}
	}
}
