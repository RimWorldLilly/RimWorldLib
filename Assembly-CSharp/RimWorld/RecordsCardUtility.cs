using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CED RID: 7405
	public static class RecordsCardUtility
	{
		// Token: 0x170022AB RID: 8875
		// (get) Token: 0x0600BE94 RID: 48788 RVA: 0x0044E288 File Offset: 0x0044C488
		private static List<RecordDef> AllRecordsSorted
		{
			get
			{
				if (RecordsCardUtility.allRecordsSorted == null)
				{
					RecordsCardUtility.allRecordsSorted = (from x in DefDatabase<RecordDef>.AllDefsListForReading
					orderby x.displayOrder
					select x).ToList<RecordDef>();
				}
				return RecordsCardUtility.allRecordsSorted;
			}
		}

		// Token: 0x0600BE95 RID: 48789 RVA: 0x0044E2D4 File Offset: 0x0044C4D4
		public static void DrawRecordsCard(Rect rect, Pawn pawn)
		{
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, RecordsCardUtility.listHeight);
			Widgets.BeginScrollView(rect, ref RecordsCardUtility.scrollPosition, rect2, true);
			Rect leftRect = rect2;
			leftRect.width *= 0.5f;
			Rect rightRect = rect2;
			rightRect.x = leftRect.xMax;
			rightRect.width = rect2.width - rightRect.x;
			leftRect.xMax -= 6f;
			rightRect.xMin += 6f;
			float a = RecordsCardUtility.DrawTimeRecords(leftRect, pawn);
			float b = RecordsCardUtility.DrawMiscRecords(rightRect, pawn);
			RecordsCardUtility.listHeight = Mathf.Max(a, b) + 100f;
			Widgets.EndScrollView();
		}

		// Token: 0x0600BE96 RID: 48790 RVA: 0x0044E3A0 File Offset: 0x0044C5A0
		private static float DrawTimeRecords(Rect leftRect, Pawn pawn)
		{
			List<RecordDef> list = RecordsCardUtility.AllRecordsSorted;
			float num = 0f;
			Widgets.BeginGroup(leftRect);
			Widgets.ListSeparator(ref num, leftRect.width, "TimeRecordsCategory".Translate());
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].type == RecordType.Time)
				{
					num += RecordsCardUtility.DrawRecord(8f, num, leftRect.width - 8f, list[i], pawn);
				}
			}
			Widgets.EndGroup();
			return num;
		}

		// Token: 0x0600BE97 RID: 48791 RVA: 0x0044E424 File Offset: 0x0044C624
		private static float DrawMiscRecords(Rect rightRect, Pawn pawn)
		{
			List<RecordDef> list = RecordsCardUtility.AllRecordsSorted;
			float num = 0f;
			Widgets.BeginGroup(rightRect);
			Widgets.ListSeparator(ref num, rightRect.width, "MiscRecordsCategory".Translate());
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].type == RecordType.Int || list[i].type == RecordType.Float)
				{
					num += RecordsCardUtility.DrawRecord(8f, num, rightRect.width - 8f, list[i], pawn);
				}
			}
			Widgets.EndGroup();
			return num;
		}

		// Token: 0x0600BE98 RID: 48792 RVA: 0x0044E4B8 File Offset: 0x0044C6B8
		private static float DrawRecord(float x, float y, float width, RecordDef record, Pawn pawn)
		{
			float num = width * 0.45f;
			string text;
			if (record.type == RecordType.Time)
			{
				text = pawn.records.GetAsInt(record).ToStringTicksToPeriod(true, false, true, true, false);
			}
			else
			{
				text = pawn.records.GetValue(record).ToString("0.##");
			}
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(text, num));
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			Rect rect2 = rect;
			rect2.width -= num;
			Widgets.Label(rect2, record.LabelCap);
			Rect rect3 = rect;
			rect3.x = rect2.xMax;
			rect3.width = num;
			Widgets.Label(rect3, text);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => record.description, record.GetHashCode()));
			}
			return rect.height;
		}

		// Token: 0x04006D67 RID: 28007
		private static Vector2 scrollPosition;

		// Token: 0x04006D68 RID: 28008
		private static float listHeight;

		// Token: 0x04006D69 RID: 28009
		private const float RecordsLeftPadding = 8f;

		// Token: 0x04006D6A RID: 28010
		private static List<RecordDef> allRecordsSorted;
	}
}
