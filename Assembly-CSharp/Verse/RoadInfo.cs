using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002A0 RID: 672
	public class RoadInfo : MapComponent
	{
		// Token: 0x06001494 RID: 5268 RVA: 0x00075A41 File Offset: 0x00073C41
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00075A55 File Offset: 0x00073C55
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04001221 RID: 4641
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
