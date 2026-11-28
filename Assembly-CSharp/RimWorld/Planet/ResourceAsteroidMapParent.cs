using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023D2 RID: 9170
	public class ResourceAsteroidMapParent : SpaceMapParent
	{
		// Token: 0x17002837 RID: 10295
		// (get) Token: 0x0600E483 RID: 58499 RVA: 0x0054EA36 File Offset: 0x0054CC36
		public override Color ExpandingIconColor
		{
			get
			{
				return this.cachedStuffColor;
			}
		}

		// Token: 0x0600E484 RID: 58500 RVA: 0x0054EA40 File Offset: 0x0054CC40
		public override void SpawnSetup()
		{
			base.SpawnSetup();
			ThingDef mineableThing = this.preciousResource.building.mineableThing;
			this.cachedStuffColor = mineableThing.stuffProps.color;
		}

		// Token: 0x04009162 RID: 37218
		private Color cachedStuffColor;
	}
}
