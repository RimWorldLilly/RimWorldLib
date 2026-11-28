using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001605 RID: 5637
	public static class FactionRelationKindUtility
	{
		// Token: 0x06008C80 RID: 35968 RVA: 0x00330E78 File Offset: 0x0032F078
		public static string GetLabel(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return "HostileLower".Translate();
			case FactionRelationKind.Neutral:
				return "NeutralLower".Translate();
			case FactionRelationKind.Ally:
				return "AllyLower".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06008C81 RID: 35969 RVA: 0x00330ED0 File Offset: 0x0032F0D0
		public static string GetLabelCap(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return "Hostile".Translate();
			case FactionRelationKind.Neutral:
				return "Neutral".Translate();
			case FactionRelationKind.Ally:
				return "Ally".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06008C82 RID: 35970 RVA: 0x00330F26 File Offset: 0x0032F126
		public static Color GetColor(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return ColorLibrary.RedReadable;
			case FactionRelationKind.Neutral:
				return new Color(0f, 0.75f, 1f);
			case FactionRelationKind.Ally:
				return Color.green;
			default:
				return Color.white;
			}
		}
	}
}
