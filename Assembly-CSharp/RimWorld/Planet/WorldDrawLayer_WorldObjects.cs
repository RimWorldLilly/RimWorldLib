using System;
using System.Collections;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002328 RID: 9000
	public abstract class WorldDrawLayer_WorldObjects : WorldDrawLayer
	{
		// Token: 0x170026FA RID: 9978
		// (get) Token: 0x0600DE46 RID: 56902 RVA: 0x0052A959 File Offset: 0x00528B59
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026FB RID: 9979
		// (get) Token: 0x0600DE47 RID: 56903 RVA: 0x0052A95C File Offset: 0x00528B5C
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026FC RID: 9980
		// (get) Token: 0x0600DE48 RID: 56904 RVA: 0x0052A95F File Offset: 0x00528B5F
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawWorldObjects;
			}
		}

		// Token: 0x0600DE49 RID: 56905
		protected abstract bool ShouldSkip(WorldObject worldObject);

		// Token: 0x0600DE4A RID: 56906 RVA: 0x0052A966 File Offset: 0x00528B66
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_WorldObjects.<Regenerate>d__7 <Regenerate>d__ = new WorldDrawLayer_WorldObjects.<Regenerate>d__7(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}
	}
}
