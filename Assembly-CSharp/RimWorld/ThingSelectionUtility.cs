using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D8A RID: 7562
	public static class ThingSelectionUtility
	{
		// Token: 0x0600C406 RID: 50182 RVA: 0x0047BA2C File Offset: 0x00479C2C
		public static bool SelectableByMapClick(Thing t)
		{
			if (!t.def.selectable)
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.IsHiddenFromPlayer())
			{
				return false;
			}
			Thing spawnedParentOrMe = t.SpawnedParentOrMe;
			if (spawnedParentOrMe == null)
			{
				return false;
			}
			if (spawnedParentOrMe.def.size.x == 1 && spawnedParentOrMe.def.size.z == 1)
			{
				return !spawnedParentOrMe.Position.Fogged(spawnedParentOrMe.Map);
			}
			using (CellRect.Enumerator enumerator = spawnedParentOrMe.OccupiedRect().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Fogged(spawnedParentOrMe.Map))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C407 RID: 50183 RVA: 0x0047BAFC File Offset: 0x00479CFC
		public static bool SelectableByHotkey(Thing t)
		{
			return t.def.selectable && t.Spawned;
		}

		// Token: 0x0600C408 RID: 50184 RVA: 0x0047BB13 File Offset: 0x00479D13
		public static IEnumerable<Thing> MultiSelectableThingsInScreenRectDistinct(Rect rect)
		{
			ThingSelectionUtility.<MultiSelectableThingsInScreenRectDistinct>d__3 <MultiSelectableThingsInScreenRectDistinct>d__ = new ThingSelectionUtility.<MultiSelectableThingsInScreenRectDistinct>d__3(-2);
			<MultiSelectableThingsInScreenRectDistinct>d__.<>3__rect = rect;
			return <MultiSelectableThingsInScreenRectDistinct>d__;
		}

		// Token: 0x0600C409 RID: 50185 RVA: 0x0047BB24 File Offset: 0x00479D24
		private static Rect GetRectInWorldSpace(Rect rect)
		{
			Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
			Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
			Vector3 vector = UI.UIToMapPosition(screenLoc);
			Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
			return new Rect(vector.x, vector2.z, vector2.x - vector.x, vector.z - vector2.z);
		}

		// Token: 0x0600C40A RID: 50186 RVA: 0x0047BBAF File Offset: 0x00479DAF
		public static IEnumerable<Plan> MultiSelectablePlansInScreenRectDistinct(Rect rect, ColorDef match = null)
		{
			ThingSelectionUtility.<MultiSelectablePlansInScreenRectDistinct>d__6 <MultiSelectablePlansInScreenRectDistinct>d__ = new ThingSelectionUtility.<MultiSelectablePlansInScreenRectDistinct>d__6(-2);
			<MultiSelectablePlansInScreenRectDistinct>d__.<>3__rect = rect;
			<MultiSelectablePlansInScreenRectDistinct>d__.<>3__match = match;
			return <MultiSelectablePlansInScreenRectDistinct>d__;
		}

		// Token: 0x0600C40B RID: 50187 RVA: 0x0047BBC6 File Offset: 0x00479DC6
		public static IEnumerable<Zone> MultiSelectableZonesInScreenRectDistinct(Rect rect, Zone matchType = null)
		{
			ThingSelectionUtility.<MultiSelectableZonesInScreenRectDistinct>d__8 <MultiSelectableZonesInScreenRectDistinct>d__ = new ThingSelectionUtility.<MultiSelectableZonesInScreenRectDistinct>d__8(-2);
			<MultiSelectableZonesInScreenRectDistinct>d__.<>3__rect = rect;
			<MultiSelectableZonesInScreenRectDistinct>d__.<>3__matchType = matchType;
			return <MultiSelectableZonesInScreenRectDistinct>d__;
		}

		// Token: 0x0600C40C RID: 50188 RVA: 0x0047BBE0 File Offset: 0x00479DE0
		private static CellRect GetMapRect(Rect rect)
		{
			Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
			Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
			Vector3 vector = UI.UIToMapPosition(screenLoc);
			Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
			return new CellRect
			{
				minX = Mathf.FloorToInt(vector.x),
				minZ = Mathf.FloorToInt(vector2.z),
				maxX = Mathf.FloorToInt(vector2.x),
				maxZ = Mathf.FloorToInt(vector.z)
			};
		}

		// Token: 0x0600C40D RID: 50189 RVA: 0x0047BC94 File Offset: 0x00479E94
		public static void SelectNextColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count == 0)
			{
				return;
			}
			bool worldSelected = WorldRendererUtility.WorldSelected;
			int num = -1;
			for (int i = ThingSelectionUtility.tmpColonists.Count - 1; i >= 0; i--)
			{
				if ((!worldSelected && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldSelected && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[0], CameraJumper.MovementMode.Pan);
			}
			else
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[(num + 1) % ThingSelectionUtility.tmpColonists.Count], CameraJumper.MovementMode.Pan);
			}
			ThingSelectionUtility.tmpColonists.Clear();
		}

		// Token: 0x0600C40E RID: 50190 RVA: 0x0047BD94 File Offset: 0x00479F94
		public static void SelectPreviousColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count == 0)
			{
				return;
			}
			bool worldSelected = WorldRendererUtility.WorldSelected;
			int num = -1;
			for (int i = 0; i < ThingSelectionUtility.tmpColonists.Count; i++)
			{
				if ((!worldSelected && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldSelected && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[ThingSelectionUtility.tmpColonists.Count - 1], CameraJumper.MovementMode.Pan);
			}
			else
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[GenMath.PositiveMod(num - 1, ThingSelectionUtility.tmpColonists.Count)], CameraJumper.MovementMode.Pan);
			}
			ThingSelectionUtility.tmpColonists.Clear();
		}

		// Token: 0x0400700E RID: 28686
		private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

		// Token: 0x0400700F RID: 28687
		private static readonly HashSet<Plan> yieldedPlans = new HashSet<Plan>();

		// Token: 0x04007010 RID: 28688
		private static readonly HashSet<Zone> yieldedZones = new HashSet<Zone>();

		// Token: 0x04007011 RID: 28689
		private static readonly List<Pawn> tmpColonists = new List<Pawn>();
	}
}
