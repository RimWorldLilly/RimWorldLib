using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000140 RID: 320
	public class AsymmetricLinkData
	{
		// Token: 0x040005E4 RID: 1508
		public LinkFlags linkFlags;

		// Token: 0x040005E5 RID: 1509
		public bool linkToDoors;

		// Token: 0x040005E6 RID: 1510
		public AsymmetricLinkData.BorderData drawDoorBorderEast;

		// Token: 0x040005E7 RID: 1511
		public AsymmetricLinkData.BorderData drawDoorBorderWest;

		// Token: 0x02002529 RID: 9513
		public class BorderData
		{
			// Token: 0x170029EE RID: 10734
			// (get) Token: 0x0600ED4B RID: 60747 RVA: 0x0057706E File Offset: 0x0057526E
			public Material Mat
			{
				get
				{
					if (this.colorMat == null)
					{
						this.colorMat = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					}
					return this.colorMat;
				}
			}

			// Token: 0x04009806 RID: 38918
			public Color color = Color.black;

			// Token: 0x04009807 RID: 38919
			public Vector2 size;

			// Token: 0x04009808 RID: 38920
			public Vector3 offset;

			// Token: 0x04009809 RID: 38921
			private Material colorMat;
		}
	}
}
