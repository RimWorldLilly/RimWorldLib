using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001841 RID: 6209
	public class CompProperties_CableConnection : CompProperties
	{
		// Token: 0x06009B75 RID: 39797 RVA: 0x0038082F File Offset: 0x0037EA2F
		public CompProperties_CableConnection()
		{
			this.compClass = typeof(CompCableConnection);
		}

		// Token: 0x04005C61 RID: 23649
		public Color color = Color.white;

		// Token: 0x04005C62 RID: 23650
		public List<List<Vector3>> offsets = new List<List<Vector3>>();

		// Token: 0x04005C63 RID: 23651
		public bool drawMote;

		// Token: 0x04005C64 RID: 23652
		public ThingDef moteDef;
	}
}
