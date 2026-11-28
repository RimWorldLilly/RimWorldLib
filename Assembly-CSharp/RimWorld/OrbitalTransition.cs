using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CD RID: 6093
	public class OrbitalTransition : MusicTransition
	{
		// Token: 0x06009816 RID: 38934 RVA: 0x00372574 File Offset: 0x00370774
		public override bool IsTransitionSatisfied()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Tile.LayerDef.isSpace)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
