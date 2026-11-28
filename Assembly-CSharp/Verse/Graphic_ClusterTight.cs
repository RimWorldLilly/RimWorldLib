using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000560 RID: 1376
	public class Graphic_ClusterTight : Graphic_Cluster
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x000FBE6E File Offset: 0x000FA06E
		protected override float PositionVariance
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x060028F9 RID: 10489 RVA: 0x000FBE75 File Offset: 0x000FA075
		protected override float SizeVariance
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000FBE7C File Offset: 0x000FA07C
		protected override int ScatterCount(Thing thing)
		{
			Filth filth = thing as Filth;
			if (filth != null && !filth.drawInstances.NullOrEmpty<Filth.FilthInstance>())
			{
				return filth.drawInstances.Count;
			}
			return 1;
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x000FBEB0 File Offset: 0x000FA0B0
		protected override Vector3 GetCenter(Thing thing, int index)
		{
			Filth filth = thing as Filth;
			if (filth != null && filth.drawInstances != null && filth.drawInstances.Count > index)
			{
				return filth.drawInstances[index].drawPos;
			}
			return thing.DrawPos;
		}
	}
}
