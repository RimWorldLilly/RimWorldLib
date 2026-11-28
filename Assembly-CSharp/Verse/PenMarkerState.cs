using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000232 RID: 562
	public class PenMarkerState
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0005DD54 File Offset: 0x0005BF54
		public bool Enclosed
		{
			get
			{
				return this.Calc().Enclosed;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x0005DD61 File Offset: 0x0005BF61
		public bool Unenclosed
		{
			get
			{
				return !this.Enclosed;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0005DD6C File Offset: 0x0005BF6C
		public bool PassableDoors
		{
			get
			{
				return this.Calc().PassableDoors;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0005DD79 File Offset: 0x0005BF79
		public bool HasOutsideAccess
		{
			get
			{
				return !this.Enclosed || this.Calc().ImpassableDoors;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x0005DD90 File Offset: 0x0005BF90
		public List<Region> DirectlyConnectedRegions
		{
			get
			{
				return this.Calc().DirectlyConnectedRegions;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x0005DD9D File Offset: 0x0005BF9D
		public HashSet<Region> ConnectedRegions
		{
			get
			{
				return this.Calc().ConnectedRegions;
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0005DDAA File Offset: 0x0005BFAA
		public bool ContainsConnectedRegion(Region r)
		{
			return this.Calc().ContainsConnectedRegion(r);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0005DDB8 File Offset: 0x0005BFB8
		public PenMarkerState(CompAnimalPenMarker marker)
		{
			this.marker = marker;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0005DDC8 File Offset: 0x0005BFC8
		private AnimalPenEnclosureStateCalculator Calc()
		{
			if (this.state == null)
			{
				this.state = new AnimalPenEnclosureStateCalculator();
				this.state.Recalulate(this.marker.parent.Position, this.marker.parent.Map);
			}
			else if (this.state.NeedsRecalculation())
			{
				this.state.Recalulate(this.marker.parent.Position, this.marker.parent.Map);
			}
			return this.state;
		}

		// Token: 0x04000F48 RID: 3912
		private readonly CompAnimalPenMarker marker;

		// Token: 0x04000F49 RID: 3913
		private AnimalPenEnclosureStateCalculator state;
	}
}
