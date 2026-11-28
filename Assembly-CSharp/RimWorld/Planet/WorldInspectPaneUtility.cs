using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023FB RID: 9211
	public static class WorldInspectPaneUtility
	{
		// Token: 0x0600E64F RID: 58959 RVA: 0x0055A90C File Offset: 0x00558B0C
		public static string AdjustedLabelFor(List<WorldObject> worldObjects, Rect rect)
		{
			if (worldObjects.Count == 1)
			{
				return worldObjects[0].LabelCap;
			}
			if (WorldInspectPaneUtility.AllLabelsAreSame(worldObjects))
			{
				return worldObjects[0].LabelCap + " x" + worldObjects.Count.ToStringCached();
			}
			return "VariousLabel".Translate();
		}

		// Token: 0x0600E650 RID: 58960 RVA: 0x0055A968 File Offset: 0x00558B68
		private static bool AllLabelsAreSame(List<WorldObject> worldObjects)
		{
			if (worldObjects.Count <= 1)
			{
				return true;
			}
			string labelCap = worldObjects[0].LabelCap;
			for (int i = 1; i < worldObjects.Count; i++)
			{
				if (worldObjects[i].LabelCap != labelCap)
				{
					return false;
				}
			}
			return true;
		}
	}
}
