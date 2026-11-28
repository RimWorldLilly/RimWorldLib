using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

// Token: 0x02000021 RID: 33
public static class DarknessCombatUtility
{
	// Token: 0x060000F1 RID: 241 RVA: 0x00008F4F File Offset: 0x0000714F
	public static bool IsOutdoorsAndDark(Thing thing)
	{
		return DarknessCombatUtility.Outdoors(thing) && !DarknessCombatUtility.IsOutdoorsLit(thing.Map);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00008F69 File Offset: 0x00007169
	public static bool IsOutdoorsAndLit(Thing thing)
	{
		return DarknessCombatUtility.Outdoors(thing) && DarknessCombatUtility.IsOutdoorsLit(thing.Map);
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00008F80 File Offset: 0x00007180
	public static bool IsIndoorsAndDark(Thing thing)
	{
		return !DarknessCombatUtility.Outdoors(thing) && (thing.Map.glowGrid.PsychGlowAt(thing.Position) == PsychGlow.Dark || DarklightUtility.IsDarklightAt(thing.Position, thing.Map));
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00008FB7 File Offset: 0x000071B7
	public static bool IsIndoorsAndLit(Thing thing)
	{
		return !DarknessCombatUtility.Outdoors(thing) && thing.Map.glowGrid.PsychGlowAt(thing.Position) == PsychGlow.Lit && !DarklightUtility.IsDarklightAt(thing.Position, thing.Map);
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00008FF0 File Offset: 0x000071F0
	private static bool Outdoors(Thing thing)
	{
		RoofDef roof = thing.Position.GetRoof(thing.Map);
		if (roof != null && (roof.isNatural || roof.isThickRoof))
		{
			return false;
		}
		Room room = thing.Position.GetRoom(thing.Map);
		return room != null && room.PsychologicallyOutdoors;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00009041 File Offset: 0x00007241
	private static bool IsOutdoorsLit(Map map)
	{
		return map.skyManager.CurSkyGlow > 0.35f;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00009055 File Offset: 0x00007255
	public static IEnumerable<StatDrawEntry> GetStatEntriesForPawn(Pawn pawn)
	{
		DarknessCombatUtility.<GetStatEntriesForPawn>d__8 <GetStatEntriesForPawn>d__ = new DarknessCombatUtility.<GetStatEntriesForPawn>d__8(-2);
		<GetStatEntriesForPawn>d__.<>3__pawn = pawn;
		return <GetStatEntriesForPawn>d__;
	}

	// Token: 0x04000048 RID: 72
	private const float SkyGlowDarkThreshold = 0.35f;

	// Token: 0x04000049 RID: 73
	private static List<StatModifier> tmpStatDefs = new List<StatModifier>();
}
