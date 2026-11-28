using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020000F1 RID: 241
	public static class CameraJumper
	{
		// Token: 0x060008AA RID: 2218 RVA: 0x0003128E File Offset: 0x0002F48E
		public static void TryJumpAndSelect(GlobalTargetInfo target, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			if (!target.IsValid)
			{
				return;
			}
			CameraJumper.TryJump(target, mode);
			CameraJumper.TrySelect(target, false);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000312A8 File Offset: 0x0002F4A8
		public static void TrySelect(GlobalTargetInfo target, bool skipTargetAdjustment = false)
		{
			if (!target.IsValid)
			{
				return;
			}
			if (!skipTargetAdjustment)
			{
				target = CameraJumper.GetAdjustedTarget(target);
			}
			if (target.HasThing)
			{
				CameraJumper.TrySelectInternal(target.Thing);
				return;
			}
			if (target.HasWorldObject)
			{
				CameraJumper.TrySelectInternal(target.WorldObject);
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000312F8 File Offset: 0x0002F4F8
		private static void TrySelectInternal(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (thing.Spawned && thing.def.selectable)
			{
				bool flag = CameraJumper.TryHideWorld();
				bool flag2 = false;
				if (thing.Map != Find.CurrentMap)
				{
					Current.Game.CurrentMap = thing.Map;
					flag2 = true;
					if (!flag)
					{
						SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					}
				}
				if (flag || flag2)
				{
					Find.CameraDriver.JumpToCurrentMapLoc(thing.Position);
				}
				Find.Selector.ClearSelection();
				Find.Selector.Select(thing, true, true);
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00031384 File Offset: 0x0002F584
		private static void TrySelectInternal(WorldObject worldObject)
		{
			if (Find.World == null)
			{
				return;
			}
			if (worldObject.Spawned && worldObject.SelectableNow)
			{
				CameraJumper.TryShowWorld();
				Find.WorldSelector.ClearSelection();
				Find.WorldSelector.Select(worldObject, true);
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000313BC File Offset: 0x0002F5BC
		public static void TryJump(GlobalTargetInfo target, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			if (!target.IsValid)
			{
				return;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				CameraJumper.TryJumpInternal(target.Thing, mode);
				return;
			}
			if (target.HasWorldObject)
			{
				CameraJumper.TryJumpInternal(target.WorldObject);
				return;
			}
			if (target.Cell.IsValid)
			{
				CameraJumper.TryJumpInternal(target.Cell, target.Map, mode);
				return;
			}
			CameraJumper.TryJumpInternal(target.Tile);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0003143A File Offset: 0x0002F63A
		public static void TryJump(IntVec3 cell, Map map, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(cell, map, false), mode);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0003144A File Offset: 0x0002F64A
		public static void TryJump(PlanetTile tile, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(tile), mode);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00031458 File Offset: 0x0002F658
		private static void TryJumpInternal(Thing thing, CameraJumper.MovementMode mode)
		{
			CameraJumper.TryJumpInternal(thing.PositionHeld, thing.MapHeld, mode);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0003146C File Offset: 0x0002F66C
		private static void TryJumpInternal(IntVec3 cell, Map map, CameraJumper.MovementMode mode)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (!cell.IsValid)
			{
				return;
			}
			if (map == null || !Find.Maps.Contains(map))
			{
				return;
			}
			if (!cell.InBounds(map))
			{
				return;
			}
			bool flag = CameraJumper.TryHideWorld();
			bool flag2 = false;
			if (Find.CurrentMap != map)
			{
				Current.Game.CurrentMap = map;
				flag2 = true;
				if (!flag)
				{
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
				}
			}
			CameraJumper.JumpLocalInternal(cell, (!Prefs.SmoothCameraJumps || flag || flag2) ? CameraJumper.MovementMode.Cut : mode);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000314EA File Offset: 0x0002F6EA
		private static void TryJumpInternal(WorldObject worldObject)
		{
			CameraJumper.TryJumpInternal(worldObject.Tile);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x000314F7 File Offset: 0x0002F6F7
		private static void TryJumpInternal(PlanetTile tile)
		{
			if (Find.World == null)
			{
				return;
			}
			if (!tile.Valid)
			{
				return;
			}
			CameraJumper.TryShowWorld();
			Find.WorldCameraDriver.JumpTo(tile);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0003151C File Offset: 0x0002F71C
		private static void JumpLocalInternal(IntVec3 localCell, CameraJumper.MovementMode mode)
		{
			if (mode != CameraJumper.MovementMode.Pan)
			{
				if (mode != CameraJumper.MovementMode.Cut)
				{
				}
				Find.CameraDriver.JumpToCurrentMapLoc(localCell);
				return;
			}
			Find.CameraDriver.PanToMapLoc(localCell);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00031540 File Offset: 0x0002F740
		public static bool CanJump(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return false;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				return target.Thing.MapHeld != null && Find.Maps.Contains(target.Thing.MapHeld) && target.Thing.PositionHeld.IsValid && target.Thing.PositionHeld.InBounds(target.Thing.MapHeld);
			}
			if (target.HasWorldObject)
			{
				return target.WorldObject.Spawned;
			}
			if (target.Cell.IsValid)
			{
				return target.Map != null && Find.Maps.Contains(target.Map) && target.Cell.IsValid && target.Cell.InBounds(target.Map);
			}
			return target.Tile.Valid;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00031640 File Offset: 0x0002F840
		public static GlobalTargetInfo GetAdjustedTarget(GlobalTargetInfo target)
		{
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					return thing;
				}
				GlobalTargetInfo result = GlobalTargetInfo.Invalid;
				for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
				{
					Thing thing2 = parentHolder as Thing;
					if (thing2 != null && thing2.Spawned)
					{
						result = thing2;
						break;
					}
					ThingComp thingComp = parentHolder as ThingComp;
					if (thingComp != null && thingComp.parent.Spawned)
					{
						result = thingComp.parent;
						break;
					}
					WorldObject worldObject = parentHolder as WorldObject;
					if (worldObject != null && worldObject.Spawned)
					{
						result = worldObject;
						break;
					}
				}
				if (result.IsValid)
				{
					return result;
				}
				if (target.Thing.TryGetComp<CompCauseGameCondition>() != null)
				{
					List<Site> sites = Find.WorldObjects.Sites;
					for (int i = 0; i < sites.Count; i++)
					{
						for (int j = 0; j < sites[i].parts.Count; j++)
						{
							if (sites[i].parts[j].conditionCauser == target.Thing)
							{
								return sites[i];
							}
						}
					}
				}
				if (thing.Tile.Valid)
				{
					return new GlobalTargetInfo(thing.Tile);
				}
			}
			else if (target.Cell.IsValid && target.Tile.Valid && target.Map != null && !Find.Maps.Contains(target.Map))
			{
				MapParent parent = target.Map.Parent;
				if (parent != null && parent.Spawned)
				{
					return parent;
				}
				return GlobalTargetInfo.Invalid;
			}
			return target;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00031804 File Offset: 0x0002FA04
		public static GlobalTargetInfo GetWorldTarget(GlobalTargetInfo target)
		{
			GlobalTargetInfo adjustedTarget = CameraJumper.GetAdjustedTarget(target);
			if (!adjustedTarget.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (adjustedTarget.IsWorldTarget)
			{
				return adjustedTarget;
			}
			return CameraJumper.GetWorldTargetOfMap(adjustedTarget.Map);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0003183E File Offset: 0x0002FA3E
		public static GlobalTargetInfo GetWorldTargetOfMap(Map map)
		{
			if (map == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (map.Parent != null && map.Parent.Spawned)
			{
				return map.Parent;
			}
			return GlobalTargetInfo.Invalid;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00031870 File Offset: 0x0002FA70
		public static bool TryHideWorld()
		{
			if (!WorldRendererUtility.WorldSelected)
			{
				return false;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (Find.World.renderer.wantedMode != WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				Find.MainTabsRoot.EscapeCurrentTab(false);
				return true;
			}
			return false;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x000318CC File Offset: 0x0002FACC
		public static bool TryShowWorld()
		{
			if (WorldRendererUtility.WorldSelected)
			{
				return true;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (Find.World.renderer.wantedMode == WorldRenderMode.None)
			{
				AmbientSoundManager.EnsureWorldAmbientSoundCreated();
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				Find.MainTabsRoot.EscapeCurrentTab(false);
				return true;
			}
			return false;
		}

		// Token: 0x02002509 RID: 9481
		public enum MovementMode
		{
			// Token: 0x04009772 RID: 38770
			Pan,
			// Token: 0x04009773 RID: 38771
			Cut
		}
	}
}
