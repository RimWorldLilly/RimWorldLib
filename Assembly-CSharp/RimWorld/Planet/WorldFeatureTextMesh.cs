using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020022EC RID: 8940
	public abstract class WorldFeatureTextMesh
	{
		// Token: 0x1700265D RID: 9821
		// (get) Token: 0x0600DC9E RID: 56478
		public abstract bool Active { get; }

		// Token: 0x1700265E RID: 9822
		// (get) Token: 0x0600DC9F RID: 56479
		public abstract Vector3 Position { get; }

		// Token: 0x1700265F RID: 9823
		// (get) Token: 0x0600DCA0 RID: 56480
		// (set) Token: 0x0600DCA1 RID: 56481
		public abstract Color Color { get; set; }

		// Token: 0x17002660 RID: 9824
		// (get) Token: 0x0600DCA2 RID: 56482
		// (set) Token: 0x0600DCA3 RID: 56483
		public abstract string Text { get; set; }

		// Token: 0x17002661 RID: 9825
		// (set) Token: 0x0600DCA4 RID: 56484
		public abstract float Size { set; }

		// Token: 0x17002662 RID: 9826
		// (get) Token: 0x0600DCA5 RID: 56485
		// (set) Token: 0x0600DCA6 RID: 56486
		public abstract Quaternion Rotation { get; set; }

		// Token: 0x17002663 RID: 9827
		// (get) Token: 0x0600DCA7 RID: 56487
		// (set) Token: 0x0600DCA8 RID: 56488
		public abstract Vector3 LocalPosition { get; set; }

		// Token: 0x0600DCA9 RID: 56489
		public abstract void SetActive(bool active);

		// Token: 0x0600DCAA RID: 56490
		public abstract void Destroy();

		// Token: 0x0600DCAB RID: 56491
		public abstract void Init();

		// Token: 0x0600DCAC RID: 56492
		public abstract void WrapAroundPlanetSurface(PlanetLayer layer);
	}
}
