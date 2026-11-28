using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200054C RID: 1356
	public class Gizmo_RoomStats : Gizmo
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x000F8355 File Offset: 0x000F6555
		private Room Room
		{
			get
			{
				return Gizmo_RoomStats.GetRoomToShowStatsFor(this.building);
			}
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000F8362 File Offset: 0x000F6562
		public Gizmo_RoomStats(Building building)
		{
			this.building = building;
			this.Order = -100f;
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x000F837C File Offset: 0x000F657C
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(300f, maxWidth);
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000F838C File Offset: 0x000F658C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Room room = this.Room;
			if (room == null)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			Text.WordWrap = false;
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero().ContractedBy(10f);
			Text.Font = GameFont.Small;
			Rect rect3 = new Rect(rect2.x, rect2.y - 2f, rect2.width, 100f);
			float stat = room.GetStat(RoomStatDefOf.Impressiveness);
			RoomStatScoreStage scoreStage = RoomStatDefOf.Impressiveness.GetScoreStage(stat);
			string str = string.Concat(new string[]
			{
				room.Role.PostProcessedLabelCap(room),
				", ",
				scoreStage.label,
				" (",
				RoomStatDefOf.Impressiveness.ScoreToString(stat),
				")"
			});
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			float num = rect2.y + Text.LineHeight + Text.SpaceBetweenLines + 7f;
			GUI.color = Gizmo_RoomStats.RoomStatsColor;
			Text.Font = GameFont.Tiny;
			List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
			int num2 = 0;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (!allDefsListForReading[i].isHidden && allDefsListForReading[i] != RoomStatDefOf.Impressiveness)
				{
					float stat2 = room.GetStat(allDefsListForReading[i]);
					RoomStatScoreStage scoreStage2 = allDefsListForReading[i].GetScoreStage(stat2);
					Rect rect4;
					if (num2 % 2 == 0)
					{
						rect4 = new Rect(rect2.x, num, rect2.width / 2f, 100f);
					}
					else
					{
						rect4 = new Rect(rect2.x + rect2.width / 2f, num, rect2.width / 2f, 100f);
					}
					string str2 = scoreStage2.label.CapitalizeFirst() + " (" + allDefsListForReading[i].ScoreToString(stat2) + ")";
					Widgets.Label(rect4, str2.Truncate(rect4.width, null));
					if (num2 % 2 == 1)
					{
						num += Text.LineHeight + Text.SpaceBetweenLines;
					}
					num2++;
				}
			}
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Widgets.EndGroup();
			Text.WordWrap = true;
			GenUI.AbsorbClicksInRect(rect);
			if (Mouse.IsOver(rect))
			{
				Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(false, true);
				Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
				{
					float num3 = 12f;
					EnvironmentStatsDrawer.DoRoomInfo(room, ref num3, windowRect);
				}, true, false, 1f, null, false);
				return new GizmoResult(GizmoState.Mouseover);
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000F8678 File Offset: 0x000F6878
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			Room room = this.Room;
			if (room != null)
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000F869C File Offset: 0x000F689C
		public static Room GetRoomToShowStatsFor(Building building)
		{
			if (!building.Spawned || building.Fogged())
			{
				return null;
			}
			Room room = null;
			if (building.def.passability != Traversability.Impassable)
			{
				room = building.GetRoom(RegionType.Set_All);
			}
			else if (building.def.hasInteractionCell)
			{
				room = building.InteractionCell.GetRoom(building.Map);
			}
			else
			{
				CellRect cellRect = building.OccupiedRect().ExpandedBy(1);
				foreach (IntVec3 intVec in cellRect)
				{
					if (cellRect.IsOnEdge(intVec))
					{
						room = intVec.GetRoom(building.Map);
						if (Gizmo_RoomStats.<GetRoomToShowStatsFor>g__IsValid|8_0(room))
						{
							break;
						}
					}
				}
			}
			if (!Gizmo_RoomStats.<GetRoomToShowStatsFor>g__IsValid|8_0(room))
			{
				return null;
			}
			return room;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000F878F File Offset: 0x000F698F
		[CompilerGenerated]
		internal static bool <GetRoomToShowStatsFor>g__IsValid|8_0(Room r)
		{
			return r != null && !r.Fogged && r.Role != RoomRoleDefOf.None;
		}

		// Token: 0x04001C61 RID: 7265
		private Building building;

		// Token: 0x04001C62 RID: 7266
		private static readonly Color RoomStatsColor = new Color(0.75f, 0.75f, 0.75f);
	}
}
