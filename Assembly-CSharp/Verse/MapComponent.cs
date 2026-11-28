using System;

namespace Verse
{
	// Token: 0x0200029D RID: 669
	public abstract class MapComponent : IExposable
	{
		// Token: 0x06001480 RID: 5248 RVA: 0x00075737 File Offset: 0x00073937
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00075746 File Offset: 0x00073946
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00075748 File Offset: 0x00073948
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0007574A File Offset: 0x0007394A
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0007574C File Offset: 0x0007394C
		public virtual void MapComponentDraw()
		{
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0007574E File Offset: 0x0007394E
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00075750 File Offset: 0x00073950
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00075752 File Offset: 0x00073952
		public virtual void MapGenerated()
		{
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00075754 File Offset: 0x00073954
		public virtual void MapRemoved()
		{
		}

		// Token: 0x0400121F RID: 4639
		public readonly Map map;
	}
}
