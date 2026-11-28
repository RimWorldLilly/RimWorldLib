using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A5E RID: 6750
	public class PortalContainerProxy : ThingOwner
	{
		// Token: 0x17001DD1 RID: 7633
		// (get) Token: 0x0600A97B RID: 43387 RVA: 0x003C1764 File Offset: 0x003BF964
		public override int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600A97C RID: 43388 RVA: 0x003C1767 File Offset: 0x003BF967
		public override int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (this.TryAdd(item, canMergeWithExistingStacks))
			{
				return count;
			}
			return 0;
		}

		// Token: 0x0600A97D RID: 43389 RVA: 0x003C1778 File Offset: 0x003BF978
		public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
		{
			Map otherMap = this.portal.GetOtherMap();
			IntVec3 destinationLocation = this.portal.GetDestinationLocation();
			this.portal.Notify_ThingAdded(item);
			Thing thing;
			GenDrop.TryDropSpawn(item, destinationLocation, otherMap, ThingPlaceMode.Near, out thing, null, null, true);
			return true;
		}

		// Token: 0x0600A97E RID: 43390 RVA: 0x003C17B9 File Offset: 0x003BF9B9
		public override int IndexOf(Thing item)
		{
			return -1;
		}

		// Token: 0x0600A97F RID: 43391 RVA: 0x003C17BC File Offset: 0x003BF9BC
		public override bool Remove(Thing item)
		{
			return false;
		}

		// Token: 0x0600A980 RID: 43392 RVA: 0x003C17BF File Offset: 0x003BF9BF
		protected override Thing GetAt(int index)
		{
			return null;
		}

		// Token: 0x04006328 RID: 25384
		public MapPortal portal;
	}
}
